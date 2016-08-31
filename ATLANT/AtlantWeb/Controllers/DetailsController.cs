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

            var stockmensBLL = atlantDbService.GetStockmens();
            Mapper.Initialize(cfg => cfg.CreateMap<AtlantBLL.Models.Stockmen, StockmenViewModel>());
            var stockmensView = Mapper.Map<IEnumerable<AtlantBLL.Models.Stockmen>, List<StockmenViewModel>>(stockmensBLL);


            SelectList stocmensSL = new SelectList(stockmensView, "StockmenId", "Name");
            ViewBag.Stockmens = stocmensSL;

            return View(newDetail);
        }


        [HttpPost]
        public ActionResult CreateDetail(DetailViewModel detail)
        {
            // TODO: ModelState.IsValid - always false because  ModelState.IsValidField("Stockmen") always false
            bool isModelValidWithoutStockmen = ModelState.IsValidField("DetailId") && ModelState.IsValidField("Code") && ModelState.IsValidField("Name") &&
                ModelState.IsValidField("Amount") && ModelState.IsValidField("Special") && ModelState.IsValidField("AddDate");

            if (isModelValidWithoutStockmen)
            {
                var stockmensBLL = atlantDbService.GetStockmens();
                AtlantBLL.Models.Stockmen stockmen = stockmensBLL.Select(s => s).Where(s => s.StockmenId == detail.Stockmen.StockmenId).First();

                Mapper.CreateMap<StockmenViewModel, AtlantBLL.Models.Stockmen>();
                Mapper.CreateMap<DetailViewModel, AtlantBLL.Models.Detail>().ForMember(x => x.Stockmen, opt => opt.MapFrom(s => stockmen));
                var detailBLL = Mapper.Map<DetailViewModel, AtlantBLL.Models.Detail>(detail);

                atlantDbService.InsertDetail(detailBLL);

                return RedirectToAction("ShowDetails");
            }

            var stockmens = atlantDbService.GetStockmens();
            Mapper.Initialize(cfg => cfg.CreateMap<AtlantBLL.Models.Stockmen, StockmenViewModel>());
            var stockmensView = Mapper.Map<IEnumerable<AtlantBLL.Models.Stockmen>, List<StockmenViewModel>>(stockmens);
            SelectList stocmensSL = new SelectList(stockmensView, "StockmenId", "Name");
            ViewBag.Stockmens = stocmensSL;
            return View(detail);
        }


        public ActionResult DeleteDetail(string id)
        {
            atlantDbService.DeleteDetail(Convert.ToInt32(id));

            return RedirectToAction("ShowDetails");
        }

        public PartialViewResult DetailsTableBody(string code = null)
        {
            IEnumerable<AtlantBLL.Models.Detail> details = atlantDbService.GetDetails();
            if(!String.IsNullOrEmpty(code))
                details = from detail in details where detail.Code == code select detail;
            
            Mapper.CreateMap<AtlantBLL.Models.Stockmen, StockmenViewModel>();
            Mapper.CreateMap<AtlantBLL.Models.Detail, DetailViewModel>().ForMember(dest => dest.Stockmen, opt => opt.MapFrom(src => src.Stockmen));
            var detailsView = Mapper.Map<IEnumerable<AtlantBLL.Models.Detail>, List<DetailViewModel>>(details);

            return PartialView(detailsView); 
        }
	}
}