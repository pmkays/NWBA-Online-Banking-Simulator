using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Models.DataManagers;
using WebApi.Models.ViewModels;
using System.Globalization;


namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private TransactionManager _repo;

        public TransactionsController(TransactionManager repo)
        {
            _repo = repo;
        }

        // GET: api/Transactions
        [HttpGet]
        public IEnumerable<Transaction> Get()
        {
            return _repo.GetAll();
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public Transaction Get(int id)
        {
            return _repo.Get(id);
        }

        // POST: api/Transactions
        [HttpPost]
        public void Post([FromBody] Transaction customer)
        {
            _repo.Add(customer);
        }

        // PUT: api/Transactions/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Transaction customer)
        {
            _repo.Update(id, customer);
        }

        // DELETE: api/Transactions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _repo.Delete(id);
        }

        //GET: api/transactions/inbar?date1={date1}&date2={date2}
        [HttpGet("inbar")]
        public IEnumerable<TransDateCount> GetBarAll(string date1, string date2)
        {
            return _repo.GetBarAll(DateTime.ParseExact(date1, "dd/MM/yyyy", CultureInfo.InvariantCulture), DateTime.ParseExact(date2, "dd/MM/yyyy", CultureInfo.InvariantCulture));
        }

        //GET: api/transactions/inbarwithid?id={id}&date1={date1}&date2={date2}
        [HttpGet("inbarwithid")]
        public IEnumerable<TransDateCount> GetBar(int id, string date1, string date2)
        {
            return _repo.GetBar(id, DateTime.ParseExact(date1, "dd/MM/yyyy", CultureInfo.InvariantCulture), DateTime.ParseExact(date2, "dd/MM/yyyy", CultureInfo.InvariantCulture));
        }

        //GET: api/transactions/inpie?date1={date1}&date2={date2}
        [HttpGet("inpie")]
        public IEnumerable<TransTypeDateCount> GetPieAll(string date1, string date2)
        {
            return _repo.GetPieAll(DateTime.ParseExact(date1, "dd/MM/yyyy", CultureInfo.InvariantCulture), DateTime.ParseExact(date2, "dd/MM/yyyy", CultureInfo.InvariantCulture));

        }

        //GET: api/transactions/inpiewithid?id={id}&date1={date1}&date2={date2}
        [HttpGet("inpiewithid")]
        public IEnumerable<TransTypeDateCount> GetPie(int id, string date1, string date2)
        {
            return _repo.GetPie(id, DateTime.ParseExact(date1, "dd/MM/yyyy", CultureInfo.InvariantCulture), DateTime.ParseExact(date2, "dd/MM/yyyy", CultureInfo.InvariantCulture));
        }

        //GET: api/transactions/inline?date1={date1}&date2={date2}
        [HttpGet("inline")]
        public IEnumerable<AmountDateCount> GetLineAll(string date1, string date2)
        {
            return _repo.GetLineAll(DateTime.ParseExact(date1, "dd/MM/yyyy", CultureInfo.InvariantCulture), DateTime.ParseExact(date2, "dd/MM/yyyy", CultureInfo.InvariantCulture));

        }

        //GET: api/transactions/inlinewithid?id={id}&date1={date1}&date2={date2}
        [HttpGet("inlinewithid")]
        public IEnumerable<AmountDateCount> GetLine(int id, string date1, string date2)
        {
            return _repo.GetLine(id, DateTime.ParseExact(date1, "dd/MM/yyyy", CultureInfo.InvariantCulture), DateTime.ParseExact(date2, "dd/MM/yyyy", CultureInfo.InvariantCulture));
        }

        //GET: api/transactions/intable?date1={date1}&date2={date2}
        [HttpGet("intable")]
        public IEnumerable<TransactionView> GetTableAll(string date1, string date2)
        {
            return _repo.GetTableAll(DateTime.ParseExact(date1, "dd/MM/yyyy", CultureInfo.InvariantCulture), DateTime.ParseExact(date2, "dd/MM/yyyy", CultureInfo.InvariantCulture));
        }

        //GET: api/transactions/intablewithid?id={id}&date1={date1}&date2={date2}
        [HttpGet("intablewithid")]
        public IEnumerable<TransactionView> GetTable(int id, string date1, string date2)
        {
            return _repo.GetTable(id, DateTime.ParseExact(date1, "dd/MM/yyyy", CultureInfo.InvariantCulture), DateTime.ParseExact(date2, "dd/MM/yyyy", CultureInfo.InvariantCulture));
        }
    }
}
