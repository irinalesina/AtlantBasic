using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtlantWeb.Models
{
    public class DetailViewModel
    {
        public int DetailId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? Amount { get; set; }
        public bool Special { get; set; }
        public DateTime AddDate { get; set; }
        public StockmenViewModel Stockmen { get; set; }
    }
}