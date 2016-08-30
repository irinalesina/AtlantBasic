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
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public int? Amount { get; set; }
        public bool Special { get; set; }
        [DataType(DataType.Date)]
        [Required]
        public DateTime? AddDate { get; set; }
        [Required]
        public StockmenViewModel Stockmen { get; set; } 
    }
}