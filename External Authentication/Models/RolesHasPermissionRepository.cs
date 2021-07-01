using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace External_Authentication.Models
{
    public class RolesHasPermissionRepository : IPermissionStoreRepository<RolesHasPermission>
    {
        private readonly AppDbContext _db;

        public RolesHasPermissionRepository(AppDbContext db)
        {
            _db = db;
        }
        public void Add(RolesHasPermission entity)
        {
            _db.RolesHasPermissions.Add(entity);
            _db.SaveChanges();
        }

        public void Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public void Delete(int PersissionId, string roleid)
        {
            var rp = Find(PersissionId, roleid);
            _db.RolesHasPermissions.Remove(rp);
            _db.SaveChanges();
        }

        public RolesHasPermission Find(int Id)
        {
            throw new NotImplementedException();
        }
        public RolesHasPermission Find(int PersissionId,string roleid)
        {
            return _db.RolesHasPermissions.SingleOrDefault(rp => rp.Permission_Id == PersissionId && rp.RoleId == roleid);
        }
        public RolesHasPermission FindTitle(string Title)
        {
            throw new NotImplementedException();
        }

        public IList<RolesHasPermission> List()
        {
            return _db.RolesHasPermissions.ToList();
        }
        public IList<RolesHasPermission> ListByRoleId(string RoleId)
        {
            return _db.RolesHasPermissions.Where(r => r.RoleId == RoleId).ToList(); 
        }


        public RolesHasPermission Update(RolesHasPermission entity)
        {
            var rolehaspersion= Find(entity.Permission_Id, entity.RoleId);
            if (rolehaspersion == null)
            {
                _db.RolesHasPermissions.Remove(entity);
            }
            _db.RolesHasPermissions.Update(rolehaspersion);
            return rolehaspersion;
        }
    }
}
