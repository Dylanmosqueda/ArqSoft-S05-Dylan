using CitasApp.Application.Interfaces;
using CitasApp.Application.DTOs;
using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CitasApp.Application.Services
{
    public class CitaService : ICitaService
    {
        private readonly ICitaRepository _citaRepository;

        public CitaService(ICitaRepository citaRepository)
        {
            _citaRepository = citaRepository;
        }

        public List<CitaViewModel> GetAll()
        {
            return ObtenerTodos();
        }

        public List<CitaViewModel> ObtenerTodos()
        {
            var citas = _citaRepository.ObtenerTodos() ?? new List<Cita>();
            return citas.Select(MapToViewModel).ToList();
        }

        public List<CitaViewModel> ObtenerPorPaciente(int pacienteId)
        {
            var citas = _citaRepository.ObtenerPorPaciente(pacienteId) ?? new List<Cita>();
            return citas.Select(MapToViewModel).ToList();
        }

        public CitaViewModel GetById(int id, CitaViewModel fallback)
        {
            var citas = _citaRepository.ObtenerTodos();
            var cita = citas?.FirstOrDefault(c => c.Id == id);
            if (cita == null) return fallback;
            return MapToViewModel(cita);
        }

        public CitaViewModel GetCita1()
        {
            return new CitaViewModel();
        }

        public void Add(Cita cita)
        {
            _citaRepository.Add(cita);
        }

        private CitaViewModel MapToViewModel(Cita cita)
        {
            return new CitaViewModel
            {
                Id = cita.Id,
                PacienteId = cita.PacienteId,
                MedicoId = cita.MedicoId,
                Fecha = cita.Fecha.ToString("yyyy-MM-dd"),
                Hora = cita.Hora,
                Motivo = cita.Motivo,
                Estado = cita.Estado
            };
        }
    }
}