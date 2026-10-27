# Protocolo de Desarrollo y Reglas de Proyecto (SDLC)

> [!IMPORTANT]
> **Fecha Actual**: 2026-01-23. Todas las decisiones técnicas deben validarse contra esta fecha. No asumir obsoletas limitaciones de 2024/2025.

Este documento define las reglas estrictas para el ciclo de vida de desarrollo de software (SDLC) en este proyecto.

## 1. Principios Fundamentales
*   **Metodicidad**: Cada tarea debe pasar por las fases definidas (Planificación -> Ejecución -> Verificación).
*   **Novedad**: Verificar siempre la "Fecha Actual" antes de afirmar que una tecnología no existe o está en beta.
*   **Modularidad**: Preferir código modular y reutilizable sobre scripts monolíticos.
*   **Seguridad**: Todo input de usuario y componente externo debe ser validado.

## 2. Ciclo de Vida (Fases)

### Fase 1: Planificación (PLANNING)
*   **Agente Responsable**: `act_as_architect` (Usar skill).
*   **Entregable**: `implementation_plan.md` actualizado.
*   **Requisitos**:
    *   Definir el problema claramente.
    *   Investigar soluciones actuales (usar `learn_new_skills` si es desconocido).
    *   Validar compatibilidad con hardware/software de 2026.
    *   Solicitar aprobación del usuario antes de pasar a ejecución.

### Fase 2: Ejecución (EXECUTION)
*   **Agente Responsable**: Ingeniero de Software (Modo por defecto).
*   **Reglas**:
    *   Seguir el plan aprobado.
    *   Escribir código autodocumentado.
    *   Mantener la estructura de directorios existente.
    *   No borrar código funcional sin respaldo.

### Fase 3: Verificación (VERIFICATION)
*   **Agente Responsable**: `act_as_qa` (Usar skill).
*   **Entregable**: `walkthrough.md`, Pruebas unitarias/manuales.
*   **Reglas**:
    *   Ejecutar pruebas (usar `run_command` o `browser_subagent`).
    *   Validar que **todos** los requisitos del plan se cumplieron.
    *   Documentar bugs encontrados y corregirlos antes de cerrar la tarea.

## 3. Uso de Agentes Especializados
Se deben invocar las siguientes "skills" (personas) para tareas específicas:

*   **Arquitectura/Diseño**: Usar `act_as_architect`.
*   **Pruebas/QA**: Usar `act_as_qa`.
*   **Seguridad**: Usar `act_as_security_engineer` (si aplica).
*   **Investigación**: Usar `learn_new_skills`.

## 4. Comunicación
*   Responder siempre en Español (según solicitud del usuario).
*   Ser conciso en artifactos, detallado en explicaciones técnicas complejas.
