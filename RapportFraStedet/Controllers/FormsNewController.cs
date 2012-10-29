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
using System.Net.Http;
using System.Web.Http;
using RapportFraStedet.Models;
using System.Web;
using System.IO;
using System.Web.Security;

namespace RapportFraStedet.Controllers
{
    //[Authorize(Roles = "Administrator")]
    public class FormsNewController : ApiController
    {
        // GET /api/form
        //RepositoryForms repository = new RepositoryForms();
        private DatabaseFormsEntities db = new DatabaseFormsEntities();
        public FormsResponse Get(string username, string password)
        {
            AccountNewModel account = MyAuthentication.authentication(username, password);
            FormsResponse model = new FormsResponse{
                IsAuthenticated = account.IsAuthenticated,
                Roles = account.Roles
            };
            if (account.IsAuthenticated)
            {
                model.Forms = new List<FormsNewModel>(db.Forms.Select(m => new FormsNewModel { Id = m.FormId, Name = m.Name }));
            }
            return model;
        }
        // GET /api/form/5
        public FormNewModel Get(int id, string username, string password)
        {
            AccountNewModel account = MyAuthentication.authentication(username, password);
            FormNewModel model = new FormNewModel{
                IsAuthenticated = account.IsAuthenticated,
                Roles = account.Roles
            };
            if (account.IsAuthenticated)
            {
                model = db.Forms.Where(m => m.FormId == id).Select(m => new FormNewModel
                {
                    IsAuthenticated = account.IsAuthenticated,
                    Roles = account.Roles,
                    Id = m.FormId,
                    Name = m.Name,
                    Email = m.Email,
                    SRS = m.SRS,
                    ResourceName = m.ResourceName,
                    ClassName = m.ClassName,
                    UploadPhysicalPath = m.UploadPhysicalPath,
                    UploadVirtualPath = m.UploadVirtualPath,
                    ReceiverOnCreate = m.ViewEmailToReceiverOnCreate,
                    ReceiverOnEdit = m.ViewEmailToReceiverOnEdit,
                    ReceiverOnDelete = m.ViewEmailToSenderOnDelete,
                    SenderOnCreate = m.ViewEmailToSenderOnCreate,
                    SenderOnEdit = m.ViewEmailToSenderOnEdit,
                    SenderOnDelete = m.ViewEmailToSenderOnDelete
                }).SingleOrDefault();
                string mailViews = HttpContext.Current.Server.MapPath("~/Views/UserMailer");
                List<string> mailViewList = new List<string>();
                foreach (string file in Directory.GetFiles(mailViews))
                {
                    mailViewList.Add(Path.GetFileNameWithoutExtension(file));
                }
                model.Selections = mailViewList;
            }
            return model;
        }

        // POST /api/form
        public void Post(string value)
        {
        }

        // PUT /api/form/5
        public void Put(int id, string value)
        {
        }

        // DELETE /api/form/5
        public void Delete(int id)
        {
        }
    }
}
