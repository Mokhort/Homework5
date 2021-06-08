using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Homework3
{
    class Context : DbContext
    {
        public DbSet<Route> Route { get; set; }
        public DbSet<Airport> Airport { get; set; }
        public DbSet<Flight> Flight { get; set; }
        public DbSet<Passanger> Passanger { get; set; }
        public DbSet<Plane> Plane { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=LAPTOP-BV1FJHR6;Database=AeroStore;Trusted_Connection=True;");
        }
    }
}