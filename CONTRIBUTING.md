# Guía de contribución

Gracias por tu interés en contribuir a **LJokerL_TTV Custom Config PRO**.
Este documento describe el proceso para proponer cambios.

## Requisitos previos

- **.NET 9 SDK** (la solución es C#/.NET 9 con WPF).
- **PowerShell 5.1+** (necesario solo para regenerar portable/instalador).
- **git** configurado con tu identidad.

## Antes de empezar

- Consulta [TODO.md](TODO.md): es el backlog de tareas y contiene el estado de
  cada elemento (P0/P1/P2).
- Lee [README.md](README.md) para entender el stack y la estructura.
- Si contribuyes mediante agentes de IA, sigue
  [AGENTS.md](AGENTS.md) y
  [.github/copilot-instructions.md](.github/copilot-instructions.md).

## Flujo de trabajo

1. **Rama**: crea una rama `feat/<tarea>` desde `develop` (usa el prefijo que
   corresponda: `feat/`, `fix/`, `docs/`...).
2. **Cambios**: implementa tu cambio acompañado de pruebas si aplica (la suite
   vive en `src/LJokerL.Tests`).
3. **Verificación local**: antes de abrir la PR, ejecuta en verde:

   ```bash
   dotnet build LJokerL.sln -c Release
   dotnet test LJokerL.sln -c Release --nologo
   ```

4. **Pull Request**: abre la PR contra `develop`. La CI
   (`build-and-test`) es obligatoria y debe pasar.
5. **Revisión**: un mantenedor revisará el cambio; atiende los comentarios
   antes de la integración.

## Commits

- Sigue **Conventional Commits**: `tipo: descripción` (por ejemplo
  `feat: anadir filtro de riesgo en Advanced Tweaks`).
- Tipos habituales: `feat`, `fix`, `docs`, `refactor`, `test`, `chore`, `ci`.
- Sin atribución de IA en los mensajes de commit.

## CHANGELOG

Añade una entrada bajo `## [Unreleased]` en [CHANGELOG.md](CHANGELOG.md)
describiendo tu cambio, respetando la sección que corresponda
(`Anadido`, `Cambios`, ...).

## Código de conducta

- Mantén el respeto en issues, PRs y discusiones.
- Sin spam ni publicidades; mantén las conversaciones sobre el proyecto.
- Los reportes de seguridad no van en issues públicos: consulta
  [SECURITY.md](SECURITY.md).
