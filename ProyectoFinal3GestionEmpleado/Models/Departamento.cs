using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal3GestionEmpleado.Models;

public partial class Departamento
{
    [Key]
    public int DepartamentoId { get; set; }
    [Required(ErrorMessage ="El departamento es requerido")]
    [MinLength(3, ErrorMessage = "El departamento debe tener al menos 3 caracteres")]
    public string Nombre { get; set; } = null!;

    public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
}
