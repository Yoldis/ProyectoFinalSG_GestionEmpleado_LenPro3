using Microsoft.AspNetCore.Mvc;

namespace ProyectoFinal3GestionEmpleado.Controllers
{
    public class DepartamentoController : Controller
    {
        public IActionResult Index()
        {
            return View("Lista");
        }
    }
}
