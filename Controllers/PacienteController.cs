using CitasApp.Application.DTOs;
using CitasApp.Application.Interfaces;
using CitasApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CitasApp.Controllers;

public class PacienteController : Controller
{
    private readonly IPacienteService _pacienteService;

    public PacienteController(IPacienteService pacienteService)
    {
        _pacienteService = pacienteService;
    }

    // Acción principal (sin parámetros que causen conflictos)
    public IActionResult Index()
    {
        // Si el servicio devuelve null, aseguramos mandar una lista vacía
        var lista = _pacienteService.GetAll() ?? new List<Paciente>();
        return View(lista);
    }

    public IActionResult Paciente()
    {
        var pacientes = _pacienteService.GetAll() ?? new List<Paciente>();
        return View(pacientes);
    }

    public IActionResult Detalle(int id)
    {
        var paciente = _pacienteService.GetById(id);
        if (paciente == null) return Content("Paciente no encontrado");
        return View(paciente);
    }

    public IActionResult Nuevo()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Nuevo(Paciente paciente)
    {
        _pacienteService.Add(paciente);
        return RedirectToAction("Paciente");
    }
}