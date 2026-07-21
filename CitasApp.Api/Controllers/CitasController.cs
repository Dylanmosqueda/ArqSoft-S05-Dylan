using Microsoft.AspNetCore.Mvc;
using CitasApp.Application.Interfaces;
using CitasApp.Domain.Models;
using System;

namespace CitasApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitasController : ControllerBase
    {
        private readonly ICitaService _citaService;

        public CitasController(ICitaService citaService)
        {
            _citaService = citaService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_citaService.ObtenerTodos());
        }

        [HttpPost]
        public IActionResult CrearCita([FromBody] Cita cita)
        {
            try
            {
                _citaService.Add(cita);
                return Ok(cita);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}