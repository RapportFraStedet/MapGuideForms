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
using System.Web.Security;
using RapportFraStedet.Models;

namespace RapportFraStedet.Controllers
{
    public class AccountNewController : ApiController
    {
        // GET /api/account
        /*public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET /api/account/5
        public string Get(int id)
        {
            return "value";
        }
        */
        // POST /api/account
        public AccountNewModel Get(string username, string password)
        {
            AccountNewModel model = new AccountNewModel { IsAuthenticated = false, Roles=new List<string>() };
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                if (Membership.ValidateUser(username, password))
                {
                    model.IsAuthenticated = true;
                    model.Roles.AddRange(Roles.GetRolesForUser(username));
                }
            }
            return model;
        }
        

        // PUT /api/account/5
        public void Put(int id, string value)
        {
        }

        // DELETE /api/account/5
        public void Delete(int id)
        {
        }
    }
}
