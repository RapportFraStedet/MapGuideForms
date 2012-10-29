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
using System.Data.SqlClient;

namespace RapportFraStedet.Models
{
    public class RepositoryTema
    {
        public IEnumerable<Tema> Get(int nr, string x, string y)
        {
            RapportFraStedetLokalDataSetTableAdapters.HentTemaerTableAdapter da = new RapportFraStedetLokalDataSetTableAdapters.HentTemaerTableAdapter();
            RapportFraStedetLokalDataSet.HentTemaerDataTable dt = da.GetData(nr,x,y);
            return dt.Select(m=>new Tema
            {
                Besked = m.Besked,
                Logo = m.Logo,
                ModtagerIndberetning = m.ModtagerIndberetning,
                Navn = m.Navn,
                MapAgent = m.MapAgent,
                Beskrivelse = m.Beskrivelse,
                Id = m.Id,
                ApplicationDefinition = m.ApplicationDefinition,
                Nr = nr
            });
        }
        public Tema Get(int id, int nr)
        {
            RapportFraStedetLokalDataSetTableAdapters.TemaTableAdapter da = new RapportFraStedetLokalDataSetTableAdapters.TemaTableAdapter();
            RapportFraStedetLokalDataSet.TemaDataTable dt = da.GetData(id,nr);
            if (dt.Rows.Count > 0)
            {
                RapportFraStedetLokalDataSet.TemaRow row = (RapportFraStedetLokalDataSet.TemaRow)dt.Rows[0];
                return new Tema
                {
                    Besked = row.Besked,
                    Logo = row.Logo,
                    ModtagerIndberetning = true,
                    Navn = row.Navn,
                    MapAgent = row.MapAgent,
                    Beskrivelse = row.Beskrivelse,
                    Id = row.Id,
                    ApplicationDefinition = row.ApplicationDefinition,
                    Nr = nr
                };
            }
            return null;
        }
        public bool Get(int id, string geometri)
        {
            if (string.IsNullOrEmpty(geometri))
                geometri = "POINT(0 0)";
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["RapportFraStedetLokalConnectionString"].ConnectionString);
            SqlCommand command = new SqlCommand("select dbo.CheckIndberetning("+id.ToString()+",'"+geometri+"')",con);
            con.Open();
            bool result = (bool)command.ExecuteScalar();
            con.Close();
            return result;
        }
    }
}