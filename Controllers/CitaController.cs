using CitasApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;              // Requerido para carpetas y rutas físicas
using System.Text.Json;        // Requerido para procesar el formato JSON
using System.Linq;

namespace CitasApp.Controllers
{
    public class CitaController : Controller
    {
        // Rutas y nombres de archivos dentro de tu carpeta Data en el proyecto
        private readonly string _folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Data");
        private readonly string _pacientesFile = "Paciente.json";
        private readonly string _medicosFile = "Medico.json";
        private readonly string _citasFile = "Cita.json";

        #region MÉTODOS INTERNOS DE PERSISTENCIA JSON

        // Método privado para cargar los datos del archivo JSON en tu carpeta Data
        private List<T> CargarDatosJSON<T>(string fileName, List<T> defaultData)
        {
            string filePath = Path.Combine(_folderPath, fileName);

            // Asegurar que la carpeta Data exista
            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }

            // Usamos System.IO.File explícitamente para evitar conflictos con el método ControllerBase.File
            if (!System.IO.File.Exists(filePath))
            {
                GuardarDatosJSON(fileName, defaultData);
                return defaultData;
            }

            try
            {
                // Usamos System.IO.File explícitamente para evitar conflictos con el método ControllerBase.File
                string json = System.IO.File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
            catch
            {
                return defaultData ?? new List<T>();
            }
        }

        // Método privado para escribir físicamente en el archivo JSON
        private void GuardarDatosJSON<T>(string fileName, List<T> data)
        {
            string filePath = Path.Combine(_folderPath, fileName);

            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(data, options);

            // Usamos System.IO.File explícitamente para evitar conflictos con el método ControllerBase.File
            System.IO.File.WriteAllText(filePath, json);
        }

        #endregion

        #region MÉTODOS DE ACCESO A DATOS

        private List<Paciente> ObtenerPacientes()
        {
            var defaultList = new List<Paciente>
            {
                new Paciente { Id = 1, Nombre = "Ana", Apellido = "García" },
                new Paciente { Id = 2, Nombre = "Luis", Apellido = "Martínez" },
                new Paciente { Id = 3, Nombre = "María", Apellido = "López" },
                new Paciente { Id = 4, Nombre = "Edith", Apellido = "Romero" }
            };
            return CargarDatosJSON(_pacientesFile, defaultList);
        }

        private List<Medico> ObtenerMedicos()
        {
            var defaultList = new List<Medico>
            {
                new Medico { Id = 1, Nombre = "Carlos Reyes", Especialidad = "Medicina General" },
                new Medico { Id = 2, Nombre = "Patricia Vega", Especialidad = "Pediatría" },
                new Medico { Id = 3, Nombre = "Roberto Sánchez", Especialidad = "Cardiología" },
                new Medico { Id = 4, Nombre = "Jorge Pedrozo", Especialidad = "Cirugía" }
            };
            return CargarDatosJSON(_medicosFile, defaultList);
        }

        private List<Cita> ObtenerCitas()
        {
            var defaultList = new List<Cita>
            {
                new Cita { Id = 1, PacienteId = 1, MedicoId = 1, Fecha = new DateOnly(2026, 6, 1), Hora = new TimeOnly(9, 00), Motivo = "Consulta general", Estado = "Confirmada" },
                new Cita { Id = 2, PacienteId = 2, MedicoId = 2, Fecha = new DateOnly(2026, 6, 1), Hora = new TimeOnly(10, 00), Motivo = "Revisión de resultados", Estado = "Pendiente" },
                new Cita { Id = 3, PacienteId = 3, MedicoId = 1, Fecha = new DateOnly(2026, 6, 3), Hora = new TimeOnly(11, 00), Motivo = "Primera consulta", Estado = "Pendiente" },
                new Cita { Id = 4, PacienteId = 4, MedicoId = 4, Fecha = new DateOnly(2026, 6, 3), Hora = new TimeOnly(12, 00), Motivo = "Cirugía", Estado = "Confirmada" }
            };
            return CargarDatosJSON(_citasFile, defaultList);
        }

        #endregion

        #region ACCIONES DEL CONTROLADOR

        // Acción Principal: Carga la lista desde los archivos de tu carpeta Data
        public IActionResult Index(string estado = null)
        {
            ViewBag.Pacientes = ObtenerPacientes();
            ViewBag.Medicos = ObtenerMedicos();

            var citas = ObtenerCitas();
            var citasFiltradas = citas;

            ViewBag.Estados = citas
                .Select(c => c.Estado)
                .Where(e => !string.IsNullOrEmpty(e))
                .Distinct()
                .ToList();

            if (!string.IsNullOrEmpty(estado))
            {
                citasFiltradas = citas
                    .Where(c => string.Equals(c.Estado, estado, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return View(citasFiltradas);
        }

        // Acción Detalle
        public IActionResult Detalle(int id)
        {
            var citas = ObtenerCitas();
            var cita = citas.FirstOrDefault(c => c.Id == id);
            if (cita == null)
            {
                return NotFound();
            }

            ViewBag.Pacientes = ObtenerPacientes();
            ViewBag.Medicos = ObtenerMedicos();

            return View(cita);
        }

        // Acción Agregar (GET)
        public IActionResult Agregar()
        {
            ViewBag.Pacientes = ObtenerPacientes();
            ViewBag.Medicos = ObtenerMedicos();
            return View();
        }

        // Acción Agregar (POST)
        [HttpPost]
        public IActionResult Agregar(Cita nuevaCita)
        {
            if (ModelState.IsValid)
            {
                var citas = ObtenerCitas();
                nuevaCita.Id = citas.Any() ? citas.Max(c => c.Id) + 1 : 1;
                citas.Add(nuevaCita);

                // Guardamos directamente usando el método privado de este controlador
                GuardarDatosJSON(_citasFile, citas);

                return RedirectToAction("Index");
            }

            ViewBag.Pacientes = ObtenerPacientes();
            ViewBag.Medicos = ObtenerMedicos();
            return View(nuevaCita);
        }

        #endregion
    }
}