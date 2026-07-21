using CitasApp.Domain.Models;

namespace CitasApp.Domain.Interfaces
{
    public interface IMedicoRepository
    {
        List<Medico> ObtenerTodos();
        Medico? ObtenerPorId(int id);
        IEnumerable<Medico> GetAll();
        Medico GetById(int id);
        void Add(Medico medico);
    }
}