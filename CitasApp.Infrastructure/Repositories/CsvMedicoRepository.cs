// CitasApp.Infrastructure/Repositories/CsvMedicoRepository.cs
// Adapter de salida — implementa IMedicoRepository leyendo un archivo CSV

using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CitasApp.Infrastructure.Repositories
{
    public class CsvMedicoRepository : IMedicoRepository
    {
        private readonly string _filePath;

        public CsvMedicoRepository(string filePath)
        {
            _filePath = filePath;

            // Sobrescribe automáticamente el archivo solo si no existe o si tiene datos viejos
            if (!File.Exists(_filePath) || !File.ReadAllText(_filePath).Contains("LIC-1001"))
            {
                var cabeceraYDatos = "Id,Nombre,Apellido,Especialidad,NumeroLicencia\n" +
                                     "201,Juan,Perez,Cardiología,LIC-1001\n" +
                                     "202,Ana,Garcia,Pediatría,LIC-1002\n" +
                                     "203,Carlos,Lopez,Dermatología,LIC-1003\n" +
                                     "204,María,Hernandez,Ginecología,LIC-1004\n" +
                                     "205,Luis,Martinez,Neurología,LIC-1005\n" +
                                     "206,Sofia,Ramirez,Oncología,LIC-1006\n" +
                                     "207,Diego,Torres,Traumatología,LIC-1007\n" +
                                     "208,Laura,Flores,Medicina General,LIC-1008\n" +
                                     "209,Jorge,Castillo,Oftalmología,LIC-1009\n" +
                                     "210,Patricia,Mendoza,Psiquiatría,LIC-1010\n";
                File.WriteAllText(_filePath, cabeceraYDatos);
            }
        }

        // ── Helpers ─────────────────────────────────────────────────────────────

        private List<Medico> LeerTodos()
        {
            var lista = new List<Medico>();

            foreach (var linea in File.ReadAllLines(_filePath).Skip(1))
            {
                if (string.IsNullOrWhiteSpace(linea)) continue;
                var p = linea.Split(',');
                if (p.Length < 5) continue;

                lista.Add(new Medico
                {
                    Id = int.Parse(p[0]),
                    Nombre = p[1],
                    Apellido = p[2],
                    Especialidad = p[3],
                    NumeroLicencia = p[4]
                });
            }

            return lista;
        }

        private void EscribirTodos(List<Medico> medicos)
        {
            var lineas = new List<string> { "Id,Nombre,Apellido,Especialidad,NumeroLicencia" };

            foreach (var m in medicos)
            {
                lineas.Add(
                    $"{m.Id}," +
                    $"{Limpiar(m.Nombre)}," +
                    $"{Limpiar(m.Apellido)}," +
                    $"{Limpiar(m.Especialidad)}," +
                    $"{Limpiar(m.NumeroLicencia)}"
                );
            }

            File.WriteAllLines(_filePath, lineas);
        }

        private static string Limpiar(string? texto) =>
            (texto ?? string.Empty).Replace(",", ";");

        // ── Port ────────────────────────────────────────────────────────────────

        public List<Medico> ObtenerTodos() => LeerTodos();

        public Medico? ObtenerPorId(int id) =>
            LeerTodos().FirstOrDefault(m => m.Id == id);

        // Se redirige el método GetAll() a la lógica del helper LeerTodos
        public IEnumerable<Medico> GetAll()
        {
            return LeerTodos();
        }

        // Se redirige el método GetById() a la de ObtenerPorId
        public Medico? GetById(int id)
        {
            return ObtenerPorId(id);
        }

        // Se reescribe el método Add para guardar los cambios físicos en el CSV
        public void Add(Medico medico)
        {
            var medicos = LeerTodos();
            medico.Id = medicos.Count > 0 ? medicos.Max(m => m.Id) + 1 : 201;
            medicos.Add(medico);
            EscribirTodos(medicos);
        }
    }
}