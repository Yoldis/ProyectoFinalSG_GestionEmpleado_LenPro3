using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoFinal3GestionEmpleado.ViewModels
{
    public class EmpleadoViewModel
    {
        public Empleado Empleado { get; set; }

        public string? DepartamentoNombre { get; set; }
        public string? CargoNombre { get; set; }

        public List<SelectListItem>? Departamentos { get; set; }
        public List<SelectListItem>? Cargos { get; set; }


    }
}
