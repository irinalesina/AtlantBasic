using System;
using System.Collections.Generic;
using System.Linq;
using AtlantBLL.Interfaces;
using AtlantBLL.Models;
using AtlantDB.Interfaces;
using AtlantBLL.Infrastructure;
using AutoMapper;

namespace AtlantBLL.Services
{
    public class AtlantDBService : IAtlantDBService
    {
        IAtlantDBRepository db { get; set; }
 
        public AtlantDBService(IAtlantDBRepository db)
        {
            this.db = db;
        }
        public int GetDetailsAmount(Stockmen stockmen)
        {
            if(stockmen == null)
                throw new ValidationException("Stockmen not found", "");
            var detailCount = 0;
            foreach (var i in (from detail in db.Details.GetAll() where detail.Stockmen.StockmenId == stockmen.StockmenId select detail.Amount).ToList())
                detailCount += (int)i;
            return detailCount;
        }

        public IEnumerable<Stockmen> GetStockmens()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<AtlantDB.Models.Stockmen, Stockmen>());
            var stockmens = Mapper.Map<IEnumerable<AtlantDB.Models.Stockmen>, List<Stockmen>>(db.Stockmens.GetAll());
            foreach (var stockmen in stockmens)
                GetDetailsAmount(stockmen);
            return stockmens;
        }

        public IEnumerable<Detail> GetDetails()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<AtlantDB.Models.Detail, Detail>());
            var details = Mapper.Map<IEnumerable<AtlantDB.Models.Detail>, List<Detail>>(db.Details.GetAll());
            return details;
        }

        public void InsertStockmen(Stockmen st)
        {
            if (st == null)
                throw new ValidationException("Stockmen not found", "");

            Mapper.Initialize(cfg => cfg.CreateMap<Stockmen, AtlantDB.Models.Stockmen>());
            var stView = Mapper.Map<Stockmen, AtlantDB.Models.Stockmen>(st);

            db.Stockmens.Create(stView);
            db.Save();
        }

        public void InsertDetail(Detail detail)
        {
            throw new NotImplementedException();
        }

        public void DeleteDetail(int id)
        {
            var detail = db.Details.Find(s => s.DetailId == id);
            if (detail == null)
                throw new InvalidOperationException("Detail not found");

            db.Details.Delete(id);
            db.Save();
        }

        public void DeleteStockmen(int id)
        {
            var stockmen = db.Stockmens.Find(s => s.StockmenId == id);
            if (stockmen == null)
                throw new InvalidOperationException("Stockmen not found");

            var details = from detail in db.Details.GetAll() where detail.Stockmen.StockmenId == id select detail;
            if(details.Count() != 0)
                throw new InvalidOperationException("Stockmen has details");

            db.Stockmens.Delete(id);
            db.Save();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
