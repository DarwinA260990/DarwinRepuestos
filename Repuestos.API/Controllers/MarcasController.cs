using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repuestos.API.Data;
using Repuestos.API.Models.Dtos;
using Repuestos.API.Models.Entities;

namespace Repuestos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarcasController : ControllerBase
    {
        private readonly DataContext _context;

        public MarcasController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Marcas
        [HttpGet]
        public ActionResult<IEnumerable<MarcaDto>> GetAll()
        {
            var marcas = _context.Marcas
                .Select(m => new MarcaDto
                {
                    Id = m.Id,
                    Nombre = m.Nombre
                })
                .ToList();

            return Ok(marcas);
        }

        // GET: api/Marcas/1
        [HttpGet("{id:int}")]
        public ActionResult<MarcaDto> GetById(int id)
        {
            var marca = _context.Marcas.Find(id);

            if (marca == null)
            {
                return NotFound("La marca no existe.");
            }

            var resultado = new MarcaDto
            {
                Id = marca.Id,
                Nombre = marca.Nombre
            };

            return Ok(resultado);
        }

        // POST: api/Marcas
        [HttpPost]
        public ActionResult<MarcaDto> Create(CreateMarcaDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre)
                || dto.Nombre.Trim().Length > 100)
            {
                return BadRequest(
                    "El nombre es obligatorio y debe tener hasta 100 caracteres.");
            }

            var marca = new Marca
            {
                Nombre = dto.Nombre.Trim()
            };

            _context.Marcas.Add(marca);
            _context.SaveChanges();

            var resultado = new MarcaDto
            {
                Id = marca.Id,
                Nombre = marca.Nombre
            };

            return CreatedAtAction(
                nameof(GetById),
                new { id = marca.Id },
                resultado);
        }

        // PUT: api/Marcas/1
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, UpdateMarcaDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(
                    "El ID de la URL debe coincidir con el ID enviado.");
            }

            if (string.IsNullOrWhiteSpace(dto.Nombre)
                || dto.Nombre.Trim().Length > 100)
            {
                return BadRequest(
                    "El nombre es obligatorio y debe tener hasta 100 caracteres.");
            }

            var marca = _context.Marcas.Find(id);

            if (marca == null)
            {
                return NotFound("La marca no existe.");
            }

            marca.Nombre = dto.Nombre.Trim();

            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/Marcas/1
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var marca = _context.Marcas.Find(id);

            if (marca == null)
            {
                return NotFound("La marca no existe.");
            }

            if (_context.Repuestos.Any(r => r.MarcaId == id))
            {
                return BadRequest(
                    "No puedes eliminar una marca que tiene repuestos asociados.");
            }

            _context.Marcas.Remove(marca);
            _context.SaveChanges();

            return NoContent();
        }
    }
}