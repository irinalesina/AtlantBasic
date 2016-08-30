using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AtlantWeb.Models
{
    public class StockmenViewModel
    {
        public int StockmenId { get; set; }
        [Required(ErrorMessage = "Field is required!")]
        public string Name { get; set; }
        public int DetailCount { get; set; }
    }
}