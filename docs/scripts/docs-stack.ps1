# Script d'orchestration de la stack de documentation BlogApi
# Usage: .\docs-stack.ps1 -Action [build|generate|clean|serve|stop|help]

param(
    [Parameter(Mandatory=$false)]
    [ValidateSet("build", "generate", "clean", "serve", "stop", "help")]
    [string]$Action = "help"
)

# Couleurs pour les messages
function Write-Info { param($Message) Write-Host $Message -ForegroundColor Cyan }
function Write-Success { param($Message) Write-Host $Message -ForegroundColor Green }
function Write-Error { param($Message) Write-Host $Message -ForegroundColor Red }
function Write-Warning { param($Message) Write-Host $Message -ForegroundColor Yellow }

# Banner
function Show-Banner {
    Write-Host ""
    Write-Host "╔════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
    Write-Host "║    📚 Stack de Documentation BlogApi                  ║" -ForegroundColor Cyan
    Write-Host "║    Génération automatisée avec Docker Compose         ║" -ForegroundColor Cyan
    Write-Host "╚════════════════════════════════════════════════════════╝" -ForegroundColor Cyan
    Write-Host ""
}

# Vérifier les prérequis
function Test-Prerequisites {
    Write-Info "🔍 Vérification des prérequis..."
    
    # Vérifier Docker
    try {
        $dockerVersion = docker --version 2>$null
        if ($LASTEXITCODE -eq 0) {
            Write-Success "✓ Docker installé : $dockerVersion"
        } else {
            Write-Error "✗ Docker n'est pas installé ou n'est pas dans le PATH"
            return $false
        }
    } catch {
        Write-Error "✗ Impossible de vérifier Docker"
        return $false
    }
    
    # Vérifier Docker Compose
    try {
        $composeVersion = docker-compose --version 2>$null
        if ($LASTEXITCODE -eq 0) {
            Write-Success "✓ Docker Compose installé : $composeVersion"
        } else {
            Write-Error "✗ Docker Compose n'est pas installé ou n'est pas dans le PATH"
            return $false
        }
    } catch {
        Write-Error "✗ Impossible de vérifier Docker Compose"
        return $false
    }
    
    Write-Host ""
    return $true
}

# Action: Build
function Invoke-Build {
    Write-Info "🏗️  Construction de l'image Docker..."
    Write-Info "⚠️  Attention : La première construction peut prendre 5-10 minutes"
    Write-Host ""
    
    docker-compose build
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "`n✓ Image construite avec succès"
        
        # Afficher la taille de l'image
        $imageSize = docker images blogapi-doxygen --format "{{.Size}}" | Select-Object -First 1
        Write-Info "📦 Taille de l'image : $imageSize"
    } else {
        Write-Error "`n✗ Erreur lors de la construction de l'image"
        exit 1
    }
}

# Action: Generate
function Invoke-Generate {
    Write-Info "📖 Génération de la documentation complète..."
    Write-Host ""
    
    # Créer le dossier de sortie si nécessaire
    if (-not (Test-Path "doxygen-output")) {
        New-Item -ItemType Directory -Path "doxygen-output" | Out-Null
    }
    
    # Étape 1: Générer HTML + LaTeX avec Doxygen
    Write-Info "1️⃣  Génération Doxygen (HTML + LaTeX)..."
    docker-compose run --rm doxygen
    
    if ($LASTEXITCODE -ne 0) {
        Write-Error "✗ Erreur lors de la génération Doxygen"
        exit 1
    }
    Write-Success "✓ Documentation Doxygen générée"
    
    # Étape 2: Compiler le PDF
    Write-Info "`n2️⃣  Compilation du PDF..."
    docker-compose --profile pdf run --rm pdflatex
    
    if ($LASTEXITCODE -ne 0) {
        Write-Warning "⚠️  Erreur lors de la compilation PDF (non bloquant)"
    } else {
        Write-Success "✓ PDF compilé avec succès"
    }
    
    # Résumé
    Write-Host ""
    Write-Success "════════════════════════════════════════════════════════"
    Write-Success "✓ Documentation générée avec succès !"
    Write-Success "════════════════════════════════════════════════════════"
    Write-Host ""
    Write-Info "📂 Fichiers générés :"
    Write-Host "   • HTML : doxygen-output/html/index.html"
    
    if (Test-Path "doxygen-output/latex/refman.pdf") {
        Write-Host "   • PDF  : doxygen-output/latex/refman.pdf"
        $pdfSize = (Get-Item "doxygen-output/latex/refman.pdf").Length / 1KB
        Write-Info "   • Taille PDF : $([math]::Round($pdfSize, 2)) KB"
        
        # Ouvrir le PDF
        Write-Info "`n🚀 Ouverture du PDF..."
        Start-Process "doxygen-output/latex/refman.pdf"
    }
    
    Write-Host ""
    Write-Info "💡 Pour visualiser la doc HTML :"
    Write-Host "   .\docs-stack.ps1 -Action serve" -ForegroundColor Yellow
}

# Action: Clean
function Invoke-Clean {
    Write-Warning "🧹 Nettoyage des fichiers générés..."
    Write-Host ""
    
    # Demander confirmation
    $confirmation = Read-Host "Êtes-vous sûr de vouloir supprimer tous les fichiers générés ? (oui/non)"
    
    if ($confirmation -eq "oui" -or $confirmation -eq "o" -or $confirmation -eq "yes" -or $confirmation -eq "y") {
        # Arrêter les conteneurs
        docker-compose down 2>$null
        
        # Supprimer le dossier de sortie
        if (Test-Path "doxygen-output") {
            Remove-Item -Recurse -Force "doxygen-output"
            Write-Success "✓ Dossier doxygen-output supprimé"
        }
        
        Write-Success "`n✓ Nettoyage terminé"
    } else {
        Write-Info "❌ Nettoyage annulé"
    }
}

# Action: Serve
function Invoke-Serve {
    Write-Info "🌐 Démarrage du serveur HTTP..."
    Write-Host ""
    
    # Vérifier que la doc existe
    if (-not (Test-Path "doxygen-output/html/index.html")) {
        Write-Error "✗ La documentation n'existe pas encore"
        Write-Info "💡 Générez d'abord la documentation :"
        Write-Host "   .\docs-stack.ps1 -Action generate" -ForegroundColor Yellow
        exit 1
    }
    
    # Démarrer le serveur
    docker-compose --profile serve up -d
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "✓ Serveur HTTP démarré"
        Write-Host ""
        Write-Info "📡 Accédez à la documentation sur :"
        Write-Host "   http://localhost:8090" -ForegroundColor Green
        Write-Host ""
        Write-Info "💡 Pour arrêter le serveur :"
        Write-Host "   .\docs-stack.ps1 -Action stop" -ForegroundColor Yellow
        
        # Ouvrir dans le navigateur
        Start-Sleep -Seconds 2
        Start-Process "http://localhost:8090"
    } else {
        Write-Error "✗ Erreur lors du démarrage du serveur"
        exit 1
    }
}

# Action: Stop
function Invoke-Stop {
    Write-Info "🛑 Arrêt des services..."
    docker-compose --profile serve down
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "`n✓ Services arrêtés"
    } else {
        Write-Error "`n✗ Erreur lors de l'arrêt des services"
    }
}

# Action: Help
function Show-Help {
    Write-Host "UTILISATION :" -ForegroundColor Yellow
    Write-Host "  .\docs-stack.ps1 -Action [action]" -ForegroundColor White
    Write-Host ""
    Write-Host "ACTIONS DISPONIBLES :" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "  build       " -ForegroundColor Cyan -NoNewline
    Write-Host "Construire l'image Docker"
    Write-Host "              Durée : 5-10 minutes (première fois uniquement)"
    Write-Host ""
    Write-Host "  generate    " -ForegroundColor Cyan -NoNewline
    Write-Host "Générer la documentation complète (HTML + PDF)"
    Write-Host "              Inclut : Doxygen + compilation LaTeX"
    Write-Host ""
    Write-Host "  serve       " -ForegroundColor Cyan -NoNewline
    Write-Host "Démarrer un serveur HTTP pour visualiser la doc HTML"
    Write-Host "              URL : http://localhost:8090"
    Write-Host ""
    Write-Host "  stop        " -ForegroundColor Cyan -NoNewline
    Write-Host "Arrêter le serveur HTTP"
    Write-Host ""
    Write-Host "  clean       " -ForegroundColor Cyan -NoNewline
    Write-Host "Supprimer tous les fichiers générés"
    Write-Host ""
    Write-Host "  help        " -ForegroundColor Cyan -NoNewline
    Write-Host "Afficher cette aide"
    Write-Host ""
    Write-Host "EXEMPLES :" -ForegroundColor Yellow
    Write-Host "  .\docs-stack.ps1 -Action generate" -ForegroundColor Green
    Write-Host "  .\docs-stack.ps1 -Action serve" -ForegroundColor Green
    Write-Host "  .\docs-stack.ps1 -Action clean" -ForegroundColor Green
    Write-Host ""
}

# ============================================
# MAIN
# ============================================

Show-Banner

# Vérifier les prérequis (sauf pour help)
if ($Action -ne "help") {
    if (-not (Test-Prerequisites)) {
        Write-Error "Prérequis manquants. Installation requise."
        exit 1
    }
}

# Exécuter l'action
switch ($Action) {
    "build"    { Invoke-Build }
    "generate" { Invoke-Generate }
    "clean"    { Invoke-Clean }
    "serve"    { Invoke-Serve }
    "stop"     { Invoke-Stop }
    "help"     { Show-Help }
}

Write-Host ""
