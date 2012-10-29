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
using RapportFraStedet.Models.Account;
using System.Web.Routing;

namespace RapportFraStedet.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        public RepositoryForms FormsRepository { get; set; }
        public RepositoryCompanies CompaniesRepository { get; set; }
        public IMembershipService MembershipService { get; set; }
        protected override void Initialize(RequestContext requestContext)
        {
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }
            if (FormsRepository == null) { FormsRepository = new RepositoryForms(); }
            if (CompaniesRepository == null) { CompaniesRepository = new RepositoryCompanies(); }
            base.Initialize(requestContext);
        }
        //
        // GET: /Users/

        public ActionResult Index()
        {
            var memberShipUsers = Membership.GetAllUsers();
            List<DetailsModel> users = new List<DetailsModel>();
            foreach (MembershipUser user in memberShipUsers)
            {
                UserProfile profile = UserProfile.GetUserProfile(user.UserName);
                var model = new DetailsModel
                {
                    UserName = user.UserName,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    Email = user.Email,
                    Phone = profile.Phone
                };
                Company company = CompaniesRepository.Get(profile.CompanyId);
                if (company != null)
                    model.Company = company.Name;
                users.Add(model);

            }
            return View(users);
        }

        //
        // GET: /Users/Create
        public ActionResult Create()
        {
            CreateModel model = new CreateModel();
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            var companies = CompaniesRepository.Get();
            model.Companies = new SelectList(companies, "CompanyId", "Name");
            string[] allRoles = Roles.GetAllRoles();
            model.Roles = new MultiSelectList(allRoles);
            return View(model);
        }

        //
        // POST: /Users/Create
        [HttpPost]
        public ActionResult Create(CreateModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName, model.Password, model.Email);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    UserProfile profile = UserProfile.GetUserProfile(model.UserName);
                    profile.FirstName = model.FirstName;
                    profile.LastName = model.LastName;
                    profile.Phone = model.Phone;
                    profile.CompanyId = model.CompanyId;
                    profile.Save();
                    if (model.SelectedRoles != null)
                    {
                        Roles.AddUserToRoles(model.UserName, model.SelectedRoles);
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
            }
            var companies = CompaniesRepository.Get();
            model.Companies = new SelectList(companies, "CompanyId", "Name");
            string[] allRoles = Roles.GetAllRoles();
            model.Roles = new MultiSelectList(allRoles);
            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(model);
        }

        //
        // GET: /Users/Edit/5
        public ActionResult Edit(string id)
        {
            UserProfile profile = UserProfile.GetUserProfile(id);
            MembershipUser user = Membership.GetUser(id);
            var model = new EditModel
            {
                UserName = id,
                Email = user.Email,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Phone = profile.Phone,
                CompanyId = profile.CompanyId
            };
            string[] allRoles = Roles.GetAllRoles();
            string[] userRoles = Roles.GetRolesForUser(id);
            model.SelectedRoles = userRoles;
            model.Roles = new MultiSelectList(allRoles, userRoles);
            var companies = CompaniesRepository.Get();
            ViewBag.Companies = new SelectList(companies, "CompanyId", "Name", model.CompanyId);
            return View(model);
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        public ActionResult Edit(EditModel model)
        {
            if (ModelState.IsValid)
            {
                MembershipUser user = Membership.GetUser(model.UserName);
                user.Email = model.Email;
                Membership.UpdateUser(user);
                UserProfile profile = UserProfile.GetUserProfile(model.UserName);
                profile.FirstName = model.FirstName;
                profile.LastName = model.LastName;
                profile.CompanyId = model.CompanyId;
                profile.Phone = model.Phone;
                profile.Save();
                string[] roles = Roles.GetRolesForUser(model.UserName);
                if(roles.Length > 0)
                {
                    Roles.RemoveUserFromRoles(model.UserName, roles);
                }
                if (model.SelectedRoles != null)
                {
                    Roles.AddUserToRoles(model.UserName, model.SelectedRoles);
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        //
        // GET: /Users/Delete/5
        public ActionResult Delete(string id)
        {
            MembershipUser user = Membership.GetUser(id);
            UserProfile profile = UserProfile.GetUserProfile(id);
            var model = new DetailsModel
            {
                UserName = id,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Phone = profile.Phone,
                Email = user.Email
            };
            Company company = CompaniesRepository.Get(profile.CompanyId);
            if (company != null)
                model.Company = company.Name;
            if (id == "admin")
                return View("NoDelete", model);
            else
                return View(model);
        }

        //
        // POST: /Users/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            string[] roles = Roles.GetRolesForUser(id);
            if (roles.Length > 0)
            {
                Roles.RemoveUserFromRoles(id, roles);
            }
            Membership.DeleteUser(id);
            return RedirectToAction("Index");
        }
    }
}
