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
   
    public class RepositoryCompanies
    {
        private DatabaseFormsEntities db = new DatabaseFormsEntities();
        public IQueryable<Company> Get()
        {
            return db.Companies;
        }
        public Company Get(int id)
        {
            return db.Companies.Single(d => d.CompanyId == id);
        }
        //
        // Insert/Delete Methods
        public void Add(Company model)
        {
            db.Companies.Add(model);
        }
        public void Delete(Company model)
        {
            db.Companies.Remove(model);
        }
        //
        // Persistence
        public void Save()
        {
            db.SaveChanges();
        }

    }
}