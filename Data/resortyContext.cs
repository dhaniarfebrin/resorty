using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using resorty.Models;

namespace resorty.Data
{
    public class resortyContext : DbContext
    {
        public resortyContext (DbContextOptions<resortyContext> options)
            : base(options)
        {
        }

        public DbSet<resorty.Models.User> User { get; set; } = default!;

       // public DbSet<resorty.Models.Room> Room { get; set; }

        public DbSet<resorty.Models.Reservation> Reservation { get; set; }

        public DbSet<resorty.Models.Bedroom> Bedroom { get; set; }
    }
}
