using CitasApp.Domain.Models;

namespace CitasApp.Domain.Factories;

public class CitaFactory
{
    /// <summary>
    /// Método de fábrica para crear una nueva instancia de Cita con valores predeterminados o válidos.
    /// </summary>
    public Cita Crear(int pacienteId, int medicoId, DateOnly fecha, string motivo, string estado = "Pendiente")
    {
        return new Cita
        {
            PacienteId = pacienteId,
            MedicoId = medicoId,
            Fecha = fecha,
            Motivo = motivo,
            Estado = estado
        };
    }
}