using System.ComponentModel.DataAnnotations;

namespace PruebaCCL.Backend.Models
{
    public class MovimientoRequest
    {
        [Required]
        public int ProductoId { get; set; }

        [Required]
        public string TipoMovimiento { get; set; } = string.Empty; // "Entrada" o "Salida"

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }
    }
}
