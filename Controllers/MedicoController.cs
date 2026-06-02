using Microsoft.AspNetCore.Mvc;

namespace CitasApp.Controllers
{
    public class MedicoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
