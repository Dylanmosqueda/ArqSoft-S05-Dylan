using CitasApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;

namespace CitasApp.Controllers
{
    public class MedicoController : Controller
    {
        private readonly string _folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Data");
        private readonly string _medicosFile = "Medico.json";

        private Medico[] CargarDatosJSON(string fileName, Medico[] defaultData)
        {
            string filePath = Path.Combine(_folderPath, fileName);

            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }

            if (!System.IO.File.Exists(filePath))
            {
                GuardarDatosJSON(fileName, defaultData);
                return defaultData;
            }

            try
            {
                string json = System.IO.File.ReadAllText(filePath);
                var array = JsonSerializer.Deserialize(json, typeof(Medico[])) as Medico[];
                return array ?? defaultData;
            }
            catch
            {
                return defaultData;
            }
        }

        private void GuardarDatosJSON(string fileName, Medico[] data)
        {
            string filePath = Path.Combine(_folderPath, fileName);

            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(data, typeof(Medico[]), options);
            System.IO.File.WriteAllText(filePath, json);
        }

        private Medico[] ObtenerMedicos()
        {
            var defaultList = new Medico[]
            {
                new Medico { Id = 1, Nombre = "Carlos Reyes", Especialidad = "Medicina General", NumeroLicencia = "MG - 10421" },
                new Medico { Id = 2, Nombre = "Patricia Vega", Especialidad = "Pediatría", NumeroLicencia = "PD - 20835" },
                new Medico { Id = 3, Nombre = "Roberto Sánchez", Especialidad = "Cardiología", NumeroLicencia = "CA - 30117" }
            };
            return CargarDatosJSON(_medicosFile, defaultList);
        }

        public IActionResult Index(string especialidad = null)
        {
            var medicos = ObtenerMedicos();

            ViewBag.Especialidades = medicos
                .Select(m => m.Especialidad)
                .Where(e => !string.IsNullOrEmpty(e))
                .Distinct()
                .ToList();

            if (!string.IsNullOrEmpty(especialidad))
            {
                medicos = medicos
                    .Where(m => string.Equals(m.Especialidad, especialidad, StringComparison.OrdinalIgnoreCase))
                    .ToArray();
            }

            return View(medicos);
        }

        public IActionResult Detalle(int id)
        {
            var medico = ObtenerMedicos().FirstOrDefault(m => m.Id == id);
            if (medico == null)
            {
                return NotFound();
            }
            return View(medico);
        }

        public IActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Agregar(Medico nuevoMedico)
        {
            var medicos = ObtenerMedicos().ToList();
            nuevoMedico.Id = medicos.Any() ? medicos.Max(m => m.Id) + 1 : 1;
            medicos.Add(nuevoMedico);

            GuardarDatosJSON(_medicosFile, medicos.ToArray());

            return RedirectToAction("Index");
        }
    }
}