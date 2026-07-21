using CitasApp.Application.Interfaces;
using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;
using System.Linq; // Asegúrate de tener este using para usar .ToList()

namespace CitasApp.Application.Services;

public class MedicoService : IMedicoService
{
    private readonly IMedicoRepository _repo;

    public MedicoService(IMedicoRepository repo) => _repo = repo;

    // Métodos en inglés
    // Es más seguro usar .ToList() para evitar excepciones de conversión (cast)
    public List<Medico> GetAll() => _repo.GetAll().ToList();
    public Medico? GetById(int id) => _repo.GetById(id);
    public void Add(Medico medico) => _repo.Add(medico);

    // Métodos en español (implementados llamando al repositorio)
    public List<Medico> ObtenerTodos() => _repo.GetAll().ToList();
    public Medico? ObtenerPorId(int id) => _repo.GetById(id);
}