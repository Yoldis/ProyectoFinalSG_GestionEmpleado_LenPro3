using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal3GestionEmpleado.Models;
using ProyectoFinal3GestionEmpleado.ViewModels;

namespace ProyectoFinal3GestionEmpleado.Controllers
{
    public class EmpleadoController : Controller
    {
        private readonly SgEmpleadosLenPro3Context _context;

        public EmpleadoController(SgEmpleadosLenPro3Context context)
        {
            _context = context;
        }

        private string calcularTiempoEnEmpresa(DateOnly fechaIngreso)
        {
            DateTime fechaActual = DateTime.Now;

            int años = fechaActual.Year - fechaIngreso.Year;
            int meses = fechaActual.Month - fechaIngreso.Month;

            if (meses < 0)
            {
                años--;
                meses += 12;
            }

            return $"{años} año(s) y {meses} mes(es)";
        }


        private double calcularArs(double salario)
        {
            double porcentajeARS = 0.0304f;
            return Math.Round(salario * porcentajeARS, 2);
        }


        private double calcularAfp(double salario)
        {
            double porcentajeAFP = 0.0287f;
            return Math.Round(salario * porcentajeAFP, 2);
        }


        private double calcularIsr(double salarioMensual)
        {
            double salarioAnual = salarioMensual * 12;

            double isr = 0.0;

            if (salarioAnual <= 416220.00)
            {
                isr = 0.0;
            }
            else if (salarioAnual <= 624329.00)
            {
                isr = (salarioAnual - 416220.01) * 0.15;
            }
            else if (salarioAnual <= 867123.00)
            {
                isr = ((salarioAnual - 624329.01) * 0.20) + 31216.00;
            }
            else
            {
                isr = ((salarioAnual - 867123.01) * 0.25) + 79776.00;
            }

            return Math.Round(isr / 12, 2);
        }


        private EmpleadoViewModel CargarSelectList(EmpleadoViewModel viewModel)
        {
            viewModel.Departamentos = _context.Departamentos
                .Select(d => new SelectListItem { Value = d.DepartamentoId.ToString(), Text = d.Nombre })
                .ToList();

            viewModel.Cargos = _context.Cargos
                .Select(c => new SelectListItem { Value = c.CargoId.ToString(), Text = c.Nombre })
                .ToList();

            return viewModel;
        }


        [HttpGet]
        public IActionResult Index()
        {
            var viewModel = _context.Empleados
                .Select(e => new EmpleadoViewModel
                {
                    Empleado = e,
                    DepartamentoNombre = e.Departamento.Nombre,
                    CargoNombre = e.Cargo.Nombre,
                }).ToList();

            return View("Lista", viewModel);
        }

        [HttpGet]
        public IActionResult Registrar()
        {
            var viewModel = CargarSelectList(new EmpleadoViewModel());

            return View("Registrar", viewModel);
        }

        [HttpPost]
        public IActionResult Registrar(EmpleadoViewModel empleadoViewModel)
        {
            // Si la validación falla, recargamos los SelectList y mostramos errores
            if (!ModelState.IsValid)
            {
                CargarSelectList(empleadoViewModel);
                TempData["MensajeError"] = "El formulario tiene errores. Verifique los campos.";
                return View(empleadoViewModel);
            }

            try
            {
                // Verificar si ya existe un empleado con el mismo nombre
                if (_context.Empleados.Any(e => e.Nombre == empleadoViewModel.Empleado.Nombre))
                {
                    ModelState.AddModelError("", "Ya existe un empleado con ese nombre.");
                    CargarSelectList(empleadoViewModel);
                    return View(empleadoViewModel);
                }

                var empleado = empleadoViewModel.Empleado;

                // Calcular valores antes de guardar
                empleado.TiempoEmpresa = calcularTiempoEnEmpresa(empleado.FechaInicio);
                empleado.Afp = calcularAfp((float)empleado.Salario);
                empleado.Ars = calcularArs((float)empleado.Salario);
                empleado.Isr = calcularIsr((float)empleado.Salario);

                // Guardar en BD
                _context.Empleados.Add(empleado);
                var resultado = _context.SaveChanges();

                if (resultado > 0)
                {
                    TempData["MensajeExito"] = $"Empleado '{empleado.Nombre}' registrado correctamente.";
                    return RedirectToAction("Index");
                }

                TempData["MensajeError"] = "No se pudo guardar el empleado. Intente nuevamente.";
            }
            catch (Exception ex)
            {
                // Loguear el error si usas ILogger
                TempData["MensajeError"] = $"Error al registrar el empleado: {ex.Message}";
            }

            // Si algo falla, recargar los selects
            CargarSelectList(empleadoViewModel);
            return View(empleadoViewModel);
        }



        [HttpGet]
        public IActionResult Editar(int id)
        {
            var viewModel = CargarSelectList(new EmpleadoViewModel());
            viewModel.Empleado = _context.Empleados.FirstOrDefault(d => d.EmpleadoId == id);

            if (viewModel.Empleado == null)
            {
                TempData["MensajeError"] = "Empleado no encontrado.";
                return RedirectToAction("Index");
            }

            return View("Editar", viewModel);
        }


        [HttpPost]
        public IActionResult Editar(EmpleadoViewModel empleadoViewModel)
        {
            if (!ModelState.IsValid)
            {
                CargarSelectList(empleadoViewModel);
                TempData["MensajeError"] = "El formulario tiene errores. Verifique los campos.";
                return View(empleadoViewModel);
            }

            try
            {
              
                var empleadoExistente = _context.Empleados.FirstOrDefault(e => e.EmpleadoId == empleadoViewModel.Empleado.EmpleadoId);
                if (empleadoExistente == null)
                {
                    TempData["MensajeError"] = "Empleado no encontrado.";
                    return RedirectToAction("Index");
                }

              
                if (_context.Empleados.Any(e => e.Nombre == empleadoViewModel.Empleado.Nombre
                                             && e.EmpleadoId != empleadoViewModel.Empleado.EmpleadoId))
                {
                    ModelState.AddModelError("", "Ya existe otro empleado con ese nombre.");
                    CargarSelectList(empleadoViewModel);
                    return View(empleadoViewModel);
                }

                var empleado = empleadoViewModel.Empleado;
                empleadoExistente.Nombre = empleado.Nombre;
                empleadoExistente.Salario = empleado.Salario;
                empleadoExistente.FechaInicio = empleado.FechaInicio;
                empleadoExistente.CargoId = empleado.CargoId;
                empleadoExistente.DepartamentoId = empleado.DepartamentoId;

               
                empleadoExistente.TiempoEmpresa = calcularTiempoEnEmpresa(empleado.FechaInicio);
                empleadoExistente.Afp = calcularAfp((float)empleado.Salario);
                empleadoExistente.Ars = calcularArs((float)empleado.Salario);
                empleadoExistente.Isr = calcularIsr((float)empleado.Salario);

               
                var resultado = _context.SaveChanges();
                if (resultado > 0)
                {
                    TempData["MensajeExito"] = $"Empleado '{empleadoExistente.Nombre}' actualizado correctamente.";
                    return RedirectToAction("Index");
                }

                TempData["MensajeError"] = "No se pudo actualizar el empleado. Intente nuevamente.";
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = $"Error al actualizar el empleado: {ex.Message}";
            }

            CargarSelectList(empleadoViewModel);
            return View(empleadoViewModel);
        }



        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var empleado = _context.Empleados
                .Include(e => e.Departamento)
                .Include(e => e.Cargo)
                .FirstOrDefault(e => e.EmpleadoId == id);

            if (empleado == null)
            {
                TempData["MensajeError"] = "Empleado no encontrado.";
                return RedirectToAction("Index");
            }

            var viewModel = new EmpleadoViewModel
            {
                Empleado = empleado,
                DepartamentoNombre = empleado.Departamento?.Nombre,
                CargoNombre = empleado.Cargo?.Nombre
            };

            return View(viewModel);
        }


        [HttpPost]
        public IActionResult Eliminar(EmpleadoViewModel empleadoViewModel)
        {
            try
            {
                var empleadoExistente = _context.Empleados
                    .Include(e => e.Departamento)
                    .Include(e => e.Cargo)
                    .FirstOrDefault(e => e.EmpleadoId == empleadoViewModel.Empleado.EmpleadoId);

                if (empleadoExistente == null)
                {
                    TempData["MensajeError"] = "Empleado no encontrado.";
                    return RedirectToAction("Index");
                }

                _context.Empleados.Remove(empleadoExistente);
                var resultado = _context.SaveChanges();

                if (resultado > 0)
                {
                    TempData["MensajeExito"] = $"Empleado '{empleadoExistente.Nombre}' eliminado correctamente.";
                    return RedirectToAction("Index");
                }

                TempData["MensajeError"] = "No se pudo eliminar el empleado. Intente nuevamente.";
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = $"Error al eliminar el empleado: {ex.Message}";
            }

            return View(empleadoViewModel);
        }

        public IActionResult ExportarCSV()
        {
            var empleados = _context.Empleados
             .Include(e => e.Departamento)
             .Include(e => e.Cargo)
             .ToList();

            if (empleados == null || !empleados.Any())
            {
                ViewBag.MensajeError = "No hay empleados para exportar.";
                return RedirectToAction("Index");
            }

            var builder = new System.Text.StringBuilder();

            builder.AppendLine("Id,Nombre, Departamento, Cargo, Fecha de Inicio, Salario, Estado, Tiempo Empresa, ARS, AFP, IRS");

            foreach (var dep in empleados)
            {
                builder.AppendLine($"{dep.EmpleadoId},{dep.Nombre},{dep.Departamento.Nombre},{dep.Cargo.Nombre},{dep.FechaInicio},{dep.Salario},{dep.Estado},{dep.TiempoEmpresa},{dep.Ars},{dep.Afp},{dep.Isr}");
            }

            var bytes = System.Text.Encoding.UTF8.GetBytes(builder.ToString());
            return File(bytes, "text/csv", "empleados.csv");
        }
    }
}
