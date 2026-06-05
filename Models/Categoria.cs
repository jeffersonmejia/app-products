using System.ComponentModel.DataAnnotations;

namespace CrudProductos.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la categoria es obligatorio")]
        [StringLength(80)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripcion es obligatoria")]
        [StringLength(200)]
        public string Descripcion { get; set; } = string.Empty;

        [Range(0, 100, ErrorMessage = "El descuento debe estar entre 0 y 100")]
        public decimal DescuentoPorcentaje { get; set; }

        public bool Activa { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }
}
