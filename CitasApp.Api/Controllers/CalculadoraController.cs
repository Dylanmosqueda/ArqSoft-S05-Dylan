using Microsoft.AspNetCore.Mvc;

namespace CitasApp.Api.Controllers; // Ajusta el namespace al de tu proyecto si es necesario

[ApiController]
[Route("api/[controller]")]
public class CalculadoraController : ControllerBase
{
    // GET: api/calculadora/sumar?a=28&b=32
    [HttpGet("sumar")]
    public IActionResult Sumar([FromQuery] double a, [FromQuery] double b)
    {
        return Ok(new
        {
            operacion = "suma",
            a = a,
            b = b,
            resultado = a + b
        });
    }

    // GET: api/calculadora/restar?a=28&b=32
    [HttpGet("restar")]
    public IActionResult Restar([FromQuery] double a, [FromQuery] double b)
    {
        return Ok(new
        {
            operacion = "resta",
            a = a,
            b = b,
            resultado = a - b
        });
    }

    // GET: api/calculadora/multiplicar?a=28&b=32
    [HttpGet("multiplicar")]
    public IActionResult Multiplicar([FromQuery] double a, [FromQuery] double b)
    {
        return Ok(new
        {
            operacion = "multiplicacion",
            a = a,
            b = b,
            resultado = a * b
        });
    }

    // GET: api/calculadora/dividir?a=28&b=32
    [HttpGet("dividir")]
    public IActionResult Dividir([FromQuery] double a, [FromQuery] double b)
    {
        // Validación de seguridad para evitar división entre cero
        if (b == 0)
        {
            return BadRequest(new { mensaje = "No es posible dividir entre cero." });
        }

        return Ok(new
        {
            operacion = "division",
            a = a,
            b = b,
            resultado = a / b
        });
   
    }
}