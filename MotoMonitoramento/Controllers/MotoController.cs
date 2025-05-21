using Microsoft.AspNetCore.Mvc;
using MotoMonitoramento.Data;
using MotoMonitoramento.Models;
using Microsoft.EntityFrameworkCore;

namespace MotoMonitoramento.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MotosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/motos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Moto>>> GetAll() =>
            await _context.Motos.ToListAsync();

        // GET: api/motos/por-setor?setor=A
        [HttpGet("por-setor")]
        public async Task<ActionResult<IEnumerable<Moto>>> GetPorSetor([FromQuery] string setor) =>
            await _context.Motos.Where(m => m.Setor == setor).ToListAsync();

        // GET: api/motos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Moto>> GetById(int id)
        {
            var moto = await _context.Motos.FindAsync(id);
            return moto == null ? NotFound() : Ok(moto);
        }

        // POST: api/motos
        [HttpPost]
        public async Task<ActionResult<Moto>> Create(Moto moto)
        {
            _context.Motos.Add(moto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = moto.Id }, moto);
        }

        // PUT: api/motos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Moto moto)
        {
            if (id != moto.Id)
                return BadRequest();

            _context.Entry(moto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MotoExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/motos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null)
                return NotFound();

            _context.Motos.Remove(moto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MotoExists(int id) =>
            _context.Motos.Any(m => m.Id == id);
    }
}
