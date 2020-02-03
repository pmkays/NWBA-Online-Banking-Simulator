using X.PagedList;

namespace NWBA_Web_Application.Models
{
    public class BillPayViewModel
    {
        public IPagedList<BillPay> BillPays { get; set; }
        public int id { get; set; }
    }
}