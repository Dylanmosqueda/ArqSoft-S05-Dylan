// CitasApp.Infrastructure/Repositories/CsvPacienteRepository.cs
// Adapter de salida — implementa IPacienteRepository leyendo un archivo CSV

using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CitasApp.Infrastructure.Repositories
{
    public class CsvPacienteRepository : IPacienteRepository
    {
        private readonly string _filePath;

        public CsvPacienteRepository(string filePath)
        {
            _filePath = filePath;

            // Sobrescribe automáticamente el archivo solo si no existe o si tiene datos viejos
            if (!File.Exists(_filePath) || !File.ReadAllText(_filePath).Contains("Carlos,García"))
            {
                var cabeceraYDatos = "Id,Nombre,Apellido,Email,Telefono\n" +
                                     "101,Carlos,García,carlos.garcia@example.com,+525551234567\n" +
                                     "102,Ana,Martínez,ana.martinez@example.com,+525557654321\n" +
                                     "103,Luis,Rodríguez,luis.rodriguez@example.com,+525559876543\n" +
                                     "104,María,López,maria.lopez@example.com,+525553456789\n" +
                                     "105,Jorge,Sánchez,jorge.sanchez@example.com,+525552345678\n";
                File.WriteAllText(_filePath, cabeceraYDatos);
            }
        }

        // ── Helpers ─────────────────────────────────────────────────────────────

        private List<Paciente> LeerTodos()
        {
            var lista = new List<Paciente>();

            foreach (var linea in File.ReadAllLines(_filePath).Skip(1))
            {
                if (string.IsNullOrWhiteSpace(linea)) continue;
                var p = linea.Split(',');
                if (p.Length < 5) continue;

                lista.Add(new Paciente
                {
                    Id = int.Parse(p[0]),
                    Nombre = p[1],
                    Apellido = p[2],
                    Email = p[3],
                    Telefono = p[4]
                });
            }

            return lista;
        }

        private void EscribirTodos(List<Paciente> pacientes)
        {
            var lineas = new List<string> { "Id,Nombre,Apellido,Email,Telefono" };

            foreach (var p in pacientes)
            {
                lineas.Add(
                    $"{p.Id}," +
                    $"{Limpiar(p.Nombre)}," +
                    $"{Limpiar(p.Apellido)}," +
                    $"{Limpiar(p.Email)}," +
                    $"{Limpiar(p.Telefono)}"
                );
            }

            File.WriteAllLines(_filePath, lineas);
        }

        private static string Limpiar(string? texto) =>
            (texto ?? string.Empty).Replace(",", ";");

        // ── Port ────────────────────────────────────────────────────────────────

        public List<Paciente> ObtenerTodos() => LeerTodos();

        public Paciente? ObtenerPorId(int id) =>
            LeerTodos().FirstOrDefault(p => p.Id == id);

        // Se redirige el método GetAll() a la lógica del helper LeerTodos
        public IEnumerable<Paciente> GetAll()
        {
            return LeerTodos();
        }

        // Se redirige el método GetById() a la lógica del helper ObtenerPorId
        public Paciente? GetById(int id)
        {
            return ObtenerPorId(id);
        }

        // Se reescribe el método Add para guardar los cambios físicos en el CSV
        public void Add(Paciente paciente)
        {
            var pacientes = LeerTodos();
            paciente.Id = pacientes.Count > 0 ? pacientes.Max(p => p.Id) + 1 : 101;
            pacientes.Add(paciente);
            EscribirTodos(pacientes);
        }
    }
}