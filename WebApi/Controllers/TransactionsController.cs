using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Models.DataManagers;

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

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _repo.Delete(id);
        }

        [HttpGet]
        public IEnumerable<int> GetRangeAll(string date1, string date2)
        {
            return _repo.GetRangeAll(DateTime.Parse(date1), DateTime.Parse(date2));
        }

        [HttpGet]
        public IEnumerable<int> GetRange(int id, string date1, string date2)
        {
            return _repo.GetRange(id, DateTime.Parse(date1), DateTime.Parse(date2));
        }
    }
}
