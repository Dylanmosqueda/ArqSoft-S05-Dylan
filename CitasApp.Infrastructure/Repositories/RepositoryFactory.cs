using System.IO; // <--- Necesario para usar Path.Combine
using CitasApp.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace CitasApp.Infrastructure.Repositories
{
    public static class RepositoryFactory
    {
        public static IPacienteRepository CrearPacienteRepository(
            string entorno, IWebHostEnvironment env)
        {
            return entorno switch
            {
                "Production" => new MemoriaPacienteRepository(),
                // Se combina la ruta del proyecto con la carpeta App_Data y el archivo JSON
                _ => new JsonPacienteRepository(Path.Combine(env.ContentRootPath, "App_Data", "pacientes.json"))
            };
        }

        public static IMedicoRepository CrearMedicoRepository(
            string entorno, IWebHostEnvironment env)
        {
            // Se combina la ruta para médicos
            var rutaMedicos = Path.Combine(env.ContentRootPath, "App_Data", "medicos.json");

            return entorno switch
            {
                "Production" => new JsonMedicoRepository(rutaMedicos),
                _ => new JsonMedicoRepository(rutaMedicos)
            };
        }

        public static ICitaRepository CrearCitaRepository(
            string entorno, IWebHostEnvironment env)
        {
            // Se combina la ruta para citas
            var rutaCitas = Path.Combine(env.ContentRootPath, "App_Data", "cita.json");

            return entorno switch
            {
                "Production" => new JsonCitaRepository(rutaCitas),
                _ => new JsonCitaRepository(rutaCitas)
            };
        }
    }
}
