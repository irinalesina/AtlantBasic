using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlantBLL.Models
{
    public class Detail
    {
        public int DetailId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? Amount { get; set; }
        public bool Special { get; set; }
        public DateTime AddDate { get; set; }
        public Stockmen Stockmen { get; set; }
    }
}
