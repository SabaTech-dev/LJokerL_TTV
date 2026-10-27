---
name: find-skill
description: Buscar skills disponibles para una tarea específica
usage: /find-skill <palabras-clave>
agent: skill-manager
---

# /find-skill

Busca y analiza skills disponibles en el catálogo local y externo.

## Uso

```
/find-skill <palabras-clave>
```

## Ejemplos

```
/find-skill csharp
/find-skill security windows
/find-skill wpf mvvm
```

## Funcionalidad

1. Busca en skills locales (meta, domain, task)
2. Busca en skills externas (antigravity-awesome-skills)
3. Retorna lista priorizada de skills relevantes
4. Sugiere skills para la tarea especificada

## Agente asignado

**skill-manager** - Gestiona búsqueda y carga de skills
