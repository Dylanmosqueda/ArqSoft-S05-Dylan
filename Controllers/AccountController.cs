using CitasApp.Application.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CitasApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IPacienteService _pacienteService;
        private readonly IMedicoService _medicoService;

        public AccountController(IPacienteService pacienteService, IMedicoService medicoService)
        {
            _pacienteService = pacienteService;
            _medicoService = medicoService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string usuario, string clave, string rol)
        {
            // ACCESO LIBRE UNIVERSAL: Se acepta cualquier contraseña
            string idBaseDatos = "1"; // ID por defecto de respaldo
            string nombreAMostrar = string.IsNullOrWhiteSpace(usuario) ? "Usuario Anonimo" : usuario;

            if (rol == "Administrador")
            {
                idBaseDatos = "0";
            }
            else if (rol == "Paciente")
            {
                // Intentamos buscar si existe en la base de datos de SQLite
                var pacientes = _pacienteService.GetAll();

                var paciente = pacientes?.FirstOrDefault(p =>
                    p.Nombre.Equals(usuario, StringComparison.OrdinalIgnoreCase) ||
                    $"{p.Nombre} {p.Apellido}".Equals(usuario, StringComparison.OrdinalIgnoreCase));

                if (paciente != null)
                {
                    // Si existe, usamos su ID real
                    idBaseDatos = paciente.Id.ToString();
                    nombreAMostrar = $"{paciente.Nombre} {paciente.Apellido}";
                }
                else
                {
                    // Si escribiste cualquier otro nombre, te dejamos entrar igualmente con ID de respaldo 1
                    idBaseDatos = "1";
                }
            }
            else if (rol == "Medico")
            {
                // Buscamos si existe en la base de datos de SQLite
                var medicos = _medicoService.GetAll();
                var medico = medicos?.FirstOrDefault(m => m.Nombre.Equals(usuario, StringComparison.OrdinalIgnoreCase));

                if (medico != null)
                {
                    // Si existe, usamos su ID real
                    idBaseDatos = medico.Id.ToString();
                    nombreAMostrar = medico.Nombre;
                }
                else
                {
                    // Si escribiste cualquier otro nombre, te dejamos entrar igualmente con ID de respaldo 1
                    idBaseDatos = "1";
                }
            }

            // Iniciar sesión con Cookie de manera universal (nunca falla)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, nombreAMostrar),
                new Claim(ClaimTypes.Role, rol),
                new Claim("UsuarioId", idBaseDatos)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    } // Cierre correcto de la clase 'AccountController'
} // Cierre correcto del espacio de nombres 'CitasApp.Controllers'