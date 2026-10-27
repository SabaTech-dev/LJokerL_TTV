---
name: test
description: Ejecutar tests con estrategia óptima
usage: /test
agent: test-engineer
---

# /test

Ejecuta tests y analiza cobertura del proyecto.

## Uso

```
/test
```

## Funcionalidad

1. Ejecuta `dotnet test`
2. Analiza resultados
3. Verifica cobertura
4. Identifica tests faltantes
5. Sugiere tests adicionales

## Tipos de tests

- **Unit tests**: Lógica de negocio
- **Integration tests**: Flujos completos
- **Coverage**: Métricas de cobertura

## Agente asignado

**test-engineer** - Ejecuta tests con GLM-5
