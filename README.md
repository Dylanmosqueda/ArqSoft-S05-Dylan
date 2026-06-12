# CitasApp

## Arquitectura
Hexagonal (Ports & Adapters) dividida en tres proyectos:

- **CitasApp.Domain** — modelos e interfaces (sin dependencias externas)
- **CitasApp.Infrastructure** — repositorios JSON (implementa las interfaces del Domain)
- **CitasApp.Web** — controllers, views y configuración (MVC)

## Entidades
- **Paciente** — lista y detalle de pacientes registrados
- **Médico** — lista y detalle de médicos disponibles
- **Cita** — agenda completa y filtro por paciente

## Persistencia
Archivos JSON en `CitasApp.Web/data/`
- `pacientes.json`
- `medicos.json`
- `citas.json`
