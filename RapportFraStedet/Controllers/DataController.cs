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
using System.Globalization;
using System.Net.Mail;
using System.Web.Routing;
using System.Text.RegularExpressions;
using System.Web.Security;

using System.IO;

namespace RapportFraStedet.Controllers
{
    
    public class DataController : Controller
    {
        
        public RepositoryData Repository { get; set; }
        Regex email = new Regex("^[a-z0-9A-Z_\\+-]+(\\.[a-z0-9A-Z\\+-]+)*@[a-z0-9A-Z-]+(\\.[a-z0-9A-Z-]+)*\\.([a-zA-Z]{2,4})$");
        protected override void Initialize(RequestContext requestContext)
        {
            if (Repository == null) { Repository = new RepositoryData(); }
            base.Initialize(requestContext);
        }
        public ActionResult About(int id)
        {
            RepositoryViews repository = new RepositoryViews();
            return View("System_About",repository.Get(id));
        }



        //
        // GET: /Data/Create
        [CustomAuthorize]
        [HttpPost]
        public ActionResult Items(int id, int formId, List<DataSelectionModel> dataSelections)
        {
            RepositoryViews repository = new RepositoryViews();
            Models.View view = repository.Get(id);
            view.Group.DefaultFormId = formId;
            RepositoryOpenLayers openLayers = new RepositoryOpenLayers();
            DataListModel model = openLayers.GetList(view, dataSelections);
            return PartialView("System_Items", model);
        }
        [CustomAuthorize]
        public ActionResult Index(int id, int? itemId, int? formId, string column, bool? ascending)
        {
            
            RepositoryViews repositoryViews = new RepositoryViews();
            Models.View view = repositoryViews.Get(id);
            if (formId.HasValue)
                view.Group.DefaultFormId = formId.Value;
            if (itemId.HasValue)
            {
                //2 = Edit
                //3 = Delete
                //4 = Details
                if (view.ViewTypeId == 2 || view.ViewTypeId == 3 || view.ViewTypeId == 4)
                {
                    RepositoryMapguide mapguide = new RepositoryMapguide();
                    DataViewModel model = new DataViewModel()
                    {
                        ItemId = itemId.Value,
                        View = view,
                        Form = view.Group.Forms.Single(m => m.FormId == view.Group.DefaultFormId)
                    };
                    model = mapguide.Get(model);
                    return View(view.Name, model);
                }
            }
            else
            {
                //1 = Create
                if (view.ViewTypeId == 1)
                {
                    DataViewModel model = new DataViewModel
                    {
                        View = view,
                        Form = view.Group.Forms.SingleOrDefault(m => m.FormId == view.Group.DefaultFormId),
                        Date = DateTime.Now,
                        UniqueId = Guid.NewGuid().ToString()
                    };
                    if (User.Identity.IsAuthenticated)
                    {
                        model.UserName = User.Identity.Name;
                        model.UserId = Membership.GetUser(User.Identity.Name).ProviderUserKey.ToString();
                        UserProfile profile = UserProfile.GetUserProfile(User.Identity.Name);
                        RepositoryCompanies rc = new RepositoryCompanies();
                        Company company = rc.Get(profile.CompanyId);
                        if (company != null)
                            model.Company = company.Name;
                    }
                    return View(view.Name, model);
                }
                //5 = List All
                else if (view.ViewTypeId == 5)
                {
                    RepositoryMapguide mapguide = new RepositoryMapguide();
                    DataListModel model = mapguide.GetList(view, "");
                    return View(view.Name, model);
                }
                //6 = List My
                else if (view.ViewTypeId == 6)
                {
                    RepositoryMapguide mapguide = new RepositoryMapguide();
                    DataListModel model = null;
                    if (User.Identity.IsAuthenticated)
                    {
                        model = mapguide.GetList(view, Membership.GetUser(User.Identity.Name).ProviderUserKey.ToString());
                    }
                    else
                    {
                        model = mapguide.GetList(view, "");
                    }
                    return View(view.Name, model);
                }
            }
            return View("Error");
        }
        
        private DataCreateModel CreateModel(DataViewModel postModel, FormCollection formValues, Operation operation)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("da-DK");
            RepositoryViews repository = new RepositoryViews();
            Models.View view = repository.Get(postModel.View.ViewId);
            
            DataViewModel model = new DataViewModel
            {
                View = view,
                Form = view.Group.Forms.Single(m => m.FormId == postModel.Form.FormId),
                Date = postModel.Date,
                UniqueId = postModel.UniqueId,
                ItemId = postModel.ItemId,
                UserName = postModel.UserName,
                UserId = postModel.UserId,
            };
            if (operation == Operation.Create)
            {
                model.Date = DateTime.Now;
                model.UniqueId = Guid.NewGuid().ToString();
                if (User.Identity.IsAuthenticated)
                {
                    model.UserName = User.Identity.Name;
                    model.UserId = Membership.GetUser(User.Identity.Name).ProviderUserKey.ToString();
                }
            }
            if (!String.IsNullOrEmpty(model.UserName))
            {
                UserProfile profile = UserProfile.GetUserProfile(model.UserName);
                RepositoryCompanies rc = new RepositoryCompanies();
                Company company = rc.Get(profile.CompanyId);
                if (company != null)
                    model.Company = company.Name;
            }

            List<DataEmailModel> emails = new List<DataEmailModel>();
            if (!String.IsNullOrEmpty(model.Form.Email))
            {
                if (operation == Operation.Create && !String.IsNullOrEmpty(model.Form.ViewEmailToReceiverOnCreate))
                {
                    emails.Add(new DataEmailModel { Email = model.Form.Email, View = model.Form.ViewEmailToReceiverOnCreate });
                }
                else if (operation == Operation.Edit && !String.IsNullOrEmpty(model.Form.ViewEmailToReceiverOnEdit))
                {
                    emails.Add(new DataEmailModel { Email = model.Form.Email, View = model.Form.ViewEmailToReceiverOnEdit });
                }
            }
            foreach (Field field in model.Form.Fields)
            {
                #region 8 Upload
                if (field.FieldTypeId == 8) //Upload
                {
                    string value = formValues[field.FieldId.ToString()];
                    if (!String.IsNullOrEmpty(value))
                    {
                        field.Data = value;
                    }
                    if (Request.Files.AllKeys.Contains(field.FieldId.ToString()))
                    {
                        HttpPostedFileBase file = Request.Files[field.FieldId.ToString()];
                        if (file.ContentLength > 0)
                        {
                            string name = model.UniqueId + "-" + field.FieldId.ToString() + Path.GetExtension(file.FileName);
                            string filePath = Path.Combine(model.Form.UploadPhysicalPath, name);
                            file.SaveAs(filePath);
                            field.Data = name;
                            try
                            {
                                int width = Properties.Settings.Default.MaxWidth;
                                int height = Properties.Settings.Default.MaxHeight;
                                System.Drawing.Image FullsizeImage = System.Drawing.Image.FromFile(filePath);

                                // Prevent using images internal thumbnail
                                FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
                                FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);

                                if (FullsizeImage.Width <= width)
                                {
                                    width = FullsizeImage.Width;
                                }


                                int NewHeight = FullsizeImage.Height * width / FullsizeImage.Width;
                                if (NewHeight > height)
                                {
                                    // Resize with height instead
                                    width = FullsizeImage.Width * height / FullsizeImage.Height;
                                    NewHeight = height;
                                }

                                System.Drawing.Image NewImage = FullsizeImage.GetThumbnailImage(width, NewHeight, null, IntPtr.Zero);

                                // Clear handle to original file so that we can overwrite it if necessary
                                FullsizeImage.Dispose();
                                string newName = "Thumb_" + Path.GetFileName(name);
                                string newFile = Path.Combine(model.Form.UploadPhysicalPath, newName);
                                // Save resized picture
                                NewImage.Save(newFile);
                                NewImage.Dispose();
                            }
                            catch
                            { }
                        }
                        else if (field.Required)
                        {
                            ModelState.AddModelError(field.FieldId.ToString(), field.Name + " er påkrævet");
                        }
                    }
                }
                #endregion
                else if (field.FieldTypeId == 16) //Geometry
                {
                    string value = formValues["Geometri"];
                    if (String.IsNullOrEmpty(value))
                    {
                        if (field.Required)
                        {
                            string error = field.Name + " er påkrævet";
                            ModelState.AddModelError("Geometri", error);
                        }

                    }
                    else
                    {
                        field.Data = value;
                    }
                }
                else if (field.FieldTypeId == 17) //Position
                {
                    string value = formValues["Position"];
                    if (String.IsNullOrEmpty(value))
                    {
                        if (field.Required)
                        {
                            string error = field.Name + " er påkrævet";
                            ModelState.AddModelError("Position", error);
                        }

                    }
                    else
                    {
                        field.Data = value;
                    }
                }
                else if (field.FieldTypeId == 18) //Accuracy
                {
                    string value = formValues["Accuracy"];
                    if (String.IsNullOrEmpty(value))
                    {
                        if (field.Required)
                        {
                            string error = field.Name + " er påkrævet";
                            ModelState.AddModelError("Accuracy", error);
                        }

                    }
                    else
                    {
                        field.Data = value;
                    }
                }
                else
                {
                    string value = formValues[field.FieldId.ToString()];
                    if (String.IsNullOrEmpty(value))
                    {
                        if (field.Required)
                        {
                            string error = field.Name + " er påkrævet";
                            ModelState.AddModelError(field.FieldId.ToString(), error);
                        }

                    }
                    else
                    {
                        field.Data = value;
                        switch (field.FieldTypeId)
                        {
                            case 7:

                                if (email.IsMatch(field.Data))
                                {
                                    string emailView = "";
                                    Field operationField = null;
                                    if (operation == Operation.Create)
                                    {
                                        emailView = field.Form.ViewEmailToSenderOnCreate;
                                        operationField = field.Form.Fields.FirstOrDefault(m => m.FieldTypeId == 12);
                                    }
                                    else if (operation == Operation.Edit)
                                    {
                                        emailView = field.Form.ViewEmailToSenderOnEdit;
                                        operationField = field.Form.Fields.FirstOrDefault(m => m.FieldTypeId == 13);
                                    }
                                    if (!String.IsNullOrEmpty(emailView) && operationField != null)
                                    {
                                        string send = formValues[operationField.FieldId.ToString()];
                                        if (!String.IsNullOrEmpty(send))
                                        {
                                            if (send != "false")
                                                emails.Add(new DataEmailModel { Email = field.Data, View = emailView });
                                        }

                                    }
                                }
                                else
                                {
                                    ModelState.AddModelError(field.FieldId.ToString(), field.Data + " er ikke en gyldig email");
                                }
                                break;
                            case 10:

                                DateTime dt;
                                if (!DateTime.TryParse(field.Data, culture, DateTimeStyles.AssumeLocal, out dt))
                                {
                                    ModelState.AddModelError(field.FieldId.ToString(), field.Data + " er ikke en gyldig dato");
                                }
                                break;
                        }
                        Selection selection = field.Selections.SingleOrDefault(m => m.Name == field.Data);
                        if (selection != null)
                        {

                            if (!String.IsNullOrEmpty(selection.Email))
                            {
                                if (operation == Operation.Create && !String.IsNullOrEmpty(model.Form.ViewEmailToReceiverOnCreate))
                                {
                                    emails.Add(new DataEmailModel { Email = selection.Email, View = model.Form.ViewEmailToReceiverOnCreate });
                                }
                                else if (operation == Operation.Edit && !String.IsNullOrEmpty(model.Form.ViewEmailToReceiverOnEdit))
                                {
                                    emails.Add(new DataEmailModel { Email = selection.Email, View = model.Form.ViewEmailToReceiverOnEdit });
                                }

                            }
                        }
                    }
                }
            }
            return new DataCreateModel()
            {
                Emails = emails,
                Model = model,
            };
        }
        
        public ActionResult Streets(int id)
        {
            ViewBag.ViewId = id;
            RepositoryAddress repository = new RepositoryAddress();
            ViewBag.Streets = repository.GetStreets();
            return PartialView("System_Address");
        }
        public ActionResult StreetsJson()
        {
            RepositoryAddress repository = new RepositoryAddress();
            SelectList streets = repository.GetStreets();
            return Json(streets);
        }
        [HttpPost]
        public ActionResult Addresses(int streetId)
        {
            RepositoryAddress repository = new RepositoryAddress();
            SelectList addresses = repository.GetAddressByStreetId(streetId);
            return Json(addresses);
        }
        [HttpPost]
        public ActionResult Address(int streetId, string name)
        {
            RepositoryAddress repository = new RepositoryAddress();
            AddressService.AddressModel model = repository.GetAddressByAddressId(streetId, name);
            return Json(model);
        }
        [CustomAuthorize]
        [HttpGet]
        public ActionResult Fields(int id, int formId)
        {
            RepositoryViews repository = new RepositoryViews();
            Models.View view = repository.Get(id);
            view.Group.DefaultFormId = formId;
            DataViewModel model = new DataViewModel
            {
                View = view,
                Form = view.Group.Forms.Single(m => m.FormId == formId)
            };
            return PartialView("System_Fields", model);
        }
        [CustomAuthorize]
        [HttpGet]
        public ActionResult Items(int id, int formId)
        {
            RepositoryViews repository = new RepositoryViews();
            Models.View view = repository.Get(id);
            view.Group.DefaultFormId = formId;
            RepositoryMapguide mapguide = new RepositoryMapguide();
            DataListModel model = null;

            if (view.ViewTypeId == 5 || !User.Identity.IsAuthenticated)
            {
                model = mapguide.GetList(view, "");
            }
            else
            {
                model = mapguide.GetList(view, Membership.GetUser(User.Identity.Name).ProviderUserKey.ToString());
            }
            return PartialView("System_Items", model);
        }
        [CustomAuthorize]
        [HttpPost]
        public ActionResult Create(DataViewModel postModel, FormCollection formValues)
        {
            DataCreateModel dataCreateModel = CreateModel(postModel, formValues,Operation.Create);
            DataViewModel model = dataCreateModel.Model;
            foreach(Field field in model.Form.Fields.Where(m => m.FieldTypeId == 11))
            {
                field.Data = Request.UserHostAddress;
            }
            
            if (ModelState.IsValid)
            {
                RepositoryMapguide repositoryMapGuide = new RepositoryMapguide();
                model = repositoryMapGuide.Add(model);
                dataCreateModel.Model = model;
                repositoryMapGuide.SendEmails(dataCreateModel);
                Models.View endView = model.View.Group.Views.SingleOrDefault(m => m.ViewTypeId == 7);
                if (endView == null)
                {
                    Models.View listUser = model.View.Group.Views.SingleOrDefault(m => m.ViewTypeId == 6);
                    if (listUser == null)
                    {
                        Models.View listAll = model.View.Group.Views.SingleOrDefault(m => m.ViewTypeId == 5);
                        if (listAll == null)
                        {
                            return View(model.View.Name, model);
                        }
                        else
                        {
                            return RedirectToAction("Index", new { id = listAll.ViewId, formId = model.Form.FormId });
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", new { id = listUser.ViewId, formId = model.Form.FormId });
                    }

                }
                else
                {
                    ViewBag.Id = model.View.ViewId;
                    model.View = endView;
                    return View(endView.Name, model);
                }
            }
            else
                return View(model.View.Name, model);
        }
        //
        // GET: /Data/Edit/5
        [CustomAuthorize]
        [HttpPost]
        public ActionResult Edit(DataViewModel postModel, FormCollection formValues)
        {
            DataCreateModel dataCreateModel = CreateModel(postModel, formValues,Operation.Edit);
            DataViewModel model = dataCreateModel.Model;
            if (ModelState.IsValid)
            {
                RepositoryMapguide repositoryMapGuide = new RepositoryMapguide();
                repositoryMapGuide.Update(model);
                repositoryMapGuide.SendEmails(dataCreateModel);
                Models.View list = null;
                try
                {
                    list = model.View.Group.Views.Single(m => m.ViewTypeId == 5);
                }
                catch { }
                Models.View endView = model.View.Group.Views.SingleOrDefault(m => m.ViewTypeId == 7);
                if (endView == null)
                {
                    Models.View listUser = model.View.Group.Views.SingleOrDefault(m => m.ViewTypeId == 6);
                    if (listUser == null)
                    {
                        Models.View listAll = model.View.Group.Views.SingleOrDefault(m => m.ViewTypeId == 5);
                        if (listAll == null)
                        {
                            return View(model.View.Name, model);
                        }
                        else
                        {
                            return RedirectToAction("Index", new { id = listAll.ViewId, formId = model.Form.FormId });
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", new { id = listUser.ViewId, formId = model.Form.FormId });
                    }

                }
                else
                {
                    ViewBag.Id = model.View.ViewId;
                    model.View = endView;
                    return View(endView.Name, model);
                }
            }
            else
                return View(model.View.Name, model);

        }



        //
        // POST: /Data/Delete/5
        [CustomAuthorize]
        [HttpPost]
        public ActionResult Delete(DataViewModel postModel)
        {
            RepositoryMapguide mapguide = new RepositoryMapguide();
            RepositoryViews repository = new RepositoryViews();
            Models.View view = repository.Get(postModel.View.ViewId);
            DataViewModel model = new DataViewModel
            {
                View = view,
                Form = view.Group.Forms.Single(m => m.FormId == postModel.Form.FormId),
                ItemId = postModel.ItemId,
                UniqueId = postModel.UniqueId
            };
            model = mapguide.Get(model);
            mapguide.Delete(model);
            List<DataEmailModel> emails = new List<DataEmailModel>();
            if (!String.IsNullOrEmpty(model.Form.Email)&&!String.IsNullOrEmpty(model.Form.ViewEmailToReceiverOnDelete))
            {
                emails.Add(new DataEmailModel { Email = model.Form.Email, View = model.Form.ViewEmailToReceiverOnDelete });
            }
            foreach (Field field in model.Form.Fields)
            {
                if (field.FieldTypeId == 7)
                {
                    if (!string.IsNullOrEmpty(field.Data))
                    {
                        if (email.IsMatch(field.Data))
                        {
                            string emailView = field.Form.ViewEmailToSenderOnDelete;
                            Field operationField = field.Form.Fields.FirstOrDefault(m => m.FieldTypeId == 13);
                            if (!String.IsNullOrEmpty(emailView) && operationField != null)
                            {
                                if (!String.IsNullOrEmpty(operationField.Data))
                                {
                                    if (operationField.Data == "1" || operationField.Data == "True")
                                    {
                                        emails.Add(new DataEmailModel { Email = field.Data, View = emailView });
                                    }
                                }

                            }
                        }
                    }
                }
                Selection selection = field.Selections.SingleOrDefault(m => m.Name == field.Data);
                if (selection != null && !String.IsNullOrEmpty(model.Form.ViewEmailToReceiverOnDelete))
                {

                    if (!String.IsNullOrEmpty(selection.Email))
                    {
                            emails.Add(new DataEmailModel { Email = selection.Email, View = model.Form.ViewEmailToReceiverOnDelete });
                    }
                }
            }
            DataCreateModel dcm = new DataCreateModel()
            {
                Model = model,
                Emails = emails
            };
            mapguide.SendEmails(dcm);
            Models.View endView = model.View.Group.Views.SingleOrDefault(m => m.ViewTypeId == 7);
            if (endView == null)
            {
                Models.View listUser = model.View.Group.Views.SingleOrDefault(m => m.ViewTypeId == 6);
                if (listUser == null)
                {
                    Models.View listAll = model.View.Group.Views.SingleOrDefault(m => m.ViewTypeId == 5);
                    if (listAll == null)
                    {
                        return View(model.View.Name, model);
                    }
                    else
                    {
                        return RedirectToAction("Index", new { id = listAll.ViewId, formId = model.Form.FormId });
                    }
                }
                else
                {
                    return RedirectToAction("Index", new { id = listUser.ViewId, formId = model.Form.FormId });
                }

            }
            else
            {
                return View(endView.Name, model);
            }
        }
    }
}
