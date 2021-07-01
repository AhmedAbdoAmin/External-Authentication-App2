using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace External_Authentication.Models
{
    public class CreateRoleViewModel
    { 
        [Required]
        [DisplayName("Name")]
        [Remote(action: "IsNameRoleInUse", controller: "Administration")]
        public string RoleName { get; set; }
        [Required]
        public string Description { get; set; }
        public ICollection<Permission> Permissions { get; set; }
        public Dictionary<string,bool> IsSelectedPermission { get; set; }
    }
}
