using System;
using System.Collections.Generic;
using System.Data.Entity;
using AtlantDB.Models;


namespace AtlantDB
{
    public class AtlantDBInitializer : DropCreateDatabaseIfModelChanges<AtlantDBContext>
    {
        protected override void Seed(AtlantDBContext db)
        {
            db.Stockmens.Add(new Stockmen { Name = "Alex" });
            db.Stockmens.Add(new Stockmen { Name = "Max" });
            db.Stockmens.Add(new Stockmen { Name = "Andrew" });
            db.Stockmens.Add(new Stockmen { Name = "Jhon" });
            db.SaveChanges();
        }
    }
}
