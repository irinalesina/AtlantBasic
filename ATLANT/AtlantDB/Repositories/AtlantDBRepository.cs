using System;
using System.Collections.Generic;
using AtlantDB.Models;
using AtlantDB.Interfaces;

namespace AtlantDB.Repositories
{
    public class AtlantDBRepository : IAtlantDBRepository
    {
        private AtlantDBContext db;
        private DetailRepository detailRepository;
        private StockmenRepository stockmenRepository;
        private bool disposed = false;

        public AtlantDBRepository(string connectionString)
        {
            db = new AtlantDBContext(connectionString);
        }
        public IRepository<Detail> Details
        {
            get
            {
                if (detailRepository == null)
                    detailRepository = new DetailRepository(db);
                return detailRepository;
            } 
        }
 
        public IRepository<Stockmen> Stockmens
        {
            get
            {
                if (stockmenRepository == null)
                    stockmenRepository = new StockmenRepository(db);
                return stockmenRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
