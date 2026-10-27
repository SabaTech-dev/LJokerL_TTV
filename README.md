# 🚀 LJokerL_TTV Custom Config PRO v13.0 WPF CONTROL CENTER

## 📋 Descripción del Proyecto

**LJokerL_TTV Custom Config PRO** es un sistema avanzado de optimización universal para Windows 11 que proporciona optimizaciones inteligentes y adaptativas basadas en la detección automática de hardware.

## 🎯 Características Principales

### 🔧 **Sistema de Optimización Adaptativa**
- Detección automática de hardware (CPU, GPU, RAM, Almacenamiento)
- Perfiles de optimización personalizados por hardware
- Clasificación inteligente de componentes por rendimiento y tipo
- Optimizaciones específicas para gaming y productividad

### 🖥️ **Componentes del Sistema**

#### **Core System (C# WPF)**
- **Lenguaje**: C# (.NET 9)
- **Arquitectura**: MVVM con módulos separados
- **Gestión de Estado**: ConfigLoader + ExecutionEngine con logs JSONL
- **Manejo de Errores**: Try/catch robusto con logging detallado

#### **Config Persistence (JSON)**
- **Tweaks Catalog**: `Config/tweaks.json` - optimizaciones curadas con riesgo y revert
- **Profiles**: `Config/profiles.json` - Competitive Latency / Smooth VRR con referencias a tweaks, servicios y apps
- **Services Catalog**: `Config/services.json` - servicios a desactivar con acción revert
- **Apps Bloatware Catalog**: `Config/apps_bloatware.json` - apps UWP a remover/instalar
- **Hardware Rules**: `Config/hardware_rules.json` - reglas de recomendación por hardware

#### **UI WPF (Preview)**
- **MainWindow.xaml**: Profiles panel + Stats + Advanced Tweaks con filtros
- **MainViewModel.cs**: lógica MVVM con comandos para aplicar/revert perfiles y tweaks individuales
- **RelayCommand.cs**: implementación ICommand básica

#### **Test Runner**
- **Program.cs**: runner custom en `src/LJokerL.Tests/` que cubre:
  - TestConfigLoad (carga de bundles)
  - TestProfileReferences (referencias de perfiles)
  - TestExecutionEngineCommands (ejecución de comandos)

### 📊 **Perfiles de Optimización**

#### **Competitive Latency** (Competitivo)
- Objetivo: Mínima latencia para gaming competitivo
- Tweaks incluidos:
  - Privacidad: advertising_id_off, telemetry_min, feedback_notifications_off, tailored_experiences_off, bing_search_off, silent_app_install_off, suggested_content_off, windows_welcome_experience_off, windows_tips_off, whats_new_in_windows_off
  - Gaming: game_dvr_off, game_mode_on, hags_on (NVIDIA/AMD), mpo_disable (condicional si síntomas), mouse_accel_off, usb_selective_suspend_off
  - Sistema: long_paths_on, bsod_auto_reboot_off (safe), delivery_optimization_off (advanced), hibernation_off (advanced)
  - Servicios: wersvc, mapsbroker, lfsvc
  - Apps bloatware: get_help, get_started, messaging, office_hub, feedback_hub, zune_music, zune_video, skype, sound_recorder, alarms, camera (advanced)
- Nota: HAGS y MPO dependen de GPU; se recomienda monitor VRR para gaming competitivo

#### **Smooth VRR** (VRR Suave)
- Objetivo: VRR fluido para gaming casual y consumo equilibrado
- Tweaks incluidos:
  - Privacidad: mismos que Competitive Latency
  - Gaming: game_dvr_off, game_mode_on, hags_on (NVIDIA/AMD), mpo_disable (solo si síntomas), mouse_accel_off, usb_selective_suspend_off
  - Sistema: long_paths_on, manage_default_printer_off, restore_previous_folders_on (safe)
  - Servicios: wersvc (solo si hay errores), lfsvc
  - Apps bloatware: get_help, get_started, messaging, office_hub, feedback_hub, zune_music, zune_video, skype, sound_recorder, alarms
- Nota: Se recomienda monitor VRR para gaming competitivo

## 🚀 **Flujo de Trabajo**

1. **Detección de Hardware** → Análisis automático de componentes (pendiente: módulo C# en Core)
2. **Clasificación** → Categorización por rendimiento y tipo
3. **Selección de Perfil** → Elección del perfil óptimo (Competitive Latency / Smooth VRR)
4. **Aplicación de Optimizaciones** → Configuración del sistema vía ExecutionEngine (registry, commands, services, apps)
5. **Validación** → Verificación de cambios aplicados
6. **Logging** → Registro JSONL de todas las operaciones con snapshot de valores previos
7. **Rollback** → Revertir por perfil desde log (registry snapshot) o revert catálogo (tweaks + services + apps)

## 🔄 **Próximos Pasos Inmediatos**

### 📋 **Prioridades Actuales**
1. **[Completado]** Finalizar consolidación de versiones múltiples → unified WPF + JSON configs
2. **[Completado]** Implementar logging JSONL persistente → ExecutionLogger con JSONL y snapshot
3. **[Completado]** Agregar rollback completo por perfil → RollbackContext + revert catálogo
4. **[Completado]** Expandir catálogos de tweaks con items validados → Config/ actualizados + Reports/Tweaks2_Validation_Status.md
5. **[Completado]** Agregar sección de logs visual en UI WPF → Advanced Tweaks con filtros + rollback
6. **[Crítico]** Mejorar manejo de errores globales → unificar try/catch y validaciones en ExecutionEngine + ViewModel
7. **[Alta]** Tests automatizados unitarios → ampliar Program.cs con tests de ExecutionEngine, ConfigLoader, filtros
8. **[Media]** Hardware detection en C# → módulo en Core para CPU/GPU/RAM (reemplaza parcial PowerShell)
9. **[Media]** Reglas de hardware → conectar Config/hardware_rules.json con detección real
10. **[Baja]** Empaquetado → build release + installer (Inno/NSIS) + portable

### 📅 **Cronograma Corto Plazo (2 semanas)**
- **Semana 1**: Mejoras de errores + tests (items 6-7)
- **Semana 2**: Hardware detection + reglas (items 8-9)
- **Semana 3**: Integración UI + recomendaciones automáticas
- **Semana 4**: Empaquetado + release + documentación final

## 🔧 **Instalación**

### 🚀 **WPF (Recomendada)**
1. Compila o publica la app WPF.
   ```bash
   dotnet publish src/LJokerL.App -c Release
   ```
2. Ejecuta **LJokerL.App.exe** desde el output generado.

> Los scripts PowerShell legacy fueron retirados en esta versión.

## 🎮 **Uso del Sistema**

### 🖥️ **Centro de Control (WPF)**
- Selecciona un perfil base y aplica/revierte desde la UI.
- Usa **Monitoring** para ver métricas en tiempo real.
- Usa **Benchmark** para pruebas rápidas/completas y comparación pre/post.

## 🛠️ **Requisitos del Sistema**

### 📋 **Requisitos Mínimos**
- Windows 11 (compatible con Windows 10)
- .NET 9 Runtime (WPF)
- PowerShell 5.1+ (solo Legacy)
- Privilegios de administrador
- 4GB RAM mínimo

### 🔧 **Hardware Recomendado**
- **CPU**: Intel Core i5/AMD Ryzen 5 o superior
- **GPU**: GPU dedicada con 4GB+ VRAM
- **RAM**: 8GB+ DDR4/DDR5
- **Almacenamiento**: SSD NVMe para mejor rendimiento

## 🏗️ **Arquitectura del Sistema**

```
┌─────────────────────────────────────────────┐
│                    LJokerL TTV Custom Config PRO            │
│                         v13.0 UNIFIED CORE WPF               │
└─────────────────────────────────────────────┘
                                 │
                     ┌───────────┴───────────┐
                     │                       │
             ┌───────▼────────┐     ┌─────────▼────────┐
             │   Core System │     │   WPF Interface  │
             │   (Config/   │     │   (src/LJokerL. │
             │    C# +     │     │    App/ +         │
             │    Execution)│     │   src/LJokerL.  │
             └───────┬─────────┘     └─────────┬─────────┘
                     │                       │
         ┌───────────▼────────┐     ┌─────────▼────────┐
         │ Hardware Detection   │     │  Optimization   │
         │      System          │     │   Engine        │
         │   (Pendiente:       │     │                  │
         │   módulo C#)       │     │                  │
         └───────────┬─────────┘     └─────────┬─────────┘
                     │                       │
         ┌───────────▼────────┐     ┌─────────▼────────┐
         │    Config JSONs  │     │  Profile/Log View  │
         │  (tweaks,      │     │  (MainWindow.xaml  │
         │ services,     │     │   + MainViewModel) │
         │ profiles,     │     │                  │
         │ hardware_rules)        │     │                  │
         └─────────────────┘     └─────────┬─────────┘
                     │                       │
         ┌───────────▼────────┐                 │
         │  Test Runner     │                 │
         │ (src/LJokerL.   │                 │
         │ Tests/Program.cs)│                 │
         └─────────────────────┘                 │
```

## 📊 **Perfiles de Optimización**

### 🎮 **Gaming Performance**
- Optimización de GPU y drivers
- Configuración de memoria y CPU
- Perfiles de juegos específicos
- Optimización de red para gaming

### 💼 **Productivity Mode**
- Optimización para aplicaciones de oficina
- Gestión eficiente de memoria
- Configuración de energía equilibrada

### 🔋 **Power Saving**
- Configuración de energía eficiente
- Optimización de CPU y GPU
- Reducción de consumo energético

### 🔒 **Seguridad y Permisos**
- Validación de integridad de archivos
- Logging de todas las operaciones
- Backup automático antes de cambios
- Rollback en caso de errores

## 🐛 **Solución de Problemas**

### ❌ **Problemas Comunes**
1. **Error de Privilegios**: Ejecutar como administrador
2. **Política de Ejecución**: Configurar PowerShell execution policy
3. **Antivirus**: Agregar excepción para el directorio del proyecto
4. **Windows Defender**: Permitir ejecución de scripts

### 🔍 **Diagnóstico**
- Logs detallados en `C:\LJokerL_TTV\Logs\`
- Información de hardware en `LJokerL_TTV_Hardware_Database.ps1`
- Sistema de benchmarking para diagnóstico

## 📈 **Rendimiento y Optimizaciones**

### 🚀 **Optimizaciones del Sistema**
- **CPU**: Ajuste de prioridades y planificación
- **GPU**: Configuración de drivers y memoria
- **RAM**: Optimización de gestión de memoria
- **Almacenamiento**: Configuración de caché y prefetch
- **Red**: Optimización de TCP/IP y DNS

### 📊 **Métricas de Rendimiento**
- Tiempo de respuesta del sistema
- Uso de memoria y CPU
- Rendimiento de gaming (FPS)
- Velocidad de almacenamiento

## 🔄 **Actualización y Mantenimiento**

### 📋 **Sistema de Actualización**
```powershell
# Actualizador automático
.\LJokerL_TTV_Auto_Updater.ps1
```

### 🧹 **Mantenimiento del Sistema**
- Limpieza automática de logs antiguos
- Optimización de base de datos de hardware
- Actualización de perfiles de optimización
- Backup automático de configuraciones

## 📝 **Estándares de Código**

### 🔧 **Desarrollo Local**
1. **Clonar el repositorio**
2. **Configurar entorno de desarrollo**
3. **Ejecutar scripts de prueba**
4. **Contribuir mejoras y correcciones**

### 📝 **Estándares de Código**
- PowerShell con comentarios detallados
- Funciones modulares y reutilizables
- Manejo robusto de errores con logging completo
- Naming conventions descriptivos y consistentes
- Formato y estilo del código consistentes

### 🤝 **Contribución y Desarrollo**
1. Clonar el repositorio
2. Crear una rama para tu feature
3. Hacer commits claros y descriptivos
4. Crear Pull Request con descripción detallada

## 📞 **Soporte y Contacto**

### 👥 **Equipo de Desarrollo**
- **Líder del Proyecto**: LJokerL_TTV
- **Desarrolladores**: 2-3 desarrolladores senior
- **QA Engineers**: 1-2 especialistas en testing
- **UX/UI Designer**: 1 diseñador de experiencia de usuario

### 🌐 **Comunidad de Usuarios**
- **Foro de Discusión**: Plataforma de discusión y soporte
- **GitHub**: Código abierto para componentes no críticos
- **Discord/Slack**: Comunidad en tiempo real
- **YouTube**: Tutoriales y demostraciones
- **Blog**: Artículos técnicos y actualizaciones

### 🔧 **Contribuciones Externas**
- **Programa de Beta Testers**: Usuarios que prueban versiones tempranas
- **Programa de Plugins**: Desarrolladores que crean extensiones
- **Programa de Traducción**: Comunidad que traduce a diferentes idiomas
- **Programa de Documentación**: Contribuciones a la documentación

## 🚨 **Riesgos y Mitigaciones**

### ⚠️ **Riesgos Técnicos**

#### **Riesgo: Compatibilidad Multiplataforma**
- **Probabilidad**: Media
- **Impacto**: Alto
- **Mitigación**:
  - Implementación gradual
  - Uso de frameworks probados
  - Testing extensivo en múltiples entornos
  - Fallbacks a métodos tradicionales

#### **Riesgo: Cambios en Windows**
- **Probabilidad**: Baja
- **Impacto**: Alto
- **Mitigación**:
  - Monitoreo de cambios de Microsoft
  - Testing en versiones beta
  - Arquitectura flexible
  - Actualizaciones rápidas

### 💰 **Riesgos de Negocio**

#### **Riesgo: Competencia del Mercado**
- **Probabilidad**: Media
- **Impacto**: Alto
- **Mitigación**:
  - Diferenciación clara del producto
  - Innovación continua
  - Comunidad activa de usuarios
  - Soporte técnico superior

## 📝 **Trazabilidad**

### 📝 **Logs Detallados**
- Registro de todas las operaciones del sistema
- Timestamps precisas para auditoría
- User Tracking: Identificación del usuario que ejecuta operaciones
- Change History: Historial completo de cambios realizados

### 📈 **Métricas de Éxito**

### 🎯 **Métricas Técnicas**
- Cobertura de Testing: >90%
- Tiempo de Respuesta: <200ms para operaciones críticas
- Uptime: >99.9%
- Tasa de Errores: <0.1%

### 📊 **Métricas de Negocio**
- Usuarios Activos: 10,000+ en 12 meses
- Satisfacción del Usuario: >4.5/5
- Tasa de Retención: >80%
- Adopción Empresarial: 100+ empresas en 18 meses

### 🔄 **Mantenibilidad y Extensibilidad**

### 📝 **Código Limpio**
- Estándares: Seguimiento de mejores prácticas de C#
- Documentación: Comentarios detallados y documentación inline
- Modularidad: Funciones pequeñas y enfocadas
- Naming Conventions: Nombres descriptivos y consistentes
- **Extensibilidad**: Sistema de plugins en roadmap

### 🔧 **Configuración Externa**
- Archivos de Configuración: Parámetros configurables sin modificar código
- Variables de Entorno: Configuración flexible por entorno
- Perfiles de Usuario: Personalización individual
- Templates: Plantillas reutilizables para configuraciones comunes

### 🚀 **Extensibilidad**
- Sistema de Plugins: Arquitectura preparada para extensiones
- API Design: Interfaces bien definidas para integración
- Custom Profiles: Capacidad de crear perfiles personalizados
- Third-party Integration: Soporte para herramientas externas

## 🎯 **Hitos Clave del Proyecto**

### 🎯 **Hito 1: Sistema Estable (Mes 3)**
- Objetivo: Sistema consolidado y estable
- Entregables:
  - Código refactorizado y limpio
  - Testing automatizado implementado
  - Documentación completa
  - Sistema de logging robusto
  - Sistema de rollback automático

### 🎯 **Hito 2: Interfaz Moderna (Mes 6)**
- Objetivo: Interfaz gráfica nativa funcional
- Entregables:
  - Aplicación WPF funcional
  - Dashboard interactivo
  - Sistema de temas y personalización
  - Configuración visual
  - Gráficos y visualizaciones

### 🎯 **Hito 3: Sistema Inteligente (Mes 12)**
- Objetivo: Capacidades de ML implementadas
- Entregables:
  - Algoritmos de ML funcionales
  - Optimización automática
  - Análisis predictivo de rendimiento
  - Recomendaciones inteligentes

### 🎯 **Hito 4: Plataforma Multiplataforma (Mes 18)**
- Objetivo: Soporte completo multiplataforma
- Entregables:
  - Versiones para Windows, Linux, macOS
  - Arquitectura de microservicios
  - API REST completa
  - Ecosistema de plugins
  - Soporte para sistemas empresariales

## 🔧 **Tecnologías y Herramientas**

### 🛠️ **Stack Tecnológico Actual**
- **Lenguaje**: C# (.NET 9), PowerShell Core
- **Frameworks**: WPF, ASP.NET Core
- **Base de Datos**: SQLite, PostgreSQL
- **Machine Learning**: ML.NET, TensorFlow.NET
- **Containerización**: Docker, Kubernetes
- **Cloud**: Azure, AWS

### 🛠️ **Herramientas de Desarrollo**
- **IDE**: Visual Studio 2022, VS Code
- **Testing**: MSTest, NUnit, Playwright
- **CI/CD**: GitHub Actions, Azure DevOps
- **Documentación**: Markdown, DocFX, Swagger

### 📊 **Métricas de Calidad**

### 🎯 **Métricas Técnicas**
- Cobertura de Testing: >90%
- Tiempo de Respuesta: <200ms para operaciones críticas
- Uptime: >99.9%
- Tasa de Errores: <0.1%

### 📈 **Métricas de Negocio**
- Usuarios Activos: 10,000+ en 12 meses
- Satisfacción del Usuario: >4.5/5
- Tasa de Retención: >80%
- Adopción Empresarial: 100+ empresas en 18 meses

## 🚨 **Riesgos y Mitigaciones**

### ⚠️ **Riesgos Técnicos**

#### **Riesgo: Compatibilidad Multiplataforma**
- **Probabilidad**: Media
- **Impacto**: Alto
- **Mitigación**:
  - Implementación gradual
  - Uso de frameworks probados
  - Testing extensivo en múltiples entornos
  - Fallbacks a métodos tradicionales

#### **Riesgo: Compatibilidad Multiplataforma**
- **Probabilidad**: Alta
- **Impacto**: Medio
- **Mitigación**:
  - Desarrollo incremental por plataforma
  - Testing en múltiples entornos
  - Uso de tecnologías cross-platform
  - Documentación detallada de limitaciones

### 💰 **Riesgos de Negocio**

#### **Riesgo: Competencia del Mercado**
- **Probabilidad**: Media
- **Impacto**: Alto
- **Mitigación**:
  - Diferenciación clara del producto
  - Innovación continua
  - Comunidad activa de usuarios
  - Soporte técnico superior

### 📝 **Trazabilidad**

### 📝 **Logs Detallados**
- Logs JSONL en `AppData/Logs/execution.jsonl`
- Información de hardware en `LJokerL_TTV_Hardware_Database.ps1`
- Sistema de benchmarking para diagnóstico

### 🔄 **Actualización y Mantenimiento**

### 📋 **Sistema de Actualización**
```powershell
# Actualizador automático
.\LJokerL_TTV_Auto_Updater.ps1
```

### 🧹 **Mantenimiento del Sistema**
- Limpieza automática de logs antiguos
- Optimización de base de datos de hardware
- Actualización de perfiles de optimización
- Backup automático de configuraciones

## 🤝 **Colaboración y Comunidad**

### 👥 **Equipo de Desarrollo**
- Líder del Proyecto: LJokerL_TTV
- Desarrolladores: 2-3 desarrolladores senior
- QA Engineers: 1-2 especialistas en testing
- UX/UI Designer: 1 diseñador de experiencia de usuario

### 🌐 **Comunidad de Usuarios**
- **Foro de Discusión**: Plataforma de discusión y soporte
- **GitHub**: Código abierto para componentes no críticos
- **Discord/Slack**: Comunidad en tiempo real
- **YouTube**: Tutoriales y demostraciones
- **Blog**: Artículos técnicos y actualizaciones

### 🔧 **Contribuciones Externas**
- **Programa de Beta Testers**: Usuarios que prueban versiones tempranas
- **Programa de Plugins**: Desarrolladores que crean extensiones
- **Programa de Traducción**: Comunidad que traduce a diferentes idiomas
- **Programa de Documentación**: Contribuciones a la documentación

## 🔬 **Licencia**

Este proyecto es propiedad de **LJokerL_TTV** y está diseñado para uso personal y educativo.

## 🚨 **Importante**

Este sistema requiere privilegios de administrador para funcionar correctamente. Siempre haga backup de su sistema antes de ejecutar optimizaciones.

## ✅ **Recientemente Completado**

- WPF Control Center: Apply/Revert perfiles + Advanced Tweaks UI con filtros + rollback
- Config Persistence: JSONL actualizados con tweaks validados y perfiles actualizados
- ExecutionEngine: snapshot de registro en logs + rollback por perfil/catálogo
- Test Runner: custom en C# cubriendo config, execution y logging
- Reports: Tweaks2_Validation_Status.md con clasificación SAFE/ADV/EXP

## 🔄 **Próximos Pasos Inmediatos**

### 📋 **Prioridades Actuales**
1. [Completado] Finalizar consolidación de versiones múltiples
2. [Completado] Implementar logging JSONL persistente
3. [Completado] Agregar rollback completo por perfil
4. [Completado] Expandir catálogos con items validados
5. [Completado] Agregar sección de logs visual en UI WPF
6. [Crítico] Mejorar manejo de errores globales
7. [Alta] Tests automatizados unitarios
8. [Media] Hardware detection en C# → módulo en Core
9. [Media] Reglas de hardware
10. [Baja] Empaquetado → build release + installer + portable

### 📅 **Cronograma Corto Plazo (2 semanas)**
- **Semana 1**: Mejoras de errores + tests (items 6-7)
- **Semana 2**: Hardware detection + reglas (items 8-9)
- **Semana 3**: Integración UI + recomendaciones automáticas
- **Semana 4**: Empaquetado + release + documentación final
