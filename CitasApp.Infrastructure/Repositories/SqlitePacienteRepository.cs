using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;

namespace CitasApp.Infrastructure.Repositories
{
    public class SqlitePacienteRepository : IPacienteRepository
    {
        private readonly string _connectionString;

        public SqlitePacienteRepository(string dbPath)
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
                CREATE TABLE IF NOT EXISTS Pacientes (
                    Id       INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nombre   TEXT NOT NULL,
                    Apellido TEXT NOT NULL,
                    Email    TEXT,
                    Telefono TEXT
                );

                -- Insertar Pacientes de prueba (Saw Theme) si la tabla está vacía
                INSERT INTO Pacientes (Id, Nombre, Apellido, Email, Telefono)
                SELECT 1, 'Amanda', 'Young', 'amanda@saw.com', '555-0606' WHERE NOT EXISTS (SELECT 1 FROM Pacientes WHERE Id = 1);
                
                INSERT INTO Pacientes (Id, Nombre, Apellido, Email, Telefono)
                SELECT 2, 'Adam', 'Stanheight', 'adam@saw.com', '555-0707' WHERE NOT EXISTS (SELECT 1 FROM Pacientes WHERE Id = 2);
                
                INSERT INTO Pacientes (Id, Nombre, Apellido, Email, Telefono)
                SELECT 3, 'Jeff', 'Denlon', 'jeff@saw.com', '555-0808' WHERE NOT EXISTS (SELECT 1 FROM Pacientes WHERE Id = 3);
            ";
            cmd.ExecuteNonQuery();
        }

        private static Paciente LeerFila(SqliteDataReader r) => new Paciente
        {
            Id = r.GetInt32(0),
            Nombre = r.GetString(1),
            Apellido = r.GetString(2),
            Email = r.IsDBNull(3) ? string.Empty : r.GetString(3),
            Telefono = r.IsDBNull(4) ? string.Empty : r.GetString(4)
        };

        public List<Paciente> ObtenerTodos()
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Nombre, Apellido, Email, Telefono FROM Pacientes;";

            var lista = new List<Paciente>();
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                lista.Add(LeerFila(r));
            }
            return lista;
        }

        public Paciente? ObtenerPorId(int id)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Nombre, Apellido, Email, Telefono FROM Pacientes WHERE Id = $id;";
            cmd.Parameters.AddWithValue("$id", id);

            using var r = cmd.ExecuteReader();
            return r.Read() ? LeerFila(r) : null;
        }

        public IEnumerable<Paciente> GetAll()
        {
            return ObtenerTodos();
        }

        public Paciente GetById(int id)
        {
            return ObtenerPorId(id);
        }

        public void Add(Paciente paciente)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Pacientes (Nombre, Apellido, Email, Telefono)
                VALUES ($nombre, $apellido, $email, $telefono);
                SELECT last_insert_rowid();";

            cmd.Parameters.AddWithValue("$nombre", paciente.Nombre);
            cmd.Parameters.AddWithValue("$apellido", paciente.Apellido ?? string.Empty);
            cmd.Parameters.AddWithValue("$email", paciente.Email ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("$telefono", paciente.Telefono ?? (object)DBNull.Value);

            var idGenerado = cmd.ExecuteScalar();
            if (idGenerado != null)
            {
                paciente.Id = Convert.ToInt32(idGenerado);
            }
        }
    }
}