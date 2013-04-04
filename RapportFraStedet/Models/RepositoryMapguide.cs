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
using RapportFraStedet.Models;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Net.Mail;
using RapportFraStedet.Mailers;
namespace RapportFraStedet.Models
{
    public class RepositoryMapguide 
    {
        private char[] SplitterCOMMA = new char[] { ';' };
        private CultureInfo culture = CultureInfo.CreateSpecificCulture("da-DK");
        class UserInfo
        {
            public string UserName { get; set; }
            public string CompanyName { get; set; }
        }
        public DataListModel GetList(View view, string userId)
        {
            
            RepositoryCompanies repositoryCompanies = new RepositoryCompanies();
            List<Field> columns = new List<Field>();
            Form form = view.Group.Forms.Single(m => m.FormId == view.Group.DefaultFormId);
            string select = "";
            bool fId = false;
            bool fUserId = false;
            bool fUniqueId = false;
            bool fDato = false;
            foreach (Field field in form.Fields.Where(a => a.FieldColumn != null && !a.FieldColumn.Equals(String.Empty)).OrderBy(b => b.FieldOrder))
            {
                Permission permission = view.Permissions.FirstOrDefault(a => a.FieldId == field.FieldId);
                if (permission == null || permission.PermissionTypeId != 0)
                {
                    columns.Add(field);
                    switch (field.FieldColumn.ToUpper())
                    {
                        case "ID":
                            select = select + "," + field.FieldColumn;
                            fId = true;
                            break;
                        case "UNIQUEID":
                            select = select + "," + field.FieldColumn;
                            fUniqueId = true;
                            break;
                        case "USERID":
                            select = select + "," + field.FieldColumn;
                            fUserId = true;
                            break;
                        case "DATO":
                            select = select + "," + field.FieldColumn;
                            fDato = true;
                            break;
                        default:
                            if (field.FieldTypeId == 16)
                            {
                                select = select + "," + field.FieldColumn + ".STAsText() AS " + field.FieldColumn;
                            }
                            else
                            {
                                select = select + "," + field.FieldColumn;
                            }

                            break;
                    }

                }

            }
            if (!fUniqueId)
                select = select + ",UniqueId";
            if (!fUserId)
                select = select + ",UserId";
            if (!fDato)
                select = select + ",Dato";
            if (!fId)
                select = select + ",Id";
            select = "SELECT " + select.Substring(1);

            DataListModel model = new DataListModel
            {
                View = view,
                Form = form,
                Columns = columns
            };
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(form.ResourceName))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = select+" FROM " + form.ClassName;
                if (!String.IsNullOrEmpty(userId))
                {
                    command.CommandText = command.CommandText + " WHERE UserId = '" + userId + "'";
                }
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
            }
            Dictionary<string, UserInfo> users = new Dictionary<string, UserInfo>();
            foreach (DataRow row in dt.Rows)
            {
                DataListItemModel listItem = new DataListItemModel();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (!row.IsNull(i))
                    {

                        string propertyName = dt.Columns[i].ColumnName;
                        string propertyNameUpper = propertyName.ToUpper();
                        switch (propertyNameUpper)
                        {
                            case "ID":
                                listItem.ItemId = (int)row[i];
                                if(fId)
                                    listItem.Data.Add(propertyName, row[i].ToString());
                                break;
                            case "UNIQUEID":

                                listItem.UniqueId = (string)row[i];
                                if (fUniqueId)
                                    listItem.Data.Add(propertyName, row[i].ToString());
                                break;
                            case "USERID":
                                if (row[i] != DBNull.Value && row[i].ToString() != "")
                                {
                                    listItem.UserId = (string)row[i];
                                    if (fUserId)
                                        listItem.Data.Add(propertyName, row[i].ToString());
                                    UserInfo user = null;
                                    if (users.ContainsKey(listItem.UserId))
                                        user = users[listItem.UserId];
                                    else
                                    {
                                        MembershipUser user1 = Membership.GetUser(new Guid(listItem.UserId));
                                        if (user1 != null)
                                        {
                                            user = new UserInfo();
                                            user.UserName = user1.UserName;
                                            UserProfile profile = UserProfile.GetUserProfile(user1.UserName);
                                            if (profile != null)
                                            {
                                                Company company = repositoryCompanies.Get(profile.CompanyId);
                                                if (company != null)
                                                {
                                                    user.CompanyName = company.Name;
                                                }
                                            }
                                        }
                                        users.Add(listItem.UserId, user);
                                    }
                                    if (user != null)
                                    {
                                        listItem.UserName = user.UserName;
                                        listItem.Company = user.CompanyName;
                                    }
                                }
                                break;
                            case "DATO":
                                listItem.Date = (DateTime)row[i];
                                if (fDato)
                                    listItem.Data.Add(propertyName, row[i].ToString());
                                break;
                            default:
                                listItem.Data.Add(propertyName, row[i].ToString());
                                break;
                        }
                    }
                }
                model.ListItemModels.Add(listItem);
            }
            return model;
        }
        
        public DataViewModel Get(DataViewModel m)
        {
            RepositoryCompanies repositoryCompanies = new RepositoryCompanies();
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            nfi.NumberGroupSeparator = ",";
            Form form = m.Form;
            View view = m.View;
            List<Field> columns = new List<Field>();
            Dictionary<string, UserInfo> users = new Dictionary<string, UserInfo>();
            string select = "";
            bool fId = false;
            bool fUserId = false;
            bool fUniqueId = false;
            bool fDato = false;
            foreach (Field field in form.Fields.Where(a => a.FieldColumn != null && !a.FieldColumn.Equals(String.Empty)).OrderBy(b => b.FieldOrder))
            {
                Permission permission = view.Permissions.FirstOrDefault(a => a.FieldId == field.FieldId);
                //if (permission == null )//|| permission.PermissionTypeId != 0)
                //{
                    columns.Add(field);
                    switch(field.FieldColumn.ToUpper())
                    {
                       
                        case "ID":
                            select = select + "," + field.FieldColumn;
                            fId = true;
                            break;
                        case "UNIQUEID":
                            select = select + "," + field.FieldColumn;
                            fUniqueId = true;
                            break;
                        case "USERID":
                            select = select + "," + field.FieldColumn;
                            fUserId = true;
                            break;
                        case "DATO":
                            select = select + "," + field.FieldColumn;
                            fDato = true;
                            break;
                        default:
                            if (field.FieldTypeId == 16 || field.FieldTypeId == 17)
                            {
                                select = select + "," + field.FieldColumn+".STAsText() AS " + field.FieldColumn;
                            }
                            else
                            {
                                select = select + "," + field.FieldColumn;
                            }
                            break;
                    }
                        
                //}

            }
            if(!fUniqueId)
                select = select + ",uniqueid";
            if (!fUserId)
                select = select + ",userid";
            if (!fDato)
                select = select + ",dato";
            if (!fId)
                select = select + ",id";
            select = "SELECT " + select.Substring(1);
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(m.Form.ResourceName))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = select +" FROM "+ m.Form.ClassName + " WHERE Id="+m.ItemId.ToString();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
            }
            DataViewModel model = new DataViewModel
            {
                View = m.View,
                Form = m.Form,
            };
            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (!row.IsNull(i))
                    {

                        string propertyName = dt.Columns[i].ColumnName;
                        string propertyNameUpper = propertyName.ToUpper();
                        switch (propertyNameUpper)
                        {
                            case "ID":
                                model.ItemId = (int)row[i];
                                foreach (Field field in model.Form.Fields)
                                {
                                    if (!string.IsNullOrEmpty(field.FieldColumn))
                                    {
                                        if (field.FieldColumn.ToUpper() == propertyNameUpper)
                                        {
                                            field.Data = row[i].ToString();
                                        }
                                    }
                                }
                                break;
                            case "UNIQUEID":
                                model.UniqueId = (string)row[i];
                                break;
                            case "USERID":
                                if (row[i] != DBNull.Value && row[i].ToString() != "")
                                {
                                    model.UserId = (string)row[i];
                                    UserInfo user = null;
                                    if (users.ContainsKey(model.UserId))
                                        user = users[model.UserId];
                                    else
                                    {
                                        MembershipUser user1 = Membership.GetUser(new Guid(model.UserId));
                                        if (user1 != null)
                                        {
                                            user = new UserInfo();
                                            user.UserName = user1.UserName;
                                            UserProfile profile = UserProfile.GetUserProfile(user1.UserName);
                                            if (profile != null)
                                            {
                                                Company company = repositoryCompanies.Get(profile.CompanyId);
                                                if (company != null)
                                                {
                                                    user.CompanyName = company.Name;
                                                }
                                            }
                                        }
                                        users.Add(model.UserId, user);
                                    }
                                    if (user != null)
                                    {
                                        model.UserName = user.UserName;
                                        model.Company = user.CompanyName;
                                    }
                                }
                                break;
                            case "DATO":
                                model.Date = (DateTime)row[i];
                                break;
                            default:
                                foreach (Field field in model.Form.Fields)
                                {
                                    if (!string.IsNullOrEmpty(field.FieldColumn))
                                    {
                                        if (field.FieldColumn.ToUpper() == propertyNameUpper)
                                        {
                                            field.Data = row[i].ToString();
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            return model;
        }
        //
        // Insert/Delete Methods
        public DataViewModel Add(DataViewModel model)
        {
            Form form = model.Form;
            /*View view = model.View;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("da-DK");*/
            Dictionary<string, string> parameters = GetProperties(model);
            string sql = "insert into " + form.ClassName + "(";
            foreach (string column in parameters.Keys)
            {
                sql = sql + column + ",";
            }
            sql = sql.TrimEnd(new char[] { ',' });
            sql = sql + ") values(";
            foreach (string value in parameters.Values)
            {
                sql = sql + value + ",";
            }
            sql = sql.TrimEnd(new char[] { ',' });
            sql = sql + ")";
            using (SqlConnection con = new SqlConnection(form.ResourceName))
            {
                SqlCommand command = new SqlCommand(sql, con);
                con.Open();
                int j = command.ExecuteNonQuery();
                if (j == 1)
                {
                    sql = "SELECT id from "+form.ClassName+" WHERE uniqueid='"+model.UniqueId+"'";
                    SqlCommand commandRead = new SqlCommand(sql, con);
                    model.ItemId = (int)commandRead.ExecuteScalar();
 
                }
            }
            return model;
        }

        public void Update(DataViewModel model)
        {
            Dictionary<string, string> parameters = GetProperties(model);
            string sql = "update " + model.Form.ClassName + " set ";
            foreach (KeyValuePair<string, string> pair in parameters)
            {
                sql = sql + pair.Key +"="+pair.Value+",";
            }
            sql = sql.TrimEnd(new char[] { ',' });
            sql = sql + " WHERE Id=" + model.ItemId;
            using (SqlConnection con = new SqlConnection(model.Form.ResourceName))
            {
                SqlCommand command = new SqlCommand(sql, con);
                con.Open();
                int j = command.ExecuteNonQuery();
            }
        }
        public void Delete(DataViewModel model)
        {
            using (SqlConnection connection = new SqlConnection(model.Form.ResourceName))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "DELETE FROM " + model.Form.ClassName + " WHERE Id=" + model.ItemId.ToString();
                int i = command.ExecuteNonQuery();
                if (i == 0)
                    throw new Exception("Data er ikke slettet");
            }
            string[] files = System.IO.Directory.GetFiles(model.Form.UploadPhysicalPath, "*" + model.UniqueId + "*.*");
            foreach (string file in files)
            {
                System.IO.File.Delete(file);
            }
        }
        public void SendEmails(DataCreateModel dataCreateModel)
        {
            IUserMailer userMailer = new UserMailer();
            DataViewModel model = dataCreateModel.Model;
            if (Properties.Settings.Default.SendEmails)
            {
                SmtpClient client = new SmtpClient();
                foreach (DataEmailModel email in dataCreateModel.Emails)
                {
                    if (!String.IsNullOrEmpty(email.Email) && !String.IsNullOrEmpty(email.View))
                    {
                        MailMessage m = userMailer.CreateMail(model, email.View);
                        //m.BodyEncoding = System.Text.Encoding.UTF8;
                        //m.SubjectEncoding = System.Text.Encoding.UTF8;
                        int start = m.Body.IndexOf("<title>") + 7;
                        int end = m.Body.IndexOf("</title>");
                        m.Subject = System.Net.WebUtility.HtmlDecode(m.Body.Substring(start, end - start).Trim(new char[] { ' ', '\r', '\n' }));
                        m.To.Add(email.Email);
                        client.Send(m);
                    }
                }
            }
        }
        private Dictionary<string, string> GetProperties(DataViewModel model)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (Properties.Settings.Default.ReverseDate)
                parameters.Add("Dato", String.Format("'{0:dd-MM-yyyy HH:mm:ss}'", model.Date));
            else
                parameters.Add("Dato", String.Format("'{0:yyyy-MM-dd HH:mm:ss}'", model.Date));
            parameters.Add("UniqueId", "'" + model.UniqueId + "'");
            if (!String.IsNullOrEmpty(model.UserId))
            {
                parameters.Add("UserId","'"+model.UserId+"'");
            }
            foreach (Field field in model.Form.Fields)
            {
                if (!String.IsNullOrEmpty(field.FieldColumn) && !String.IsNullOrEmpty(field.Data))
                {

                    if (field.FieldTypeId == 10)
                    {
                        DateTime dt = DateTime.Now;
                        DateTime.TryParse(field.Data, culture, DateTimeStyles.AssumeLocal, out dt);
                        if(Properties.Settings.Default.ReverseDate)
                            parameters.Add(field.FieldColumn, String.Format("'{0:dd-MM-yyyy HH:mm:ss}'", dt));
                        else
                            parameters.Add(field.FieldColumn, String.Format("'{0:yyyy-MM-dd HH:mm:ss}'", dt));
                    }
                    else if (field.FieldTypeId == 6 || field.FieldTypeId == 12 || field.FieldTypeId == 13)
                    {
                        string b = "0";
                        if (field.Data != "false" && field.Data != "off")
                            b = "1";
                        parameters.Add(field.FieldColumn, b);
                    }
                    else if (field.FieldTypeId == 15)
                    {
                        parameters.Add(field.FieldColumn, field.Data.Replace(',','.'));
                    }
                    else if (field.FieldTypeId == 16)
                    {
                        parameters.Add(field.FieldColumn, "geometry::STGeomFromText('" + field.Data + "'," + model.Form.SRS.ToUpper().Replace("EPSG:", "") + ")");
                    }
                    else if (field.FieldTypeId == 17)
                    {
                        parameters.Add(field.FieldColumn, "geometry::STGeomFromText('" + field.Data + "',4326)");
                    }
                    else if (field.FieldTypeId == 18)
                    {
                        parameters.Add(field.FieldColumn, field.Data);
                    }
                    else
                    {
                        if(field.FieldColumn.ToUpper()!="ID")
                            parameters.Add(field.FieldColumn, "'" + field.Data + "'");
                    }
                }
            }
            return parameters;
        }
        
    }
}