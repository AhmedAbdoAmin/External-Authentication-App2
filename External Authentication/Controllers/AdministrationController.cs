using External_Authentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace External_Authentication.Controllers
{
    [Authorize(policy: "AdminRolePolicy")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<ManagerUserRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPermissionStoreRepository<Permission> permissionStoreRepository;
        private readonly IPermissionStoreRepository<RolesHasPermission> roleHasPermissionsDb;

        public AdministrationController(RoleManager<ManagerUserRole> roleManager,
                                        UserManager<ApplicationUser> userManager,
                                        IPermissionStoreRepository<Permission> permissionStoreRepository,
                                        IPermissionStoreRepository<RolesHasPermission> RoleHasPermissionsDb)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.permissionStoreRepository = permissionStoreRepository;
            roleHasPermissionsDb = RoleHasPermissionsDb;
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            var Permissions = permissionStoreRepository.List();

            CreateRoleViewModel model = new CreateRoleViewModel { 
                Permissions = Permissions
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                ManagerUserRole userRole = new ManagerUserRole {
                    Name=model.RoleName,
                    Description = model.Description
                };
                model.Permissions = permissionStoreRepository.List();

                var r = model.IsSelectedPermission.Where(t => t.Value == true);
                if (r.Any())
                {
                    IdentityResult result = await roleManager.CreateAsync(userRole);

                    foreach (KeyValuePair<string, bool> item in model.IsSelectedPermission.Where(t => t.Value == true))
                    {
                        var p = permissionStoreRepository.FindTitle(item.Key);
                        RolesHasPermission rolesHasPermission = new RolesHasPermission
                        {
                            RoleId = userRole.Id,
                            Permission_Id = p.Permission_Id
                        };
                        roleHasPermissionsDb.Add(rolesHasPermission);
                    }
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles", "Administration");
                    }
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                else
                    ViewBag.ErrorPermission = "You must select one permission!";
                
            }
            return View(model);
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsNameRoleInUse(string RoleName)
        {
            var role = await roleManager.FindByNameAsync(RoleName);
            if (role == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Name : {RoleName} is already in use");
            }
        }
        [HttpGet]
        public IActionResult ListRoles()
        {
            var model = roleManager.Roles;
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> EditRole(string Id)
        {
            var role = await roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {Id} cannot be found";
                return View("NotFound");
            }
            var PermissionsofRoleId = roleHasPermissionsDb.ListByRoleId(Id);
            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name,
                Description = role.Description,
                Permissions = permissionStoreRepository.List(),
                RolesHasPermissions = PermissionsofRoleId
            };
            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
           
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;
                role.Description = model.Description;

                var result = await roleManager.UpdateAsync(role);
                
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                foreach (KeyValuePair<string, bool> item in model.IsSelectedPermission)
                {
                    var p = permissionStoreRepository.FindTitle(item.Key);
                    var isExistPermission = roleHasPermissionsDb.Find(p.Permission_Id, role.Id);
                    RolesHasPermission rolesHasPermission = new RolesHasPermission
                    {
                        RoleId = model.Id,
                        Permission_Id = p.Permission_Id
                    };
                    if (isExistPermission == null && item.Value == true)
                    {
                        roleHasPermissionsDb.Add(rolesHasPermission);
                    }
                    else if (isExistPermission != null && item.Value == false)
                    {
                        roleHasPermissionsDb.Delete(p.Permission_Id,model.Id);
                    }
                   
                }
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRoles(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }
            var model = new List<UserRoleViewModel>();
            foreach (var user in userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }
                model.Add(userRoleViewModel);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUsersInRoles(List<UserRoleViewModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }
            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null;
                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                    continue;
                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                    {
                        continue;
                    }

                }
            }

            return RedirectToAction("EditRole", new { Id = roleId });
        }
        [HttpPost]
        [Authorize(policy: "DeleteRolePolicy")]
        public async Task<IActionResult> DeleteRole(string Id)
        {
            var role = await roleManager.FindByIdAsync(Id);
            var roleHasPermissions =  roleHasPermissionsDb.ListByRoleId(Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                if (roleHasPermissions != null)
                {
                    foreach (var item in roleHasPermissions)
                    {
                        roleHasPermissionsDb.Delete(item.Permission_Id, role.Id);
                    }
                }
                var result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View("ListRoles");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ListUsers()
        {
            var users = userManager.Users;
            return View(users);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();

        }
    }
}
