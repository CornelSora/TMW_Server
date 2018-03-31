using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TAW_Server.Models;

namespace TAW_Server.Controllers
{
    public class BaseController : ApiController
    {
        public TMW_DatabaseEntities DbContext { get; set; }

        public BaseController()
        {
            if (DbContext == null)
            {
                DbContext = new TMW_DatabaseEntities();
            }
        }
    }
}