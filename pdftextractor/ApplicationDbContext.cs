﻿using Microsoft.EntityFrameworkCore;
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
        public DbSet<TDeputy> Deputies { get; set; }
        public DbSet<TLaw> Laws { get; set; }
        public DbSet<TVote> Votes { get; set; }
        public DbSet<TLawsAmendment> LawsAmendments { get; set; }
        public ApplicationDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=PanakotaDb;Trusted_Connection=True;");
        }
    }
}
