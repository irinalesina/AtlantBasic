using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AtlantDB.Models
{
    public class Detail
    {
        [Key]
        public int DetailId { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public int? Amount { get; set; }
        public bool Special { get; set; }
        [Required]
        public DateTime AddDate { get; set; }
        [Required]
        public virtual Stockmen Stockmen { get; set; }
    }
}
