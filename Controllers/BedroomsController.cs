using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using resorty.Data;
using resorty.Models;

namespace resorty.Controllers
{
    public class BedroomsController : Controller
    {
        private readonly resortyContext _context;

        public BedroomsController(resortyContext context)
        {
            _context = context;
        }

        // GET: Bedrooms
        public async Task<IActionResult> Index()
        {
              return View(await _context.Bedroom.ToListAsync());
        }

        // GET: Bedrooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bedroom == null)
            {
                return NotFound();
            }

            var bedroom = await _context.Bedroom
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bedroom == null)
            {
                return NotFound();
            }

            return View(bedroom);
        }

        // GET: Bedrooms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bedrooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,Stocks")] Bedroom bedroom)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bedroom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bedroom);
        }

        // GET: Bedrooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bedroom == null)
            {
                return NotFound();
            }

            var bedroom = await _context.Bedroom.FindAsync(id);
            if (bedroom == null)
            {
                return NotFound();
            }
            return View(bedroom);
        }

        // POST: Bedrooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Stocks")] Bedroom bedroom)
        {
            if (id != bedroom.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bedroom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BedroomExists(bedroom.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bedroom);
        }

        // GET: Bedrooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bedroom == null)
            {
                return NotFound();
            }

            var bedroom = await _context.Bedroom
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bedroom == null)
            {
                return NotFound();
            }

            return View(bedroom);
        }

        // POST: Bedrooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bedroom == null)
            {
                return Problem("Entity set 'resortyContext.Bedroom'  is null.");
            }
            var bedroom = await _context.Bedroom.FindAsync(id);
            if (bedroom != null)
            {
                _context.Bedroom.Remove(bedroom);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BedroomExists(int id)
        {
          return _context.Bedroom.Any(e => e.Id == id);
        }
    }
}
