using AtlantBLL.Interfaces;
using AtlantWeb.Models;
using AutoMapper;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
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


        public FileStreamResult GetPDF(string id)
        {
            MemoryStream workStream = new MemoryStream();
            Document document = new Document();
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            document.Open();
            var paragraphName = new Paragraph("Details");
            paragraphName.Alignment = Element.ALIGN_CENTER;

            var paragraphDate = new Paragraph(DateTime.Now.ToString());
            paragraphDate.Alignment = Element.ALIGN_CENTER;

            document.Add(paragraphName);
            document.Add(paragraphDate);
            document.Add(new Chunk("\n"));

            PdfPTable table = new PdfPTable(7);
            table.TotalWidth = 550f;
            table.LockedWidth = true;
            float[] widths = new float[] { 50f, 100f, 100f, 50f, 50f, 100f, 100f };
            table.SetWidths(widths);
            table.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell("Id");
            table.AddCell("Code");
            table.AddCell("DetailName");
            table.AddCell("Amount");
            table.AddCell("IsSpecial");
            table.AddCell("AddDate");
            table.AddCell("StockmenName");

            var details = atlantDbService.GetDetails();
            if (!String.IsNullOrEmpty(id))
                details = from det in details where det.Code == id select det;

            foreach (var detail in details)
            {
                table.AddCell(detail.DetailId.ToString());
                table.AddCell(detail.Code);
                table.AddCell(detail.Name);
                table.AddCell(detail.Amount.ToString());
                table.AddCell(detail.Special? "yes" : "no");
                table.AddCell(detail.AddDate.Value.ToString("dd.MM.yyyy"));
                table.AddCell(detail.Stockmen.Name);
            }

            document.Add(table);

            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "details/pdf");
        }
	}
}