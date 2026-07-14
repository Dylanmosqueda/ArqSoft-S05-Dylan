using CitasApp.Application.Interfaces;
using CitasApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CitasApp.Controllers;

public class CitaController : Controller
{
    private readonly ICitaService _citaService;
    private readonly IPacienteService _pacienteService;
    private readonly IMedicoService _medicoService;

    public CitaController(ICitaService citaService, IPacienteService pacienteService, IMedicoService medicoService)
    {
        _citaService = citaService;
        _pacienteService = pacienteService;
        _medicoService = medicoService;
    }

    public IActionResult Index()
    {
        ViewBag.Pacientes = _pacienteService.GetAll() ?? new List<Paciente>();
        ViewBag.Medicos = _medicoService.GetAll() ?? new List<Medico>();

        var citasDto = _citaService.GetAll() ?? new List<CitasApp.Application.DTOs.CitaViewModel>();

        // Corregido: Usamos llaves {} para la inicialización y DateOnly.Parse para la fecha
        var citas = citasDto.Select(vm => new Cita
        {
            Id = vm.Id,
            PacienteId = vm.PacienteId,
            MedicoId = vm.MedicoId,
            Fecha = DateOnly.Parse(vm.Fecha),
            Hora = vm.Hora,
            Motivo = vm.Motivo,
            Estado = vm.Estado
        }).ToList();

        return View(citas);
    }

    public IActionResult Cita()
    {
        ViewBag.Pacientes = _pacienteService.GetAll() ?? new List<Paciente>();
        ViewBag.Medicos = _medicoService.GetAll() ?? new List<Medico>();

        var viewModel = _citaService.GetAll() ?? new List<CitasApp.Application.DTOs.CitaViewModel>();
        return View(viewModel);
    }

    public IActionResult Detalle(int id)
    {
        var viewModel = _citaService.GetById(id, _citaService.GetCita1());
        if (viewModel == null) return Content("Cita no encontrada");

        // Mapeamos el ViewModel de vuelta a la entidad Cita para cumplir con el @model de Detalle.cshtml
        var cita = new Cita
        {
            Id = viewModel.Id,
            PacienteId = viewModel.PacienteId,
            MedicoId = viewModel.MedicoId,
            Fecha = DateOnly.Parse(viewModel.Fecha), // Convertimos la fecha de string a DateOnly
            Hora = viewModel.Hora,                   // Asignación directa de TimeOnly
            Motivo = viewModel.Motivo,
            Estado = viewModel.Estado
        };

        return View(cita);
    }

    public IActionResult Nuevo()
    {
        ViewBag.Pacientes = _pacienteService.GetAll() ?? new List<Paciente>();
        ViewBag.Medicos = _medicoService.GetAll() ?? new List<Medico>();
        return View();
    }

    [HttpPost]
    public IActionResult Nuevo(Cita cita)
    {
        _citaService.Add(cita);
        return RedirectToAction("Cita");
    }
}
