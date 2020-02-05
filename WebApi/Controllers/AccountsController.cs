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
    public class AccountsController : ControllerBase
    {
        private AccountManager _repo;

        public AccountsController(AccountManager repo)
        {
            _repo = repo;
        }

        // GET: api/Accounts
        [HttpGet]
        public IEnumerable<Account> Get()
        {
            return _repo.GetAll();
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        public Account Get(int id)
        {
            return _repo.Get(id);
        }

        [HttpGet("AccountFromCustomer/{id}")]
        public IEnumerable<Account> AccountFromCustomer(int id)
        {
          
            return _repo.GetAccountFromCustomer(id);
        }

        // POST: api/Accounts
        [HttpPost]
        public void Post([FromBody] Account customer)
        {
            _repo.Add(customer);
        }

        // PUT: api/Accounts/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Account customer)
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
