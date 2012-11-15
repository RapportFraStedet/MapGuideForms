using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RapportFraStedet.Controllers
{
    public class GuidApiController : ApiController
    {
        public string Get()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
