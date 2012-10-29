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
    public class FieldController : ApiController
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
                model = db.Fields.Where(m => m.FieldId == fieldId).Select(m => new FieldNewModel
                {
                    IsAuthenticated = account.IsAuthenticated,
                    Roles = account.Roles,
                    Data = m.Data,
                    FieldColumn = m.FieldColumn,
                    FieldId = m.FieldId,
                    FieldOrder = m.FieldOrder,
                    FieldTypeId = m.FieldTypeId,
                    FormId = m.FormId,
                    Name = m.Name,
                    Required = m.Required
                }).SingleOrDefault();
                model.Selections = new List<FieldTypeModel>(db.FieldTypes.Select(n => new FieldTypeModel { FieldTypeId = n.FieldTypeId, Name = n.Name }));
            }
            return model;
        }

        public FieldNewModel Get(string data,string fieldColumn,int? fieldOrder,int? fieldTypeId,int formId,string name,bool required, string username, string password)
        {
            AccountNewModel account = MyAuthentication.authentication(username, password);
            FieldNewModel model = new FieldNewModel
            {
                IsAuthenticated = account.IsAuthenticated,
                Roles = account.Roles,
                FormId = formId
            };
            

            if (account.IsAuthenticated)
            {
                Field field = new Field{
                    Data = data,
                    FieldColumn = fieldColumn,
                    FieldOrder = 0,
                    FieldTypeId = 0,
                    FormId = formId,
                    Name = name,
                    Required = required
                };
                if (fieldTypeId.HasValue)
                    field.FieldTypeId = fieldTypeId.Value;
                if (fieldOrder.HasValue)
                    field.FieldOrder = fieldOrder.Value;
                db.Fields.Add(field);
                db.SaveChanges();
            }
            return model;
        }
        public FieldNewModel Get(string data, string fieldColumn, int? fieldOrder, int? fieldTypeId, int formId, int fieldId, string name, bool required, string username, string password)
        {
            AccountNewModel account = MyAuthentication.authentication(username, password);
            FieldNewModel model = new FieldNewModel
            {
                IsAuthenticated = account.IsAuthenticated,
                Roles = account.Roles
            };


            if (account.IsAuthenticated)
            {

                Field field = db.Fields.Where(m => m.FieldId == fieldId).SingleOrDefault();
                field.FieldColumn = fieldColumn;
                field.Data = data;
                field.FieldColumn = fieldColumn;
                field.Name = name;
                field.Required = required;
                if (fieldTypeId.HasValue)
                    field.FieldTypeId = fieldTypeId.Value;
                if (fieldOrder.HasValue)
                    field.FieldOrder = fieldOrder.Value;
                db.SaveChanges();
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
