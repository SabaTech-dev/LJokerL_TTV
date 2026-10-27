# AGENTS.md - LJokerL_TTV Custom Config PRO

## 📋 Visión General del Proyecto

**LJokerL_TTV Custom Config PRO** es un sistema de optimización de Windows basado en C# WPF con persistencia JSON para proporcionar optimizaciones curadas, reversibles y con rollback completo.

### Stack Tecnológico
- **Lenguaje**: C# (.NET 9)
- **Framework**: WPF (Windows Presentation Foundation)
- **Arquitectura**: MVVM (Model-View-ViewModel)
- **Sistema Operativo**: Windows 10/11
- **Persistencia**: JSON

## 🏗️ Estructura del Proyecto

```
C:\LJokerL_TTV\
├── src/
│   ├── LJokerL.Core/          # Lógica de negocio y modelos
│   │   ├── Services/          # Servicios de detección y optimización
│   │   └── Class1.cs
│   ├── LJokerL.App/           # Aplicación WPF
│   │   ├── MainWindow.xaml    # Interfaz principal
│   │   └── MainViewModel.cs   # Lógica MVVM
│   └── LJokerL.Tests/         # Tests unitarios
│       └── TestRunner.cs
├── Config/                    # Configuraciones JSON
│   ├── tweaks.json           # Catálogo de optimizaciones
│   ├── profiles.json         # Perfiles de optimización
│   ├── services.json         # Catálogo de servicios
│   ├── apps_bloatware.json   # Apps UWP a remover
│   └── hardware_rules.json   # Reglas por hardware
├── Installer/                 # Scripts de instalación
├── Resources/                 # Recursos y herramientas
└── .opencode/                # Configuración de OpenCode
    ├── opencode.json         # Configuración del proyecto
    ├── AGENTS.md             # Este archivo
    ├── skills/               # Skills del agente
    ├── commands/             # Comandos personalizados
    └── agents/               # Definiciones de agentes
```

## 🎯 Componentes Principales

### 1. Core System (C#)
- **ConfigLoader**: Carga y gestiona configuraciones JSON
- **ExecutionEngine**: Ejecuta optimizaciones con rollback
- **HardwareDetector**: Detecta CPU, GPU, RAM, Almacenamiento
- **ExecutionLogger**: Logging persistente en JSONL

### 2. Config Persistence (JSON)
- **tweaks.json**: Optimizaciones con nivel de riesgo (SAFE/ADVANCED/EXPERIMENTAL)
- **profiles.json**: Perfiles predefinidos (Competitive Latency, Smooth VRR)
- **services.json**: Servicios Windows con acciones de revert
- **apps_bloatware.json**: Apps UWP para remover/instalar
- **hardware_rules.json**: Reglas de recomendación por hardware

### 3. UI WPF
- **MainWindow.xaml**: Panel de perfiles, estadísticas, acciones
- **MainViewModel.cs**: Lógica MVVM con comandos
- **RelayCommand.cs**: Implementación ICommand

## 🎨 Patrones de Diseño

### Patrones Implementados
1. **Factory Pattern**: Creación de perfiles de optimización
2. **Strategy Pattern**: Selección de algoritmos por tipo de hardware
3. **Observer Pattern**: Monitoreo de cambios en el sistema
4. **Repository Pattern**: Acceso a datos de hardware y configuraciones
5. **MVVM**: Separación de UI y lógica de negocio

### Convenciones de Código
- **Naming**: PascalCase para clases, camelCase para variables
- **Namespaces**: `LJokerL.Core`, `LJokerL.App`, `LJokerL.Tests`
- **Async/Await**: Usar para operaciones I/O y registry
- **Nullable Reference Types**: Habilitados (.NET 9)

## 🛡️ Consideraciones de Seguridad

### Manejo de Privilegios
- Requiere privilegios de Administrador
- Validación de permisos antes de operaciones críticas
- Logging de operaciones privilegiadas
- Capacidad de rollback completo

### Operaciones Críticas
- Modificaciones de registro Windows
- Gestión de servicios
- Remoción de apps UWP
- Cambios de políticas de grupo

### Validación
- Sanitización de inputs del usuario
- Verificación de parámetros antes de procesamiento
- Prevención de inyección de código
- Auditoría completa con timestamps

## 🔧 Configs JSON Disponibles

### tweaks.json
```json
{
  "tweaks": [
    {
      "id": "game_dvr_off",
      "name": "Disable Game DVR",
      "description": "Disables Windows Game DVR to reduce latency",
      "risk": "SAFE",
      "category": "GAMING",
      "actions": [...]
    }
  ]
}
```

### profiles.json
```json
{
  "profiles": [
    {
      "id": "competitive_latency",
      "name": "Competitive Latency",
      "description": "Minimum latency for competitive gaming",
      "tweaks": ["game_dvr_off", "game_mode_on", ...],
      "services": ["wersvc", "mapsbroker", ...],
      "apps": ["get_help", "get_started", ...]
    }
  ]
}
```

## 🚀 Comandos Útiles

### Build y Test
```bash
# Build del proyecto
dotnet build

# Build en Release
dotnet build -c Release

# Ejecutar tests
dotnet test

# Formatear código
dotnet format
```

### Operaciones de OpenCode
```bash
# Revisar código
/review

# Ejecutar tests
/test

# Auditar seguridad
/audit

# Actualizar documentación
/docs

# Buscar skill
/find-skill <tema>

# Cargar skills externas
/load-external-skills <categoría>
```

## 📊 Proveedores de Modelos Configurados

### Jerarquía de Uso
1. **OpenCode Zen (FREE)** - Principal
   - Kimi K2.5 (FREE)
   - GLM-5 (FREE)
   - MiniMax M2.5 (FREE)

2. **GitHub Copilot (Pro+)** - Fallback Premium
   - GPT-5.3-Codex (mejor para código)
   - Claude Opus 4.6 (máxima capacidad)
   - Claude Sonnet 4.6 (balanceado)

3. **Z.AI Coding Plan** - Alternativa
   - GLM-5
   - GLM-4.7
   - GLM-4.7flash

## 🎯 Agentes Especializados Disponibles

- **skill-manager**: Gestiona catálogo de skills
- **agent-dispatcher**: Asigna agentes según tarea
- **code-reviewer**: Revisa código C#/.NET 9
- **security-auditor**: Audita seguridad de cambios
- **test-engineer**: Diseña y ejecuta tests
- **technical-writer**: Actualiza documentación
- **wpf-developer**: Desarrolla interfaces WPF
- **optimization-engineer**: Implementa optimizaciones Windows

## 📝 Skills Disponibles

### Meta-Skills
- **skill-discovery**: Busca skills existentes
- **skill-creator**: Crea nuevas skills
- **external-skill-loader**: Carga skills de repositorio externo
- **agent-orchestrator**: Orquesta agentes

### Domain-Skills
- **windows-optimization**: Tweaks de Windows
- **wpf-mvvm**: Patrones WPF/MVVM
- **dotnet-csharp**: Convenciones C#/.NET 9
- **registry-security**: Seguridad en registry

### Task-Skills
- **code-review**: Revisión de código
- **testing**: Testing strategies
- **documentation**: Documentación
- **security-audit**: Auditoría de seguridad

### Skills Externas (860+)
Repositorio: `sickn33/antigravity-awesome-skills`
Categorías relevantes: Security, Development, Testing, Architecture

## 🔄 Flujo de Trabajo Típico

1. **Análisis de tarea**: Agent-dispatcher analiza y asigna agente
2. **Carga de skills**: Skill-manager carga skills relevantes
3. **Ejecución**: Agente especializado ejecuta con skills cargadas
4. **Revisión**: Code-reviewer revisa cambios si es código
5. **Testing**: Test-engineer verifica tests si aplica
6. **Auditoría**: Security-auditor verifica seguridad si es crítico

## ⚠️ Reglas Importantes

### Seguridad
- NUNCA ejecutar cambios de registry sin validación
- SIEMPRE crear snapshot antes de modificaciones
- VERIFICAR permisos de administrador antes de operaciones críticas
- LOGGEAR todas las operaciones en JSONL

### Código
- Seguir convenciones .NET 9
- Usar nullable reference types
- Implementar try/catch robusto
- Documentar métodos públicos
- Mantener separación MVVM

### Testing
- Cobertura mínima del 80%
- Tests unitarios para lógica de negocio
- Tests de integración para UI
- Validar rollback funciona correctamente

## 📚 Recursos Adicionales

- [ARCHITECTURE.md](../ARCHITECTURE.md) - Documentación de arquitectura
- [ROADMAP.md](../ROADMAP.md) - Roadmap del proyecto
- [USER_GUIDE.md](../USER_GUIDE.md) - Guía de usuario

## 🆘 Soporte

Para dudas o problemas:
1. Consultar skills disponibles con `/find-skill`
2. Cargar skills externas con `/load-external-skills`
3. Asignar agente especializado con `/assign-agent`

---

**Última actualización**: Febrero 2026
**Versión**: 13.0 UNIFIED CORE WPF
