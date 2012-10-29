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
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
namespace RapportFraStedet.Models
{
    public class DataCreateModel
    {
        public DataViewModel Model { get; set; }
        public List<DataEmailModel> Emails { get; set; }
    }
    public class DataEmailModel
    {
        public string Email { get; set; }
        public string View { get; set; }
    }
    public class DataViewModel
    {
        public Models.View View { get; set; }
        public Form Form { get; set; }
        
        #region Required database fields
        public int ItemId { get; set; }
        public string UniqueId { get; set; }
        
        [Display(Name = "Dato")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Display(Name = "Bruger ID")]
        [DataType(DataType.Text)]
        public string UserId { get; set; }
        #endregion

        #region LookUp
        [Display(Name = "Brugernavn")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Display(Name = "Firma")]
        [DataType(DataType.Text)]
        public string Company { get; set; }
        #endregion
    }
    public class DataListModel
    {
        public Models.View View { get; set; }
        public Form Form { get; set; }
        public List<Field> Columns { get; set; }
        public List<DataListItemModel> ListItemModels { get; set; }
        public DataListModel()
        {
            ListItemModels = new List<DataListItemModel>();
        }
    }
    public class DataListItemModel
    {
        [Display(Name = "Id")]
        [DataType(DataType.Text)]
        public int ItemId { get; set; }

        [Display(Name="UniqueId")]
        [DataType(DataType.Text)]
        public string UniqueId { get; set; }

        [Display(Name="Dato")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Display(Name="Firma")]
        [DataType(DataType.Text)]
        public string Company { get; set; }

        [Display(Name="Brugernavn")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Display(Name="Bruger ID")]
        [DataType(DataType.Text)]
        public string UserId { get; set; }

        public Dictionary<string,string> Data { get; set; }

        public DataListItemModel()
        {
            Data = new Dictionary<string, string>();
        }
    }
    public class DataSelectionModel
    {
        public string name { get; set; }
        public int nElements { get; set; }
        public List<List<string>> aElements { get; set; }
        public int nProperties { get; set; }
        public List<string> aPropertiesName { get; set; }
        public List<int> aPropertiesTypes { get; set; }
        public string area { get; set; }
        public string distance { get; set; }
    }
}