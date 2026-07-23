using CitasApp.Domain.Models;

namespace CitasApp.Domain.Factories; // O el namespace que utilices en tu proyecto Domain

public class CitaFactory
{
    public Cita Construir(int pacienteId, int medicoId, DateOnly fecha, TimeOnly hora, string motivo)
    {
        return new Cita
        {
            PacienteId = pacienteId,
            MedicoId = medicoId,
            Fecha = fecha,
            // Si tu modelo Cita también tiene una propiedad de Hora, descomenta la siguiente línea:
            // Hora = hora, 
            Motivo = motivo,
            Estado = "Pendiente"
        };
    }
}