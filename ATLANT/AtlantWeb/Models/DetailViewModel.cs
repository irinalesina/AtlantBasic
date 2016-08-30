using AtlantWeb.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AtlantWeb.Models
{
    public class DetailViewModel
    {
        public int DetailId { get; set; }

        [Required(ErrorMessage = "Field is required!")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Field is required!")]
        public string Name { get; set; }
        public int? Amount { get; set; }
        public bool Special { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Field is required!")]
        [DateNotWeekend(ErrorMessage ="Date is weekend!")]
        public DateTime? AddDate { get; set; }

        public StockmenViewModel Stockmen { get; set; } 
    }
}