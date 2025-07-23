using System;
using System.Collections.Generic;

namespace ProyectoFinal3GestionEmpleado.Models;

public partial class Cargo
{
    public int CargoId { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
}
