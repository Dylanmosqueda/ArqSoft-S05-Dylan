using System.Security.Claims;
using CitasApp.Application.Services;
using CitasApp.Domain.Interfaces;
using CitasApp.Domain.Models;
using CitasApp.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace CitasApp.Tests.Controllers
{
    public class CitaRepositoryFake : ICitaRepository
    {
        private readonly List<Cita> _citas;

        public CitaRepositoryFake(List<Cita> citas) => _citas = citas;

        public List<Cita> ObtenerTodos() => _citas;

        public List<Cita> ObtenerPorPaciente(int pacienteId)
            => _citas.Where(c => c.PacienteId == pacienteId).ToList();

        // === Agrega estos tres métodos ===
        public void Add(Cita cita) => _citas.Add(cita);

        public void Update(Cita cita)
        {
            var index = _citas.FindIndex(c => c.Id == cita.Id);
            if (index != -1)
            {
                _citas[index] = cita;
            }
        }

        public Cita? GetById(int id) => _citas.FirstOrDefault(c => c.Id == id);
    }

    public class PacienteRepositoryFake : IPacienteRepository
    {
        private readonly List<Paciente> _pacientes;

        public PacienteRepositoryFake(List<Paciente> pacientes) => _pacientes = pacientes;

        // === Cambia 'List<Paciente>' por 'IEnumerable<Paciente>' ===
        public IEnumerable<Paciente> GetAll() => _pacientes;

        public Paciente? GetById(int id) => _pacientes.FirstOrDefault(p => p.Id == id);

        public void Add(Paciente paciente) => _pacientes.Add(paciente);

        public List<Paciente> ObtenerTodos() => _pacientes;
        public Paciente? ObtenerPorId(int id) => _pacientes.FirstOrDefault(p => p.Id == id);
    }

    public class MedicoRepositoryFake : IMedicoRepository
    {
        private readonly List<Medico> _medicos;

        public MedicoRepositoryFake(List<Medico> medicos) => _medicos = medicos;

        // === Cambia 'List<Medico>' por 'IEnumerable<Medico>' ===
        public IEnumerable<Medico> GetAll() => _medicos;

        public Medico? GetById(int id) => _medicos.FirstOrDefault(m => m.Id == id);

        public void Add(Medico medico) => _medicos.Add(medico);

        public List<Medico> ObtenerTodos() => _medicos;
        public Medico? ObtenerPorId(int id) => _medicos.FirstOrDefault(m => m.Id == id);
    }

    // ---------------------------------------------------------------------
    // Pruebas — solo el camino del administrador
    // ---------------------------------------------------------------------
    public class CitaControllerAdminTests
    {
        private CitaController CrearControllerConDatosDePrueba(out List<Cita> citasEsperadas)
        {
            // Arrange — datos de prueba en memoria
            citasEsperadas = new List<Cita>
            {
                new Cita { Id = 1, PacienteId = 10, Estado = "Pendiente" },
                new Cita { Id = 2, PacienteId = 20, Estado = "Confirmada" },
                new Cita { Id = 3, PacienteId = 10, Estado = "Pendiente" }
            };

            var pacientes = new List<Paciente>
            {
                new Paciente { Id = 10, Email = "paciente1@correo.com" },
                new Paciente { Id = 20, Email = "paciente2@correo.com" }
            };

            var medicos = new List<Medico>
            {
                new Medico { Id = 1, Nombre = "Dr. Pérez" }
            };

            // Servicios reales, con repositorios fake en vez de reales (JSON/SQL)
            var citaService = new CitaService(new CitaRepositoryFake(citasEsperadas));
            var pacienteService = new PacienteService(new PacienteRepositoryFake(pacientes));
            var medicoService = new MedicoService(new MedicoRepositoryFake(medicos));

            var controller = new CitaController(citaService, pacienteService, medicoService);

            // Simular usuario admin logueado
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, "jorge@admin.com") };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };

            return controller;
        }

        [Fact]
        public void Index_ConCuentaAdmin_RegresaTodasLasCitasSinFiltrar()
        {
            // Arrange
            var controller = CrearControllerConDatosDePrueba(out var citasEsperadas);

            // Act
            var resultado = controller.Index() as ViewResult;
            var modelo = resultado?.Model as List<Cita>;

            // Assert            
            Assert.NotNull(modelo);
            Assert.Equal(citasEsperadas.Count, modelo.Count);
            Assert.Equal(citasEsperadas, modelo);
        }

        [Fact]
        public void Index_ConCuentaAdmin_IncluyeCitasDeMasDeUnPaciente()
        {
            // Arrange
            var controller = CrearControllerConDatosDePrueba(out _);

            // Act
            var resultado = controller.Index() as ViewResult;
            var modelo = resultado?.Model as List<Cita>;

            // Assert — el admin debe ver citas de distintos pacientes, no solo uno            
            Assert.NotNull(modelo);
            var pacientesDistintos = modelo.Select(c => c.PacienteId).Distinct().Count();
            Assert.True(pacientesDistintos > 1);
        }

        [Fact]
        public void Index_ConCuentaAdmin_CargaCatalogosDePacientesYMedicosEnViewBag()
        {
            // Arrange
            var controller = CrearControllerConDatosDePrueba(out _);

            // Act
            controller.Index();

            // Assert            
            Assert.NotNull(controller.ViewBag.Pacientes);
            Assert.NotNull(controller.ViewBag.Medicos);
        }
    }
}