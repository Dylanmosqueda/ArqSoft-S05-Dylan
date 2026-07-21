using CitasApp.Domain.Models;

namespace CitasApp.Domain.Interfaces
{
    public interface IPacienteRepository
    {
        List<Paciente> ObtenerTodos();
        Paciente? ObtenerPorId(int id);
        IEnumerable<Paciente> GetAll();
        Paciente GetById(int id);
        void Add(Paciente paciente);
    }
}