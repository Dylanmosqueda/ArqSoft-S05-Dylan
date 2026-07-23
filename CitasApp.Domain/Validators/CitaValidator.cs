using System.Collections.Generic;
using CitasApp.Domain.Models;

namespace CitasApp.Domain.Validators;

public class CitaValidator
{
    /// <summary>
    /// Valida si una cita cumple con las reglas de negocio principales.
    /// </summary>
    public bool EsValida(Cita cita)
    {
        return !ObtenerErrores(cita).Any();
    }

    /// <summary>
    /// Retorna una lista con los mensajes de error si la cita no cumple las reglas.
    /// </summary>
    public IEnumerable<string> ObtenerErrores(Cita cita)
    {
        if (cita == null)
        {
            yield return "La cita no puede ser nula.";
            yield break;
        }

        if (cita.PacienteId <= 0)
        {
            yield return "El PacienteId es obligatorio y debe ser mayor a 0.";
        }

        if (cita.MedicoId <= 0)
        {
            yield return "El MedicoId es obligatorio y debe ser mayor a 0.";
        }

        // Si usas DateOnly, evaluamos que no sea una fecha por defecto (0001-01-01)
        if (cita.Fecha == default)
        {
            yield return "La fecha de la cita es obligatoria.";
        }

        if (string.IsNullOrWhiteSpace(cita.Motivo))
        {
            yield return "El motivo de la cita es obligatorio.";
        }
    }
}

