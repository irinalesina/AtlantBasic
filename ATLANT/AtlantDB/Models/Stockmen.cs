using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AtlantDB.Models
{
    public class Stockmen
    {
        [Key]
        public int StockmenId { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual List<Detail> Details { get; set; }
    }
}
