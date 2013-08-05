using RapportFraStedet.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RapportFraStedet.Controllers
{
    public class UploadController : Controller
    {
        [HttpPost]
        public JsonResult UploadImage()
        {
            try
            {
                HttpFileCollectionWrapper files = ((System.Web.HttpFileCollectionWrapper)Request.Files);
                HttpPostedFileBase file = files[0];
                string filePath = file.FileName;
                if (!string.IsNullOrEmpty(filePath))
                {

                    int width = Properties.Settings.Default.MaxWidth;
                    int height = Properties.Settings.Default.MaxHeight;

                    file.SaveAs(filePath);
                    System.Drawing.Image FullsizeImage = System.Drawing.Image.FromFile(filePath);
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
                    string newName = "Thumb_" + Path.GetFileName(filePath);

                    string newFile = Path.Combine(Path.GetDirectoryName(filePath), newName);
                    // Save resized picture
                    NewImage.Save(newFile);
                    NewImage.Dispose();
                }
                return Json(new {result="ok"});

                /*RepositorySaveData r = new RepositorySaveData();
                DataCreateModel dataCreateModel = r.CreateModel(Request.Form, Request.Files, Operation.Create);
                DataViewModel model = dataCreateModel.Model;

                foreach (Field field in model.Form.Fields)
                {
                    if (field.FieldTypeId == 11)
                        field.Data = Request.UserHostAddress;
                }
                RepositoryMapguide repositoryMapGuide = new RepositoryMapguide();
                model = repositoryMapGuide.Add(model);
                dataCreateModel.Model = model;
                repositoryMapGuide.SendEmails(dataCreateModel);
                return Json(new { id = model.UniqueId });*/
            }
            catch (Exception ex)
            {
                string root = Server.MapPath("~/App_Data");
                System.IO.StreamWriter log;
                string file = System.IO.Path.Combine(root, "log.txt");
                if (!System.IO.File.Exists(file))
                {
                    log = new System.IO.StreamWriter(file);
                }
                else
                {
                    log = System.IO.File.AppendText(file);
                }

                // Write to the file:
                log.WriteLine(DateTime.Now);
                log.WriteLine(ex.Message);
                log.WriteLine(ex.StackTrace);

                // Close the stream:
                log.Close();
               return Json(new { error = ex.Message });
            }
        }

    }
}
