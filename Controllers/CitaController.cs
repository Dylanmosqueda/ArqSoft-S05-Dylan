using Microsoft.AspNetCore.Mvc;

namespace CitasApp.Controllers
{
    public class CitaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
