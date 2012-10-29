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
    public class DeleteFieldController : ApiController
    {
        // GET /api/form
        private DatabaseFormsEntities db = new DatabaseFormsEntities();

        // GET /api/form/5
        public FieldNewModel Get(int fieldId, string username, string password)
        {
            AccountNewModel account = MyAuthentication.authentication(username, password);
            FieldNewModel model = new FieldNewModel{
                IsAuthenticated = account.IsAuthenticated,
                Roles = account.Roles
            };
            if (account.IsAuthenticated)
            {
                Field field = db.Fields.Where(m => m.FieldId == fieldId).SingleOrDefault();
                model.FormId = field.FormId;
                db.Fields.Remove(field);
                db.SaveChanges();
            }
            return model;
        }
    }
}
