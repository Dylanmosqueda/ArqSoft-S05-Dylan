using System;
using System.Collections.Generic;
using System.Linq;
using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;

namespace CitasApp.Infrastructure.Repositories
{
    public class MemoriaPacienteRepository : IPacienteRepository
    {
        // 'static' permite que la lista persista en memoria mientras la API esté corriendo
        private static readonly List<Paciente> _pacientes = new List<Paciente>();

        // --- IMPLEMENTACIÓN DE MÉTODOS EN INGLÉS ---

        public IEnumerable<Paciente> GetAll()
        {
            return _pacientes;
        }

        public Paciente? GetById(int id)
        {
            // Busca al paciente por su Id en la lista
            return _pacientes.FirstOrDefault(p => p.Id == id);
        }

        public void Add(Paciente paciente)
        {
            // Agrega el nuevo paciente a la lista
            _pacientes.Add(paciente);
        }

        // --- IMPLEMENTACIÓN DE MÉTODOS EN ESPAÑOL ---

        public List<Paciente> ObtenerTodos()
        {
            return _pacientes;
        }

        public Paciente? ObtenerPorId(int id)
        {
            return _pacientes.FirstOrDefault(p => p.Id == id);
        }
    }
}
