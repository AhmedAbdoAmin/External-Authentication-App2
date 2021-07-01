using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace External_Authentication.Models
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }
        public string Id { get; set; }
        [Required(ErrorMessage = "Role Name is required.")]
        [DisplayName("Name")]
        public string RoleName { get; set; }
        public string Description { get; set; }
        public ICollection<Permission> Permissions { get; set; }
        public ICollection<RolesHasPermission> RolesHasPermissions { get; set; }
        public Dictionary<string, bool> IsSelectedPermission { get; set; }
        public List<string> Users { get; set; }
    }
}
