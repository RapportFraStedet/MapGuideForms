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

namespace RapportFraStedet.Models
{
   
    public class RepositoryFormsNew
    {
        private DatabaseFormsEntities db = new DatabaseFormsEntities();
        public IQueryable<FormsNewModel> Get()
        {
            return db.Forms.Select(m=>new FormsNewModel{ Id=m.FormId,Name=m.Name});
        }
        public FormNewModel Get(int id)
        {
            return  db.Forms.Where(m => m.FormId == id).Select(m=>new FormNewModel{ 
                Id=m.FormId,
                Name=m.Name,
                Email=m.Email,
                SRS=m.SRS,
                ResourceName=m.ResourceName,
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
        }
        //
        // Insert/Delete Methods
        public void Add(Form item)
        {
            db.Forms.Add(item);
        }
        public void Delete(Form item)
        {
            db.Forms.Remove(item);
        }
        //
        // Persistence
        public void Save()
        {
            db.SaveChanges();
        }

    }
}