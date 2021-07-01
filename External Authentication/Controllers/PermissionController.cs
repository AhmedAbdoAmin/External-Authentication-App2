using External_Authentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace External_Authentication.Controllers
{
    [Authorize(policy: "AdminRolePolicy")]
    public class PermissionController : Controller
    {
        private readonly IPermissionStoreRepository<Permission> permissionDb;

        public PermissionController(IPermissionStoreRepository<Permission> permissionStoreRepository)
        {
            this.permissionDb = permissionStoreRepository;
        }
        public IActionResult ListPermission()
        {
            var Permissions = permissionDb.List();

            return View(Permissions);
        }
        [HttpGet]
        public IActionResult CreatePermission()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreatePermission(CreatePermissionViewModel model)
        {
            if (ModelState.IsValid)
            {
                Permission newPermission = new Permission
                {
                    Title = model.Title,
                    Description = model.Description
                };
                permissionDb.Add(newPermission);
                return Redirect("ListPermission");
            }
            return View(model);
        }
        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public IActionResult IsTitleInUse(string Title)
        {
            var permission = permissionDb.FindTitle(Title);
            if (permission == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Title {Title} is already in use");
            }
        }

        [HttpGet]
        public IActionResult EditPermission(int Id)
        {
            var p = permissionDb.Find(Id);
            return View(p);
        }
        [HttpPost]
        public IActionResult EditPermission(Permission model)
        {
            if (ModelState.IsValid)
            {
                permissionDb.Update(model);
                return RedirectToAction("ListPermission");
            }
            return View(model);
        }
        [HttpPost]
        [Authorize(policy: "DeleteRolePolicy")]
        public IActionResult DeletePermission(int Id)
        {
            if (ModelState.IsValid)
            {
                permissionDb.Delete(Id);
            }
            return RedirectToAction("ListPermission");
        }

    }
}
