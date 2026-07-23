using CitasApp.Domain.Models;

namespace CitasApp.Domain.Factories;
public class CitaFactory
{
    public Cita Construir(int pacienteId, int medicoId, DateOnly fecha, TimeOnly hora, string motivo)
    {
        return new Cita
        {
            PacienteId = pacienteId,
            MedicoId = medicoId,
            Fecha = fecha,
            Motivo = motivo,
            Estado = "Pendiente"
        };
    }
}