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
    public class SelectionsController : Controller
    {
        public RepositorySelections Repository { get; set; }
        protected override void Initialize(RequestContext requestContext)
        {
            if (Repository == null) { Repository = new RepositorySelections(); }
            base.Initialize(requestContext);
        }
        //
        // GET: /Selections/

        public ActionResult Index(int id)
        {
            RepositoryFields repository = new RepositoryFields();
            ViewBag.Field = repository.Get(id);
            return View(Repository.GetSelections(id));
        }


        //
        // GET: /Selections/Create

        public ActionResult Create(int id)
        {
            Selection model = new Selection();
            model.FieldId = id;
            return View(model);
        } 

        //
        // POST: /Selections/Create

        [HttpPost]
        public ActionResult Create(Selection model, FormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Repository.Add(model);
                    Repository.Save();
                    return RedirectToAction("Index", new { id = model.FieldId });
                }
                return View(model);
            }
            catch
            {
                Selection model2 = new Selection();
                model2.FieldId = model.FieldId;

                return View(model2);
            }
        }
        
        //
        // GET: /Selections/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View(Repository.Get(id));
        }

        //
        // POST: /Selections/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                Selection model = Repository.Get(id);
                if (TryUpdateModel<Selection>(model))
                {
                    UpdateModel(model);
                    Repository.Save();
                    return RedirectToAction("Index", new { id = model.FieldId });
                }
                return View(model);
            }
            catch
            {
                return View(Repository.Get(id));
            }
        }

        //
        // GET: /Selections/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(Repository.Get(id));
        }

        //
        // POST: /Selections/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Selection model = Repository.Get(id);
                int fieldId = model.FieldId;
                Repository.Delete(model);
                Repository.Save();
                return RedirectToAction("Index", new { id = fieldId });
            }
            catch
            {
                return View(Repository.Get(id));
            }
        }
    }
}
