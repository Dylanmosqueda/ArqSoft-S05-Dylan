using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;

namespace CitasApp.Infrastructure.Repositories
{
    public class JsonMedicoRepository : IMedicoRepository
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions { WriteIndented = true };

        public JsonMedicoRepository(string filePath)
        {
            _filePath = filePath;

            var directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        // --- MÉTODOS EN INGLÉS ---
        public IEnumerable<Medico> GetAll()
        {
            if (!File.Exists(_filePath)) return new List<Medico>();
            var json = File.ReadAllText(_filePath);
            if (string.IsNullOrWhiteSpace(json)) return new List<Medico>();
            try { return JsonSerializer.Deserialize<List<Medico>>(json, _options) ?? new List<Medico>(); }
            catch { return new List<Medico>(); }
        }

        public Medico? GetById(int id)
        {
            return GetAll().FirstOrDefault(m => m.Id == id);
        }

        public void Add(Medico medico)
        {
            var medicos = GetAll().ToList();
            medicos.Add(medico);
            File.WriteAllText(_filePath, JsonSerializer.Serialize(medicos, _options));
        }

        // --- MÉTODOS EN ESPAÑOL ---
        public List<Medico> ObtenerTodos()
        {
            return GetAll().ToList();
        }

        public Medico? ObtenerPorId(int id)
        {
            return GetById(id);
        }
    }
}