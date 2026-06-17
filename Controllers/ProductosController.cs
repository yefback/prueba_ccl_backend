using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaCCL.Backend.Data;
using PruebaCCL.Backend.Models;

namespace PruebaCCL.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // La protección ([Authorize]) ya se aplica globalmente en Program.cs
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("inventario")]
        public async Task<IActionResult> GetInventario()
        {
            var productos = await _context.Productos.OrderBy(p => p.Id).ToListAsync();
            return Ok(productos);
        }

        [HttpPost("movimiento")]
        public async Task<IActionResult> RegistrarMovimiento([FromBody] MovimientoRequest request)
        {
            var producto = await _context.Productos.FindAsync(request.ProductoId);
            
            if (producto == null)
            {
                return NotFound(new { message = "Producto no encontrado." });
            }

            if (request.TipoMovimiento.Equals("Entrada", StringComparison.OrdinalIgnoreCase))
            {
                producto.Cantidad += request.Cantidad;
            }
            else if (request.TipoMovimiento.Equals("Salida", StringComparison.OrdinalIgnoreCase))
            {
                if (producto.Cantidad < request.Cantidad)
                {
                    return BadRequest(new { message = "Stock insuficiente para realizar la salida." });
                }
                producto.Cantidad -= request.Cantidad;
            }
            else
            {
                return BadRequest(new { message = "Tipo de movimiento no válido. Use 'Entrada' o 'Salida'." });
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Movimiento registrado exitosamente", producto });
        }
    }
}
