---
name: assign-agent
description: Asignar el mejor agente para una tarea
usage: /assign-agent <descripción de la tarea>
agent: agent-dispatcher
---

# /assign-agent

Analiza la tarea y asigna el agente especializado más adecuado.

## Uso

```
/assign-agent <descripción de tarea>
```

## Ejemplos

```
/assign-agent "Revisar código del ExecutionEngine"
/assign-agent "Auditar cambios en registry"
/assign-agent "Crear tests para ConfigLoader"
```

## Lógica de asignación

1. Analiza tipo de tarea (code, security, test, doc, etc.)
2. Identifica tecnologías involucradas
3. Determina complejidad y modelo apropiado
4. Selecciona agente con skills matching
5. Carga skills necesarias

## Agente asignado

**agent-dispatcher** - Orquesta asignación de agentes
