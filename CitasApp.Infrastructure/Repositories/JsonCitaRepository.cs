using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;

namespace CitasApp.Infrastructure.Repositories
{
    public class JsonCitaRepository : ICitaRepository
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions { WriteIndented = true };

        public JsonCitaRepository(string filePath)
        {
            _filePath = filePath;

            var directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        // --- MÉTODOS EN INGLÉS ---
        public IEnumerable<Cita> GetAll()
        {
            if (!File.Exists(_filePath)) return new List<Cita>();
            var json = File.ReadAllText(_filePath);
            if (string.IsNullOrWhiteSpace(json)) return new List<Cita>();
            try { return JsonSerializer.Deserialize<List<Cita>>(json, _options) ?? new List<Cita>(); }
            catch { return new List<Cita>(); }
        }

        public Cita? GetById(int id)
        {
            return GetAll().FirstOrDefault(c => c.Id == id);
        }

        public void Add(Cita cita)
        {
            var citas = GetAll().ToList();
            citas.Add(cita);
            File.WriteAllText(_filePath, JsonSerializer.Serialize(citas, _options));
        }

        public void Update(Cita cita)
        {
            var citas = GetAll().ToList();
            var index = citas.FindIndex(c => c.Id == cita.Id);
            if (index != -1)
            {
                citas[index] = cita;
                File.WriteAllText(_filePath, JsonSerializer.Serialize(citas, _options));
            }
        }

        // --- MÉTODOS EN ESPAÑOL ---

        public List<Cita> ObtenerTodos()
        {
            return GetAll().ToList();
        }

        public Cita? ObtenerPorId(int id)
        {
            return GetById(id);
        }

        // AGREGADO: Método requerido por tu interfaz ICitaRepository
        public List<Cita> ObtenerPorPaciente(int pacienteId)
        {
            // Filtra y devuelve las citas correspondientes al paciente.
            // NOTA: Si en tu clase modelo 'Cita' la propiedad se llama 'IdPaciente' 
            // en lugar de 'PacienteId', cambia 'c.PacienteId' por 'c.IdPaciente'.
            return GetAll().Where(c => c.PacienteId == pacienteId).ToList();
        }
    }
}