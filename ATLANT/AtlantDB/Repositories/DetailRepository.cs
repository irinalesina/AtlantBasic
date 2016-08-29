using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AtlantDB.Models;
using AtlantDB.Interfaces;

namespace AtlantDB.Repositories
{
    public class DetailRepository : IRepository<Detail>
    {
        private AtlantDBContext db;

        public DetailRepository(AtlantDBContext context)
        {
            this.db = context;
        }
 
        public IEnumerable<Detail> GetAll()
        {
            return db.Details;
        }
 
        public Detail Get(int id)
        {
            return db.Details.Find(id);
        }
 
        public void Create(Detail detail)
        {
            db.Details.Add(detail);
        }

        public void Update(Detail detail)
        {
            db.Entry(detail).State = EntityState.Modified;
        }
 
        public IEnumerable<Detail> Find(Func<Detail, Boolean> predicate)
        {
            return db.Details.Where(predicate).ToList();
        }
 
        public void Delete(int id)
        {
            Detail detail = db.Details.Find(id);
            if (detail != null)
                db.Details.Remove(detail);
        }
    }
}
