using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NWBA_Web_Admin.Models.ViewModels
{
    public class GraphViewModel
    {

        public int CustomerID { get; set; }
        public DateTime Date1 { get; set; }
        public DateTime Date2 { get; set; }

        public string GraphType { get; set; }

    }
}
