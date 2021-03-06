﻿// Copyright 2012, MapGuideForm user group, Frederikssund Kommune and Helsingør Kommune - att. Anette Poulsen and Erling Kristensen
// 
// This file is part of "RapportFraStedet". 
// "RapportFraStedet" is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or any later version.
// "RapportFraStedet" is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License along with "RapportFraStedet". If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;

namespace RapportFraStedet.Controllers
{
    public class SqlController : ApiController
    {
        public DataTable get(string q, string c, string l)
        {
            DataTable dt = new DataTable();
            System.Configuration.ConnectionStringSettings setting = WebConfigurationManager.ConnectionStrings[c];
            if(setting!=null)
            {
                using(SqlConnection con = new SqlConnection(setting.ConnectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter(q,con);
                    
                    da.Fill(dt);
                    

                }
            }
            if (!string.IsNullOrEmpty(l))
            {
                StreamWriter log;
                string root = HttpContext.Current.Server.MapPath("~/App_Data");
                string file = Path.Combine(root, l);
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
                log.WriteLine(q);

                // Close the stream:
                log.Close();
            }
            return dt;
        }
        public DataTable get(string q, string c)
        {
            DataTable dt = new DataTable();
            System.Configuration.ConnectionStringSettings setting = WebConfigurationManager.ConnectionStrings[c];
            if (setting != null)
            {
                using (SqlConnection con = new SqlConnection(setting.ConnectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter(q, con);

                    da.Fill(dt);


                }
            }
            return dt;
        }
    }
}
