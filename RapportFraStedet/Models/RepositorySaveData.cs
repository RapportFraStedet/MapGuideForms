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
using System.Data.Objects;
using System.Data.SqlClient;
using System.Net;
using System.Xml;
using System.Globalization;
using System.IO;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text.RegularExpressions;
namespace RapportFraStedet.Models
{

    public class RepositorySaveData
    {
        private DatabaseFormsEntities db = new DatabaseFormsEntities();
        private WebClient wc = new WebClient();
        Regex email = new Regex("^[a-z0-9A-Z_\\+-]+(\\.[a-z0-9A-Z\\+-]+)*@[a-z0-9A-Z-]+(\\.[a-z0-9A-Z-]+)*\\.([a-zA-Z]{2,4})$");
        public DataCreateModel CreateModel(MultipartFormDataStreamProvider provider, Operation operation)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("da-DK");
            NameValueCollection formValues = provider.FormData;
            char[] trim = new char[] { '\"' };
            DataViewModel model = new DataViewModel();
            foreach (string key in formValues.Keys)
            {
                if (formValues[key].ToLower() != "null")
                {
                    switch (key)
                    {
                        case "FormId":
                            int formid = 0;
                            if (int.TryParse(formValues[key], out formid))
                            {
                                DatabaseFormsEntities db = new DatabaseFormsEntities();
                                model.Form = db.Forms.SingleOrDefault(m => m.FormId == formid);
                            }
                            break;
                        case "ViewId":
                            int viewid = 0;
                            if (int.TryParse(formValues[key], out viewid))
                            {
                                RepositoryViews repository = new RepositoryViews();
                                model.View = repository.Get(viewid);
                            }
                            break;
                        case "UniqueId":
                            model.UniqueId = formValues[key];
                            break;
                        case "UserId":
                            model.UserId = formValues[key];
                            break;
                        case "ItemId":
                            int itemid = 0;
                            if (int.TryParse(formValues[key], out itemid))
                            {
                                model.ItemId = itemid;
                            }
                            break;
                        case "Date":
                            DateTime date = DateTime.Now;
                            if (DateTime.TryParse(formValues[key], culture, DateTimeStyles.AssumeLocal, out date))
                            {
                                model.Date = date;
                            }
                            break;

                    }
                }
            }
            if (operation == Operation.Create)
            {
                model.Date = DateTime.Now;
                model.UniqueId = Guid.NewGuid().ToString();
                /*if (User.Identity.IsAuthenticated)
                {
                    model.UserName = User.Identity.Name;
                    model.UserId = Membership.GetUser(User.Identity.Name).ProviderUserKey.ToString();
                }*/
            }
            if (!String.IsNullOrEmpty(model.UserName))
            {
                UserProfile profile = UserProfile.GetUserProfile(model.UserName);
                RepositoryCompanies rc = new RepositoryCompanies();
                Company company = rc.Get(profile.CompanyId);
                if (company != null)
                    model.Company = company.Name;
            }

            List<DataEmailModel> emails = new List<DataEmailModel>();
            if (!String.IsNullOrEmpty(model.Form.Email))
            {
                if (operation == Operation.Create && !String.IsNullOrEmpty(model.Form.ViewEmailToReceiverOnCreate))
                {
                    emails.Add(new DataEmailModel { Email = model.Form.Email, View = model.Form.ViewEmailToReceiverOnCreate });
                }
                else if (operation == Operation.Edit && !String.IsNullOrEmpty(model.Form.ViewEmailToReceiverOnEdit))
                {
                    emails.Add(new DataEmailModel { Email = model.Form.Email, View = model.Form.ViewEmailToReceiverOnEdit });
                }
            }
            foreach (Field field in model.Form.Fields)
            {
                if (!string.IsNullOrEmpty(field.FieldColumn))
                {
                    #region 8 Upload
                    if (field.FieldTypeId == 8) //Upload
                    {
                        bool fundet = false;
                        foreach (MultipartFileData file in provider.FileData)
                        {

                            string id = file.Headers.ContentDisposition.Name.Trim(trim);
                            
                            int id2 = 0;
                            int.TryParse(id, out id2);
                            if (field.FieldId == id2)
                            {
                                fundet = true;
                                string filename = file.Headers.ContentDisposition.FileName.Trim(trim);
                                if (!string.IsNullOrEmpty(filename))
                                {
                                    string name = model.UniqueId + "-" + field.FieldId.ToString() + Path.GetExtension(filename).ToLower();
                                    string filePath = Path.Combine(model.Form.UploadPhysicalPath, name);
                                    field.Data = name;

                                    int width = Properties.Settings.Default.MaxWidth;
                                    int height = Properties.Settings.Default.MaxHeight;
                                    System.Drawing.Image FullsizeImage = System.Drawing.Image.FromFile(file.LocalFileName);
                                    FullsizeImage.Save(filePath);
                                    FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
                                    FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);

                                    if (FullsizeImage.Width <= width)
                                    {
                                        width = FullsizeImage.Width;
                                    }


                                    int NewHeight = FullsizeImage.Height * width / FullsizeImage.Width;
                                    if (NewHeight > height)
                                    {
                                        // Resize with height instead
                                        width = FullsizeImage.Width * height / FullsizeImage.Height;
                                        NewHeight = height;
                                    }

                                    System.Drawing.Image NewImage = FullsizeImage.GetThumbnailImage(width, NewHeight, null, IntPtr.Zero);

                                    // Clear handle to original file so that we can overwrite it if necessary
                                    FullsizeImage.Dispose();
                                    string newName = "Thumb_" + Path.GetFileName(name);
                                    string newFile = Path.Combine(model.Form.UploadPhysicalPath, newName);
                                    // Save resized picture
                                    NewImage.Save(newFile);
                                    NewImage.Dispose();
                                    File.Delete(file.LocalFileName);
                                }
                            }

                        }
                        if (!fundet && formValues[field.FieldId.ToString()] != null)
                        {
                            string[] info = formValues[field.FieldId.ToString()].Split(new char[] { ';' });
                            int index = info[1].IndexOf(",");
                            string image = info[1].Substring(index + 1);
                            string name = model.UniqueId + "-" + field.FieldId.ToString() + info[0].Replace("data:image/", ".");
                            string filePath = Path.Combine(model.Form.UploadPhysicalPath, name);
                            using (var fs = new FileStream(filePath, FileMode.Create))
                            {
                                byte[] byteFromString;
                                byteFromString = Convert.FromBase64String(image);
                                fs.Write(byteFromString, 0, byteFromString.Length);

                            }
                            try
                            {
                                int width = Properties.Settings.Default.MaxWidth;
                                int height = Properties.Settings.Default.MaxHeight;
                                System.Drawing.Image FullsizeImage = System.Drawing.Image.FromFile(filePath);

                                // Prevent using images internal thumbnail
                                FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
                                FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);

                                if (FullsizeImage.Width <= width)
                                {
                                    width = FullsizeImage.Width;
                                }


                                int NewHeight = FullsizeImage.Height * width / FullsizeImage.Width;
                                if (NewHeight > height)
                                {
                                    // Resize with height instead
                                    width = FullsizeImage.Width * height / FullsizeImage.Height;
                                    NewHeight = height;
                                }

                                System.Drawing.Image NewImage = FullsizeImage.GetThumbnailImage(width, NewHeight, null, IntPtr.Zero);

                                // Clear handle to original file so that we can overwrite it if necessary
                                FullsizeImage.Dispose();
                                string newName = "Thumb_" + Path.GetFileName(name);
                                string newFile = Path.Combine(model.Form.UploadPhysicalPath, newName);
                                // Save resized picture
                                NewImage.Save(newFile);
                                NewImage.Dispose();
                            }
                            catch
                            { }
                        }
                    }
                    #endregion
                    else if (field.FieldTypeId == 16) //Upload
                    {
                        field.Data = formValues["Geometri"];
                    }
                    else if (field.FieldTypeId == 17) //Upload
                    {
                        field.Data = formValues["Position"];
                    }
                    else if (field.FieldTypeId == 18) //Upload
                    {
                        field.Data = formValues["Accuracy"];
                    }
                    else
                    {
                        string value = formValues[field.FieldId.ToString()];
                        field.Data = value;
                        switch (field.FieldTypeId)
                        {
                            case 7:

                                if (email.IsMatch(field.Data))
                                {
                                    string emailView = "";
                                    Field operationField = null;
                                    if (operation == Operation.Create)
                                    {
                                        emailView = field.Form.ViewEmailToSenderOnCreate;
                                        operationField = field.Form.Fields.FirstOrDefault(m => m.FieldTypeId == 12);
                                    }
                                    else if (operation == Operation.Edit)
                                    {
                                        emailView = field.Form.ViewEmailToSenderOnEdit;
                                        operationField = field.Form.Fields.FirstOrDefault(m => m.FieldTypeId == 13);
                                    }
                                    if (!String.IsNullOrEmpty(emailView) && operationField != null)
                                    {
                                        string send = formValues[operationField.FieldId.ToString()];
                                        if (!String.IsNullOrEmpty(send))
                                        {
                                            if (send != "false" && send != "off")
                                                emails.Add(new DataEmailModel { Email = field.Data, View = emailView });
                                        }

                                    }
                                }
                                break;
                            /*case 10:

                                DateTime dt;
                                if (!DateTime.TryParse(field.Data, culture, DateTimeStyles.AssumeLocal, out dt))
                                {
                                    ModelState.AddModelError(field.FieldId.ToString(), field.Data + " er ikke en gyldig dato");
                                }
                                break;*/
                        }
                        Selection selection = field.Selections.SingleOrDefault(m => m.Name == field.Data);
                        if (selection != null)
                        {

                            if (!String.IsNullOrEmpty(selection.Email))
                            {
                                if (operation == Operation.Create && !String.IsNullOrEmpty(model.Form.ViewEmailToReceiverOnCreate))
                                {
                                    emails.Add(new DataEmailModel { Email = selection.Email, View = model.Form.ViewEmailToReceiverOnCreate });
                                }
                                else if (operation == Operation.Edit && !String.IsNullOrEmpty(model.Form.ViewEmailToReceiverOnEdit))
                                {
                                    emails.Add(new DataEmailModel { Email = selection.Email, View = model.Form.ViewEmailToReceiverOnEdit });
                                }

                            }
                        }

                    }
                }
            }
            return new DataCreateModel()
            {
                Emails = emails,
                Model = model,
            };
        }
        public Data Add(Data data)
        {
            return data;
        }
        public Data Get(View view)
        {
            Data data = new Data();
            data.ViewId = view.ViewId;
            List<Felt> fields = new List<Felt>();
            Form form = db.Forms.SingleOrDefault(d => d.FormId == view.Group.DefaultFormId);
            if (form != null)
            {
                data.SRS = form.SRS;
                data.FormId = form.FormId;
                foreach (Field field in form.Fields.OrderBy(m => m.FieldOrder))
                {
                    Felt felt = new Felt()
                    {
                        Data = field.Data,
                        Name = field.Name,
                        Required = field.Required,
                        TypeId = field.FieldTypeId,
                        Id = field.FieldId
                    };
                    List<Udvælgelse> selections = new List<Udvælgelse>();
                    int i = 0;
                    foreach (Models.Selection selection in field.Selections.OrderBy(m => m.SelectionOrder))
                    {
                        i++;
                        Udvælgelse udvælgelse = new Udvælgelse();
                        udvælgelse.Name = selection.Name;
                        udvælgelse.Id = field.FieldId.ToString() + "-" + i.ToString();
                        if (selection.Name == field.Data)
                            udvælgelse.Selected = true;
                        else
                            udvælgelse.Selected = false;
                        selections.Add(udvælgelse);
                    }
                    felt.Selections = selections;
                    Permission permission = view.Permissions.SingleOrDefault(m => m.FieldId == field.FieldId);
                    if (permission != null)
                    {
                        felt.Permission = permission.PermissionTypeId;
                    }
                    else
                        felt.Permission = 3;
                    fields.Add(felt);
                }
            }
            data.Felter = fields;
            return data;

        }
        public bool Insert(NameValueCollection formData)
        {
            Form form = null;
            int j = 0;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("da-DK");
            List<string> columns = new List<string>();
            List<string> values = new List<string>();
            string projCode = "";
            string position = "";
            foreach (string key in formData.Keys)
            {
                switch (key)
                {
                    case "FormId":
                        int id =0;
                        if (int.TryParse(formData[key], out id))
                        {
                            DatabaseFormsEntities db = new DatabaseFormsEntities();
                            form = db.Forms.SingleOrDefault(m => m.FormId == id);
                        }
                        break;
                    case "projCode":
                        projCode = formData[key];
                        break;
                    case "position":
                        position = formData[key];
                        break;
                    case "UserId":
                        columns.Add("UserId");
                        values.Add("'" + formData[key] + "'");
                        break;
                    default:
                        foreach (Field field in form.Fields)
                        {
                            if (!String.IsNullOrEmpty(field.FieldColumn) && formData[key] == field.FieldId.ToString())
                            {
                                #region 8 Upload
                                if (field.FieldTypeId == 8) //Upload
                                {
                                    string value = formData[key];
                                }
                                #endregion
                                else if (field.FieldTypeId == 10)
                                {
                                    DateTime dt = DateTime.Now;
                                    DateTime.TryParse(formData[key], culture, DateTimeStyles.AssumeLocal, out dt);
                                    columns.Add(field.FieldColumn);
                                    if (Properties.Settings.Default.ReverseDate)
                                        values.Add(String.Format("'{0:dd-MM-yyyy HH:mm:ss}'", dt));
                                    else
                                        values.Add(String.Format("'{0:yyyy-MM-dd HH:mm:ss}'", dt));
                                }
                                else if (field.FieldTypeId == 6 || field.FieldTypeId == 13)
                                {
                                    string b = "0";
                                    if (formData[key] != "false")
                                        b = "1";
                                    columns.Add(field.FieldColumn);
                                    values.Add("'" + b + "'");
                                }
                                else
                                {
                                    columns.Add(field.FieldColumn);
                                    values.Add("'" + formData[key] + "'");
                                }
                            }
                        }
                        break;
                }
            }
            columns.Add("UniqueId");
            string uniqueId = Guid.NewGuid().ToString();
            values.Add("'"+uniqueId+"'");
            columns.Add("Dato");
            if (Properties.Settings.Default.ReverseDate)
                values.Add(String.Format("'{0:dd-MM-yyyy HH:mm:ss}'", DateTime.Now));
            else
                values.Add(String.Format("'{0:yyyy-MM-dd HH:mm:ss}'",DateTime.Now));
            string sql = "insert into "+form.ClassName+ "(";
            foreach(string column in columns)
            {
                sql=sql+column+",";
            }
            sql = sql.TrimEnd(new char[]{','});
            sql=sql+") values(";
            foreach(string value in values)
            {
                sql=sql+value+",";
            }
            sql = sql.TrimEnd(new char[]{','});
            sql = sql + ")";
            SqlConnection con = new SqlConnection(form.ResourceName);
            SqlCommand command = new SqlCommand(sql, con);
            con.Open();
            try
            {
                j = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            { }
            finally
            {
                con.Close();
            }
            if (j > 0)
                return true;
            else
                return false;
        }

        public bool Insert(MultipartFormDataStreamProvider provider)
        {
            NumberFormatInfo number = new NumberFormatInfo();
            number.NumberDecimalSeparator = ".";
            number.NumberGroupSeparator = ",";
            char[] trim = new char[] { '\"' };
            string projCode = "";
            string position = "";
            double accuracy = 0;
            string geometry = "";
            int formId = 0;
            Dictionary<string, string> data = new Dictionary<string, string>();
            foreach (string key in provider.FormData.Keys)
            {
                switch (key)
                {
                    case "FormId":
                        int.TryParse(provider.FormData[key], out formId);
                        break;
                    case "ProjCode":
                        projCode = provider.FormData[key];
                        break;
                    case "Position":
                        position = provider.FormData[key];
                        break;
                    case "Accuracy":
                        double.TryParse(provider.FormData[key], NumberStyles.Number, number, out accuracy);
                        break;
                    case "Geometry":
                        geometry = provider.FormData[key];
                        break;
                    default:
                        data.Add(key, provider.FormData[key]);
                        break;
                }
            }
            int j =0;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("da-DK");
            DatabaseFormsEntities db = new DatabaseFormsEntities();
            Form form = db.Forms.SingleOrDefault(m=>m.FormId == formId);
            List<string> columns = new List<string>();
            List<string> values = new List<string>();
            if (geometry != "" && projCode != "")
            {
                columns.Add("Geometri");
                values.Add("geometry::STGeomFromText('" + geometry + "'," + projCode.Substring(5) + ")");
            }
            columns.Add("UniqueId");
            string uniqueId = Guid.NewGuid().ToString();
            values.Add("'"+uniqueId+"'");
            columns.Add("Dato");
            if (Properties.Settings.Default.ReverseDate)
                values.Add(String.Format("'{0:dd-MM-yyyy HH:mm:ss}'", DateTime.Now));
            else
                values.Add(String.Format("'{0:yyyy-MM-dd HH:mm:ss}'",DateTime.Now));
            if (data.ContainsKey("UserId"))
            {
                columns.Add("UserId");
                values.Add("'"+data["UserId"]+"'");
            }
            foreach (Field field in form.Fields)
            {
                #region 8 Upload
                if (field.FieldTypeId == 8) //Upload
                {
                    bool fundet = false;
                    foreach (MultipartFileData file in provider.FileData)
                    {

                        string id = file.Headers.ContentDisposition.Name.Trim(trim);
                        string filename = file.Headers.ContentDisposition.FileName.Trim(trim);
                        int id2 = 0;
                        int.TryParse(id, out id2);
                        if (field.FieldId == id2)
                        {
                            fundet = true;
                            string name = uniqueId + "-" + field.FieldId.ToString() + Path.GetExtension(filename).ToLower();
                            string filePath = Path.Combine(form.UploadPhysicalPath, name);
                            columns.Add(field.FieldColumn);
                            values.Add("'" + name + "'");

                            int width = Properties.Settings.Default.MaxWidth;
                            int height = Properties.Settings.Default.MaxHeight;
                            System.Drawing.Image FullsizeImage = System.Drawing.Image.FromFile(file.LocalFileName);
                            FullsizeImage.Save(filePath);
                            FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
                            FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);

                            if (FullsizeImage.Width <= width)
                            {
                                width = FullsizeImage.Width;
                            }


                            int NewHeight = FullsizeImage.Height * width / FullsizeImage.Width;
                            if (NewHeight > height)
                            {
                                // Resize with height instead
                                width = FullsizeImage.Width * height / FullsizeImage.Height;
                                NewHeight = height;
                            }

                            System.Drawing.Image NewImage = FullsizeImage.GetThumbnailImage(width, NewHeight, null, IntPtr.Zero);

                            // Clear handle to original file so that we can overwrite it if necessary
                            FullsizeImage.Dispose();
                            string newName = "Thumb_" + Path.GetFileName(name);
                            string newFile = Path.Combine(form.UploadPhysicalPath, newName);
                            // Save resized picture
                            NewImage.Save(newFile);
                            NewImage.Dispose();

                        }

                    }
                    if (!fundet && data.ContainsKey(field.FieldId.ToString()))
                    {
                        string[] info = data[field.FieldId.ToString()].Split(new char[] { ';' });
                        int index = info[1].IndexOf(",");
                        string image = info[1].Substring(index + 1);
                        string name = uniqueId + "-" + field.FieldId.ToString() + info[0].Replace("data:image/", ".");
                        string filePath = Path.Combine(form.UploadPhysicalPath, name);
                        using (var fs = new FileStream(filePath, FileMode.Create))
                        {
                            byte[] byteFromString;
                            byteFromString = Convert.FromBase64String(image);
                            fs.Write(byteFromString, 0, byteFromString.Length);

                        }
                        try
                        {
                            int width = Properties.Settings.Default.MaxWidth;
                            int height = Properties.Settings.Default.MaxHeight;
                            System.Drawing.Image FullsizeImage = System.Drawing.Image.FromFile(filePath);

                            // Prevent using images internal thumbnail
                            FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
                            FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);

                            if (FullsizeImage.Width <= width)
                            {
                                width = FullsizeImage.Width;
                            }


                            int NewHeight = FullsizeImage.Height * width / FullsizeImage.Width;
                            if (NewHeight > height)
                            {
                                // Resize with height instead
                                width = FullsizeImage.Width * height / FullsizeImage.Height;
                                NewHeight = height;
                            }

                            System.Drawing.Image NewImage = FullsizeImage.GetThumbnailImage(width, NewHeight, null, IntPtr.Zero);

                            // Clear handle to original file so that we can overwrite it if necessary
                            FullsizeImage.Dispose();
                            string newName = "Thumb_" + Path.GetFileName(name);
                            string newFile = Path.Combine(form.UploadPhysicalPath, newName);
                            // Save resized picture
                            NewImage.Save(newFile);
                            NewImage.Dispose();
                        }
                        catch
                        { }
                    }
                }
                #endregion
                else if (!String.IsNullOrEmpty(field.FieldColumn) && data.ContainsKey(field.FieldId.ToString()))
                {

                    if (field.FieldTypeId == 10)
                    {
                        DateTime dt = DateTime.Now;
                        DateTime.TryParse(data[field.FieldId.ToString()], culture, DateTimeStyles.AssumeLocal, out dt);
                        columns.Add(field.FieldColumn);
                        if (Properties.Settings.Default.ReverseDate)
                            values.Add(String.Format("'{0:dd-MM-yyyy HH:mm:ss}'", dt));
                        else
                            values.Add(String.Format("'{0:yyyy-MM-dd HH:mm:ss}'", dt));
                    }
                    else if (field.FieldTypeId == 6 || field.FieldTypeId == 13)
                    {
                        string b = "0";
                        if (data[field.FieldId.ToString()] != "false")
                            b = "1";
                        columns.Add(field.FieldColumn);
                        values.Add("'" + b + "'");
                    }
                    else
                    {
                        columns.Add(field.FieldColumn);
                        values.Add("'" + data[field.FieldId.ToString()] + "'");
                    }
                }
            }
            string sql = "insert into "+form.ClassName+ "(";
            foreach(string column in columns)
            {
                sql=sql+column+",";
            }
            sql = sql.TrimEnd(new char[]{','});
            sql=sql+") values(";
            foreach(string value in values)
            {
                sql=sql+value+",";
            }
            sql = sql.TrimEnd(new char[]{','});
            sql = sql + ")";
            SqlConnection con = new SqlConnection(form.ResourceName);
            SqlCommand command = new SqlCommand(sql, con);
            con.Open();
            try
            {
                j = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            { }
            finally
            {
                con.Close();
            }
            if (j > 0)
                return true;
            else
                return false;
        }
    }
}
