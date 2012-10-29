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
using RapportFraStedet.Models;
using System.Web.Routing;

namespace RapportFraStedet.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class PermissionsController : Controller
    {
        public RepositoryPermissions Repository { get; set; }
        protected override void Initialize(RequestContext requestContext)
        {
            if (Repository == null) { Repository = new RepositoryPermissions(); }
            base.Initialize(requestContext);
        }
        //
        // GET: /Permissions/

        public ActionResult Index(int id)
        {
            RepositoryViews repository = new RepositoryViews();
            ViewBag.View = repository.Get(id);
            return View(Repository.GetPermissions(id));
        }

        
        //
        // GET: /Permissions/Create

        public ActionResult Create(int id)
        {
            RepositoryViews repository = new RepositoryViews();
            Permission model = new Permission();
            model.ViewId = id;
            model.View = repository.Get(id);
            ViewBag.PermissionTypes = new SelectList(Repository.GetPermissionTypes(), "PermissionTypeId", "Name");
            return View(model);
        } 

        //
        // POST: /Permissions/Create

        [HttpPost]
        public ActionResult Create(Permission model, FormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Permission item = new Permission();
                    item.ViewId = model.ViewId;
                    item.FieldId = model.FieldId;
                    item.PermissionTypeId = model.PermissionTypeId;
                    Repository.Add(item);
                    Repository.Save();
                    return RedirectToAction("Index", new { id = model.ViewId });
                }
                ViewBag.PermissionTypes = new SelectList(Repository.GetPermissionTypes(), "PermissionTypeId", "Name");
                return View(model);

            }
            catch
            {
                Permission model2 = new Permission();
                model2.ViewId = model.ViewId;
                ViewBag.PermissionTypes = new SelectList(Repository.GetPermissionTypes(), "PermissionTypeId", "Name");
                return View(model2);
            }
        }
        
        //
        // GET: /Permissions/Edit/5

        public ActionResult Edit(int id)
        {
            ViewBag.PermissionTypes = new SelectList(Repository.GetPermissionTypes(), "PermissionTypeId", "Name");
            return View(Repository.Get(id));
        }

        //
        // POST: /Permissions/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                Permission model = Repository.Get(id);
                if (TryUpdateModel(model))
                {
                    Repository.Save();
                    return RedirectToAction("Index", new { id = model.ViewId });
                }
                ViewBag.PermissionTypes = new SelectList(Repository.GetPermissionTypes(), "PermissionTypeId", "Name");
                return View(model);
            }
            catch
            {
                ViewBag.PermissionTypes = new SelectList(Repository.GetPermissionTypes(), "PermissionTypeId", "Name");
                return View(Repository.Get(id));
            }
        }

        //
        // GET: /Permissions/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(Repository.Get(id));
        }

        //
        // POST: /Permissions/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Permission model = Repository.Get(id);
                int viewId = model.ViewId;
                Repository.Delete(model);
                Repository.Save();
                return RedirectToAction("Index", new { id = viewId });
            }
            catch
            {
                return View(Repository.Get(id));
            }
        }

        public ActionResult Fields(int formId)
        {
            RepositoryForms repository = new RepositoryForms();
            Form form = repository.Get(formId);
            return Json(form.Fields.Select(x => new { value = x.FieldId, text = x.Name }), JsonRequestBehavior.AllowGet);
        }
    }
}
