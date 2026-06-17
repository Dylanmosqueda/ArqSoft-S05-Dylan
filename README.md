# CitasApp


App de citas médicas construida con ASP.NET Core (.NET 10).

## Arquitectura
Hexagonal (Ports & Adapters) dividida en cinco proyectos:

- **CitasApp.Domain** — modelos e interfaces (sin dependencias externas)
- **CitasApp.Application** — servicios de aplicación (orquesta el Domain)
- **CitasApp.Infrastructure** — repositorios JSON y en memoria
- **CitasApp.Web** — cliente MVC para navegador
- **CitasApp.Api** — cliente API REST para cualquier dispositivo

## Flujo de dependencias
```bash
Web  → Application → Domain ← Infrastructure
Api  → Application → Domain ← Infrastructure
```

## Entidades
- **Paciente** — lista y detalle de pacientes registrados
- **Médico** — lista y detalle de médicos disponibles
- **Cita** — agenda completa y filtro por paciente

## Persistencia
Archivos JSON en `data/` dentro de cada proyecto cliente.

## Endpoints API REST
- `GET /api/pacientes` — lista de pacientes
- `GET /api/pacientes/{id}` — detalle de un paciente
- `GET /api/medicos` — lista de médicos
- `GET /api/medicos/{id}` — detalle de un médico
- `GET /api/citas` — agenda completa
- `GET /api/citas/porpaciente/{pacienteId}` — citas de un paciente

## Navegación Web (MVC)
- `/Paciente` — lista de pacientes
- `/Medico` — lista de médicos
- `/Cita` — agenda completa
- `/Cita/PorPaciente?pacienteId=1` — citas de un paciente

## Requisitos
- .NET 10.0
- Visual Studio 2022

## Ramas
- `main` — estado evaluable con persistencia JSON en un solo proyecto
- `hexagonal` — arquitectura hexagonal multi-proyecto con capa de aplicación
- `Api` — API REST expuesta como segundo cliente del núcleo de negocio


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


# CitasApp

#Descripcion de la aplicación:
CitasApp es una aplicación web de gestión de citas médicas y control de expedientes clínicos estructurada bajo el patrón de diseño Model-View-Controller (MVC) de ASP.NET Core.
Su propósito central es coordinar y facilitar las operaciones diarias de una clínica, hospital o centro de salud, gestionando tres áreas clave: Pacientes, Médicos y Agenda de Citas.

Inteligencias Utilizadas:
IA Studio, para cambiarle el estilo de las paginas

Imagenes:
<img width="1920" height="1080" alt="Captura de pantalla 2026-06-05 202804" src="https://github.com/user-attachments/assets/c853873c-9a3f-4861-8ca5-8d9e364094b7" />
<img width="1920" height="1080" alt="Captura de pantalla 2026-06-05 203035" src="https://github.com/user-attachments/assets/0ed55dcc-9f54-4605-a005-6c0a215b750f" />
<img width="1920" height="1080" alt="Captura de pantalla 2026-06-05 203207" src="https://github.com/user-attachments/assets/64d3a7e3-fafd-428d-8694-44e83875e648" />
<img width="1920" height="1080" alt="Captura de pantalla 2026-06-05 203224" src="https://github.com/user-attachments/assets/1f57a725-26eb-439b-b3c7-9e619e978b28" />
<img width="1920" height="1080" alt="Captura de pantalla 2026-06-05 203224" src="https://github.com/user-attachments/assets/1f57a725-26eb-439b-b3c7-9e619e978b28" />

