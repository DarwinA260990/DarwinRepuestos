using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repuestos.API.Data;
using Repuestos.API.Models.Dtos;
using Repuestos.API.Models.Entities;

namespace Repuestos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepuestosController : ControllerBase
    {
        private readonly DataContext _context;

        public RepuestosController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Repuestos
        [HttpGet]
        public ActionResult<IEnumerable<RepuestoDto>> GetAll()
        {
            var repuestos = _context.Repuestos
                .Include(r => r.Marca)
                .Select(r => new RepuestoDto
                {
                    Id = r.Id,
                    Codigo = r.Codigo,
                    Nombre = r.Nombre,
                    Precio = r.Precio,
                    Cantidad = r.Cantidad,
                    MarcaId = r.MarcaId,
                    MarcaNombre = r.Marca.Nombre
                })
                .ToList();

            return Ok(repuestos);
        }

        // GET: api/Repuestos/1
        [HttpGet("{id:int}")]
        public ActionResult<RepuestoDto> GetById(int id)
        {
            var repuesto = _context.Repuestos
                .Include(r => r.Marca)
                .FirstOrDefault(r => r.Id == id);

            if (repuesto == null)
            {
                return NotFound("El repuesto no existe.");
            }

            var resultado = new RepuestoDto
            {
                Id = repuesto.Id,
                Codigo = repuesto.Codigo,
                Nombre = repuesto.Nombre,
                Precio = repuesto.Precio,
                Cantidad = repuesto.Cantidad,
                MarcaId = repuesto.MarcaId,
                MarcaNombre = repuesto.Marca.Nombre
            };

            return Ok(resultado);
        }

        // POST: api/Repuestos
        [HttpPost]
        public ActionResult<RepuestoDto> Create(CreateRepuestoDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Codigo)
                || dto.Codigo.Trim().Length > 50)
            {
                return BadRequest(
                    "El código es obligatorio y debe tener hasta 50 caracteres.");
            }

            if (string.IsNullOrWhiteSpace(dto.Nombre)
                || dto.Nombre.Trim().Length > 150)
            {
                return BadRequest(
                    "El nombre es obligatorio y debe tener hasta 150 caracteres.");
            }

            if (dto.Precio <= 0 || dto.Precio > 9999999999999999.99m
                || decimal.Round(dto.Precio, 2) != dto.Precio)
            {
                return BadRequest(
                    "El precio debe ser mayor que cero, tener como máximo "
                    + "dos decimales y no superar 9999999999999999.99.");
            }

            if (dto.Cantidad < 0)
            {
                return BadRequest("La cantidad no puede ser negativa.");
            }

            var marca = _context.Marcas.Find(dto.MarcaId);

            if (marca == null)
            {
                return BadRequest("La marca indicada no existe.");
            }

            var codigo = dto.Codigo.Trim();

            if (_context.Repuestos.Any(r => r.Codigo == codigo))
            {
                return Conflict("Ya existe un repuesto con ese código.");
            }

            var repuesto = new Repuesto
            {
                Codigo = codigo,
                Nombre = dto.Nombre.Trim(),
                Precio = dto.Precio,
                Cantidad = dto.Cantidad,
                MarcaId = dto.MarcaId
            };

            _context.Repuestos.Add(repuesto);
            _context.SaveChanges();

            var resultado = new RepuestoDto
            {
                Id = repuesto.Id,
                Codigo = repuesto.Codigo,
                Nombre = repuesto.Nombre,
                Precio = repuesto.Precio,
                Cantidad = repuesto.Cantidad,
                MarcaId = repuesto.MarcaId,
                MarcaNombre = marca.Nombre
            };

            return CreatedAtAction(
                nameof(GetById),
                new { id = repuesto.Id },
                resultado);
        }

        // PUT: api/Repuestos/1
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, UpdateRepuestoDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(
                    "El ID de la URL debe coincidir con el ID enviado.");
            }

            var repuesto = _context.Repuestos.Find(id);

            if (repuesto == null)
            {
                return NotFound("El repuesto no existe.");
            }

            if (string.IsNullOrWhiteSpace(dto.Codigo)
                || dto.Codigo.Trim().Length > 50)
            {
                return BadRequest(
                    "El código es obligatorio y debe tener hasta 50 caracteres.");
            }

            if (string.IsNullOrWhiteSpace(dto.Nombre)
                || dto.Nombre.Trim().Length > 150)
            {
                return BadRequest(
                    "El nombre es obligatorio y debe tener hasta 150 caracteres.");
            }

            if (dto.Precio <= 0 || dto.Precio > 9999999999999999.99m
                || decimal.Round(dto.Precio, 2) != dto.Precio)
            {
                return BadRequest(
                    "El precio debe ser mayor que cero, tener como máximo "
                    + "dos decimales y no superar 9999999999999999.99.");
            }

            if (dto.Cantidad < 0)
            {
                return BadRequest("La cantidad no puede ser negativa.");
            }

            if (!_context.Marcas.Any(m => m.Id == dto.MarcaId))
            {
                return BadRequest("La marca indicada no existe.");
            }

            var codigo = dto.Codigo.Trim();

            if (_context.Repuestos.Any(
                r => r.Codigo == codigo && r.Id != id))
            {
                return Conflict("Ya existe otro repuesto con ese código.");
            }

            repuesto.Codigo = codigo;
            repuesto.Nombre = dto.Nombre.Trim();
            repuesto.Precio = dto.Precio;
            repuesto.Cantidad = dto.Cantidad;
            repuesto.MarcaId = dto.MarcaId;

            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/Repuestos/1
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var repuesto = _context.Repuestos.Find(id);

            if (repuesto == null)
            {
                return NotFound("El repuesto no existe.");
            }

            _context.Repuestos.Remove(repuesto);
            _context.SaveChanges();

            return NoContent();
        }
    }
}