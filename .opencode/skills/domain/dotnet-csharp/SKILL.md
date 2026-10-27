---
description: Convenciones y mejores prácticas para C# y .NET 9
version: 1.0.0
tags: [csharp, dotnet, net9, conventions, coding-standards]
priority: high
---

# .NET C# Conventions

Convenciones de código, patrones y mejores prácticas para desarrollo en C# con .NET 9.

## Casos de uso

- Escribir código C# consistente
- Seguir estándares de la industria
- Implementar patrones modernos
- Aprovechar características de .NET 9

## Convenciones de nomenclatura

### PascalCase
- Clases: `ExecutionEngine`
- Métodos: `ApplyTweak()`
- Propiedades: `SelectedProfile`
- Interfaces: `IExecutionEngine`
- Enums: `OptimizationLevel`

### camelCase
- Variables: `selectedProfile`
- Parámetros: `profileId`
- Campos privados: `_selectedProfile`

### UPPER_SNAKE_CASE
- Constantes: `MAX_RETRY_COUNT`

## Estructura de clases

```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LJokerL.Core.Services
{
    /// <summary>
    /// Descripción de la clase
    /// </summary>
    public class ExecutionEngine : IExecutionEngine
    {
        #region Constants
        private const int MAX_RETRY_COUNT = 3;
        #endregion

        #region Fields
        private readonly IConfigLoader _configLoader;
        private readonly IExecutionLogger _logger;
        #endregion

        #region Constructor
        public ExecutionEngine(IConfigLoader configLoader, IExecutionLogger logger)
        {
            _configLoader = configLoader ?? throw new ArgumentNullException(nameof(configLoader));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        #endregion

        #region Public Methods
        public async Task<ExecutionResult> ApplyTweakAsync(string tweakId)
        {
            // Implementación
        }
        #endregion

        #region Private Methods
        private void ValidateTweak(Tweak tweak)
        {
            // Implementación
        }
        #endregion
    }
}
```

## Nullable Reference Types

Habilitar en `.csproj`:
```xml
<Nullable>enable</Nullable>
```

Uso correcto:
```csharp
// No nullable (por defecto)
public string Name { get; set; } = string.Empty;

// Nullable explícito
public string? Description { get; set; }

// Null-forgiving operator cuando se garantiza no-null
public string GetValue(string? input) => input!;
```

## Async/Await

### Nomenclatura
- Métodos async terminan en `Async`: `ApplyTweakAsync()`
- Devolver `Task` o `Task<T>`

### Buenas prácticas
```csharp
// CORRECTO
public async Task<Profile> GetProfileAsync(string id)
{
    return await _repository.GetAsync(id);
}

// CORRECTO: Async void solo para event handlers
private async void OnButtonClick(object sender, EventArgs e)
{
    await LoadDataAsync();
}

// EVITAR: Async sin await
public async Task DoWork() // No es realmente async
{
    Thread.Sleep(1000); // Bloquea el hilo
}

// CORRECTO: Usar Task.Delay
public async Task DoWorkAsync()
{
    await Task.Delay(1000); // No bloquea
}
```

## Manejo de excepciones

```csharp
public async Task<ExecutionResult> ApplyTweakAsync(string tweakId)
{
    try
    {
        var tweak = await _configLoader.GetTweakAsync(tweakId);
        return await ExecuteTweakAsync(tweak);
    }
    catch (TweakNotFoundException ex)
    {
        _logger.LogError(ex, "Tweak no encontrado: {TweakId}", tweakId);
        return ExecutionResult.Failure($"Tweak '{tweakId}' no existe");
    }
    catch (UnauthorizedAccessException ex)
    {
        _logger.LogError(ex, "Permisos insuficientes");
        return ExecutionResult.Failure("Se requieren privilegios de administrador");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error inesperado aplicando tweak");
        throw; // Re-lanzar excepciones no manejadas
    }
}
```

## Inyección de dependencias

```csharp
// Interfaz
public interface IExecutionEngine
{
    Task<ExecutionResult> ApplyTweakAsync(string tweakId);
}

// Implementación
public class ExecutionEngine : IExecutionEngine
{
    private readonly IConfigLoader _configLoader;
    private readonly IExecutionLogger _logger;

    public ExecutionEngine(IConfigLoader configLoader, IExecutionLogger logger)
    {
        _configLoader = configLoader;
        _logger = logger;
    }
}

// Registro en DI
services.AddSingleton<IConfigLoader, ConfigLoader>();
services.AddSingleton<IExecutionLogger, ExecutionLogger>();
services.AddTransient<IExecutionEngine, ExecutionEngine>();
```

## Records (C# 9+)

```csharp
// Record inmutable
public record Profile(
    string Id,
    string Name,
    string Description,
    OptimizationLevel Risk);

// Record mutable (raro)
public record MutableProfile
{
    public string Id { get; set; } = string.Empty;
}

// Record con validación
public record Tweak
{
    public string Id { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    
    public Tweak(string id, string name)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Id requerido", nameof(id));
        
        Id = id;
        Name = name;
    }
}
```

## Pattern Matching

```csharp
// Switch expression
public string GetRiskDescription(OptimizationLevel level) => level switch
{
    OptimizationLevel.Safe => "Cambio seguro",
    OptimizationLevel.Advanced => "Cambio avanzado",
    OptimizationLevel.Experimental => "Cambio experimental",
    _ => "Desconocido"
};

// Property pattern
if (result is { Success: true, Changes: { Count: > 0 } })
{
    // Resultado exitoso con cambios
}

// Null checking moderno
if (profile is not null)
{
    // Usar profile
}
```

## LINQ

```csharp
// Usar métodos de extensión (más legible en C#)
var safeTweaks = tweaks
    .Where(t => t.Risk == OptimizationLevel.Safe)
    .OrderBy(t => t.Name)
    .Select(t => t.Id)
    .ToList();

// FirstOrDefault en lugar de First
var tweak = tweaks.FirstOrDefault(t => t.Id == id);

// Any en lugar de Count
if (tweaks.Any(t => t.IsApplied))
{
    // Hay tweaks aplicados
}
```

## Configuración del proyecto

### .csproj básico
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0-windows</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <LangVersion>latest</LangVersion>
  </PropertyGroup>
</Project>
```

## Dependencias

- wpf-mvvm: Para integración con UI
- windows-optimization: Para implementación de optimizaciones

## Notas

- Usar `var` cuando el tipo es obvio
- Preferir expresiones switch sobre if-else anidados
- Usar records para DTOs inmutables
- Implementar IDisposable para recursos no manejados
- Usar `ConfigureAwait(false)` en librerías (no en UI)
