using System;
using System.Collections.Generic;
using System.Linq;
using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;

namespace CitasApp.Infrastructure.Repositories
{
    // DECORATOR — agrega logging sin modificar el repositorio original
    public class LoggingPacienteRepository : IPacienteRepository
    {
        private readonly IPacienteRepository _inner;

        public LoggingPacienteRepository(IPacienteRepository inner)
        {
            _inner = inner;
        }

        // --- MÉTODOS EN INGLÉS ---

        public IEnumerable<Paciente> GetAll()
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] GetAll — inicio");

            var resultado = _inner.GetAll();

            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] GetAll — {resultado.Count()} registros");

            return resultado;
        }

        public Paciente? GetById(int id)
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] GetById({id}) — inicio");

            var resultado = _inner.GetById(id);

            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] GetById({id}) — {(resultado != null ? "encontrado" : "no encontrado")}");

            return resultado;
        }

        public void Add(Paciente paciente)
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Add — inicio");

            _inner.Add(paciente);

            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Add — completado");
        }

        // --- MÉTODOS EN ESPAÑOL ---

        public List<Paciente> ObtenerTodos()
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ObtenerTodos — inicio");

            var resultado = _inner.ObtenerTodos();

            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ObtenerTodos — {resultado.Count} registros");

            return resultado;
        }

        public Paciente? ObtenerPorId(int id)
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ObtenerPorId({id}) — inicio");

            var resultado = _inner.ObtenerPorId(id);

            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ObtenerPorId({id}) — {(resultado != null ? "encontrado" : "no encontrado")}");

            return resultado;
        }
    }
}
