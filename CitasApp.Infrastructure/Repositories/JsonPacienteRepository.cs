// CitasApp.Infrastructure/Repositories/JsonPacienteRepository.cs
// Adapter de salida — implementa IPacienteRepository leyendo un archivo JSON

using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace CitasApp.Infrastructure.Repositories
{
    public class JsonPacienteRepository : IPacienteRepository
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, WriteIndented = true };

        public JsonPacienteRepository(string filePath)
        {
            _filePath = filePath;
            var dir = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);

            if (!File.Exists(_filePath) || !File.ReadAllText(_filePath).Contains("ana@mail.com"))
            {
                var semilla = new List<Paciente>
                {
                    new Paciente { Id = 1, Nombre = "Ana", Apellido = "García", Email = "ana@mail.com", Telefono = "555-0001" },
                    new Paciente { Id = 2, Nombre = "Luis", Apellido = "Martínez", Email = "luis@mail.com", Telefono = "555-0002" },
                    new Paciente { Id = 3, Nombre = "María", Apellido = "López", Email = "maria@mail.com", Telefono = "555-0003" },
                    new Paciente { Id = 4, Nombre = "Manu", Apellido = "Torres", Email = "manu@mail.com", Telefono = "1234919" }
                };
                File.WriteAllText(_filePath, JsonSerializer.Serialize(semilla, _options));
            }
        }

        public IEnumerable<Paciente> GetAll()
        {
            if (!File.Exists(_filePath)) return new List<Paciente>();
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Paciente>>(json, _options) ?? new List<Paciente>();
        }

        public Paciente? GetById(int id) => GetAll().FirstOrDefault(p => p.Id == id);

        public void Add(Paciente paciente)
        {
            var lista = GetAll().ToList();
            paciente.Id = lista.Any() ? lista.Max(p => p.Id) + 1 : 1;
            lista.Add(paciente);
            File.WriteAllText(_filePath, JsonSerializer.Serialize(lista, _options));
        }

        // ── Métodos requeridos por la interfaz ──────────────────────────────────

        public List<Paciente> ObtenerTodos() => GetAll().ToList();

        public Paciente? ObtenerPorId(int id) => GetById(id);
    }
}