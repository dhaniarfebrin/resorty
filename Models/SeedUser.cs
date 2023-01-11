using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using resorty.Data;
using System;
using System.Linq;

namespace resorty.Models
{
    public class SeedUser
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new resortyContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<resortyContext>>()))
            {
                // Look for any movies.
                if (context.User.Any())
                {
                    return;   // DB has been seeded
                }
                context.User.AddRange(
                    new User
                    {
                        Name = "Super Admin",
                        Username = "admin",
                        Password = "admin"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
