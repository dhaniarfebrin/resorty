using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using resorty.Data;
using resorty.Models;

namespace resorty.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly resortyContext _context;

        public ReservationsController(resortyContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            DateTime TodayDate = DateTime.Today;
            List<Reservation> reservations = _context.Reservation.ToList();
            List<Bedroom> rooms = _context.Bedroom.ToList(); 

            foreach(var reservation in reservations)
            {
                if(TodayDate > reservation.EndDate && reservation.Status == "Ordered")
                {
                    reservation.Status = "Finished";

                    foreach(var room in rooms)
                    {
                        if(room.Name == reservation.Room)
                        {
                            room.Stocks = room.Stocks + 1;
                        }
                    }
                    await _context.SaveChangesAsync();
                }
            }

            var countDataReservations = await _context.Reservation.Where(r => r.Status == "Ordered").CountAsync();
            var countDataFinished = await _context.Reservation.Where(r => r.Status == "Finished").CountAsync();
            var countDataCanceled = await _context.Reservation.Where(r => r.Status == "Canceled").CountAsync();

            int roomAvailable = 0;
            var countDataRooms = await _context.Bedroom.ToListAsync();

            foreach(var room in countDataRooms)
            {
                roomAvailable = roomAvailable + room.Stocks;
            }

            ViewData["countDataReservations"] = countDataReservations;
            ViewData["countDataFinished"] = countDataFinished;
            ViewData["countDataCanceled"] = countDataCanceled;
            ViewData["countDataRooms"] = roomAvailable;

            return View();
        } 

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            // ga nampilin reservasi kalo dah lewat tanggalnya
            // auto merubah status room nya

            DateTime TodayDate = DateTime.Today;
            List<Reservation> reservations = _context.Reservation.ToList();
            List<Bedroom> rooms = _context.Bedroom.ToList();

            foreach (var reservation in reservations)
            {
                if (TodayDate > reservation.EndDate && reservation.Status == "Ordered")
                {
                    reservation.Status = "Finished";

                    foreach (var room in rooms)
                    {
                        if (room.Name == reservation.Room)
                        {
                            room.Stocks = room.Stocks + 1;
                        }
                    }
                    await _context.SaveChangesAsync();
                }
            }

            var dataReservation = await _context.Reservation.Where(r => r.Status == "Ordered").ToListAsync();       

            return View(dataReservation); // mengembalikan data reservasi yang masih dipesan
        }

        public async Task<IActionResult> HistoryReservations()
        {
            var reservations = await _context.Reservation.Where(r => r.Status != "Ordered").ToListAsync();

            return View(reservations);
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reservation == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public async Task<IActionResult> Create()
        {
            var roomAvailable = await _context.Bedroom.Where(r => r.Stocks > 0).ToListAsync();

            return View(roomAvailable);
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Room,StartDate,EndDate,Status,PriceTotal")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                var roomData = _context.Bedroom.Single(r => r.Name == reservation.Room);
                var numberOfDays = (reservation.EndDate - reservation.StartDate).TotalDays;

                double priceTotal = (double)((numberOfDays + 1) * roomData.Price);
                
                reservation.PriceTotal = priceTotal;
                reservation.Status = "Ordered";
                roomData.Stocks = roomData.Stocks - 1;
           
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reservation == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Room,StartDate,EndDate,Status,PriceTotal")] Reservation reservation)
        {
            // PR - hanya boleh extend
            // gaboleh ganti nama, tgl checkin, ganti ruangan
            // kalo extend, harga ganti juga
            // status ganti extend

            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
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
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reservation == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reservation == null)
            {
                return Problem("Entity set 'resortyContext.Reservation'  is null.");
            }
            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation != null)
            {
                var roomData = _context.Bedroom.Single(r => r.Name == reservation.Room);
                roomData.Stocks = roomData.Stocks + 1;
                _context.Reservation.Remove(reservation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
          return _context.Reservation.Any(e => e.Id == id);
        }

        // ini jadi cancel
        // GET method
        public async Task<IActionResult> Cancel(int? id)
        {
            if (id == null || _context.Reservation == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation.FindAsync(id);
            reservation.Status = "Canceled";

            if (reservation == null)
            {
                return NotFound();
            }

            var roomData = _context.Bedroom.Single(r => r.Name == reservation.Room);
            roomData.Stocks = roomData.Stocks + 1;

            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }
    }
}
