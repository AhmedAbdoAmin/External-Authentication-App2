using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace External_Authentication.Models
{

    public class PermissionRepository : IPermissionStoreRepository<Permission>
    {
        private readonly AppDbContext _db;

        public PermissionRepository(AppDbContext db)
        {
           _db = db;
        }
        public void Add(Permission entity)
        {
            _db.Permissions.Add(entity);
            _db.SaveChanges();
        }

        public void Delete(int Id)
        {
            var Permission = Find(Id);
            _db.Permissions.Remove(Permission);
            _db.SaveChanges();
        }

        public void Delete(int PersissionId, string roleid)
        {
            throw new NotImplementedException();
        }

        public Permission Find(int Id)
        {
            var permission = _db.Permissions.SingleOrDefault(p => p.Permission_Id == Id);
            return permission;
        }

        public Permission Find(int PersissionId, string roleid)
        {
            throw new NotImplementedException();
        }

        public Permission FindTitle(string Title)
        {
            var permission = _db.Permissions.SingleOrDefault(p => p.Title == Title);
            return permission;
        }

        public IList<Permission> List()
        {
            return _db.Permissions.ToList();
        }

        public IList<Permission> ListByRoleId(string RoleId)
        {
            throw new NotImplementedException();
        }

        public Permission Update(Permission entity)
        {
            var oldP = Find(entity.Permission_Id);
            if (oldP.Title != entity.Title)
            {
                oldP.Title = entity.Title;
            }
            oldP.Description = entity.Description;
            _db.SaveChanges();
            return entity;
        }

      
    }
}
