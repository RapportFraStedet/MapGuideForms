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
    public class RepositoryKommune
    {
        public Kommune Get(string x, string y)
        {
            RapportFraStedetGlobalDataSetTableAdapters.HentKommuneTableAdapter da = new RapportFraStedetGlobalDataSetTableAdapters.HentKommuneTableAdapter();
            RapportFraStedetGlobalDataSet.HentKommuneDataTable dt = da.GetData(x, y);
            if (dt.Rows.Count == 0)
            {
                RapportFraStedetGlobalDataSetTableAdapters.HentDefaultKommuneTableAdapter daDefault = new RapportFraStedetGlobalDataSetTableAdapters.HentDefaultKommuneTableAdapter();
                RapportFraStedetGlobalDataSet.HentDefaultKommuneDataTable dtDefault = daDefault.GetData();
                RapportFraStedetGlobalDataSet.HentDefaultKommuneRow rowDefault = (RapportFraStedetGlobalDataSet.HentDefaultKommuneRow)dtDefault.Rows[0];
                return new Kommune()
                {
                    Besked = rowDefault.Besked,
                    Logo = rowDefault.Logo,
                    ModtagerIndberetning = rowDefault.ModtagerIndberetning,
                    Navn = rowDefault.Navn,
                    URL = rowDefault.URL,
                    Nr = rowDefault.Nr
                };
            }
            else
            {
                RapportFraStedetGlobalDataSet.HentKommuneRow row = (RapportFraStedetGlobalDataSet.HentKommuneRow)dt.Rows[0];
                return new Kommune()
                {
                    Besked = row.Besked,
                    Logo = row.Logo,
                    ModtagerIndberetning = row.ModtagerIndberetning,
                    Navn = row.Navn,
                    URL = row.URL,
                    Nr = row.Nr
                };
            }
        }
        public Kommune Get(int nr)
        {
            RapportFraStedetGlobalDataSetTableAdapters.KommuneTableAdapter da = new RapportFraStedetGlobalDataSetTableAdapters.KommuneTableAdapter();
            RapportFraStedetGlobalDataSet.KommuneDataTable dt = da.GetData(nr);
            if (dt.Rows.Count == 0)
            {
                RapportFraStedetGlobalDataSetTableAdapters.HentDefaultKommuneTableAdapter daDefault = new RapportFraStedetGlobalDataSetTableAdapters.HentDefaultKommuneTableAdapter();
                RapportFraStedetGlobalDataSet.HentDefaultKommuneDataTable dtDefault = daDefault.GetData();
                RapportFraStedetGlobalDataSet.HentDefaultKommuneRow rowDefault = (RapportFraStedetGlobalDataSet.HentDefaultKommuneRow)dtDefault.Rows[0];
                return new Kommune()
                {
                    Besked = rowDefault.Besked,
                    Logo = rowDefault.Logo,
                    ModtagerIndberetning = rowDefault.ModtagerIndberetning,
                    Navn = rowDefault.Navn,
                    URL = rowDefault.URL,
                    Nr = rowDefault.Nr
                };
            }
            else
            {
                RapportFraStedetGlobalDataSet.KommuneRow row = (RapportFraStedetGlobalDataSet.KommuneRow)dt.Rows[0];
                return new Kommune()
                {
                    Besked = row.Besked,
                    Logo = row.Logo,
                    ModtagerIndberetning = row.ModtagerIndberetning,
                    Navn = row.Navn,
                    URL = row.URL,
                    Nr = row.Nr
                };
            }
        }
        public IEnumerable<Kommune> Get()
        {
            RapportFraStedetGlobalDataSetTableAdapters.HentAlleKommunerTableAdapter da = new RapportFraStedetGlobalDataSetTableAdapters.HentAlleKommunerTableAdapter();
            RapportFraStedetGlobalDataSet.HentAlleKommunerDataTable dt = da.GetData();
            return dt.Select(m=>new Kommune
            {
                Besked = m.Besked,
                Logo = m.Logo,
                ModtagerIndberetning = m.ModtagerIndberetning,
                Navn = m.Navn,
                URL = m.URL,
                Nr = m.Nr
            });
        }
    }
}