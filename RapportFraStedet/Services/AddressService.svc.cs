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
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace RapportFraStedet.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AddressService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AddressService.svc or AddressService.svc.cs at the Solution Explorer and start debugging.
    public class AddressService : IAddressService
    {
        public Models.StreetModel[] GetStreets()
        {
            RepositoryAddress repository = new RepositoryAddress();
            return repository.GetStreetList();

        }

        public Models.AddressModel[] GetAddresses(int streetId)
        {
            RepositoryAddress repository = new RepositoryAddress();
            return repository.GetAddressesByStreetId(streetId);
        }
        public Models.AddressModel GetAddress(int streetId, string name)
        {
            RepositoryAddress repository = new RepositoryAddress();
            return repository.GetAddress(streetId, name);
        }
    }
}
