#!/bin/bash
# Script pour générer les images PlantUML avant Doxygen

PLANTUML_JAR="/opt/plantuml.jar"
DIAGRAMS_DIR="docs/doxygen/diagrams"
HTML_DIR="docs/doxygen-output/html"
LATEX_DIR="docs/doxygen-output/latex"

echo "🔷 Génération des diagrammes PlantUML..."

# Créer les dossiers de sortie s'ils n'existent pas
mkdir -p "$HTML_DIR"
mkdir -p "$LATEX_DIR"

# Générer les PNG pour HTML
echo "📊 Génération des PNG pour HTML..."
for puml_file in "$DIAGRAMS_DIR"/*.puml; do
    filename=$(basename "$puml_file" .puml)
    echo "  - $filename.png"
    java -Dfile.encoding=UTF-8 -jar "$PLANTUML_JAR" -charset UTF-8 -tpng "$puml_file" -o "../../doxygen-output/html"
done

# Générer les EPS pour LaTeX/PDF
echo "📊 Génération des EPS pour LaTeX..."
for puml_file in "$DIAGRAMS_DIR"/*.puml; do
    filename=$(basename "$puml_file" .puml)
    echo "  - $filename.eps"
    java -Dfile.encoding=UTF-8 -jar "$PLANTUML_JAR" -charset UTF-8 -teps "$puml_file" -o "../../doxygen-output/latex"
done

echo "✅ Diagrammes PlantUML générés avec succès !"
echo ""
echo "🔷 Génération de la documentation Doxygen..."
doxygen docs/doxygen/Doxyfile

echo ""
echo "🔷 Compilation du PDF..."
cd docs/doxygen-output/latex
# Utiliser pdflatex en mode batch (non-interactif)
pdflatex -interaction=nonstopmode refman.tex > /dev/null
makeindex refman.idx > /dev/null
pdflatex -interaction=nonstopmode refman.tex > /dev/null
pdflatex -interaction=nonstopmode refman.tex > /dev/null

if [ -f refman.pdf ]; then
    echo "✅ PDF généré avec succès : refman.pdf"
    ls -lh refman.pdf
else
    echo "❌ Erreur lors de la génération du PDF"
    echo "Vérifiez les logs LaTeX dans docs/doxygen-output/latex/"
fi

echo ""
echo "✅ Documentation générée avec succès !"
