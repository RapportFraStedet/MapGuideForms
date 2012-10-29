// Copyright 2012, MapGuideForm user group, Frederikssund Kommune and Helsingør Kommune - att. Anette Poulsen and Erling Kristensen
// 
// This file is part of "RapportFraStedet". 
// "RapportFraStedet" is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or any later version.
// "RapportFraStedet" is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License along with "RapportFraStedet". If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using RapportFraStedet.Models;
namespace RapportFraStedet.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RolesController : Controller
    {
        //
        // GET: /Roles/

        public ActionResult Index()
        {
            return View(Roles.GetAllRoles().AsEnumerable<string>());
        }


        //
        // GET: /Roles/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Roles/Create

        [HttpPost]
        public ActionResult Create(RoleModel model)
        {
            try
            {
                Roles.CreateRole(model.RoleName);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Roles/Edit/5
 
        public ActionResult Edit(string roleName)
        {
            RoleModel model = new RoleModel();
            var allUsers = Membership.GetAllUsers();
            string[] userRoles = Roles.GetUsersInRole(roleName);
            model.SelectedUsers = userRoles;
            model.Users = new MultiSelectList(allUsers, "UserName", "UserName" ,userRoles);
            model.RoleName = roleName;
            return View(model);
        }

        //
        // POST: /Roles/Edit/5

        [HttpPost]
        public ActionResult Edit(RoleModel model)
        {
            try
            {
                // TODO: Add update logic here
                string[] users = Roles.GetUsersInRole(model.RoleName);
                if (users.Length > 0)
                {
                    Roles.RemoveUsersFromRole(users, model.RoleName);
                }
                if (model.SelectedUsers != null)
                {
                    Roles.AddUsersToRole(model.SelectedUsers, model.RoleName);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Roles/Delete/5
 
        public ActionResult Delete(string id)
        {
            RoleModel model = new RoleModel();
            model.RoleName = id;
            if (id == "Administrator" || id == "Editor")
                return View("NoDelete",model);
            else
                return View(model);
        }

        //
        // POST: /Roles/Delete/5

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                string[] users = Roles.GetUsersInRole(id);
                if(users.Length > 0)
                {
                    Roles.RemoveUsersFromRole(users, id);
                }
                Roles.DeleteRole(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
