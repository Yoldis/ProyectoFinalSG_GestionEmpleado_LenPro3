using System;
using System.Collections.Generic;

namespace ProyectoFinal3GestionEmpleado.Models;

public partial class Empleado
{
    public int EmpleadoId { get; set; }

    public string Nombre { get; set; } = null!;

    public int DepartamentoId { get; set; }

    public int CargoId { get; set; }

    public DateOnly FechaInicio { get; set; }

    public decimal Salario { get; set; }

    public string Estado { get; set; } = null!;

    public string TiempoEmpresa { get; set; } = null!;

    public double Afp { get; set; }

    public double Ars { get; set; }

    public double Isr { get; set; }

    public virtual Cargo Cargo { get; set; } = null!;

    public virtual Departamento Departamento { get; set; } = null!;
}
