#!/bin/bash

# Script d'orchestration de la stack de documentation BlogApi
# Usage: ./docs-stack.sh <build|generate|clean|serve|stop|help>

# Couleurs
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
CYAN='\033[0;36m'
NC='\033[0m' # No Color

# Fonctions de messages
info() { echo -e "${CYAN}$1${NC}"; }
success() { echo -e "${GREEN}$1${NC}"; }
error() { echo -e "${RED}$1${NC}"; }
warning() { echo -e "${YELLOW}$1${NC}"; }

# Banner
show_banner() {
    echo ""
    echo -e "${CYAN}╔════════════════════════════════════════════════════════╗${NC}"
    echo -e "${CYAN}║    📚 Stack de Documentation BlogApi                  ║${NC}"
    echo -e "${CYAN}║    Génération automatisée avec Docker Compose         ║${NC}"
    echo -e "${CYAN}╚════════════════════════════════════════════════════════╝${NC}"
    echo ""
}

# Vérifier les prérequis
check_prerequisites() {
    info "🔍 Vérification des prérequis..."
    
    # Vérifier Docker
    if command -v docker &> /dev/null; then
        success "✓ Docker installé : $(docker --version)"
    else
        error "✗ Docker n'est pas installé"
        return 1
    fi
    
    # Vérifier Docker Compose
    if command -v docker-compose &> /dev/null; then
        success "✓ Docker Compose installé : $(docker-compose --version)"
    else
        error "✗ Docker Compose n'est pas installé"
        return 1
    fi
    
    echo ""
    return 0
}

# Action: Build
action_build() {
    info "🏗️  Construction de l'image Docker..."
    warning "⚠️  Attention : La première construction peut prendre 5-10 minutes"
    echo ""
    
    docker-compose build
    
    if [ $? -eq 0 ]; then
        success "\n✓ Image construite avec succès"
        
        # Afficher la taille de l'image
        image_size=$(docker images blogapi-doxygen --format "{{.Size}}" | head -n1)
        info "📦 Taille de l'image : $image_size"
    else
        error "\n✗ Erreur lors de la construction de l'image"
        exit 1
    fi
}

# Action: Generate
action_generate() {
    info "📖 Génération de la documentation complète..."
    echo ""
    
    # Créer le dossier de sortie si nécessaire
    mkdir -p doxygen-output
    
    # Étape 1: Générer HTML + LaTeX avec Doxygen
    info "1️⃣  Génération Doxygen (HTML + LaTeX)..."
    docker-compose run --rm doxygen
    
    if [ $? -ne 0 ]; then
        error "✗ Erreur lors de la génération Doxygen"
        exit 1
    fi
    success "✓ Documentation Doxygen générée"
    
    # Étape 2: Compiler le PDF
    info "\n2️⃣  Compilation du PDF..."
    docker-compose --profile pdf run --rm pdflatex
    
    if [ $? -ne 0 ]; then
        warning "⚠️  Erreur lors de la compilation PDF (non bloquant)"
    else
        success "✓ PDF compilé avec succès"
    fi
    
    # Résumé
    echo ""
    success "════════════════════════════════════════════════════════"
    success "✓ Documentation générée avec succès !"
    success "════════════════════════════════════════════════════════"
    echo ""
    info "📂 Fichiers générés :"
    echo "   • HTML : doxygen-output/html/index.html"
    
    if [ -f "doxygen-output/latex/refman.pdf" ]; then
        echo "   • PDF  : doxygen-output/latex/refman.pdf"
        pdf_size=$(du -h "doxygen-output/latex/refman.pdf" | cut -f1)
        info "   • Taille PDF : $pdf_size"
        
        # Ouvrir le PDF (si possible)
        if command -v xdg-open &> /dev/null; then
            info "\n🚀 Ouverture du PDF..."
            xdg-open "doxygen-output/latex/refman.pdf" &
        elif command -v open &> /dev/null; then
            info "\n🚀 Ouverture du PDF..."
            open "doxygen-output/latex/refman.pdf"
        fi
    fi
    
    echo ""
    info "💡 Pour visualiser la doc HTML :"
    warning "   ./docs-stack.sh serve"
}

# Action: Clean
action_clean() {
    warning "🧹 Nettoyage des fichiers générés..."
    echo ""
    
    # Demander confirmation
    read -p "Êtes-vous sûr de vouloir supprimer tous les fichiers générés ? (oui/non) " confirmation
    
    if [[ "$confirmation" == "oui" ]] || [[ "$confirmation" == "o" ]] || [[ "$confirmation" == "yes" ]] || [[ "$confirmation" == "y" ]]; then
        # Arrêter les conteneurs
        docker-compose down 2>/dev/null
        
        # Supprimer le dossier de sortie
        if [ -d "doxygen-output" ]; then
            rm -rf doxygen-output
            success "✓ Dossier doxygen-output supprimé"
        fi
        
        success "\n✓ Nettoyage terminé"
    else
        info "❌ Nettoyage annulé"
    fi
}

# Action: Serve
action_serve() {
    info "🌐 Démarrage du serveur HTTP..."
    echo ""
    
    # Vérifier que la doc existe
    if [ ! -f "doxygen-output/html/index.html" ]; then
        error "✗ La documentation n'existe pas encore"
        info "💡 Générez d'abord la documentation :"
        warning "   ./docs-stack.sh generate"
        exit 1
    fi
    
    # Démarrer le serveur
    docker-compose --profile serve up -d
    
    if [ $? -eq 0 ]; then
        success "✓ Serveur HTTP démarré"
        echo ""
        info "📡 Accédez à la documentation sur :"
        success "   http://localhost:8090"
        echo ""
        info "💡 Pour arrêter le serveur :"
        warning "   ./docs-stack.sh stop"
        
        # Ouvrir dans le navigateur (si possible)
        sleep 2
        if command -v xdg-open &> /dev/null; then
            xdg-open "http://localhost:8090" &
        elif command -v open &> /dev/null; then
            open "http://localhost:8090"
        fi
    else
        error "✗ Erreur lors du démarrage du serveur"
        exit 1
    fi
}

# Action: Stop
action_stop() {
    info "🛑 Arrêt des services..."
    docker-compose --profile serve down
    
    if [ $? -eq 0 ]; then
        success "\n✓ Services arrêtés"
    else
        error "\n✗ Erreur lors de l'arrêt des services"
    fi
}

# Action: Help
show_help() {
    warning "UTILISATION :"
    echo "  ./docs-stack.sh <action>"
    echo ""
    warning "ACTIONS DISPONIBLES :"
    echo ""
    echo -e "  ${CYAN}build${NC}       Construire l'image Docker"
    echo "              Durée : 5-10 minutes (première fois uniquement)"
    echo ""
    echo -e "  ${CYAN}generate${NC}    Générer la documentation complète (HTML + PDF)"
    echo "              Inclut : Doxygen + compilation LaTeX"
    echo ""
    echo -e "  ${CYAN}serve${NC}       Démarrer un serveur HTTP pour visualiser la doc HTML"
    echo "              URL : http://localhost:8090"
    echo ""
    echo -e "  ${CYAN}stop${NC}        Arrêter le serveur HTTP"
    echo ""
    echo -e "  ${CYAN}clean${NC}       Supprimer tous les fichiers générés"
    echo ""
    echo -e "  ${CYAN}help${NC}        Afficher cette aide"
    echo ""
    warning "EXEMPLES :"
    success "  ./docs-stack.sh generate"
    success "  ./docs-stack.sh serve"
    success "  ./docs-stack.sh clean"
    echo ""
}

# ============================================
# MAIN
# ============================================

show_banner

ACTION=${1:-help}

# Vérifier les prérequis (sauf pour help)
if [ "$ACTION" != "help" ]; then
    check_prerequisites || {
        error "Prérequis manquants. Installation requise."
        exit 1
    }
fi

# Exécuter l'action
case "$ACTION" in
    build)
        action_build
        ;;
    generate)
        action_generate
        ;;
    clean)
        action_clean
        ;;
    serve)
        action_serve
        ;;
    stop)
        action_stop
        ;;
    help|*)
        show_help
        ;;
esac

echo ""
