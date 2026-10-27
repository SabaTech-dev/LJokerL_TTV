---
description: Tweaks y optimizaciones de Windows con manejo de registro, servicios y apps
version: 1.0.0
tags: [windows, registry, optimization, tweaks, services, performance]
priority: high
---

# Windows Optimization

Implementa optimizaciones de Windows mediante modificaciones de registro, gestión de servicios y configuración de aplicaciones UWP.

## Casos de uso

- Optimizar rendimiento de Windows para gaming
- Reducir latencia del sistema
- Desactivar servicios innecesarios
- Remover bloatware de Windows
- Configurar perfiles de energía

## Configs JSON

### tweaks.json
Catálogo de optimizaciones con niveles de riesgo:
- **SAFE**: Cambios seguros y reversibles
- **ADVANCED**: Cambios que pueden afectar funcionalidad
- **EXPERIMENTAL**: Cambios riesgosos, requieren backup

Ejemplo de tweak:
```json
{
  "id": "game_dvr_off",
  "name": "Disable Game DVR",
  "description": "Disables Windows Game DVR to reduce latency",
  "risk": "SAFE",
  "category": "GAMING",
  "actions": [
    {
      "type": "registry",
      "path": "HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\GameDVR",
      "name": "AppCaptureEnabled",
      "value": 0
    }
  ]
}
```

### profiles.json
Perfiles predefinidos:
- **Competitive Latency**: Mínima latencia para gaming competitivo
- **Smooth VRR**: Balance entre latencia y calidad visual

### services.json
Servicios Windows con acciones:
- **disable**: Desactivar servicio
- **manual**: Cambiar a inicio manual
- **enable**: Asegurar que esté activo

### apps_bloatware.json
Aplicaciones UWP para remover/instalar.

### hardware_rules.json
Reglas de recomendación basadas en hardware detectado.

## Acciones soportadas

### 1. Registry
```csharp
{
  "type": "registry",
  "path": "HKLM\\SOFTWARE\\...",
  "name": "ValueName",
  "value": "value",
  "valueType": "DWord"
}
```

### 2. Service
```csharp
{
  "type": "service",
  "name": "ServiceName",
  "action": "disable|manual|enable"
}
```

### 3. App
```csharp
{
  "type": "app",
  "name": "AppName",
  "action": "remove|install"
}
```

### 4. Power Profile
```csharp
{
  "type": "power",
  "profile": "High Performance|Balanced|Power Saver"
}
```

## Best practices

1. **Siempre crear snapshot** antes de aplicar tweaks
2. **Validar permisos de admin** antes de operaciones
3. **Loggear todas las operaciones** en JSONL
4. **Probar en modo SAFE** antes de ADVANCED/EXPERIMENTAL
5. **Proporcionar revert** para cada modificación
6. **Verificar compatibilidad** con versión de Windows

## Niveles de riesgo

### SAFE
- Cambios estándar de Windows
- Fácilmente reversibles
- No afectan funcionalidad crítica
- Ejemplo: Desactivar Game DVR

### ADVANCED
- Modificaciones significativas
- Pueden afectar funcionalidad
- Requieren conocimiento del usuario
- Ejemplo: Desactivar Windows Update

### EXPERIMENTAL
- Cambios profundos del sistema
- Riesgo de inestabilidad
- Solo para usuarios avanzados
- Siempre requieren backup
- Ejemplo: Modificar kernel parameters

## Ejemplo de uso

```csharp
// Cargar configuración
var config = ConfigLoader.Load("Config/tweaks.json");

// Aplicar tweak
var result = ExecutionEngine.ApplyTweak("game_dvr_off");

// Verificar éxito
if (result.Success) {
    ExecutionLogger.Log(result);
}

// Revertir si es necesario
ExecutionEngine.RevertTweak("game_dvr_off");
```

## Dependencias

- registry-security: Para operaciones seguras de registro
- dotnet-csharp: Para implementación en C#

## Notas importantes

- Este proyecto requiere **privilegios de Administrador**
- Las modificaciones son **reversibles** mediante logs
- Usar **ExecutionLogger** para auditoría completa
- Validar **hardware compatible** antes de aplicar reglas
