using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace External_Authentication.Models
{
    public class ManagerUserRole : IdentityRole
    {
        public string Description { get; set; }
        public ICollection<RolesHasPermission> RolesHasPermissions { get; set; }
    }
}
