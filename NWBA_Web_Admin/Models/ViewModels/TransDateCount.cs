using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NWBA_Web_Admin.Models.ViewModels
{
    public class TransDateCount
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }

        public TransDateCount (DateTime date, int count)
        {
            this.Date = date;
            this.Count = count;
        }
      
    }
}
