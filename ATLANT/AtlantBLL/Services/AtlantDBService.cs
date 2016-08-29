﻿using System;
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
                throw new ValidationException("Рабочий не найден", "");
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
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
