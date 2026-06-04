using CitasApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CitasApp.Controllers
{
    public class CitaController : Controller
    {
        private List<Paciente> _pacientes = new List<Paciente>
        {
            new Paciente { Id = 1, Nombre = "Ana", Apellido = "García" },
            new Paciente { Id = 2, Nombre = "Luis", Apellido = "Martínez" },
            new Paciente { Id = 3, Nombre = "María", Apellido = "López" },
        };

        private List<Medico> _medicos = new List<Medico>
        {
            new Medico { Id = 1, Nombre = "Carlos Reyes", Especialidad = "Medicina General" },
            new Medico { Id = 2, Nombre = "Patricia Vega", Especialidad = "Pediatría" },
            new Medico { Id = 3, Nombre = "Roberto Sánchez", Especialidad = "Cardiología" },
        };

        private List<Cita> _citas = new()
        {
            new Cita
            {
                Id = 1,
                PacienteId = 1,
                MedicoId = 1,
                Fecha = new DateOnly(2026, 6, 1),
                Hora = new TimeOnly(9, 00),
                Motivo = "Consulta general",
                Estado = "Confirmada"
            },
            new Cita
            {
                Id = 2,
                PacienteId = 2,
                MedicoId = 2,
                Fecha = new DateOnly(2026, 6, 1),
                Hora = new TimeOnly(10, 00),
                Motivo = "Revisión de resultados",
                Estado = "Pendiente"
            },
            new Cita
            {
                Id = 3,
                PacienteId = 3,
                MedicoId = 1,
                Fecha = new DateOnly(2026, 6, 3),
                Hora = new TimeOnly(11, 00),
                Motivo = "Primera consulta",
                Estado = "Pendiente"
            },
        };

        // Acción Principal: Carga el listado y envía los catálogos en ViewBag para la vinculación
        public IActionResult Index(string estado = null)
        {
            // Pasamos los catálogos a la vista
            ViewBag.Pacientes = _pacientes;
            ViewBag.Medicos = _medicos;

            var citasFiltradas = _citas;

            // Extraemos los estados únicos para pintar los botones de filtro
            ViewBag.Estados = _citas
                .Select(c => c.Estado)
                .Where(e => !string.IsNullOrEmpty(e))
                .Distinct()
                .ToList();

            if (!string.IsNullOrEmpty(estado))
            {
                citasFiltradas = _citas
                    .Where(c => string.Equals(c.Estado, estado, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return View(citasFiltradas);
        }

        // Acción Detalle
        public IActionResult Detalle(int id)
        {
            var cita = _citas.FirstOrDefault(c => c.Id == id);
            if (cita == null)
            {
                return NotFound();
            }

            ViewBag.Pacientes = _pacientes;
            ViewBag.Medicos = _medicos;

            return View(cita);
        }

        // Acción Agregar
        public IActionResult Agregar()
        {
            return View();
        }
    }
}