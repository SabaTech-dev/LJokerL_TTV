# Política de seguridad

## Alcance

**LJokerL_TTV Custom Config PRO** es una aplicación de escritorio para Windows
que modifica el registro del sistema, servicios y paquetes Appx **con
privilegios de administrador**. Una vulnerabilidad en este proyecto puede
afectar directamente a la configuración y estabilidad del sistema operativo del
usuario, por lo que los reportes se tratan con prioridad.

Sistemas afectados: Windows 11 y Windows 10 (build 19041 o superior).

## Versiones soportadas

| Versión          | Rama          | Soporte de seguridad |
| ---------------- | ------------- | -------------------- |
| 13.0.x           | `develop` / `main` | Sí              |
| Versiones anteriores | —         | No                  |

## Cómo reportar una vulnerabilidad

**Canal preferido**: el informe privado de vulnerabilidades de GitHub de este
repositorio (`SabaTech-dev/LJokerL_TTV`), disponible en la pestaña
*Security* → *Report a vulnerability*. Permite una conversación privada sin
exponer el detalle público.

**Alternativa**: abre un issue marcándolo como confidencial y con la etiqueta
de seguridad, evitando incluir detalles explotables en el texto público.

**Plazos objetivo de respuesta**:

- **Acuse de recibo**: 7 días desde el reporte.
- **Valoración y clasificación**: 30 días desde el reporte.

### Qué incluir en el reporte

- Versión de la aplicación y build de Windows afectados.
- Perfil, tweak, servicio o Appx implicado.
- Pasos mínimos para reproducir el problema.
- Impacto esperado (pérdida de datos, escalada de privilegios, inestabilidad).
- Cualquier log relevante de `Logs\execution.jsonl` (sin datos personales).

## Fuera de ámbito

- **Vulnerabilidades de dependencias de terceros** ubicadas en `Resources/`:
  repórtalas directamente al proyecto upstream correspondiente. Este
  repositorio no mantiene ese código.

## Prácticas de seguridad del proyecto

- **Snapshot de registro** antes de aplicar cualquier cambio: los valores
  previos se registran en el log de ejecución.
- **Rollback limitado al registro**: la reversión reconstruye el estado previo
  desde ese snapshot; no toca nada fuera del ámbito aplicado.
- **Sin telemetría**: ningún dato sale de la máquina; la aplicación no realiza
  conexiones de red.
- **Logs locales JSONL**: `Logs\execution.jsonl` y `Logs\metrics.jsonl` se
  escriben únicamente en el directorio de instalación de la aplicación.

## Aviso

La aplicación requiere privilegios de administrador. Ejecútala únicamente si
comprendes los cambios que va a aplicar y dispones de una copia de seguridad
del sistema.
