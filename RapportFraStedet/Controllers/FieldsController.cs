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
    public class FieldsController : Controller
    {
        public RepositoryFields Repository { get; set; }
        protected override void Initialize(RequestContext requestContext)
        {
            if (Repository == null) { Repository = new RepositoryFields(); }
            base.Initialize(requestContext);
        }
        //
        // GET: /Fields/

        public ActionResult Index(int id)
        {
            ViewBag.FormId = id;
            return View(Repository.GetFields(id));
        }

        //
        // GET: /Fields/Create

        public ActionResult Create(int id)
        {
            Field model = new Field();
            model.FormId = id;
            ViewBag.FieldTypes = new SelectList(Repository.GetFieldTypes(), "FieldTypeId", "Name");
            return View(model);
        } 

        //
        // POST: /Fields/Create

        [HttpPost]
        public ActionResult Create(Field model, FormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Repository.Add(model);
                    Repository.Save();
                    return RedirectToAction("Index", new { id = model.FormId });
                }
                ViewBag.FieldTypes = new SelectList(Repository.GetFieldTypes(), "FieldTypeId", "Name");
                return View(model);
                
            }
            catch
            {
                Field model2 = new Field();
                model2.FormId = model.FormId;
                ViewBag.FieldTypes = new SelectList(Repository.GetFieldTypes(), "FieldTypeId", "Name");
                return View(model2);
            }
        }
        
        //
        // GET: /Fields/Edit/5
 
        public ActionResult Edit(int id)
        {
            ViewBag.FieldTypes = new SelectList(Repository.GetFieldTypes(), "FieldTypeId", "Name");
            return View(Repository.Get(id));
        }

        //
        // POST: /Fields/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                Field model = Repository.Get(id);
                if (TryUpdateModel(model))
                {
                    Repository.Save();
                    return RedirectToAction("Index", new { id = model.FormId });
                }
                ViewBag.FieldTypes = new SelectList(Repository.GetFieldTypes(), "FieldTypeId", "Name");
                return View(model);
            }
            catch
            {
                ViewBag.FieldTypes = new SelectList(Repository.GetFieldTypes(), "FieldTypeId", "Name");
                return View(Repository.Get(id));
            }
        }

        //
        // GET: /Fields/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(Repository.Get(id));
        }

        //
        // POST: /Fields/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        { 
            try 
            {
                Field model = Repository.Get(id);
                int formId = model.FormId;
                Repository.Delete(model);
                Repository.Save();
                return RedirectToAction("Index", new { id = formId });
            }
            catch
            {
                return View(Repository.Get(id));
            }
        }
    }
}
