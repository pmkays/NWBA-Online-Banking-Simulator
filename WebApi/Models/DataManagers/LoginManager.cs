using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models.DataRepository;

namespace WebApi.Models.DataManagers
{
    public class LoginManager : IDataRepository<Login, int>
    {
        public NWBAContext _context;

        public LoginManager(NWBAContext context)
        {
            _context = context;
        }
        public int Add(Login item)
        {
            _context.Login.Add(item);
            _context.SaveChanges();
            //can't return userid since that's a string
            return item.CustomerId;
        }

        public int Delete(int id)
        {
            _context.Login.Remove(this.Get(id));
            _context.SaveChanges();
            return id;
        }

        public Login Get(int id)
        {
            return _context.Login.Include(x => x.Customer).Where(x => x.CustomerId == id).FirstOrDefault();
        }

        public IEnumerable<Login> GetAll()
        {
            return _context.Login.Include(x => x.Customer).ToList();
        }

        public int Update(int id, Login item)
        {
            _context.Login.Update(item);
            _context.SaveChanges();
            return id;
        }
    }
}
