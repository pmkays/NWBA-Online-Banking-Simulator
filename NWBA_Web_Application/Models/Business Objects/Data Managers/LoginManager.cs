using Microsoft.EntityFrameworkCore;
using NWBA_Web_Application.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NWBA_Web_Application.Models.Business_Objects
{
    public class LoginManager : IDataRepository<Login, int>
    {
        private readonly NWBAContext _context;

        public LoginManager(NWBAContext context)
        {
            _context = context;
        }
        public async Task<Login> Get(int id)
        {
            var login = await _context.Login.Include("Customer").Where(x => x.CustomerID == id).FirstOrDefaultAsync();
            return login;
        }

        public async Task<Login> GetAlternative(int? id)
        {
            var login = await _context.Login.Include("Customer").Where(x => x.CustomerID == id).FirstOrDefaultAsync();
            return login;
        }

        public async Task<Login> GetFromUserID(string id)
        {
            var login = await _context.Login.Include("Customer").Where(x => x.UserID == id).FirstOrDefaultAsync();
            return login;
        }

        public async Task<IEnumerable<Login>> GetAll()
        {
            var login = await _context.Login.Include("Customer").ToListAsync();
            return login;
        }

        public void Add(Login login)
        {
            _context.Add(login);
            _context.SaveChanges();
        }

        public void Update(Login login)
        {
            _context.Update(login);
            _context.SaveChanges();
        }

        public void Delete(Login login)
        {
            _context.Remove(login);
            _context.SaveChanges();
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
