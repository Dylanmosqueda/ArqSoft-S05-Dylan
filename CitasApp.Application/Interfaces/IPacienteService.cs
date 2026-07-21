using CitasApp.Domain.Models;

namespace CitasApp.Application.Interfaces;

public interface IPacienteService
{
    List<Paciente> GetAll();
    Paciente? GetById(int id);
    void Add(Paciente paciente);
}