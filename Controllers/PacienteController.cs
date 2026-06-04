using CitasApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CitasApp.Controllers
{
    public class PacienteController : Controller
    {
        private Paciente _paciente = new Paciente
        {
            Id = 1,
            Nombre = "Ana",
            Apellido = "García",
            Email = "ana@mail.com",
            Telefono = "555-0001"
        };
        private Paciente _paciente2 = new Paciente
        {
            Id = 2,
            Nombre = "Luis",
            Apellido = "Martínez",
            Email = "luis@mail.com",
            Telefono = "555-0002"
        };
        private Paciente _paciente3 = new Paciente
        {
            Id = 3,
            Nombre = "María",
            Apellido = "López",
            Email = "maria@mail.com",
            Telefono = "555-0003"
        };

        // Método auxiliar para agrupar los pacientes existentes
        private List<Paciente> ObtenerPacientes()
        {
            return new List<Paciente> { _paciente, _paciente2, _paciente3 };
        }

        // Acción Principal: Muestra la lista y maneja el filtro de género
        public IActionResult Index(string genero = null)
        {
            var pacientes = ObtenerPacientes();

            // Poblamos los géneros que se muestran en los botones superiores
            ViewBag.Generos = new List<string> { "Femenino", "Masculino" };

            // Filtrado simulado basado en los datos disponibles
            if (!string.IsNullOrEmpty(genero))
            {
                if (genero.Equals("Femenino", StringComparison.OrdinalIgnoreCase))
                {
                    pacientes = pacientes.Where(p => p.Nombre == "Ana" || p.Nombre == "María").ToList();
                }
                else if (genero.Equals("Masculino", StringComparison.OrdinalIgnoreCase))
                {
                    pacientes = pacientes.Where(p => p.Nombre == "Luis").ToList();
                }
            }

            return View(pacientes);
        }

        // Acción para ver el detalle de un paciente específico
        public IActionResult Detalle(int id)
        {
            var paciente = ObtenerPacientes().FirstOrDefault(p => p.Id == id);
            if (paciente == null)
            {
                return NotFound();
            }
            return View(paciente);
        }

        // Acción para el botón de registrar un nuevo paciente
        public IActionResult Agregar()
        {
            return View();
        }
    }
}