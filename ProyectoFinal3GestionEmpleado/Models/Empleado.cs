using ProyectoFinal3GestionEmpleado.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class Empleado
{
    [Key]
    public int EmpleadoId { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    [MinLength(3, ErrorMessage = "El nombre debe tener al menos 3 caracteres")]
    [MaxLength(100, ErrorMessage = "El nombre no debe superar los 100 caracteres")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "El departamento es requerido")]
    public int DepartamentoId { get; set; }

    [Required(ErrorMessage = "El cargo es requerido")]
    public int CargoId { get; set; }

    [Required(ErrorMessage = "La fecha de inicio es requerida")]
    public DateOnly FechaInicio { get; set; }

    [Required(ErrorMessage = "El salario es requerido")]
    [Range(0.01, 99999999.99, ErrorMessage = "El salario debe ser mayor que 0 y menor a 100,000,000")]
    public decimal Salario { get; set; }

    [Required(ErrorMessage = "El estado es requerido")]
    [RegularExpression("No|Vigente", ErrorMessage = "El estado debe ser 'No Vigente' o 'Vigente'")]
    public string Estado { get; set; } = null!;

    public string? TiempoEmpresa { get; set; } = null!;

    public double? Afp { get; set; }

    public double? Ars { get; set; }

    public double? Isr { get; set; }

    [ForeignKey("CargoId")]
    public virtual Cargo? Cargo { get; set; }

    [ForeignKey("DepartamentoId")]
    public virtual Departamento? Departamento { get; set; }

}
