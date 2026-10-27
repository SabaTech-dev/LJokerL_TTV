---
name: create-skill
description: Crear una nueva skill según necesidad detectada
usage: /create-skill <descripción de la necesidad>
agent: skill-manager
---

# /create-skill

Crea una nueva skill cuando no existe una adecuada para la tarea.

## Uso

```
/create-skill <descripción>
```

## Ejemplos

```
/create-skill "Necesito manejar errores WMI específicos"
/create-skill "Crear skill para logging avanzado"
```

## Proceso

1. Analiza la necesidad descrita
2. Verifica que no exista skill similar
3. Determina tipo (meta, domain, task)
4. Crea estructura de archivos
5. Genera SKILL.md con template

## Agente asignado

**skill-manager** - Gestiona creación de nuevas skills
