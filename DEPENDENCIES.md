# 🔗 Análisis de Dependencias - LJokerL_TTV Custom Config PRO

## 📋 Resumen Ejecutivo

Este documento analiza las dependencias del sistema **LJokerL_TTV Custom Config PRO v12.5 WPF**, identificando componentes críticos, dependencias externas y relaciones internas entre módulos.

## 🎯 Dependencias del Sistema Operativo

### 🪟 **Windows 11/10**
- **Versión Mínima**: Windows 10 (Build 19041)
- **Versión Recomendada**: Windows 11 (Build 22000+)
- **Arquitectura**: x64 (64-bit)
- **Edición**: Pro, Enterprise, Education (recomendado)

### 🔧 **PowerShell (Legacy)**
- **Uso**: Solo para scripts en `Legacy/`
- **Versión Mínima**: PowerShell 5.1
- **Execution Policy**: RemoteSigned o Unrestricted

### 🚀 **.NET Runtime (WPF)**
- **Versión Mínima**: .NET 9
- **Componentes**:
  - WPF (UI nativa)
  - System.Management (WMI)
  - System.Diagnostics.PerformanceCounter

## 🖥️ Dependencias de Hardware

### 🔍 **Componentes Detectados**
- **CPU**: Procesador compatible con x64
- **GPU**: Tarjeta gráfica con drivers actualizados
- **RAM**: Mínimo 4GB, recomendado 8GB+
- **Almacenamiento**: 2GB+ espacio libre
- **Red**: Adaptador de red funcional

### 📊 **Requisitos de Rendimiento**
- **CPU**: Single-thread performance > 1000 (PassMark)
- **GPU**: Compatible con DirectX 11+
- **RAM**: Velocidad mínima 2133 MHz
- **Almacenamiento**: SSD recomendado para mejor rendimiento

## 🔌 Dependencias Externas

### 🛠️ **Herramientas de Terceros**
- **NVIDIA Profile Inspector**: Para optimizaciones de GPU NVIDIA
- **MSI Utility**: Para overclocking y monitoreo
- **Autoruns**: Para gestión de procesos de inicio
- **Windows Update Blocker**: Para control de actualizaciones

### 📦 **Librerías y Frameworks**
- **NuGet**:
  - System.Management
  - System.Diagnostics.PerformanceCounter
- **APIs del Sistema**:
  - Windows Management Instrumentation (WMI)
  - Windows Registry API
  - Windows Performance Counters
  - Windows Event Log

### 🌐 **Servicios del Sistema**
- **Windows Services**:
  - Windows Management Instrumentation (Winmgmt)
  - Windows Performance Counter Library
  - Windows Event Log
  - Windows Registry Service

## 🔄 Dependencias Internas del Sistema

### 📊 **Mapa de Dependencias**
```
LJokerL.App (WPF)
├── LJokerL.Core
│   ├── ExecutionEngine (registry/command/services/apps)
│   ├── HardwareDetector + HardwareRuleEvaluator
│   ├── SystemMetricsService (monitoring)
│   └── BenchmarkRunner/Storage/Comparer
└── Config/*.json (tweaks/services/apps/profiles/hardware_rules)

Legacy (PowerShell)
└── retirado en v12.5 (disponible solo en versiones anteriores)
```

### 🔗 **Relaciones de Dependencia**

#### **Dependencias Críticas (Hard Dependencies)**
- **LJokerL.App** → **LJokerL.Core**: La UI depende de servicios y modelos principales
- **ExecutionEngine** → **Config JSON**: Requiere catálogos de tweaks/servicios/apps
- **Monitoring** → **WMI/Performance Counters**: Métricas en tiempo real
- **Benchmark** → **System APIs**: Pruebas de CPU/RAM/Disk/Network

#### **Dependencias Opcionales (Soft Dependencies)**
- **Optimization Scripts (Legacy)** → **Hardware Database**: Pueden funcionar con valores por defecto
- **Monitoring UI** → **Performance Counters**: Funciona con valores "N/A" si faltan contadores
- **Installer (Legacy)** → **All Scripts**: Puede instalar parcialmente si algunos scripts fallan

## 🚨 Dependencias Críticas y Puntos de Fallo

### ❌ **Puntos de Fallo Críticos**
1. **WMI Service Down**: Bloquea toda la detección de hardware
2. **Registry Access Denied**: Impide configuraciones del sistema
3. **PowerShell Execution Policy**: Bloquea ejecución de scripts
4. **Administrator Privileges**: Requerido para todas las operaciones

### ⚠️ **Puntos de Fallo Secundarios**
1. **Hardware Detection Failure**: Sistema funciona con valores por defecto
2. **Profile Loading Error**: Usa perfil básico estándar
3. **Monitoring Counters Missing**: Muestra métricas parciales
4. **Logging Directory Full**: Continúa sin logging

## 🔧 Gestión de Dependencias

### 📋 **Verificación de Dependencias**
```powershell
# Verificación de PowerShell
if ($PSVersionTable.PSVersion.Major -lt 5) {
    throw "PowerShell 5.1+ requerido"
}

# Verificación de privilegios
if (-not ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator")) {
    throw "Privilegios de administrador requeridos"
}

# Verificación de servicios WMI
try {
    Get-WmiObject -Class Win32_ComputerSystem | Out-Null
} catch {
    throw "Servicio WMI no disponible"
}
```

### 🛡️ **Manejo de Fallos de Dependencias**
```powershell
# Fallback para hardware detection
function Get-HardwareInfo {
    try {
        # Intento de detección automática
        return Get-AutomaticHardwareInfo
    } catch {
        Write-Warning "Detección automática falló, usando valores por defecto"
        return Get-DefaultHardwareInfo
    }
}

# Fallback para perfiles
function Get-OptimizationProfile {
    try {
        return Get-HardwareBasedProfile
    } catch {
        Write-Warning "Perfil basado en hardware no disponible, usando perfil estándar"
        return Get-StandardProfile
    }
}
```

## 📊 Análisis de Impacto de Dependencias

### 🔴 **Alto Impacto**
- **WMI Service**: Bloquea 90% de funcionalidades
- **Registry Access**: Impide 70% de optimizaciones
- **Admin Privileges**: Bloquea 100% de funcionalidades

### 🟡 **Impacto Medio**
- **Hardware Detection**: Reduce efectividad en 40%
- **Profile System**: Reduce personalización en 30%
- **Monitoring UI**: Reduce monitoreo en 25%

### 🟢 **Bajo Impacto**
- **CSS Styles**: Solo afecta apariencia visual
- **Logging**: No afecta funcionalidad principal
- **Backup System**: No crítico para operación

## 🚀 Estrategias de Mitigación

### 🛡️ **Prevención de Fallos**
1. **Verificación Previa**: Check de dependencias antes de ejecución
2. **Fallbacks Robustos**: Alternativas para cada dependencia crítica
3. **Graceful Degradation**: Sistema funciona con funcionalidad reducida
4. **User Notifications**: Información clara sobre problemas de dependencias

### 🔄 **Recuperación Automática**
1. **Service Restart**: Reinicio automático de servicios críticos
2. **Permission Elevation**: Solicitud automática de privilegios
3. **Alternative Methods**: Uso de métodos alternativos cuando sea posible
4. **Configuration Reset**: Restauración de configuraciones por defecto

## 📈 Monitoreo de Dependencias

### 🔍 **Métricas de Monitoreo**
- **WMI Service Status**: Disponibilidad del servicio
- **Registry Access Success Rate**: Tasa de éxito de acceso al registro
- **Admin Privilege Validation**: Verificación de permisos
- **Hardware Detection Accuracy**: Precisión de detección de hardware

### 📊 **Alertas y Notificaciones**
- **Critical Dependencies**: Alertas inmediatas para dependencias críticas
- **Performance Impact**: Notificaciones sobre degradación de rendimiento
- **User Guidance**: Instrucciones para resolver problemas de dependencias
- **System Health**: Estado general del sistema de dependencias

## 🔮 Evolución de Dependencias

### 📅 **Versión Actual (v12.0)**
- **PowerShell Native**: Sin dependencias externas de paquetes
- **Windows APIs**: Uso de APIs nativas del sistema
- **Third-party Tools**: Herramientas opcionales para funcionalidades avanzadas

### 🚀 **Versión Futura (v13.0+)**
- **WPF/.NET**: Interfaz gráfica nativa
- **SQLite**: Base de datos local para configuraciones
- **NuGet Packages**: Librerías para funcionalidades avanzadas
- **Cross-platform**: Soporte para Linux y macOS

## 📋 Checklist de Verificación de Dependencias

### ✅ **Pre-Instalación**
- [ ] Windows 11/10 x64 verificado
- [ ] PowerShell 5.1+ disponible
- [ ] Privilegios de administrador confirmados
- [ ] Servicios WMI funcionando
- [ ] Acceso al registro verificado

### ✅ **Post-Instalación**
- [ ] Todas las dependencias críticas verificadas
- [ ] Fallbacks configurados correctamente
- [ ] Sistema de monitoreo activo
- [ ] Alertas configuradas
- [ ] Documentación de dependencias actualizada

---

**📋 Nota**: Este análisis de dependencias es fundamental para garantizar la estabilidad y confiabilidad del sistema. Las dependencias críticas deben ser monitoreadas continuamente y tener estrategias de mitigación implementadas.
