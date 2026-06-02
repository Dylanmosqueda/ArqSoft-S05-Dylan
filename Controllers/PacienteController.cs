using Microsoft.AspNetCore.Mvc;

namespace CitasApp.Controllers
{
    public class PacienteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
