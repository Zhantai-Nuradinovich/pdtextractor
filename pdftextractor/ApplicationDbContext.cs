using Microsoft.EntityFrameworkCore;
using pdftextractor.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdftextractor
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Deputy> Deputies { get; set; }
        public DbSet<Law> Laws { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public ApplicationDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS01;Database=pdftextractor;Trusted_Connection=True;");
        }
    }
}
