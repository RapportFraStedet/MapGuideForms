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
    public class FieldTypesController : ApiController
    {
        // GET /api/form
        private DatabaseFormsEntities db = new DatabaseFormsEntities();
        public FieldTypesResponse Get(string username, string password)
        {
            AccountNewModel account = MyAuthentication.authentication(username, password);
            FieldTypesResponse model = new FieldTypesResponse
            {
                IsAuthenticated = account.IsAuthenticated,
                Roles = account.Roles
            };
            if (account.IsAuthenticated)
            {
                model.FieldTypes = new List<FieldTypeModel>(db.FieldTypes.Select(m => new FieldTypeModel { Name = m.Name, FieldTypeId=m.FieldTypeId }));
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
