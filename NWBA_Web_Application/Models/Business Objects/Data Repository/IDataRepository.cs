using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NWBA_Web_Application.Models.Business_Objects
{
    interface IDataRepository<TEntity, TKey>  where TEntity : class
    {
        //although not all methods are used in A2, will come in handy for A3
        void Add(TEntity item);
        void Update(TEntity item);
        void Delete(TEntity item);
        void Save();
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> Get(TKey id);
    }
}
