---
name: retrieve_forum_trends
description: Habilidad para monitorear, extraer y analizar las 10 mejores discusiones técnicas de foros especializados en latencia, optimización y hardware.
---
# Skill: Retrieve Forum Trends

Esta habilidad permite al agente actuar como un investigador técnico avanzado, extrayendo "conocimiento tribal" y tendencias emergentes de las comunidades de hardware y optimización más prestigiosas.

## Fuentes de Información (The Sources)

### 1. Templos de Latencia y Rendimiento (Nivel Experto)
*   **Blur Busters Forums** (`forums.blurbusters.com`):
    *   *Foco:* Input lag, G-Sync/V-Sync, Motion Blur reduction.
    *   *Estrategia:* Buscar hilos de "Chief Blur Buster" o secciones "General Display & Monitor Guidelines".
*   **Guru3D Forums** (`forums.guru3d.com`):
    *   *Foco:* Drivers (NVIDIA/AMD), MSI Mode Utility, RTSS.
    *   *Secciones Clave:* "Videocards - NVIDIA GeForce Drivers / AMD Radeon Drivers".
*   **Overclock.net (OCN)** (`overclock.net`):
    *   *Foco:* RAM timings (tRFC/tREFI), CPU tweaking (Intel/AMD).
    *   *Secciones Clave:* "Intel CPUs", "AMD CPUs", "Memory".

### 2. Optimización de Windows (Kernel & OS)
*   **TeamOS** (`teamos.xyz`):
    *   *Foco:* Windows Lites, Tweaks de registro.
*   **GitHub (Issues/Discussions)**:
    *   *Proyectos:* `Fr33thy`, `Chris Titus Tech` (winutil), `AtlasOS`.
    *   *Estrategia:* Buscar en pestañas "Issues" con etiquetas como "enhancement" o discusiones recientes.

### 3. Gaming Competitivo y Soporte Real
*   **Reddit**:
    *   `r/buildapc`: Tendencias generales.
    *   `r/CompetitiveApex` / `r/ValorantTechSupport`: Problemas específicos de engines.
    *   `r/overclocking`: Validación de voltajes seguros.
*   **Steam Community**:
    *   *Estrategia:* Buscar "Launch Options" o "FPS Fix" en los Hubs de juegos específicos.

### 4. Comunidad Hispana
*   **El Chapuzas Informático** (`foro.elchapuzasinformatico.com`): Presupuestos y OC.
*   **Mediavida** (`mediavida.com/foro/hard-soft`): CS2/Valorant y problemas de parches.
*   **ForoCoches** (`forocoches.com`): Sección Hardware/Gaming.

## Procedimiento de Ejecución

Para extraer el "Top 10" actual:

1.  **Identificar el Objetivo**: ¿Se busca latencia extrema, estabilidad de OS, o solución de problemas específicos?
2.  **Búsqueda y Navegación**:
    *   Utilizar `search_web` con operadores avanzados: `site:forums.blurbusters.com "input lag" after:2024-01-01`.
    *   Utilizar `browser_subagent` para visitar las secciones de "Hot" o "Trending" de los foros principales.
3.  **Filtrado de Calidad**:
    *   Priorizar hilos con >10 respuestas o marcados como [SOLVED] / [GUIDE].
    *   Descartar preguntas básicas de soporte técnico (ej. "¿Mi PC corre esto?").
4.  **Síntesis**: Resumir el hallazgo técnico (ej. "Nuevo driver NVIDIA 551.xx reduce latencia DPC según usuarios de Guru3D").

## Ejemplo de Prompt para el Agente
> "Usa la skill retrieve_forum_trends para encontrar las últimas discusiones sobre 'Windows 11 25H2 latencia' en OCN y Reddit."
