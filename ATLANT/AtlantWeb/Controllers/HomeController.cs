using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AtlantBLL.Interfaces;
using AutoMapper;
using AtlantWeb.Models;

namespace AtlantWeb.Controllers
{
    public class HomeController : Controller
    {
        IAtlantDBService atlantDbService;

        public HomeController(IAtlantDBService serv)
        {
            atlantDbService = serv;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details()
        {
            ViewBag.Title = "Details info";

            IEnumerable<AtlantBLL.Models.Detail> details = atlantDbService.GetDetails();
            Mapper.Initialize(cfg => cfg.CreateMap<AtlantBLL.Models.Detail, DetailViewModel>());
            var detailsView = Mapper.Map<IEnumerable<AtlantBLL.Models.Detail>, List<DetailViewModel>>(details);

            return View(detailsView);
        }

        public ActionResult Stockmens()
        {
            ViewBag.Title = "Stockmens info";

            IEnumerable<AtlantBLL.Models.Stockmen> stockmens = atlantDbService.GetStockmens();
            Mapper.Initialize(cfg => cfg.CreateMap<AtlantBLL.Models.Stockmen, StockmenViewModel>());
            var stockmensView = Mapper.Map<IEnumerable<AtlantBLL.Models.Stockmen>, List<StockmenViewModel>>(stockmens);

            return View(stockmensView);
        }
    }
}