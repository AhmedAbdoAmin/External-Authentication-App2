using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace External_Authentication.Models
{
    public class RolesHasPermission
    {
        public int Permission_Id { get; set; }
        public Permission Permission { get; set; }
        public string RoleId { get; set; }
        public ManagerUserRole ManagerUserRole { get; set; }
    }
}
