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
    public class LoginsController : ControllerBase
    {
        private LoginManager _repo;

        public LoginsController(LoginManager repo)
        {
            _repo = repo;
        }

        // GET: api/Logins
        [HttpGet]
        public IEnumerable<Login> Get()
        {
            return _repo.GetAll();
        }

        // GET: api/Logins/5
        [HttpGet("{id}")]
        public Login Get(int id)
        {
            return _repo.Get(id);
        }

        // POST: api/Logins
        [HttpPost]
        public void Post([FromBody] Login customer)
        {
            _repo.Add(customer);
        }

        // PUT: api/Logins/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Login customer)
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
