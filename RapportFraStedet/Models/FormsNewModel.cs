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
    public class FormsResponse:AccountNewModel
    {
        public List<FormsNewModel> Forms { get; set; }
    }
    public class FormsNewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class FormNewModel:AccountNewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MapAgent { get; set; }
        public string ApplicationDefinition { get; set; }
        public string SRS { get; set; }
        public string ResourceName { get; set; }
        public string ClassName { get; set; }
        public string UploadPhysicalPath { get; set; }
        public string UploadVirtualPath { get; set; }
        public string ReceiverOnCreate { get; set; }
        public string ReceiverOnEdit { get; set; }
        public string ReceiverOnDelete { get; set; }
        public string SenderOnCreate { get; set; }
        public string SenderOnEdit { get; set; }
        public string SenderOnDelete { get; set; }
        public List<string> Selections { get; set; }
    }
}