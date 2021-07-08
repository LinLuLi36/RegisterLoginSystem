using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegisterLoginSystem.Models
{
    public class Entities: DbContext
    {
        public Entities(DbContextOptions<Entities> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
