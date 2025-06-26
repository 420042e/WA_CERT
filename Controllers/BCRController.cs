using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WA_CERT.Data;
using WA_CERT.Model;

namespace WA_CERT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BCRController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // El DbContext se inyecta automáticamente gracias a la configuración en Program.cs
        public BCRController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Certificados
        // Endpoint para obtener todos los certificados
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Servidor>>> GetCertificados()
        {
            // Usa el DbContext para consultar la tabla y devolver los resultados
            return await _context.Servidor.ToListAsync();
        }

        // Endpoint para llamar al Stored Procedure
        [HttpGet("produccionbcr")] // Ruta: api/Servidores/desarrollo
        public async Task<ActionResult<IEnumerable<Servidor>>> GetServidoresDeDesarrollo()
        {
            // --- ¡AQUÍ ESTÁ LA MAGIA! ---

            // 1. Especifica el DbSet al que se mapearán los resultados.
            // 2. Llama a FromSqlRaw con el comando EXEC para tu SP.
            var servidores = await _context.Servidor
                                           .FromSqlRaw("EXEC [dbo].[usp_ObtenerServidoresBCR]")
                                           .ToListAsync();

            // --- FIN DE LA MAGIA ---

            if (servidores == null || !servidores.Any())
            {
                return NotFound("No se encontraron servidores de desarrollo.");
            }

            return Ok(servidores);
        }

        // POST: api/Certificados
        // Endpoint para crear un nuevo registro de certificado
        [HttpPost]
        public async Task<ActionResult<Servidor>> PostCertificado(Servidor certificado)
        {
            // La FechaRegistro será añadida por la base de datos gracias al DEFAULT.
            // No necesitamos asignarla aquí.

            _context.Servidor.Add(certificado);
            await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos

            // Devuelve el objeto creado (con el Id generado por la BD)
            return CreatedAtAction(nameof(GetCertificados), new { id = certificado.Id }, certificado);
        }
    }
}