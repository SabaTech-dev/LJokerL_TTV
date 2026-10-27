#!/bin/bash
# Setup script para LJokerL_TTV con OpenCode
# Configura skills, agentes y repositorio externo

set -e

echo "========================================"
echo "🔧 Configurando OpenCode para LJokerL_TTV"
echo "========================================"
echo ""

# Colores
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Función de error
error_exit() {
    echo -e "${RED}❌ Error: $1${NC}" >&2
    exit 1
}

# Función de éxito
success() {
    echo -e "${GREEN}✅ $1${NC}"
}

# Función de info
info() {
    echo -e "${YELLOW}ℹ️  $1${NC}"
}

# Directorio base
PROJECT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$PROJECT_DIR"

echo "📂 Directorio del proyecto: $PROJECT_DIR"
echo ""

# ============================================
# 1. Verificar estructura de directorios
# ============================================
echo "📁 Verificando estructura de directorios..."

DIRS=(
    ".opencode/skills/meta/skill-discovery"
    ".opencode/skills/meta/skill-creator"
    ".opencode/skills/meta/external-skill-loader"
    ".opencode/skills/meta/agent-orchestrator"
    ".opencode/skills/domain/windows-optimization"
    ".opencode/skills/domain/wpf-mvvm"
    ".opencode/skills/domain/dotnet-csharp"
    ".opencode/skills/domain/registry-security"
    ".opencode/skills/task/code-review"
    ".opencode/skills/task/testing"
    ".opencode/skills/task/documentation"
    ".opencode/skills/task/security-audit"
    ".opencode/skills/external"
    ".opencode/commands"
    ".opencode/agents"
)

for dir in "${DIRS[@]}"; do
    if [ ! -d "$dir" ]; then
        mkdir -p "$dir"
        info "Creado: $dir"
    fi
done

success "Estructura de directorios verificada"
echo ""

# ============================================
# 2. Clonar repositorio externo de skills
# ============================================
echo "📥 Configurando repositorio externo de skills..."

EXTERNAL_DIR=".opencode/skills/external/antigravity-awesome-skills"

if [ -d "$EXTERNAL_DIR/.git" ]; then
    info "Repositorio externo encontrado. Actualizando..."
    cd "$EXTERNAL_DIR"
    git pull --quiet
    cd "$PROJECT_DIR"
    success "Repositorio externo actualizado"
else
    info "Clonando antigravity-awesome-skills..."
    git clone --quiet https://github.com/sickn33/antigravity-awesome-skills.git "$EXTERNAL_DIR"
    success "Repositorio externo clonado"
fi

echo ""

# ============================================
# 3. Verificar archivos de configuración
# ============================================
echo "⚙️  Verificando archivos de configuración..."

if [ ! -f "opencode.json" ]; then
    error_exit "No se encontró opencode.json en el directorio raíz"
fi

if [ ! -f "AGENTS.md" ]; then
    error_exit "No se encontró AGENTS.md en el directorio raíz"
fi

success "Archivos de configuración encontrados"
echo ""

# ============================================
# 4. Contar skills disponibles
# ============================================
echo "📊 Contabilizando skills disponibles..."

INTERNAL_SKILLS=$(find .opencode/skills -name "SKILL.md" -not -path "*/external/*" | wc -l)
EXTERNAL_SKILLS=$(find .opencode/skills/external -name "SKILL.md" 2>/dev/null | wc -l)

echo "   Skills internas: $INTERNAL_SKILLS"
echo "   Skills externas: $EXTERNAL_SKILLS"
echo "   Total: $((INTERNAL_SKILLS + EXTERNAL_SKILLS))"
echo ""

# ============================================
# 5. Verificar configuración global
# ============================================
echo "🌐 Verificando configuración global..."

GLOBAL_CONFIG="$HOME/.config/opencode/opencode.json"
if [ -f "$GLOBAL_CONFIG" ]; then
    success "Configuración global encontrada"
else
    info "No se encontró configuración global"
    info "Se usará configuración por defecto"
fi

echo ""

# ============================================
# 6. Mostrar resumen
# ============================================
echo "========================================"
echo "📋 RESUMEN DE CONFIGURACIÓN"
echo "========================================"
echo ""
echo "✅ Configuración completada exitosamente!"
echo ""
echo "Estructura creada:"
echo "  📁 .opencode/"
echo "  ├── 📂 skills/"
echo "  │   ├── 📂 meta/ (4 skills)"
echo "  │   ├── 📂 domain/ (4 skills)"
echo "  │   ├── 📂 task/ (4 skills)"
echo "  │   └── 📂 external/ ($EXTERNAL_SKILLS skills)"
echo "  ├── 📂 commands/"
echo "  └── 📂 agents/"
echo ""
echo "Archivos de configuración:"
echo "  ⚙️  opencode.json (proyecto)"
echo "  📄 AGENTS.md (instrucciones)"
echo "  ⚙️  ~/.config/opencode/opencode.json (global)"
echo ""
echo "Comandos disponibles:"
echo "  /find-skill <tema>           - Buscar skills"
echo "  /create-skill <descripción>  - Crear nueva skill"
echo "  /assign-agent <tarea>        - Asignar agente especializado"
echo "  /load-external-skills <cat>  - Cargar skills externas"
echo "  /update-skills               - Actualizar repo externo"
echo "  /review                      - Revisar código"
echo "  /audit                       - Auditar seguridad"
echo "  /test                        - Ejecutar tests"
echo "  /docs                        - Actualizar documentación"
echo ""
echo "Agentes especializados:"
echo "  🔍 skill-manager         - Gestión de skills"
echo "  🎯 agent-dispatcher      - Asignación de agentes"
echo "  👀 code-reviewer         - Revisión de código"
echo "  🛡️ security-auditor     - Auditoría de seguridad"
echo "  🧪 test-engineer         - Testing"
echo "  📝 technical-writer      - Documentación"
echo "  🎨 wpf-developer         - Desarrollo WPF"
echo "  ⚡ optimization-engineer - Optimizaciones"
echo ""
echo "Proveedores configurados:"
echo "  1. OpenCode Zen (FREE) - Kimi K2.5, GLM-5, MiniMax M2.5"
echo "  2. GitHub Copilot (Pro+) - GPT-5.3-Codex, Claude Opus 4.6"
echo "  3. Z.AI Coding Plan - GLM-5, GLM-4.7"
echo ""
echo "========================================"
echo "🚀 OpenCode está listo para usar!"
echo "========================================"
echo ""
echo "Próximos pasos:"
echo "  1. Conectar proveedores: /connect"
echo "  2. Probar comando: /find-skill csharp"
echo "  3. Revisar código: /review"
echo ""
