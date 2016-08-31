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
            atlantDbService.DeleteStockmen(Convert.ToInt32(id));

            return RedirectToAction("ShowStockmens");
        }


        public FileStreamResult GetPDF()
        {
            MemoryStream workStream = new MemoryStream();
            Document document = new Document();
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            document.Open();
            var paragraphName = new Paragraph("Stockmens");
            paragraphName.Alignment = Element.ALIGN_CENTER;

            var paragraphDate = new Paragraph(DateTime.Now.ToString());
            paragraphDate.Alignment = Element.ALIGN_CENTER;

            document.Add(paragraphName);
            document.Add(paragraphDate);
            document.Add(new Chunk("\n"));

            PdfPTable table = new PdfPTable(3);
            table.TotalWidth = 450f;
            table.LockedWidth = true;
            float[] widths = new float[] { 50f, 250f, 150f };
            table.SetWidths(widths);
            table.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell("Id");
            table.AddCell("Name");
            table.AddCell("DetailsCount");

            foreach (var stockmen in atlantDbService.GetStockmens())
            {
                table.AddCell(stockmen.StockmenId.ToString());
                table.AddCell(stockmen.Name);
                table.AddCell(stockmen.DetailCount.ToString());
            }

            document.Add(table);

            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "stockmens/pdf");
        }
	}
}