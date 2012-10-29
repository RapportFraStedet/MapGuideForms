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
using System.Data.SqlClient;
using System.Web.Configuration;
namespace RapportFraStedet.Models
{
    public class RepositoryAddress
    {
        private AddressService.AddressService client = new AddressService.AddressService();
        public SelectList GetStreets()
        {
            return new SelectList(client.GetStreets(), "StreetId", "Name");
        }
        public StreetModel[] GetStreetList()
        {
            List<StreetModel> streets = new List<StreetModel>();

            string constr = WebConfigurationManager.ConnectionStrings["Address"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand selectCommand = new SqlCommand(
                "SELECT " +
                Properties.Settings.Default.ColumnStreetId + ", " +
                Properties.Settings.Default.ColumnStreetName +
                " FROM " + Properties.Settings.Default.TableStreet+
                " ORDER BY " + Properties.Settings.Default.ColumnStreetOrder,
            con);
            SqlDataReader reader = selectCommand.ExecuteReader();

            while (reader.Read())
            {
                StreetModel street = new StreetModel
                {
                    StreetId = Convert.ToInt32(reader.GetValue(0)),
                    Name = reader.GetString(1)
                };
                streets.Add(street);
            }
            reader.Close();
            con.Close();
            return streets.ToArray();
        }
        public SelectList GetAddressByStreetId(int streetId)
        {
            return new SelectList(client.GetAddresses(streetId, true), "Name", "Name");
        }
        public AddressModel[] GetAddressesByStreetId(int streetId)
        {

            List<AddressModel> addresses = new List<AddressModel>();

            string constr = WebConfigurationManager.ConnectionStrings["Address"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand selectCommand = new SqlCommand(
                "SELECT " +
                //Properties.Settings.Default.ColumnAddressId + ", " +
                Properties.Settings.Default.ColumnAddressName +
                " FROM " + Properties.Settings.Default.TableAddress +
                " WHERE " + 
                Properties.Settings.Default.ColumnAddressStreetId + "=" + streetId.ToString() +
                " ORDER BY " + Properties.Settings.Default.ColumnAddressOrder,
            con);
            SqlDataReader reader = selectCommand.ExecuteReader();

            while (reader.Read())
            {
                AddressModel address = new AddressModel
                {
                    //AddressId = reader.GetInt32(0),
                    Name = reader.GetString(0)
                };
                addresses.Add(address);
            }
            reader.Close();
            con.Close();
            return addresses.ToArray();
        }
        public AddressService.AddressModel GetAddressByAddressId(int streetId, string name)
        {
            return client.GetAddress(streetId, true, name);
        }
        public AddressModel GetAddress(int streetId, string name)
        {
            List<AddressModel> addresses = new List<AddressModel>();
            string constr = WebConfigurationManager.ConnectionStrings["Address"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand selectCommand = new SqlCommand(
                "SELECT " +
                Properties.Settings.Default.ColumnAddressX + ", " +
                Properties.Settings.Default.ColumnAddressY +
                " FROM " + Properties.Settings.Default.TableAddress +
                " WHERE " + 
                Properties.Settings.Default.ColumnAddressName + "='" + name + "' AND " +
                Properties.Settings.Default.ColumnAddressStreetId + "=" + streetId,
            con);
            SqlDataReader reader = selectCommand.ExecuteReader();

            while (reader.Read())
            {
                AddressModel address = new AddressModel
                {
                    X = Convert.ToDouble(reader.GetValue(0)),
                    Y = Convert.ToDouble(reader.GetValue(1)),
                    SRS = Properties.Settings.Default.AddressSRS
                };
                addresses.Add(address);
            }
            reader.Close();
            con.Close();
            return addresses[0];
        }
    }
}