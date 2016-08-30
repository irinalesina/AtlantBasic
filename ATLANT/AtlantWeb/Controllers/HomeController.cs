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
            newDetail.Stockmen = new StockmenViewModel();

            IEnumerable<AtlantBLL.Models.Stockmen> stockmens = atlantDbService.GetStockmens();
            Mapper.Initialize(cfg => cfg.CreateMap<AtlantBLL.Models.Stockmen, StockmenViewModel>());
            var stockmensView = Mapper.Map<IEnumerable<AtlantBLL.Models.Stockmen>, List<StockmenViewModel>>(stockmens);


            SelectList stocmensSL = new SelectList(stockmensView, "StockmenId", "Name");
            ViewBag.Stockmens = stocmensSL;

            return View(newDetail);
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

        [HttpPost]
        public ActionResult CreateDetail(DetailViewModel detail)
        { //ModelState.IsValid
            if (true)
            {
                Mapper.CreateMap<DetailViewModel, AtlantBLL.Models.Detail>().ForMember(x => x.Stockmen, opt => opt.Ignore());
                var detailBLL = Mapper.Map<DetailViewModel, AtlantBLL.Models.Detail>(detail);
                var stockmensRes = from st in atlantDbService.GetStockmens() where st.StockmenId == detail.Stockmen.StockmenId select st;
                detailBLL.Stockmen = stockmensRes.First();
                atlantDbService.InsertDetail(detailBLL);
                return RedirectToAction("Details");
            }
            return RedirectToAction("CreateDetail");
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