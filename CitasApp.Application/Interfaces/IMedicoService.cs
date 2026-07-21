using CitasApp.Domain.Models;

namespace CitasApp.Application.Interfaces;

public interface IMedicoService
{
    List<Medico> GetAll();
    Medico? GetById(int id);
    void Add(Medico medico);
}