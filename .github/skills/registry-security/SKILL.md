---
name: registry-security
description: 'Seguridad y mejores prácticas para operaciones de registro Windows: validación, snapshots, rollback y auditoría en LJokerL_TTV.'
argument-hint: 'Describe la operación de registro o el tweak a validar'
version: 1.0.0
tags: [registry, security, windows, admin, safety, validation]
priority: high
---

# Registry Security

> Origen: migrado de `.opencode/skills/domain/registry-security` durante la consolidación P1-6.
> Propósito: seguridad y mejores prácticas para operaciones de registro Windows en el sistema de optimización (C#/.NET).

## Casos de uso

- Modificar claves de registro de forma segura
- Validar permisos antes de operaciones
- Crear snapshots para rollback
- Prevenir modificaciones peligrosas
- Auditar cambios de registro

## Jerarquía de claves

```
HKEY_CLASSES_ROOT (HKCR)
HKEY_CURRENT_USER (HKCU)
HKEY_LOCAL_MACHINE (HKLM)
HKEY_USERS (HKU)
HKEY_CURRENT_CONFIG (HKCC)
```

## Claves críticas (NO MODIFICAR sin validación)

### Sistema
- `HKLM\SYSTEM\CurrentControlSet\Control\Session Manager`
- `HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon`
- `HKLM\BCD00000000` (Boot Configuration Data)

### Hardware
- `HKLM\SYSTEM\CurrentControlSet\Enum`
- `HKLM\SYSTEM\CurrentControlSet\Services`

### Seguridad
- `HKLM\SAM`
- `HKLM\SECURITY`

## Validaciones requeridas

### 1. Permisos de administrador
```csharp
public static bool IsAdministrator()
{
    using var identity = WindowsIdentity.GetCurrent();
    var principal = new WindowsPrincipal(identity);
    return principal.IsInRole(WindowsBuiltInRole.Administrator);
}
```

### 2. Riesgo del tweak
```csharp
public void ValidateTweakRisk(Tweak tweak)
{
    if (tweak.Risk == OptimizationLevel.Experimental)
    {
        // Requerir confirmación explícita
        RequireExplicitConfirmation();

        // Crear backup obligatorio
        CreateMandatoryBackup();
    }
}
```

### 3. Path validation
```csharp
public void ValidateRegistryPath(string path)
{
    // Rechazar paths peligrosos
    var dangerousPaths = new[]
    {
        @"HKLM\SAM",
        @"HKLM\SECURITY",
        @"HKLM\SYSTEM\CurrentControlSet\Control\Session Manager"
    };

    if (dangerousPaths.Any(p => path.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
    {
        throw new SecurityException($"Path protegido: {path}");
    }
}
```

## Snapshot y Rollback

### Crear snapshot
```csharp
public RegistrySnapshot CreateSnapshot(string tweakId)
{
    var snapshot = new RegistrySnapshot
    {
        TweakId = tweakId,
        Timestamp = DateTime.UtcNow,
        Entries = new List<RegistryEntry>()
    };

    // Guardar valores actuales antes de modificar
    foreach (var action in tweak.Actions.Where(a => a.Type == "registry"))
    {
        var currentValue = ReadRegistryValue(action.Path, action.Name);
        snapshot.Entries.Add(new RegistryEntry
        {
            Path = action.Path,
            Name = action.Name,
            OriginalValue = currentValue,
            ValueType = action.ValueType
        });
    }

    return snapshot;
}
```

### Rollback
```csharp
public void RollbackFromSnapshot(RegistrySnapshot snapshot)
{
    foreach (var entry in snapshot.Entries)
    {
        try
        {
            WriteRegistryValue(entry.Path, entry.Name, entry.OriginalValue, entry.ValueType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en rollback: {Path}\\{Name}", entry.Path, entry.Name);
        }
    }
}
```

## Operaciones seguras

### Lectura
```csharp
public object? ReadRegistryValue(string path, string name)
{
    try
    {
        using var key = Registry.GetValue(path, name, null);
        return key;
    }
    catch (SecurityException ex)
    {
        _logger.LogError(ex, "Acceso denegado al leer registro");
        throw new RegistryOperationException("Se requieren permisos de administrador", ex);
    }
}
```

### Escritura
```csharp
public void WriteRegistryValue(string path, string name, object value, RegistryValueKind kind)
{
    ValidateRegistryPath(path);

    if (!IsAdministrator())
    {
        throw new UnauthorizedAccessException("Se requieren permisos de administrador");
    }

    try
    {
        using var key = Registry.CurrentUser.CreateSubKey(path);
        key?.SetValue(name, value, kind);

        _logger.LogInformation("Registro modificado: {Path}\\{Name}", path, name);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error al escribir registro");
        throw;
    }
}
```

## Tipos de valores

```csharp
public enum RegistryValueType
{
    String = RegistryValueKind.String,
    DWord = RegistryValueKind.DWord,
    QWord = RegistryValueKind.QWord,
    Binary = RegistryValueKind.Binary,
    MultiString = RegistryValueKind.MultiString,
    ExpandString = RegistryValueKind.ExpandString
}
```

## Logging de auditoría

```csharp
public void LogRegistryChange(string tweakId, string path, string name, object oldValue, object newValue)
{
    var logEntry = new RegistryChangeLog
    {
        Timestamp = DateTime.UtcNow,
        TweakId = tweakId,
        Path = path,
        Name = name,
        OldValue = oldValue?.ToString(),
        NewValue = newValue?.ToString(),
        User = Environment.UserName,
        Machine = Environment.MachineName
    };

    _logger.LogInformation("Registry change: {@LogEntry}", logEntry);
}
```

## Lista de verificación de seguridad

Antes de modificar registro:
- [ ] Verificar permisos de administrador
- [ ] Validar que el path no es protegido
- [ ] Determinar nivel de riesgo del tweak
- [ ] Crear snapshot de valores actuales
- [ ] Verificar que el valor nuevo es válido
- [ ] Preparar rollback en caso de error
- [ ] Registrar operación en log

## Recuperación de errores

```csharp
try
{
    ApplyRegistryChange(action);
}
catch (RegistryOperationException ex)
{
    _logger.LogError(ex, "Error aplicando cambio de registro");

    // Intentar rollback automático
    if (snapshot != null)
    {
        RollbackFromSnapshot(snapshot);
    }

    // Notificar al usuario
    ShowErrorMessage($"Error aplicando tweak: {ex.Message}");
}
```

## Notas críticas

- **SIEMPRE** crear snapshot antes de modificar
- **NUNCA** modificar claves del sistema sin validación
- **SIEMPRE** verificar permisos de admin
- **LOGGEAR** todas las operaciones
- **PROBAR** cambios en entorno seguro primero
- **DOCUMENTAR** qué cambios se realizan
