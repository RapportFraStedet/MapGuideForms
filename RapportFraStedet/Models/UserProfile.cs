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
using System.Web.Profile;
using System.Web.Security;

namespace RapportFraStedet.Models
{
    public class UserProfile : ProfileBase
    {
        [SettingsAllowAnonymous(false)]
        public string FirstName
        {
            get { return base["FirstName"] as string; }
            set { base["FirstName"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        public string LastName
        {
            get { return base["LastName"] as string; }
            set { base["LastName"] = value; }
        }
        [SettingsAllowAnonymous(false)]
        public int CompanyId
        {
            get {
                int id = 0;
                int.TryParse(base["CompanyId"].ToString(), out id);
                return id; 
            }
            set { base["CompanyId"] = value; }
        }
        [SettingsAllowAnonymous(false)]
        public string Phone
        {
            get { return base["Phone"] as string; }
            set { base["Phone"] = value; }
        }
        public static UserProfile GetUserProfile(string username)
        {
            return Create(username) as UserProfile;
        }

        public static UserProfile GetUserProfile()
        {
            return Create(Membership.GetUser().UserName) as UserProfile;
        }
    }
}