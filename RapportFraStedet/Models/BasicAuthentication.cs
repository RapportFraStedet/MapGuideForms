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
using System.Text;
using System.Web.Http.Services;
using System.Security.Principal;
using System.Web.Security;

namespace RapportFraStedet.Models
{
    public class BasicAuthenticationAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            string username="";
            string password="";
            string[] hash = actionContext.Request.RequestUri.Query.Split(new char[] { '?', '&' },StringSplitOptions.RemoveEmptyEntries);
            foreach (string h in hash)
            {
                string[] keys = h.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (keys.Length == 2)
                {
                    switch (keys[0])
                    {
                        case "username":
                            username = keys[1];
                            break;
                        case "password":
                            password = keys[1];
                            break;

                    }
                }
            }

            if (username != "" && password != "")
            {
                if (Membership.ValidateUser(username, password))
                {
                    //HttpContext.Current.User = new GenericPrincipal(new ApiIdentity { Name = username }, new string[] { });
                    //if (HttpContext.Current.User.IsInRole("Administrator"))
                    if (Roles.IsUserInRole(username, "Administrator"))
                    {
                        base.OnActionExecuting(actionContext);
                    }
                    else
                    {

                    }
                }
                else
                {
                    actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                }
            }
            /*
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }
            else
            {
                string authToken = actionContext.Request.Headers.Authorization.Parameter;
                string decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));

                string username = decodedToken.Substring(0, decodedToken.IndexOf(":"));
                string password = decodedToken.Substring(decodedToken.IndexOf(":") + 1);
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                }
                else
                {
                    try
                    {

                        if (Membership.ValidateUser(username, password))
                        {
                            //HttpContext.Current.User = new GenericPrincipal(new ApiIdentity { Name = username }, new string[] { });
                            //if (HttpContext.Current.User.IsInRole("Administrator"))
                            if (Roles.IsUserInRole(username, "Administrator"))
                            {
                                base.OnActionExecuting(actionContext);
                            }
                            else
                            {
 
                            }
                        }
                        else
                        {
                            actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                        }
                    }
                    catch (Exception ex)
                    {
                        actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                    }
                }
            }*/
            /*IPasswordTransform transform = DependencyResolver.Current.GetService<IPasswordTransform>();
            IRepository<User> userRepository = DependencyResolver.Current.GetService<IRepository<User>>();

            User user = userRepository.All(u => u.Username == username && u.PasswordHash == transform.Transform(password)).SingleOrDefault();

            if (user != null)
            {
                HttpContext.Current.User = new GenericPrincipal(new ApiIdentity(user), new string[] { });

                base.OnActionExecuting(actionContext);
            }
            else
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }*/



        }
    }
    public class ApiIdentity : IIdentity
    {

        public string Name
        {
            get;
            set;
        }

        public string AuthenticationType
        {
            get { return "Basic"; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }
    }


}