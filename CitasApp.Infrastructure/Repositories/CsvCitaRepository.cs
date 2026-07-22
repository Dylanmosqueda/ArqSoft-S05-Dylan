// CitasApp.Infrastructure/Repositories/CsvCitaRepository.cs
// Adapter de salida — implementa ICitaRepository leyendo un archivo CSV

using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CitasApp.Infrastructure.Repositories
{
    public class CsvCitaRepository : ICitaRepository
    {
        private readonly string _filePath;

        public CsvCitaRepository(string filePath)
        {
            _filePath = filePath;

            // Sobrescribe automáticamente el archivo solo si no existe o si tiene datos viejos
            if (!File.Exists(_filePath) || !File.ReadAllText(_filePath).Contains("Control de rutina mensual"))
            {
                var cabeceraYDatos = "Id,PacienteId,MedicoId,Fecha,Hora,Motivo,Estado\n" +
                                     "1,101,201,2026-06-11,09:00,Control de rutina mensual,Pendiente\n" +
                                     "2,102,202,2026-06-11,10:30,Revisión de resultados de laboratorio,Pendiente\n" +
                                     "3,103,201,2026-06-11,11:15,Dolor de cabeza crónico y fatiga,Pendiente\n" +
                                     "4,104,203,2026-06-12,16:00,Consulta inicial por dolor lumbar,Pendiente\n" +
                                     "5,105,202,2026-06-12,17:30,Seguimiento de tratamiento de hipertensión,Pendiente\n";
                File.WriteAllText(_filePath, cabeceraYDatos);
            }
        }


        // ── Helpers ─────────────────────────────────────────────────────────────

        private List<Cita> LeerTodos()
        {
            var lista = new List<Cita>();

            foreach (var linea in File.ReadAllLines(_filePath).Skip(1))
            {
                if (string.IsNullOrWhiteSpace(linea)) continue;
                var p = linea.Split(',');
                if (p.Length < 7) continue;

                lista.Add(new Cita
                {
                    Id = int.Parse(p[0]),
                    PacienteId = int.Parse(p[1]),
                    MedicoId = int.Parse(p[2]),
                    Fecha = DateOnly.ParseExact(p[3], "yyyy-MM-dd"),
                    Hora = TimeOnly.ParseExact(p[4], "HH:mm"),
                    Motivo = p[5],
                    Estado = p[6]
                });
            }

            return lista;
        }

        private void EscribirTodos(List<Cita> citas)
        {
            var lineas = new List<string>
                { "Id,PacienteId,MedicoId,Fecha,Hora,Motivo,Estado" };

            foreach (var c in citas)
            {
                lineas.Add(
                    $"{c.Id}," +
                    $"{c.PacienteId}," +
                    $"{c.MedicoId}," +
                    $"{c.Fecha:yyyy-MM-dd}," +
                    $"{c.Hora:HH:mm}," +
                    $"{Limpiar(c.Motivo)}," +
                    $"{Limpiar(c.Estado)}"
                );
            }

            File.WriteAllLines(_filePath, lineas);
        }

        private static string Limpiar(string texto) =>
            (texto ?? string.Empty).Replace(",", ";");

        // ── Port ────────────────────────────────────────────────────────────────

        public List<Cita> ObtenerTodos() => LeerTodos();

        public Cita? ObtenerPorId(int id) =>
            LeerTodos().FirstOrDefault(c => c.Id == id);

        public List<Cita> ObtenerPorPaciente(int pacienteId) =>
            LeerTodos().Where(c => c.PacienteId == pacienteId).ToList();

        public void Agregar(Cita cita)
        {
            var citas = LeerTodos();
            cita.Id = citas.Count > 0 ? citas.Max(c => c.Id) + 1 : 1;
            citas.Add(cita);
            EscribirTodos(citas);
        }

        public void ConfirmarCita(int id)
        {
            var citas = LeerTodos();
            var cita = citas.FirstOrDefault(c => c.Id == id);

            if (cita is not null)
            {
                cita.Estado = "Confirmada";
                EscribirTodos(citas);
            }
        }

        // Se redirige el método GetAll() a la lógica de LeerTodos()
        public IEnumerable<Cita> GetAll()
        {
            return LeerTodos();
        }

        // Se redirige el método GetById() a la de ObtenerPorId()
        public Cita? GetById(int id)
        {
            return ObtenerPorId(id);
        }

        // Se enlaza el método Add de la interfaz con la lógica de Agregar()
        public void Add(Cita cita)
        {
            Agregar(cita);


        }
        public void Update(Cita cita)
        {
            throw new NotImplementedException();
        }
    }
}