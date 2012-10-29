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
    public class Udvælgelse
    {
        public string Name { get; set; }
        public bool Selected { get; set; }
        public string Id { get; set; }
    }
    public class Felt
    {
        /*public Felt(Field field, bool readOnly)
        {
            Name = field.Name;
            ReadOnly = readOnly;
            TypeId = field.FieldTypeId;
            Data = "rune";
            Required = field.Required;
        }*/
        public IEnumerable<Udvælgelse> Selections { get; set; }
        public string Name { get; set; }
        public int Permission { get; set; }
        public int TypeId { get; set; }
        public string Data { get; set; }
        public bool Required { get; set; }
        public int Id { get; set; }
    }
    public class Data
    {
        public IEnumerable<Felt> Felter { get; set; }
        public int FormId { get; set; }
        public string SRS { get; set; }
        public string UserId { get; set; }
        public string UniqueId { get; set; }
        public int ViewId { get; set; }
        public int ItemId { get; set; }
        public string Date { get; set; }

        /*public string MapAgent { get; set; }
        public string ApplicationDefinition { get; set; }
        public MapSet MapSet { get; set; }
        public string Guid { get; set; }*/
    }
    public class MapSet
    {
        public List<MapGroup> MapGroups { get; set; }
    }
    public class MapGroup
    {
        public string Name { get; set; }
        public List<Map> Maps { get; set; }
    }
    public class Map
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public string MapDefinition { get; set; }
        //public string CoordinateSystem { get; set; }
        //public string MinX { get; set; }
        //public string MaxX { get; set; }
        //public string MinY { get; set; }
        //public string MaxY { get; set; }
        public bool SingleTile { get; set; }
        //public string BaseMapLayerGroupName { get; set; }
        public bool UseHttpTile { get; set; }
        public bool UseOverlay { get; set; }
        public bool IsBaseLayer { get; set; }
        public List<string> TileCacheUrl { get; set; }
    }

}