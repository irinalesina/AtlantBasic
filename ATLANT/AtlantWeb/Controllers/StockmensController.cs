using AtlantBLL.Interfaces;
using AtlantWeb.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AtlantWeb.Controllers
{
    public class StockmensController : Controller
    {
        IAtlantDBService atlantDbService;

        public StockmensController(IAtlantDBService serv)
        {
            atlantDbService = serv;
        }


        public ActionResult ShowStockmens()
        {
            ViewBag.Title = "Stockmens";

            IEnumerable<AtlantBLL.Models.Stockmen> stockmens = atlantDbService.GetStockmens();
            Mapper.Initialize(cfg => cfg.CreateMap<AtlantBLL.Models.Stockmen, StockmenViewModel>());
            var stockmensView = Mapper.Map<IEnumerable<AtlantBLL.Models.Stockmen>, List<StockmenViewModel>>(stockmens);

            return View(stockmensView);
        }


        public ActionResult CreateStockmen()
        {
            ViewBag.Title = "Create stockmen";

            StockmenViewModel stockmen = new StockmenViewModel();
            Mapper.Initialize(cfg => cfg.CreateMap<AtlantBLL.Models.Stockmen, StockmenViewModel>());

            return View(stockmen);
        }


        [HttpPost]
        public ActionResult CreateStockmen(StockmenViewModel stockmen)
        {
            if (ModelState.IsValid)
            {
                Mapper.Initialize(cfg => cfg.CreateMap<StockmenViewModel, AtlantBLL.Models.Stockmen>());
                var stockmenBLL = Mapper.Map<StockmenViewModel, AtlantBLL.Models.Stockmen>(stockmen);

                atlantDbService.InsertStockmen(stockmenBLL);
                return RedirectToAction("ShowStockmens");
            }
            return View(stockmen);
        }


        public ActionResult DeleteStockmen(string id)
        {
            try
            {
                atlantDbService.DeleteStockmen(Convert.ToInt32(id));

            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("ShowStockmens");
        }
	}
}