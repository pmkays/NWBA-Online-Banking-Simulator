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
    public class PayeesController : ControllerBase
    {
        private PayeeManager _repo;

        public PayeesController(PayeeManager repo)
        {
            _repo = repo;
        }

        // GET: api/Payees
        [HttpGet]
        public IEnumerable<Payee> Get()
        {
            return _repo.GetAll();
        }

        // GET: api/Payees/5
        [HttpGet("{id}")]
        public Payee Get(int id)
        {
            return _repo.Get(id);
        }

        // POST: api/Payees
        [HttpPost]
        public void Post([FromBody] Payee customer)
        {
            _repo.Add(customer);
        }

        // PUT: api/Payees/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Payee customer)
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
