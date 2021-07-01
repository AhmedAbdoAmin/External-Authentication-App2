using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace External_Authentication.Models
{
    public class CreatePermissionViewModel
    {
        public int Id { get; set; }
        [Required]
        [Remote(action: "IsTitleInUse", controller: "Permission")]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
