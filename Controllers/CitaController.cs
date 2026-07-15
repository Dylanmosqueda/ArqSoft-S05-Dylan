using CitasApp.Application.Interfaces;
using CitasApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CitasApp.Controllers
{
    [Authorize]
    public class CitaController : Controller
    {
        private readonly ICitaService _citaService;
        private readonly IPacienteService _pacienteService;
        private readonly IMedicoService _medicoService;

        public CitaController(ICitaService citaService,
                              IPacienteService pacienteService,
                              IMedicoService medicoService)
        {
            _citaService = citaService;
            _pacienteService = pacienteService;
            _medicoService = medicoService;
        }

        public IActionResult Index()
        {
            CargarListasEnViewBag();

            var citasDto = _citaService.GetAll() ?? new List<CitasApp.Application.DTOs.CitaViewModel>();

            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userIdStr = User.FindFirst("UsuarioId")?.Value;

            if (int.TryParse(userIdStr, out int userId))
            {
                if (userRole == "Paciente")
                {
                    citasDto = citasDto.Where(c => c.PacienteId == userId).ToList();
                }
                else if (userRole == "Medico")
                {
                    citasDto = citasDto.Where(c => c.MedicoId == userId).ToList();
                }
            }

            var citas = citasDto.Select(MapToCita).ToList();
            return View(citas);
        }

        public IActionResult Cita()
        {
            CargarListasEnViewBag();

            var viewModel = _citaService.GetAll() ?? new List<CitasApp.Application.DTOs.CitaViewModel>();

            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userIdStr = User.FindFirst("UsuarioId")?.Value;

            if (int.TryParse(userIdStr, out int userId))
            {
                if (userRole == "Paciente")
                {
                    viewModel = viewModel.Where(c => c.PacienteId == userId).ToList();
                }
                else if (userRole == "Medico")
                {
                    viewModel = viewModel.Where(c => c.MedicoId == userId).ToList();
                }
            }

            return View(viewModel);
        }

        public IActionResult Detalle(int id)
        {
            var viewModel = _citaService.GetById(id, _citaService.GetCita1());
            if (viewModel == null) return Content("Cita no encontrada");

            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userIdStr = User.FindFirst("UsuarioId")?.Value;

            if (int.TryParse(userIdStr, out int userId))
            {
                if (userRole == "Paciente" && viewModel.PacienteId != userId)
                    return Content("No tienes permiso para ver esta cita.");

                if (userRole == "Medico" && viewModel.MedicoId != userId)
                    return Content("No tienes permiso para ver esta cita.");
            }

            var cita = MapToCita(viewModel);
            return View(cita);
        }

        public IActionResult Nuevo()
        {
            CargarListasEnViewBag();
            return View();
        }

        [HttpPost]
        public IActionResult Nuevo(Cita cita)
        {
            _citaService.Add(cita);
            return RedirectToAction("Cita");
        }

        // =========================================================================
        // MÉTODOS REFACTORIZADOS (Extract Method)
        // =========================================================================

        private Cita MapToCita(CitasApp.Application.DTOs.CitaViewModel viewModel)
        {
            return new Cita
            {
                Id = viewModel.Id,
                PacienteId = viewModel.PacienteId,
                MedicoId = viewModel.MedicoId,
                Fecha = DateOnly.Parse(viewModel.Fecha),
                Hora = viewModel.Hora,
                Motivo = viewModel.Motivo,
                Estado = viewModel.Estado
            };
        }

        /// <summary>
        /// Filtra las listas del ViewBag según el rol del usuario logueado en SQLite.
        /// </summary>
        private void CargarListasEnViewBag()
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userIdStr = User.FindFirst("UsuarioId")?.Value;

            // REFACTOR: Declaramos la variable 'userId' una sola vez al inicio del método
            int.TryParse(userIdStr, out int userId);

            if (userRole == "Paciente" && userId > 0)
            {
                ViewBag.Pacientes = _pacienteService.GetAll()?.Where(p => p.Id == userId).ToList() ?? new List<Paciente>();
                ViewBag.Medicos = _medicoService.GetAll() ?? new List<Medico>();
            }
            else if (userRole == "Medico" && userId > 0)
            {
                ViewBag.Pacientes = _pacienteService.GetAll() ?? new List<Paciente>();
                ViewBag.Medicos = _medicoService.GetAll()?.Where(m => m.Id == userId).ToList() ?? new List<Medico>();
            }
            else
            {
                ViewBag.Pacientes = _pacienteService.GetAll() ?? new List<Paciente>();
                ViewBag.Medicos = _medicoService.GetAll() ?? new List<Medico>();
            }
        }
    }
}