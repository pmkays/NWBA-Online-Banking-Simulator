using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace NWBA_Web_Application.Models
{
    public class StatementsViewModel
    {

        public IPagedList<Transaction> Transactions { get; set; }
        public int id { get; set; }
    }
}
