using CitasApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;              // Requerido para carpetas y rutas físicas
using System.Text.Json;        // Requerido para procesar el formato JSON
using System.Linq;

namespace CitasApp.Controllers
{
    public class PacienteController : Controller
    {
        // Rutas y nombre de archivo dentro de tu carpeta Data en el proyecto
        private readonly string _folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Data");
        private readonly string _pacientesFile = "Paciente.json";

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

        // Carga y retorna la lista desde Paciente.json (con fallback si el archivo no existe)
        private List<Paciente> ObtenerPacientes()
        {
            var defaultList = new List<Paciente>
            {
                new Paciente
                {
                    Id = 1,
                    Nombre = "Ana",
                    Apellido = "García",
                    Email = "ana@mail.com",
                    Telefono = "555-0001"
                },
                new Paciente
                {
                    Id = 2,
                    Nombre = "Luis",
                    Apellido = "Martínez",
                    Email = "luis@mail.com",
                    Telefono = "555-0002"
                },
                new Paciente
                {
                    Id = 3,
                    Nombre = "María",
                    Apellido = "López",
                    Email = "maria@mail.com",
                    Telefono = "555-0003"
                }
            };
            return CargarDatosJSON(_pacientesFile, defaultList);
        }

        #endregion

        #region ACCIONES DEL CONTROLADOR

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

        // Acción para el botón de registrar un nuevo paciente (GET)
        public IActionResult Agregar()
        {
            return View();
        }

        // Acción para procesar el registro del nuevo paciente (POST) y guardarlo en Paciente.json
        [HttpPost]
        public IActionResult Agregar(Paciente nuevoPaciente)
        {
            if (ModelState.IsValid)
            {
                var pacientes = ObtenerPacientes();

                // Generamos un ID incremental para el nuevo paciente
                nuevoPaciente.Id = pacientes.Any() ? pacientes.Max(p => p.Id) + 1 : 1;

                pacientes.Add(nuevoPaciente);

                // Guardamos directamente en el archivo Paciente.json de tu carpeta Data
                GuardarDatosJSON(_pacientesFile, pacientes);

                return RedirectToAction("Index");
            }
            return View(nuevoPaciente);
        }

        #endregion
    }
}