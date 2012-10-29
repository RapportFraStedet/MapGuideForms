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
   
    public class RepositoryGroups
    {
        private DatabaseFormsEntities db = new DatabaseFormsEntities();
        public IQueryable<Group> Get()
        {
            return db.Groups;
        }
        public Group Get(int id)
        {
            return db.Groups.Single(d => d.GroupId == id);
        }
        public GroupModel NewGroupModel()
        {
            return new GroupModel()
            {
                Forms = new System.Web.Mvc.MultiSelectList(db.Forms, "FormId", "Name"),
                DefaultForms = new System.Web.Mvc.SelectList(db.Forms, "FormId", "Name"),
                Roles = new System.Web.Mvc.SelectList(System.Web.Security.Roles.GetAllRoles())
            };
        }
        public GroupModel GetGroupModel(int id)
        {
            Group group =db.Groups.Single(d => d.GroupId == id);
            List<int> selectedForms = new List<int>();
            foreach(Form form in group.Forms)
            {
                selectedForms.Add(form.FormId);
            }
            return new GroupModel()
            {
                Name = group.Name,
                GroupId = group.GroupId,
                Forms = new System.Web.Mvc.MultiSelectList(db.Forms, "FormId", "Name"),
                DefaultForms = new System.Web.Mvc.SelectList(db.Forms, "FormId", "Name"),
                DefaultFormId = group.DefaultFormId,
                SelectedForms = selectedForms.ToArray(),
                Role = group.Role,
                Roles = new System.Web.Mvc.SelectList(System.Web.Security.Roles.GetAllRoles())
            };
        }
        public void Update(GroupModel model)
        {
            Group group = db.Groups.Single(d => d.GroupId == model.GroupId);
            group.Name = model.Name;
            group.DefaultFormId = model.DefaultFormId;
            group.Role = model.Role;
            foreach(Form form in group.Forms)
            {
                if(!model.SelectedForms.Contains(form.FormId))
                {
                    foreach(Permission permission in db.Permissions.Where(m=>m.View.GroupId == model.GroupId && m.Field.FormId==form.FormId))
                    {
                        db.Permissions.Remove(permission);
                    }
                }
            }
            group.Forms.Clear();
            if (model.SelectedForms != null)
            {
                foreach (int id in model.SelectedForms)
                {
                    Form form = db.Forms.Single(a => a.FormId == id);
                    group.Forms.Add(form);
                }
            }
            db.SaveChanges();

        }
        public void Add(GroupModel model)
        {
            Group group = new Group()
            {
                Name = model.Name,
                DefaultFormId = model.DefaultFormId,
                Role = model.Role
            };
            if (model.SelectedForms != null)
            {
                foreach (int id in model.SelectedForms)
                {
                    Form form = db.Forms.Single(a => a.FormId == id);
                    group.Forms.Add(form);
                }
            }
            db.Groups.Add(group);
        }
        public void Delete(Group model)
        {
            db.Groups.Remove(model);
        }
        //
        // Persistence
        public void Save()
        {
            db.SaveChanges();
        }

    }
}