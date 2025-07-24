using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal3GestionEmpleado.Models;

public partial class Cargo
{
    [Key]
    public int CargoId { get; set; }
    [Required(ErrorMessage = "El cargo es requerido")]
    [MinLength(6, ErrorMessage = "El cargo debe tener al menos 6 caracteres")]
    public string Nombre { get; set; } = null!;

    public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
}
