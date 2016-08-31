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
    public class DetailsController : Controller
    {
        IAtlantDBService atlantDbService;

        public DetailsController(IAtlantDBService serv)
        {
            atlantDbService = serv;
        }


        public ActionResult ShowDetails()
        {
            ViewBag.Title = "Details";

            IEnumerable<AtlantBLL.Models.Detail> details = atlantDbService.GetDetails();
            Mapper.CreateMap<AtlantBLL.Models.Stockmen, StockmenViewModel>();
            Mapper.CreateMap<AtlantBLL.Models.Detail, DetailViewModel>().ForMember(dest => dest.Stockmen, opt => opt.MapFrom(src => src.Stockmen));
            var detailsView = Mapper.Map<IEnumerable<AtlantBLL.Models.Detail>, List<DetailViewModel>>(details);

            return View(detailsView);
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


        [HttpPost]
        public ActionResult CreateDetail(DetailViewModel detail)
        {
            bool isModelValidWithoutStockmen = ModelState.IsValidField("DetailId") && ModelState.IsValidField("Code") && ModelState.IsValidField("Name") &&
                ModelState.IsValidField("Amount") && ModelState.IsValidField("Special") && ModelState.IsValidField("AddDate");
            if (isModelValidWithoutStockmen)
            {
                Mapper.CreateMap<DetailViewModel, AtlantBLL.Models.Detail>().ForMember(x => x.Stockmen, opt => opt.Ignore());
                var detailBLL = Mapper.Map<DetailViewModel, AtlantBLL.Models.Detail>(detail);
                var stockmensRes = from st in atlantDbService.GetStockmens() where st.StockmenId == detail.Stockmen.StockmenId select st;
                detailBLL.Stockmen = stockmensRes.First();
                atlantDbService.InsertDetail(detailBLL);
                var f = atlantDbService.GetStockmens();
                return RedirectToAction("ShowDetails");
            }
            IEnumerable<AtlantBLL.Models.Stockmen> stockmens = atlantDbService.GetStockmens();
            Mapper.Initialize(cfg => cfg.CreateMap<AtlantBLL.Models.Stockmen, StockmenViewModel>());
            var stockmensView = Mapper.Map<IEnumerable<AtlantBLL.Models.Stockmen>, List<StockmenViewModel>>(stockmens);
            SelectList stocmensSL = new SelectList(stockmensView, "StockmenId", "Name");
            ViewBag.Stockmens = stocmensSL;
            return View(detail);
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
            return RedirectToAction("ShowDetails");
        }
	}
}