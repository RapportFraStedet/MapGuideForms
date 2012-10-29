﻿// Copyright 2012, MapGuideForm user group, Frederikssund Kommune and Helsingør Kommune - att. Anette Poulsen and Erling Kristensen
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
using RapportFraStedet.Models;

namespace RapportFraStedet.Controllers
{
    public class TemaController : ApiController
    {
        // GET /api/kommune
        RepositoryTema repository = new RepositoryTema();
        public IEnumerable<Tema> Get(int nr, string x, string y)
        {
            return repository.Get(nr,x,y);
        }
        public Tema Get(int id, int nr)
        {
            return repository.Get(id, nr);
        }
        public bool Get(int id, string geometri)
        {
            return repository.Get(id, geometri);
        }
    }
}
