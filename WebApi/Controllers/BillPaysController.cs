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
    public class BillPaysController : ControllerBase
    {
        private BillPayManager _repo;

        public BillPaysController(BillPayManager repo)
        {
            _repo = repo;
        }

        // GET: api/BillPays
        [HttpGet]
        public IEnumerable<BillPay> Get()
        {
            return _repo.GetAll();
        }

        // GET: api/BillPays/5
        [HttpGet("{id}")]
        public BillPay Get(int id)
        {
            return _repo.Get(id);
        }

        //GET: api/BillPaysFromAccount/5
        [HttpGet("BillPaysFromAccount/{id}")]
        public IEnumerable<BillPay> BillPaysFromAccount(int id)
        {

            return _repo.BillPaysFromAccount(id);
        }

        // POST: api/BillPays
        [HttpPost]
        public void Post([FromBody] BillPay customer)
        {
            _repo.Add(customer);
        }

        // PUT: api/BillPays/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] BillPay customer)
        {
            _repo.Update(id, customer);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _repo.Delete(id);
        }
    }
}
