using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;

namespace CitasApp.Infrastructure.Repositories
{
    public class SqliteMedicoRepository : IMedicoRepository
    {
        private readonly string _connectionString;

        public SqliteMedicoRepository(string dbPath)
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
                CREATE TABLE IF NOT EXISTS Medicos (
                    Id           INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nombre       TEXT NOT NULL,
                    Especialidad TEXT NOT NULL
                );

                -- Insertar Médicos de prueba (Saw Theme) si la tabla está vacía
                INSERT INTO Medicos (Id, Nombre, Especialidad)
                SELECT 1, 'Dr. Lawrence Gordon', 'Cirugía General' WHERE NOT EXISTS (SELECT 1 FROM Medicos WHERE Id = 1);
                
                INSERT INTO Medicos (Id, Nombre, Especialidad)
                SELECT 2, 'Dr. John Kramer', 'Terapia de Vida' WHERE NOT EXISTS (SELECT 1 FROM Medicos WHERE Id = 2);
                
                INSERT INTO Medicos (Id, Nombre, Especialidad)
                SELECT 3, 'Dr. Logan Nelson', 'Patología Forense' WHERE NOT EXISTS (SELECT 1 FROM Medicos WHERE Id = 3);
            ";
            cmd.ExecuteNonQuery();
        }

        private static Medico LeerFila(SqliteDataReader r) => new Medico
        {
            Id = r.GetInt32(0),
            Nombre = r.GetString(1),
            Especialidad = r.GetString(2)
        };

        public List<Medico> ObtenerTodos()
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Nombre, Especialidad FROM Medicos;";

            var lista = new List<Medico>();
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                lista.Add(LeerFila(r));
            }
            return lista;
        }

        public Medico? ObtenerPorId(int id)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Nombre, Especialidad FROM Medicos WHERE Id = $id;";
            cmd.Parameters.AddWithValue("$id", id);

            using var r = cmd.ExecuteReader();
            return r.Read() ? LeerFila(r) : null;
        }

        public IEnumerable<Medico> GetAll() => ObtenerTodos();
        public Medico GetById(int id) => ObtenerPorId(id);

        public void Add(Medico medico)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Medicos (Nombre, Especialidad)
                VALUES ($nombre, $especialidad);
                SELECT last_insert_rowid();";

            cmd.Parameters.AddWithValue("$nombre", medico.Nombre);
            cmd.Parameters.AddWithValue("$especialidad", medico.Especialidad ?? string.Empty);

            var idGenerado = cmd.ExecuteScalar();
            if (idGenerado != null)
            {
                medico.Id = Convert.ToInt32(idGenerado);
            }
        }
    }
}