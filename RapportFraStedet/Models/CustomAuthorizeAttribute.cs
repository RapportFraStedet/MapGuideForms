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
using System.Web.Mvc;

namespace RapportFraStedet.Models
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            string[] data = httpContext.Request.PathInfo.Split(new char[]{'/'},StringSplitOptions.RemoveEmptyEntries);
            string action = data[0].ToLower();
            int? viewId = null;
            int? formId = null;
            int? itemId = null;
            int id = 0;
            if (int.TryParse(data[1], out id))
            {
                viewId = id;
            }
            if(httpContext.Request.QueryString.AllKeys.Contains("formId"))
            {
                int o = 0;
                int.TryParse(httpContext.Request.QueryString["formId"],out o);
                formId = o;
            }
            else if (httpContext.Request.Params.AllKeys.Contains("Form.FormId"))
            {
                int o = 0;
                int.TryParse(httpContext.Request.Params["Form.FormId"], out o);
                formId = o;
            }
            if(httpContext.Request.QueryString.AllKeys.Contains("itemId"))
            {
                int o = 0;
                int.TryParse(httpContext.Request.QueryString["itemId"],out o);
                itemId = o;
            }
            else if (httpContext.Request.Params.AllKeys.Contains("ItemId"))
            {
                int o = 0;
                int.TryParse(httpContext.Request.Params["ItemId"], out o);
                itemId = o;
            }

            if (viewId.HasValue)
            {
                RepositoryViews repositoryViews = new RepositoryViews();
                Models.View view = repositoryViews.Get(viewId.Value);
                if (view.ViewTypeId == 2 || view.ViewTypeId == 3)
                {
                    if (formId.HasValue)
                    {
                        view.Group.DefaultFormId = formId.Value;
                        if (itemId.HasValue)
                        {
                            DataViewModel model = new DataViewModel()
                            {
                                ItemId = itemId.Value,
                                View = view,
                                Form = view.Group.Forms.Single(m => m.FormId == view.Group.DefaultFormId)
                            };
                            RepositoryMapguide repositoryMapGuide = new RepositoryMapguide();
                            model = repositoryMapGuide.Get(model);
                            if (model.UserName == httpContext.User.Identity.Name || httpContext.User.IsInRole("Administrator") || httpContext.User.IsInRole("Editor"))
                            {
                                return true;
                            }
                        }
                    }
                }
                else if (String.IsNullOrEmpty(view.Group.Role))
                {
                    return true;
                }
                else
                {
                    if (httpContext.User.IsInRole(view.Group.Role) || httpContext.User.IsInRole("Administrator") || httpContext.User.IsInRole("Editor"))
                        return true;
                    else
                    {
                        return false;
                    }
                }

            }
            return false;
        }
    }
}