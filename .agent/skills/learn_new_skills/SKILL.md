---
name: learn_new_skills
description: Una skill para investigar y adquirir nuevas capacidades técnicas o metodológicas necesarias para completar tareas complejas.
---

# Learn New Skills

Esta skill define el procedimiento estándar para cuando el agente se encuentra con una tarea, tecnología o problema desconocido.

## Procedimiento

1.  **Análisis de la Brecha de Conocimiento**
    *   Identificar qué conocimiento específico falta (ej. sintaxis de un lenguaje, uso de una API, configuración de una herramienta).
    *   Formular preguntas específicas de búsqueda.

2.  **Investigación Activa**
    *   Utilizar `search_web` para buscar documentación oficial, tutoriales y foros (StackOverflow, GitHub Issues) relevantes.
    *   Utilizar `read_url_content` para leer documentación técnica profunda.
    *   Buscar ejemplos de código o patrones de implementación.

3.  **Síntesis y Planificación**
    *   Resumir lo aprendido en un plan de acción concreto.
    *   Si la nueva capacidad es reutilizable (ej. "Cómo desplegar en Kubernetes"), considerar crear una nueva **Skill** dedicada para el futuro.

4.  **Implementación de Nueva Skill (Opcional)**
    *   Si se decide crear una skill permanente:
        *   Crear directorio: `.agent/skills/<nombre_skill>`
        *   Crear archivo: `.agent/skills/<nombre_skill>/SKILL.md`
        *   Incluir Frontmatter YAML (name, description).
        *   Documentar los pasos, comandos y scripts necesarios.

5.  **Verificación**
    *   Probar el nuevo conocimiento con un prototipo o una implementación aislada antes de aplicarlo al código principal del usuario.

## Recursos
- [Google Antigravity Docs](https://antigravity.google) (Referencia general)
