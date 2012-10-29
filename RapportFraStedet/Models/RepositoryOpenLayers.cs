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
using System.Web.Security;
using System.Globalization;

namespace RapportFraStedet.Models
{
    public class RepositoryOpenLayers
    {
        private CultureInfo culture = CultureInfo.CreateSpecificCulture("da-DK");

        public DataListModel GetList(Models.View view, List<DataSelectionModel> dataSelections)
        {
            Form form = view.Group.Forms.Single(m => m.FormId == view.Group.DefaultFormId);
            List<Field> columns = new List<Field>();
            foreach (Field field in form.Fields.Where(m => m.FieldColumn != null && !m.FieldColumn.Equals(String.Empty)).OrderBy(a => a.FieldOrder))
            {
                Permission permission = view.Permissions.FirstOrDefault(m => m.FieldId == field.FieldId);
                if (permission == null)
                {
                    columns.Add(field);
                }
                else
                {
                    if (permission.PermissionTypeId != 0)
                    {
                        columns.Add(field);
                    }
                }

            }
            DataListModel model = new DataListModel
            {
                View = view,
                Form = form,
                Columns = columns
            };
            RepositoryCompanies repositoryCompanies = new RepositoryCompanies();
            DataSelectionModel layer = null;
            foreach (DataSelectionModel selection in dataSelections)
            {
                if (selection.name == form.Name)
                {
                    layer = selection;
                    break;
                }
            }
            if (layer != null)
            {
                foreach (List<string> row in layer.aElements)
                {
                    DataListItemModel listItem = new DataListItemModel();
                    for (int i = 0; i < layer.nProperties; i++)
                    {
                        if (!String.IsNullOrEmpty(row[i]))
                        {
                            string upperPropertyName = layer.aPropertiesName[i].ToUpper();
                            switch (upperPropertyName)
                            {
                                case "ID":
                                    int id = 0;
                                    int.TryParse(row[i], out id);
                                    listItem.ItemId = id;
                                    break;
                                case "UNIQUEID":
                                    listItem.UniqueId = row[i];
                                    break;
                                case "USERID":
                                    listItem.UserId = row[i];
                                    MembershipUser user = Membership.GetUser(new Guid(listItem.UserId));
                                    if (user != null)
                                    {
                                        listItem.UserName = user.UserName;
                                        UserProfile profile = UserProfile.GetUserProfile(user.UserName);
                                        Company company = repositoryCompanies.Get(profile.CompanyId);
                                        if (company != null)
                                            listItem.Company = company.Name;
                                    }
                                    break;
                                case "DATO":
                                    DateTime dt = DateTime.Now;
                                    DateTime.TryParse(row[i], culture, DateTimeStyles.AssumeLocal, out dt);
                                    listItem.Date = dt;
                                    break;
                                default:
                                    foreach (Field field in model.Columns)
                                    {

                                        if (layer.aPropertiesName[i].ToUpper() == field.FieldColumn.ToUpper())
                                        {
                                            if (field.FieldTypeId == 10)
                                            {

                                                DateTime dt1 = DateTime.Now;
                                                DateTime.TryParse(row[i], culture, DateTimeStyles.AssumeLocal, out dt1);
                                                listItem.Data.Add(field.FieldColumn, dt1.ToString("d", culture));
                                            }
                                            else
                                            {
                                                listItem.Data.Add(field.FieldColumn, row[i]);
                                            }
                                            break;
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    model.ListItemModels.Add(listItem);
                }
            }
            return model;
        }
    }
}