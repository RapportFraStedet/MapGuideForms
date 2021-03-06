﻿// Copyright 2012, MapGuideForm user group, Frederikssund Kommune and Helsingør Kommune - att. Anette Poulsen and Erling Kristensen
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
   
    public class RepositoryViews
    {
        private DatabaseFormsEntities db = new DatabaseFormsEntities();
        public IQueryable<View> GetViews(int id)
        {
            return db.Views.Where(a => a.GroupId == id);
        }
        public IQueryable<ViewType> GetViewTypes()
        {
            return db.ViewTypes;
        }
        public View Get(int id)
        {
            return db.Views.Single(m => m.ViewId == id);
        }
        //
        // Insert/Delete Methods
        public void Add(View item)
        {
            db.Views.Add(item);
        }
        public void Delete(View item)
        {
            db.Views.Remove(item);
        }
        //
        // Persistence
        public void Save()
        {
            db.SaveChanges();
        }

    }
}