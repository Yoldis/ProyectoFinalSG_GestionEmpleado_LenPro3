using Microsoft.AspNetCore.Mvc;
using ProyectoFinal3GestionEmpleado.Models;

namespace ProyectoFinal3GestionEmpleado.Controllers
{
    public class CargoController : Controller
    {
        private readonly SgEmpleadosLenPro3Context _context;

        public CargoController(SgEmpleadosLenPro3Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var cargos = _context.Cargos.ToList();
            ViewBag.Cargos = cargos;
            return View("Lista");
        }

        [HttpGet]
        public IActionResult Registrar()
        {

            return View("Registrar");
        }

        [HttpPost]
        public IActionResult Registrar(Cargo cargo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Cargos.Add(cargo);
                    var resultado = _context.SaveChanges();
                    if (resultado > 0)
                    {
                        TempData["MensajeExito"] = $"Cargo '{cargo.Nombre}' registrado correctamente.";
                        ModelState.Clear();
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.MensajeError = $"Error al registrar el cargo: {cargo.Nombre} ya existe";
                    return View(cargo);
                }

            }
            return View(cargo);
        }


        [HttpGet]
        public IActionResult Editar(int id)
        {
            var cargo = _context.Cargos.FirstOrDefault(d => d.CargoId == id);
            if (cargo == null)
            {
                TempData["MensajeError"] = "Cargo no encontrado.";
                return RedirectToAction("Index");
            }
            return View("Editar", cargo);
        }

        [HttpPost]
        public IActionResult Editar(Cargo cargo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.MensajeError = "Hay campos obligatorios que debes completar correctamente.";
                    return View("Editar", cargo);
                }

                var cargoExistente = _context.Cargos.FirstOrDefault(d => d.CargoId == cargo.CargoId);
                if (cargoExistente == null)
                {
                    ViewBag.MensajeError = "Cargo no encontrado.";
                }
                else
                {
                    cargoExistente.Nombre = cargo.Nombre;
                    var resultado = _context.SaveChanges();
                    if (resultado > 0)
                    {
                        TempData["MensajeExito"] = $"Cargo '{cargo.Nombre}' actualizado correctamente.";
                        ModelState.Clear();
                        return RedirectToAction("Index");
                    }
                }

            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = $"Error al actualizar el cargo: {cargo.Nombre} ya existe";
            }

            return View("Editar", cargo);
        }



        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var cargo = _context.Cargos.FirstOrDefault(d => d.CargoId == id);
            if (cargo == null)
            {
                TempData["MensajeError"] = "Cargo no encontrado.";
                return RedirectToAction("Index");
            }
            return View("Eliminar", cargo);
        }

        [HttpPost]
        public IActionResult Eliminar(Cargo cargo)
        {
            try
            {
                var cargoExistente = _context.Cargos.FirstOrDefault(d => d.CargoId == cargo.CargoId);
                if (cargoExistente == null)
                {
                    ViewBag.MensajeError = "Departamento no encontrado.";
                }
                else
                {
                    _context.Cargos.Remove(cargoExistente);
                    var resultado = _context.SaveChanges();
                    if (resultado > 0)
                    {
                        TempData["MensajeExito"] = $"Cargo '{cargoExistente.Nombre}' eliminado correctamente.";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = $"Error al eliminar el cargo: {ex.Message}";
            }
            return View(cargo);
        }
    }
}
