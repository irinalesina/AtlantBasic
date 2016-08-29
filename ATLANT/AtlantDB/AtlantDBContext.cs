using System;
using System.Collections.Generic;
using System.Data.Entity;
using AtlantDB.Models;

namespace AtlantDB
{
    public class AtlantDBContext : DbContext
    {
        public DbSet<Stockmen> Stockmens { get; set; }
        public DbSet<Detail> Details { get; set; }

        static AtlantDBContext()
        {
            Database.SetInitializer<AtlantDBContext>(new AtlantDBInitializer());
        }
        public AtlantDBContext(string connectionString) : base(connectionString) { }
    }
}
