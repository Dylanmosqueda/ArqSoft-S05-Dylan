using CitasApp.Domain.Models;
using System.Collections.Generic;

namespace CitasApp.Domain.Interfaces
{
    public interface ICitaRepository
    {
        List<Cita> ObtenerTodos();
        List<Cita> ObtenerPorPaciente(int pacienteId);
        void Update(Cita cita);
        void Add(Cita cita);
        Cita GetById(int id);
    }

}