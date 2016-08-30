using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AtlantDB.Models;
using AtlantDB.Interfaces;


namespace AtlantDB.Repositories
{
    public class StockmenRepository : IRepository<Stockmen>
    {
        private AtlantDBContext db;

        public StockmenRepository(AtlantDBContext context)
        {
            this.db = context;
        }

        public IEnumerable<Stockmen> GetAll()
        {
            return db.Stockmens;
        }

        public Stockmen Get(int id)
        {
            return db.Stockmens.Find(id);
        }

        public void Create(Stockmen stockmen)
        {
            db.Stockmens.Add(stockmen);
        }

        public void Update(Stockmen stockmen)
        {
            db.Entry(stockmen).State = EntityState.Modified;
        }

        public IEnumerable<Stockmen> Find(Func<Stockmen, Boolean> predicate)
        {
            return db.Stockmens.Where(predicate).ToList();
        }
 
        public void Delete(int id)
        {
            Stockmen stockmen = (from st in db.Stockmens where st.StockmenId == id select st).First();
            if (stockmen != null)
                db.Stockmens.Remove(stockmen);
        }
    }
}
