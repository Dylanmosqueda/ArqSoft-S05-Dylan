using CitasApp.Application.Interfaces;
using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;

namespace CitasApp.Application.Services;

public class PacienteService : IPacienteService
{
    private readonly IPacienteRepository _repo;

    public PacienteService(IPacienteRepository repo) => _repo = repo;

    // Métodos en inglés (ya estaban implementados)
    public List<Paciente> GetAll() => _repo.GetAll().ToList();
    public Paciente? GetById(int id) => _repo.GetById(id);
    public void Add(Paciente paciente) => _repo.Add(paciente);

    // Métodos en español (reutilizan la lógica de tu repositorio)
    public List<Paciente> ObtenerTodos() => _repo.GetAll().ToList();
    public Paciente? ObtenerPorId(int id) => _repo.GetById(id);
}