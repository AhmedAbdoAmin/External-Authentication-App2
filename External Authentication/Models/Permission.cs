using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace External_Authentication.Models
{
    public class Permission
    {
        [Key]
        [DisplayName("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Permission_Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<RolesHasPermission> RolesHasPermissions { get; set; }

    }
}
