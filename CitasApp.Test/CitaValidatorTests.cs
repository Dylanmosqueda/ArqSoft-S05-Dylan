using CitasApp.Domain.Models;
using CitasApp.Domain.Validators;
using Xunit;

namespace CitasApp.Tests;

public class CitaValidatorTests
{
    private readonly CitaValidator _validator = new();

    [Fact]
    public void EsValida_ConCitaCompleta_RetornaTrue()
    {
        // Arrange
        var cita = new Cita
        {
            PacienteId = 1,
            MedicoId = 2,
            Fecha = DateOnly.Parse("2026-07-21"), // <-- Corregido aquí
            Motivo = "Consulta general",
            Estado = "Pendiente"
        };

        // Act
        var resultado = _validator.EsValida(cita);

        // Assert
        Assert.True(resultado);
    }

    [Fact]
    public void EsValida_SinMedico_RetornaFalse()
    {
        // Arrange
        var cita = new Cita
        {
            PacienteId = 1,
            MedicoId = 0,
            Fecha = DateOnly.Parse("2026-07-21"), // <-- Corregido aquí
            Motivo = "Revisión"
        };

        // Act
        var resultado = _validator.EsValida(cita);

        // Assert
        Assert.False(resultado);
    }

    [Fact]
    public void EsValida_SinMotivo_RetornaFalse()
    {
        // Arrange
        var cita = new Cita
        {
            PacienteId = 1,
            MedicoId = 2,
            Fecha = DateOnly.Parse("2026-07-21"), // <-- Corregido aquí
            Motivo = ""
        };

        // Act
        var resultado = _validator.EsValida(cita);

        // Assert
        Assert.False(resultado);
    }

    [Fact]
    public void ObtenerErrores_CitaVacia_RetornaCuatroErrores()
    {
        // Arrange
        var cita = new Cita();

        // Act
        var errores = _validator.ObtenerErrores(cita).ToList();

        // Assert
        Assert.Equal(4, errores.Count);
    }
}