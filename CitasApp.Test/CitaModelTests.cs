using System.ComponentModel.DataAnnotations;
using CitasApp.Domain.Models;
using Xunit;

namespace CitasApp.Tests;

public class CitaModelTests
{
    private static List<ValidationResult> Validar(Cita cita)
    {
        var ctx = new ValidationContext(cita);
        var resultados = new List<ValidationResult>();
        Validator.TryValidateObject(cita, ctx, resultados, validateAllProperties: true);
        return resultados;
    }

    [Fact]
    public void Cita_ConTodosLosCampos_NoTieneErroresDeValidacion()
    {
        // Arrange
        var cita = new Cita
        {
            PacienteId = 1,
            MedicoId = 2,
            Fecha = DateOnly.Parse("2026-07-21"), // <-- Corregido aquí
            Motivo = "Consulta",
            Estado = "Pendiente"
        };

        // Act
        var errores = Validar(cita);

        // Assert
        Assert.Empty(errores);
    }

    [Fact]
    public void Cita_SinFecha_TieneErrorDeValidacion()
    {
        // Arrange
        var cita = new Cita
        {
            PacienteId = 1,
            MedicoId = 2,
            Fecha = default, // <-- Corregido aquí (un DateOnly vacío/por defecto)
            Motivo = "Consulta"
        };

        // Act
        var errores = Validar(cita);

        // Assert
        Assert.Contains(errores, e => e.MemberNames.Contains("Fecha"));
    }

    [Fact]
    public void Cita_ConPacienteIdCero_TieneErrorDeRango()
    {
        // Arrange
        var cita = new Cita
        {
            PacienteId = 0,
            MedicoId = 1,
            Fecha = DateOnly.Parse("2026-07-21"), // <-- Corregido aquí
            Motivo = "Revisión"
        };

        // Act
        var errores = Validar(cita);

        // Assert
        Assert.Contains(errores, e => e.MemberNames.Contains("PacienteId"));
    }
}