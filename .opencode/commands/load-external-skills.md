---
name: load-external-skills
description: Cargar skills específicas del repositorio externo
usage: /load-external-skills <categoría>
agent: skill-manager
---

# /load-external-skills

Carga skills del repositorio externo sickn33/antigravity-awesome-skills.

## Uso

```
/load-external-skills <categoría>
```

## Ejemplos

```
/load-external-skills security
/load-external-skills csharp
/load-external-skills testing
```

## Funcionalidad

1. Verifica repositorio externo clonado
2. Busca skills por categoría o keywords
3. Carga skills relevantes para el contexto actual
4. Integra con skills locales

## Agente asignado

**skill-manager** - Gestiona carga de skills externas
