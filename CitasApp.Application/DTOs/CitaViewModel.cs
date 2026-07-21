using System;
using System.Collections.Generic;
using System.Text;
using CitasApp.Domain.Models;

namespace CitasApp.Application.DTOs
{
    public class CitaViewModel
    {
        public int Id { get; set; }
        public string NombrePaciente { get; set; }
        public string NombreMedico { get; set; }
        public string Fecha { get; set; }
        public string FechaHora { get; set; }
        public string Motivo { get; set; }
        public string Estado { get; set; }
        public TimeOnly Hora { get; internal set; }
        public int PacienteId { get; set; }
        public int MedicoId { get; set; }

        public static implicit operator CitaViewModel(Cita cita)
        {
            throw new NotImplementedException();
        }
    }
}
