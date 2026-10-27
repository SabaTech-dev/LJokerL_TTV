---
name: review
description: Revisión profunda de código con best practices
usage: /review
agent: code-reviewer
---

# /review

Realiza una revisión exhaustiva del código con reglas de calidad.

## Uso

```
/review
```

## Funcionalidad

1. Analiza cambios con `git diff`
2. Aplica reglas de .NET 9 y MVVM
3. Verifica convenciones de código
4. Identifica bugs potenciales
5. Sugiere mejoras

## Checklist de revisión

- [ ] Convenciones de nomenclatura
- [ ] Nullable reference types
- [ ] Async/await correcto
- [ ] Patrones MVVM
- [ ] Principios SOLID
- [ ] Seguridad
- [ ] Rendimiento

## Agente asignado

**code-reviewer** - Revisa código con GPT-5.3-Codex
