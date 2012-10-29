// Copyright 2012, MapGuideForm user group, Frederikssund Kommune and Helsingør Kommune - att. Anette Poulsen and Erling Kristensen
// 
// This file is part of "RapportFraStedet". 
// "RapportFraStedet" is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or any later version.
// "RapportFraStedet" is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License along with "RapportFraStedet". If not, see <http://www.gnu.org/licenses/>.
using RapportFraStedet.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RapportFraStedet.Controllers
{
    public class UploadController : ApiController
    {
        // GET api/upload
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/upload/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/upload
        public string Post()
        {
            /*
            this.Request.Content.CopyToAsync(bufferedContent);
            StreamContent sr = this.Request.Content
            bufferedContent.Write(new byte[256],0,this.Request.Headers..
            content.CopyTo(bufferedContent);*/
            using (var ms = new MemoryStream())
            {
                this.Request.Content.CopyToAsync(ms).Wait();
                ms.Position = 0;
                StreamReader sr = new StreamReader(ms);
                string s = sr.ReadToEnd();
                int index = s.IndexOf(",");
                string image = s.Substring(index + 1);
                string type = s.Substring(0, index);
                string[] info = type.Split(new char[] { ';' });
                int formId = Convert.ToInt32(info[0]);
                string name = info[1] + info[2].Replace("data:image/", ".");
                DatabaseFormsEntities db = new DatabaseFormsEntities();
                Form form = db.Forms.SingleOrDefault(m => m.FormId == formId);
                if (form != null)
                {
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
                /*var sr = new StreamReader(ms);
                var mystr = sr..ReadToEnd();*/
                //ms.Flush();
            }
            /*using (var ms = new FileStream(@"c:\temp\img.jpg",FileMode.Create))
            {
                this.Request.Content.CopyToAsync(ms).Wait();
                //var sr = new StreamReader(ms);
                //var mystr = sr..ReadToEnd();
                //ms.Flush();
            }*/

            
            /*System.Drawing.Image img = System.Drawing.Image.FromStream(
            for(int i =0;i<sc.Headers.ContentLength;i++)
            {
                bufferedContent
            }
            bufferedContent.
            sc.Headers.ContentLength*/
            Console.Write("ok");
            return "ok";
        }

        // PUT api/upload/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/upload/5
        public void Delete(int id)
        {
        }
    }
}
