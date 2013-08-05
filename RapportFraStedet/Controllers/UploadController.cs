using RapportFraStedet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
namespace RapportFraStedet.Controllers
{
    public class UploadController : Controller
    {
        [HttpPost]
        public JsonResult UploadImage()
        {
            try
            {
                RepositorySaveData r = new RepositorySaveData();
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
                return Json(new { id = Path.Combine(model.Form.UploadPhysicalPath, model.UniqueId) });
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