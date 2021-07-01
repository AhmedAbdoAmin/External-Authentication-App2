using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace External_Authentication.Models
{
    public interface IPermissionStoreRepository<TEntity>
    {
        IList<TEntity> List();
        IList<TEntity> ListByRoleId(string RoleId);
        TEntity Find(int Id);
        TEntity Find(int PersissionId, string roleid);
        TEntity FindTitle(string Title);
        void Add(TEntity entity);
        TEntity Update(TEntity entity);
        void Delete(int Id);
        void Delete(int PersissionId, string roleid);

    }
}
