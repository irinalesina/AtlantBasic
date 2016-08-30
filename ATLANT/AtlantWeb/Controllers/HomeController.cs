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
            ViewBag.Title = "Details";

            IEnumerable<AtlantBLL.Models.Detail> details = atlantDbService.GetDetails();
            Mapper.Initialize(cfg => cfg.CreateMap<AtlantBLL.Models.Detail, DetailViewModel>());
            var detailsView = Mapper.Map<IEnumerable<AtlantBLL.Models.Detail>, List<DetailViewModel>>(details);

            return View(detailsView);
        }


        public ActionResult Stockmens()
        {
            ViewBag.Title = "Stockmens";

            IEnumerable<AtlantBLL.Models.Stockmen> stockmens = atlantDbService.GetStockmens();
            Mapper.Initialize(cfg => cfg.CreateMap<AtlantBLL.Models.Stockmen, StockmenViewModel>());
            var stockmensView = Mapper.Map<IEnumerable<AtlantBLL.Models.Stockmen>, List<StockmenViewModel>>(stockmens);

            return View(stockmensView);
        }


        public ActionResult CreateDetail()
        {
            ViewBag.Title = "Create detail";

            DetailViewModel newDetail = new DetailViewModel();

            IEnumerable<AtlantBLL.Models.Stockmen> stockmens = atlantDbService.GetStockmens();
            Mapper.Initialize(cfg => cfg.CreateMap<AtlantBLL.Models.Stockmen, StockmenViewModel>());
            var stockmensView = Mapper.Map<IEnumerable<AtlantBLL.Models.Stockmen>, List<StockmenViewModel>>(stockmens);
            IEnumerable<SelectListItem> myCollection = stockmensView
                                           .Select(i => new SelectListItem()
                                           {
                                               Text = i.Name,
                                               Value = i.StockmenId.ToString()
                                           });
            ViewData["stockmens"] = myCollection;
            return View(newDetail);
        }


        [HttpPost]
        public string CreateDetail(DetailViewModel detail)
        {
            return "Detail is created";
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
                return RedirectToAction("Stockmens");
            }
            return RedirectToAction("CreateStockmen");
        }

        public ActionResult DeleteStockmen(string id)
        {
            try
            {
                atlantDbService.DeleteStockmen(Convert.ToInt32(id));
                
            }
            catch(Exception ex)
            {

            }
            return RedirectToAction("Stockmens");
        }

        public ActionResult DeleteDetail(string id)
        {
            try
            {
                atlantDbService.DeleteDetail(Convert.ToInt32(id));

            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Details");
        }
    }

}