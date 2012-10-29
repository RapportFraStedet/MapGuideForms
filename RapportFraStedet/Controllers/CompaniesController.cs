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
    public class CompaniesController : Controller
    {
        public RepositoryCompanies Repository { get; set; }
        protected override void Initialize(RequestContext requestContext)
        {
            if (Repository == null) { Repository = new RepositoryCompanies(); }
            base.Initialize(requestContext);
        }
        //
        // GET: /Companies/

        public ActionResult Index()
        {
            return View(Repository.Get());
        }


        //
        // GET: /Companies/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Companies/Create

        [HttpPost]
        public ActionResult Create(Company model, FormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Repository.Add(model);
                    Repository.Save();
                    return RedirectToAction("Index");
                }

                return View(model);
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Companies/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View(Repository.Get(id));
        }

        //
        // POST: /Companies/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                Company model = Repository.Get(id);
                if (TryUpdateModel(model))
                {
                    Repository.Save();
                    return RedirectToAction("Index");
                }
 
                return View(model);
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Companies/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(Repository.Get(id));
        }

        //
        // POST: /Companies/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Company model = Repository.Get(id);
                Repository.Delete(model);
                Repository.Save();
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
