using System;
using System.Collections.Generic;
using AtlantDB.Models;

namespace AtlantDB.Interfaces
{
    public interface IAtlantDBRepository : IDisposable
    {
        IRepository<Detail> Details { get; }
        IRepository<Stockmen> Stockmens { get; }
        void Save();
    }
}
