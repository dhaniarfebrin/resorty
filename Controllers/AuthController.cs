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
    public class AuthController : Controller
    {
        private readonly resortyContext _context;

        [TempData]
        public string Message { get; set; }

        public AuthController(resortyContext context)
        {
            _context = context;
        }

        // GET: Auth
        public async Task<IActionResult> Index()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Username,Password")] User user)
        {
            var userTarget = await _context.User.ToListAsync();

            foreach(var userItem in userTarget)
            {
                if (userItem.Username == user.Username)
                {
                    if (userItem.Password == user.Password)
                    {
                        return RedirectToAction("Dashboard", "Reservations");
                    } 
                    else
                    {
                        Message = "The Password is incorrect";
                    }
                } 
                else
                {

                    Message = "There is no account for that username";
                }
            }

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Logout()
        {
            return RedirectToAction(nameof(Index));
        }





























        // GET: Auth/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Auth/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Auth/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Username,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                
            }
            return View(user);
        }

        // GET: Auth/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Auth/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Username,Password")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: Auth/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Auth/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'resortyContext.User'  is null.");
            }
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
          return _context.User.Any(e => e.Id == id);
        }
    }
}
