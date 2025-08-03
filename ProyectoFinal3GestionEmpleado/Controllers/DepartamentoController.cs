using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using ProyectoFinal3GestionEmpleado.Models;

namespace ProyectoFinal3GestionEmpleado.Controllers
{
    public class DepartamentoController : Controller
    {
        private readonly SgEmpleadosLenPro3Context _context;

        public DepartamentoController(SgEmpleadosLenPro3Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var departamentos = _context.Departamentos.ToList();
            ViewBag.Departamentos = departamentos;
            return View("Lista");
        }

        [HttpGet]
        public IActionResult Registrar()
        {
         
            return View("Registrar");
        }

        [HttpPost]
        public IActionResult Registrar(Departamento departamento)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Departamentos.Add(departamento);
                    var resultado = _context.SaveChanges();
                    if (resultado > 0)
                    {
                        TempData["MensajeExito"] = $"Departamento '{departamento.Nombre}' registrado correctamente.";
                        ModelState.Clear();
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.MensajeError = $"Error al registrar el departamento: {departamento.Nombre} ya existe";
                    return View(departamento);
                }
               
            }
            return View(departamento);
        }


        [HttpGet]
        public IActionResult Editar(int id)
        {
            var departamento = _context.Departamentos.FirstOrDefault(d => d.DepartamentoId == id);
            if (departamento == null)
            {
                TempData["MensajeError"] = "Departamento no encontrado.";
                return RedirectToAction("Index");
            }
            return View("Editar", departamento);
        }

        [HttpPost]
        public IActionResult Editar(Departamento departamento)
        {   
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.MensajeError = "Hay campos obligatorios que debes completar correctamente.";
                    return View("Editar", departamento);
                }

                var departamentoExistente = _context.Departamentos.FirstOrDefault(d => d.DepartamentoId == departamento.DepartamentoId);
                if (departamentoExistente == null)
                {
                    ViewBag.MensajeError = "Departamento no encontrado.";
                }
                else
                {
                    if( departamentoExistente.Nombre == departamento.Nombre)
                    {
                        ViewBag.MensajeError = "El nombre del departamento no ha cambiado.";
                        return View("Editar", departamento);
                    }
                    else
                    {
                        departamentoExistente.Nombre = departamento.Nombre;
                        var resultado = _context.SaveChanges();
                        if (resultado > 0)
                        {
                            TempData["MensajeExito"] = $"Departamento '{departamento.Nombre}' actualizado correctamente.";
                            ModelState.Clear();
                            return RedirectToAction("Index");
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = $"Error al actualizar el departamento: {departamento.Nombre} ya existe";
            }

            return View("Editar", departamento);
        }



        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var departamento = _context.Departamentos.FirstOrDefault(d => d.DepartamentoId == id);
            if (departamento == null)
            {
                TempData["MensajeError"] = "Departamento no encontrado.";
                return RedirectToAction("Index");
            }
            return View("Eliminar", departamento);
        }

        [HttpPost]
        public IActionResult Eliminar(Departamento departamento)
        {
            try
            {
                var departamentoExistente = _context.Departamentos.FirstOrDefault(d => d.DepartamentoId == departamento.DepartamentoId);
                if (departamentoExistente == null)
                {
                    ViewBag.MensajeError = "Departamento no encontrado.";
                }
                else { 
                    _context.Departamentos.Remove(departamentoExistente);
                    var resultado = _context.SaveChanges();
                    if (resultado > 0)
                    {
                        TempData["MensajeExito"] = $"Departamento '{departamentoExistente.Nombre}' eliminado correctamente.";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = $"Error al eliminar el departamento: {ex.Message}";
            }
            return View(departamento);
        }

        public IActionResult ExportarCSV()
        {
            var departamentos = _context.Departamentos.ToList();
            if (departamentos == null || !departamentos.Any())
            {
                ViewBag.MensajeError = "No hay departamentos para exportar.";
                return RedirectToAction("Index");
            }

            var builder = new System.Text.StringBuilder();
            builder.AppendLine("Id,Nombre");

            foreach (var dep in departamentos)
            {
                builder.AppendLine($"{dep.DepartamentoId},{dep.Nombre}");
            }

            var bytes = System.Text.Encoding.UTF8.GetBytes(builder.ToString());
            return File(bytes, "text/csv", "departamentos.csv");
        }

    }
}
