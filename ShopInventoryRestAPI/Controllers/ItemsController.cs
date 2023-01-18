using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopInventoryRestAPI.Data;
using ShopInventoryRestAPI.Models;

namespace ShopInventoryRestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly InventoryDbContext _context;

        public ItemsController(InventoryDbContext context)
        {
            _context = context;
        }

        [HttpGet("All")]
        public async Task<IEnumerable<Item>> Get()
        {
            return await _context.Items.ToListAsync();
        }

        [HttpGet("Id")]
        public async Task<IActionResult> GetById(int id)
        {
            Item item = await _context.Items.FindAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Item item)
        {
            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Create), item);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(int id, Item item)
        {
            if (id != item.Id)
                return BadRequest();

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Items.FindAsync(id);

            if(item == null)
                return NotFound();

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
