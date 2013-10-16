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
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace RapportFraStedet.Controllers
{
    public class SaveFormsData2Controller : ApiController
    {
        [AcceptVerbs("OPTIONS")]
        public HttpResponseMessage Options()
        {
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Headers.Add("Access-Control-Allow-Methods", "POST");
            return resp;
        }
        public class NameValue
        {
            public int name { get; set; }
            public string value { get; set; }
        }
        //public async Task<HttpResponseMessage> PostFormData()
        public Task<HttpResponseMessage> PostFormData()
        {
            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            StreamWriter log1;
            string file1 = Path.Combine(@"d:\inetpub\wwwroot\rapportfrastedet\app_data\", "log1.txt");
            if (!File.Exists(file1))
            {
                log1 = new StreamWriter(file1);
            }
            else
            {
                log1 = File.AppendText(file1);
            }

            // Write to the file:
            log1.WriteLine(DateTime.Now);
            log1.WriteLine(root);

            // Close the stream:
            log1.Close();

            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            
            var provider = new MultipartFormDataStreamProvider(root);

            /**/
            // Read the form data.
            //await Request.Content.ReadAsMultipartAsync(provider);
            return Request.Content.ReadAsMultipartAsync(provider).ContinueWith(data =>
            {//.Wait();

                // This illustrates how to get the file names.
                /*foreach (MultipartFileData file in provider.FileData)
                {
                    Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                    Trace.WriteLine("Server file path: " + file.LocalFileName);
                }*/
                try
                {
                    HttpContextWrapper http = (HttpContextWrapper)Request.Properties["MS_HttpContext"];
                    HttpContext.Current = http.ApplicationInstance.Context;
                    RepositorySaveData r = new RepositorySaveData();
                    DataCreateModel dataCreateModel = r.CreateModel(data.Result.FormData, data.Result.FileData, Operation.Create);
                    DataViewModel model = dataCreateModel.Model;

                    foreach (Field field in model.Form.Fields)
                    {
                        if (field.FieldTypeId == 11)
                            field.Data = http.Request.UserHostAddress;
                    }
                    RepositoryMapguide repositoryMapGuide = new RepositoryMapguide();
                    model = repositoryMapGuide.Add(model);
                    dataCreateModel.Model = model;
                    repositoryMapGuide.SendEmails(dataCreateModel);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                catch (System.Exception e)
                {
                    StreamWriter log;
                    string file = Path.Combine(root, "log.txt");
                    if (!File.Exists(file))
                    {
                        log = new StreamWriter(file);
                    }
                    else
                    {
                        log = File.AppendText(file);
                    }

                    // Write to the file:
                    log.WriteLine(DateTime.Now);
                    log.WriteLine(e.Message);
                    log.WriteLine(e.StackTrace);

                    // Close the stream:
                    log.Close();
                    HttpResponseMessage r = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
                    System.Net.Http.Headers.MediaTypeWithQualityHeaderValue json = new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json");
                    System.Net.Http.Headers.MediaTypeWithQualityHeaderValue plain = new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/plain");
                    if (!Request.Headers.Accept.Contains(json))
                    {
                        r.Content.Headers.ContentType = plain;
                    }
                    return r;

                }
            });
            /**/
        }
        private string GetClientIp(HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                HttpContextWrapper http = (HttpContextWrapper)request.Properties["MS_HttpContext"];

                return http.Request.UserHostAddress;
            }
            else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop;
                prop = (RemoteEndpointMessageProperty)this.Request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }
            else
            {
                return null;
            }
        }
        // GET /api/saveformsdata
        /*public bool Get(int FormId, string projCode, string position, string gps)
        {
            string query = HttpUtility.UrlDecode(this.Request.RequestUri.Query);
            string[] felter = query.Split(new char[] { '&' });
            Dictionary<string, string> data = new Dictionary<string, string>();
            char[] trim = new char[]{'='};
            for (int i = 0; i < felter.Length; i++)
            {
                if (felter[i].StartsWith("Felter"))
                {
                    string name = felter[i].Substring(felter[i].IndexOf('=')).TrimStart(trim);
                    i++;
                    string value = felter[i].Substring(felter[i].IndexOf('=')).TrimStart(trim);
                    data.Add(name, value);
                    int n = 0;
                    if (int.TryParse(name, out n))
                    {
                        data.Add(n, value);
                    }
                }
            }
            Models.RepositoryData r = new Models.RepositoryData();
            return r.Insert(FormId, data, projCode,  position);
        }*/
    }
}