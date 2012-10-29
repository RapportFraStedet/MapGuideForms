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

namespace RapportFraStedet.Controllers
{
    //[Authorize(Roles = "Administrator")]
    public class FieldsNewController : ApiController
    {
        // GET /api/form
        private DatabaseFormsEntities db = new DatabaseFormsEntities();
        public FieldsResponse Get(int formId, string username, string password)
        {
            AccountNewModel account = MyAuthentication.authentication(username, password);
            FieldsResponse model = new FieldsResponse
            {
                IsAuthenticated = account.IsAuthenticated,
                Roles = account.Roles
            };
            if (account.IsAuthenticated)
            {
                model.Fields = new List<FieldsNewModel>(db.Fields.Where(m => m.FormId == formId).OrderBy(m=>m.FieldOrder).Select(m => new FieldsNewModel { FieldId = m.FieldId, Name = m.Name }));
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
