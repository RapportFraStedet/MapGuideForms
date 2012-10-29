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
    public class FormsController : Controller
    {
        public RepositoryForms FormsRepository { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsRepository == null) { FormsRepository = new RepositoryForms(); }
            base.Initialize(requestContext);
        }
        //
        // GET: /Forms/

        public ActionResult Index()
        {
            return View(FormsRepository.Get());
        }


        //
        // GET: /Forms/Create

        public ActionResult Create()
        {
            string mailViews = Server.MapPath("~/Views/UserMailer");
            List<string> mailViewList = new List<string>();
            foreach (string file in Directory.GetFiles(mailViews))
            {
                mailViewList.Add(Path.GetFileNameWithoutExtension(file));
            }
            SelectList mails = new SelectList(mailViewList);
            ViewBag.EmailViews = mails;
            return View();
        } 

        //
        // POST: /Forms/Create

        [HttpPost]
        public ActionResult Create(Form model, FormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    FormsRepository.Add(model);
                    FormsRepository.Save();
                    return RedirectToAction("Index");
                }
                string mailViews = Server.MapPath("~/Views/UserMailer");
                List<string> mailViewList = new List<string>();
                foreach (string file in Directory.GetFiles(mailViews))
                {
                    mailViewList.Add(Path.GetFileNameWithoutExtension(file));
                }
                SelectList mails = new SelectList(mailViewList);
                ViewBag.EmailViews = mails;

                return View(model);
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Forms/Edit/5
 
        public ActionResult Edit(int id)
        {
            string mailViews = Server.MapPath("~/Views/UserMailer");
            List<string> mailViewList = new List<string>();
            foreach (string file in Directory.GetFiles(mailViews))
            {
                mailViewList.Add(Path.GetFileNameWithoutExtension(file));
            }
            SelectList mails = new SelectList(mailViewList);
            ViewBag.EmailViews = mails;

            return View(FormsRepository.Get(id));
        }

        //
        // POST: /Forms/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                Form model = FormsRepository.Get(id);
                if (TryUpdateModel(model))
                {
                    FormsRepository.Save();
                    return RedirectToAction("Index");
                }
                string mailViews = Server.MapPath("~/Views/UserMailer");
                List<string> mailViewList = new List<string>();
                foreach (string file in Directory.GetFiles(mailViews))
                {
                    mailViewList.Add(Path.GetFileNameWithoutExtension(file));
                }
                SelectList mails = new SelectList(mailViewList);
                ViewBag.EmailViews = mails;

                return View(model);
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Forms/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(FormsRepository.Get(id));
        }

        //
        // POST: /Forms/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                Form model = FormsRepository.Get(id);
                FormsRepository.Delete(model);
                FormsRepository.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
