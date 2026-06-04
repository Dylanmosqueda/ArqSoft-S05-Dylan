using CitasApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CitasApp.Controllers
{
    public class MedicoController : Controller
    {
        private Medico _Medico = new Medico
        {
            Id = 1,
            Nombre = "Dr. Carlos Reyes",
            Especialidad = "Medicina General",
            NumeroLicencia = "MG - 10421"
        };
        private Medico _Medico2 = new Medico
        {
            Id = 2,
            Nombre = "Dra. Patricia Vega",
            Especialidad = "Pediatría",
            NumeroLicencia = "PD - 20835"
        };
        private Medico _Medico3 = new Medico
        {
            Id = 3,
            Nombre = "Dr. Roberto Sánchez",
            Especialidad = "Cardiología",
            NumeroLicencia = "CA - 30117"
        };

        // Método auxiliar para agrupar los médicos existentes en una lista
        private List<Medico> ObtenerMedicos()
        {
            return new List<Medico> { _Medico, _Medico2, _Medico3 };
        }

        // Acción Principal: Muestra la lista y maneja el filtro de especialidad
        public IActionResult Index(string especialidad = null)
        {
            var medicos = ObtenerMedicos();

            // Obtenemos las especialidades únicas de la lista para cargarlas en la vista
            ViewBag.Especialidades = medicos
                .Select(m => m.Especialidad)
                .Where(e => !string.IsNullOrEmpty(e))
                .Distinct()
                .ToList();

            // Aplicamos el filtro si se selecciona alguna especialidad
            if (!string.IsNullOrEmpty(especialidad))
            {
                medicos = medicos
                    .Where(m => string.Equals(m.Especialidad, especialidad, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return View(medicos);
        }

        // Acción para ver el perfil y horarios de un médico específico
        public IActionResult Detalle(int id)
        {
            var medico = ObtenerMedicos().FirstOrDefault(m => m.Id == id);
            if (medico == null)
            {
                return NotFound();
            }
            return View(medico);
        }

        // Acción para el formulario de registro de un nuevo médico
        public IActionResult Agregar()
        {
            return View();
        }
    }
}