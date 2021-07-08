using Microsoft.EntityFrameworkCore;
using RegisterLoginSystem.Models;

namespace RegisterLoginSystem.Dal
{
    public class Entities : DbContext
    {
        public Entities(DbContextOptions<Entities> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
