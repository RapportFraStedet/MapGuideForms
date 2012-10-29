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

namespace RapportFraStedet.Models
{
    public class FieldsResponse : AccountNewModel
    {
        public List<FieldsNewModel> Fields { get; set; }
    }
    public class FieldsNewModel
    {
        public int FieldId { get; set; }
        public string Name { get; set; }
    }
    public class FieldNewModel:AccountNewModel
    {
        public int FieldId { get; set; }
        public string Name { get; set; }
        public int FormId { get; set; }
        public int FieldTypeId { get; set; }
        public int FieldOrder { get; set; }
        public bool Required { get; set; }
        public string FieldColumn { get; set; }
        public string Data { get; set; }
        public List<FieldTypeModel> Selections { get; set; }
    }
    public class FieldTypesResponse : AccountNewModel
    {
        public List<FieldTypeModel> FieldTypes { get; set; }
    }
    public class FieldTypeModel
    {
        public string Name { get; set; }
        public int FieldTypeId { get; set; }
    }
}