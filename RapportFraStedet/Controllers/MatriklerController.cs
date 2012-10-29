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
using System.Net.Http;
using System.Web.Http;

namespace RapportFraStedet.Controllers
{
    public class MatriklerController : ApiController
    {
        private GeokeysV4.IGeoKeysFacadeService service = new GeokeysV4.IGeoKeysFacadeService();// GET /api/geokeysv4
        public List<GeokeysV4.MATRLandParcelIdentificationStructure> Get(int id, string name)
        {
            GeokeysV4.MATRLandParcelIdentificationStructure[] items = service.getLandParcelIdentificationInCadastralDistrict(id, Properties.Settings.Default.KMS_USERNAME, Properties.Settings.Default.KMS_PASSWORD);
            List<GeokeysV4.MATRLandParcelIdentificationStructure> items2 = new List<GeokeysV4.MATRLandParcelIdentificationStructure>();
            if (name == "")
            {
                foreach (GeokeysV4.MATRLandParcelIdentificationStructure item in items)
                {
                    items2.Add(item);
                }
            }
            else
            {
                foreach (GeokeysV4.MATRLandParcelIdentificationStructure item in items.Where(m => m.landParcelIdentifier.StartsWith(name)))
                {
                    items2.Add(item);
                }
            }
            //return items.Where(m => m.landParcelIdentifier.StartsWith(name));
            return items2;
        }

    }
}
