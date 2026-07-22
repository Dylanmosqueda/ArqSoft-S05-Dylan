using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;

namespace CitasApp.Infrastructure.Repositories
{
    public class SqliteCitaRepository : ICitaRepository
    {
        private readonly string _connectionString;

        public SqliteCitaRepository(string dbPath)
        {
            _connectionString = $"Data Source={Path.GetFullPath(dbPath)}";
            InicializarTabla(); // Se restaura la inicialización autónoma
        }

        private void InicializarTabla()
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Citas (
                    Id         INTEGER PRIMARY KEY AUTOINCREMENT,
                    PacienteId INTEGER NOT NULL,
                    MedicoId   INTEGER NOT NULL,
                    Fecha      TEXT NOT NULL,
                    Hora       TEXT NOT NULL,
                    Motivo     TEXT,
                    Estado     TEXT NOT NULL
                );

                -- Insertar Citas cruzadas (Saw Theme) si la tabla está vacía
                INSERT INTO Citas (Id, PacienteId, MedicoId, Fecha, Hora, Motivo, Estado)
                SELECT 1, 1, 2, '2026-07-20', '09:00:00', 'Prueba de la trampa para osos inversa', 'Confirmada' WHERE NOT EXISTS (SELECT 1 FROM Citas WHERE Id = 1);
                
                INSERT INTO Citas (Id, PacienteId, MedicoId, Fecha, Hora, Motivo, Estado)
                SELECT 2, 2, 1, '2026-07-21', '10:30:00', 'Juego de la sierra de mano industrial', 'Pendiente' WHERE NOT EXISTS (SELECT 1 FROM Citas WHERE Id = 2);
                
                INSERT INTO Citas (Id, PacienteId, MedicoId, Fecha, Hora, Motivo, Estado)
                SELECT 3, 1, 3, '2026-07-22', '14:00:00', 'Prueba del ciclo de cajas de recuerdos', 'Confirmada' WHERE NOT EXISTS (SELECT 1 FROM Citas WHERE Id = 3);
                
                INSERT INTO Citas (Id, PacienteId, MedicoId, Fecha, Hora, Motivo, Estado)
                SELECT 4, 3, 2, '2026-07-23', '11:15:00', 'Gripe severa y fiebre', 'Pendiente' WHERE NOT EXISTS (SELECT 1 FROM Citas WHERE Id = 4);
                
                INSERT INTO Citas (Id, PacienteId, MedicoId, Fecha, Hora, Motivo, Estado)
                SELECT 5, 3, 3, '2026-07-24', '16:30:00', 'Examen de reflejos neurológicos', 'Completada' WHERE NOT EXISTS (SELECT 1 FROM Citas WHERE Id = 5);
            ";
            cmd.ExecuteNonQuery();
        }

        private static Cita LeerFila(SqliteDataReader r) => new Cita
        {
            Id = r.GetInt32(0),
            PacienteId = r.GetInt32(1),
            MedicoId = r.GetInt32(2),
            Fecha = DateOnly.Parse(r.GetString(3)),
            Hora = TimeOnly.Parse(r.GetString(4)),
            Motivo = r.IsDBNull(5) ? string.Empty : r.GetString(5),
            Estado = r.IsDBNull(6) ? string.Empty : r.GetString(6)
        };

        public List<Cita> ObtenerTodos()
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, PacienteId, MedicoId, Fecha, Hora, Motivo, Estado FROM Citas;";

            var lista = new List<Cita>();
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                lista.Add(LeerFila(r));
            }
            return lista;
        }

        public List<Cita> ObtenerPorPaciente(int pacienteId)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, PacienteId, MedicoId, Fecha, Hora, Motivo, Estado FROM Citas WHERE PacienteId = $pacienteId;";
            cmd.Parameters.AddWithValue("$pacienteId", pacienteId);

            var lista = new List<Cita>();
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                lista.Add(LeerFila(r));
            }
            return lista;
        }

        public void Update(Cita cita)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                UPDATE Citas 
                SET PacienteId = $pacienteId, 
                    MedicoId = $medicoId, 
                    Fecha = $fecha, 
                    Hora = $hora, 
                    Motivo = $motivo, 
                    Estado = $estado 
                WHERE Id = $id;";

            cmd.Parameters.AddWithValue("$id", cita.Id);
            cmd.Parameters.AddWithValue("$pacienteId", cita.PacienteId);
            cmd.Parameters.AddWithValue("$medicoId", cita.MedicoId);
            cmd.Parameters.AddWithValue("$fecha", cita.Fecha.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("$hora", cita.Hora.ToString("HH:mm:ss"));
            cmd.Parameters.AddWithValue("$motivo", cita.Motivo ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$estado", cita.Estado ?? "Pendiente");

            cmd.ExecuteNonQuery();
        }

        public IEnumerable<Cita> GetAll() => ObtenerTodos();

        public Cita GetById(int id)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, PacienteId, MedicoId, Fecha, Hora, Motivo, Estado FROM Citas WHERE Id = $id;";
            cmd.Parameters.AddWithValue("$id", id);

            using var r = cmd.ExecuteReader();
            return r.Read() ? LeerFila(r) : null;
        }

        public void Add(Cita cita)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Citas (PacienteId, MedicoId, Fecha, Hora, Motivo, Estado)
                VALUES ($pacienteId, $medicoId, $fecha, $hora, $motivo, $estado);
                SELECT last_insert_rowid();";

            cmd.Parameters.AddWithValue("$pacienteId", cita.PacienteId);
            cmd.Parameters.AddWithValue("$medicoId", cita.MedicoId);
            cmd.Parameters.AddWithValue("$fecha", cita.Fecha.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("$hora", cita.Hora.ToString("HH:mm:ss"));
            cmd.Parameters.AddWithValue("$motivo", cita.Motivo ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$estado", cita.Estado ?? "Pendiente");

            var idGenerado = cmd.ExecuteScalar();
            if (idGenerado != null)
            {
                cita.Id = Convert.ToInt32(idGenerado);
            }
        }
    }
}