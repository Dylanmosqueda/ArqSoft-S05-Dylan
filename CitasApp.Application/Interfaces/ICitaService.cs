using CitasApp.Domain.Models;
using CitasApp.Application.DTOs;
using System.Collections.Generic;

namespace CitasApp.Application.Interfaces
{
    public interface ICitaService
    {
        List<CitaViewModel> GetAll();
        List<CitaViewModel> ObtenerTodos();
        List<CitaViewModel> ObtenerPorPaciente(int pacienteId);
        CitaViewModel GetById(int id, CitaViewModel fallback);
        CitaViewModel GetCita1();
        void Add(Cita cita);
    }
}