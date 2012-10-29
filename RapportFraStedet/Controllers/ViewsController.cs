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
using System.IO;

namespace RapportFraStedet.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ViewsController : Controller
    {
        public RepositoryViews Repository { get; set; }
        protected override void Initialize(RequestContext requestContext)
        {
            if (Repository == null) { Repository = new RepositoryViews(); }
            base.Initialize(requestContext);
        }
        //
        // GET: /Field

        public ActionResult Index(int id)
        {
            RepositoryGroups repository = new RepositoryGroups();
            ViewBag.Group = repository.Get(id);
            return View(Repository.GetViews(id));
        }

        //
        // GET: /Fields/Create

        public ActionResult Create(int id)
        {
            string dataViews = Server.MapPath("~/Views/Data");
            List<string> names = new List<string>();
            foreach (string file in Directory.GetFiles(dataViews))
            {
                names.Add(Path.GetFileNameWithoutExtension(file));
            }
            SelectList views = new SelectList(names);
            ViewBag.Names = views;
            Models.View model = new Models.View();
            model.GroupId = id;
            ViewBag.ViewTypes = new SelectList(Repository.GetViewTypes(), "ViewTypeId", "Name");
            return View(model);
        } 

        //
        // POST: /Fields/Create

        [HttpPost]
        public ActionResult Create(Models.View model, FormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Repository.Add(model);
                    Repository.Save();
                    return RedirectToAction("Index", new { id = model.GroupId });
                }
                string dataViews = Server.MapPath("~/Views/Data");
                List<string> names = new List<string>();
                foreach (string file in Directory.GetFiles(dataViews))
                {
                    names.Add(Path.GetFileNameWithoutExtension(file));
                }
                SelectList views = new SelectList(names);
                ViewBag.Names = views;
                ViewBag.ViewTypes = new SelectList(Repository.GetViewTypes(), "ViewTypeId", "Name");
                return View(model);
                
            }
            catch
            {
                Models.View model2 = new Models.View();
                model2.ViewId = model.ViewId;
                ViewBag.ViewTypes = new SelectList(Repository.GetViewTypes(), "ViewTypeId", "Name");
                return View(model2);
            }
        }
        
        //
        // GET: /Fields/Edit/5
 
        public ActionResult Edit(int id)
        {
            string dataViews = Server.MapPath("~/Views/Data");
            List<string> names = new List<string>();
            foreach (string file in Directory.GetFiles(dataViews))
            {
                names.Add(Path.GetFileNameWithoutExtension(file));
            }
            SelectList views = new SelectList(names);
            ViewBag.Names = views;
            ViewBag.ViewTypes = new SelectList(Repository.GetViewTypes(), "ViewTypeId", "Name");
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
                Models.View model = Repository.Get(id);
                if (TryUpdateModel(model))
                {
                    Repository.Save();
                    return RedirectToAction("Index", new { id = model.GroupId });
                }
                string dataViews = Server.MapPath("~/Views/Data");
                List<string> names = new List<string>();
                foreach (string file in Directory.GetFiles(dataViews))
                {
                    names.Add(Path.GetFileNameWithoutExtension(file));
                }
                SelectList views = new SelectList(names);
                ViewBag.Names = views;
                ViewBag.ViewTypes = new SelectList(Repository.GetViewTypes(), "ViewTypeId", "Name");
                return View(model);
            }
            catch
            {
                ViewBag.ViewTypes = new SelectList(Repository.GetViewTypes(), "ViewTypeId", "Name");
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
                Models.View model = Repository.Get(id);
                int groupId = model.GroupId;
                Repository.Delete(model);
                Repository.Save();
                return RedirectToAction("Index", new { id = groupId });
            }
            catch
            {
                return View(Repository.Get(id));
            }
        }
    }
}
