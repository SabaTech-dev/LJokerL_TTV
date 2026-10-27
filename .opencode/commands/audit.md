---
name: audit
description: Auditoría de seguridad de cambios
usage: /audit
agent: security-auditor
---

# /audit

Audita los cambios pendientes enfocándose en seguridad.

## Uso

```
/audit
```

## Funcionalidad

1. Analiza cambios pendientes
2. Revisa operaciones de registro
3. Valida manejo de permisos
4. Verifica logging de auditoría
5. Detecta vulnerabilidades

## Checklist de seguridad

- [ ] Validación de inputs
- [ ] Manejo de excepciones
- [ ] Paths protegidos
- [ ] Permisos de admin
- [ ] Secrets hardcoded
- [ ] Logging adecuado
- [ ] Rollback disponible

## Agente asignado

**security-auditor** - Audita seguridad con Claude Opus 4.6
