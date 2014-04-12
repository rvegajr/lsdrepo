using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Simple.Data;
using LaserSportDataObjects;
namespace LaserSportDataAPI.Controllers
{        
    //http://www.toptensoftware.com/petapoco/
    public class ApiControllerDB : ApiController
    {
        PetaPoco.Database _db = new PetaPoco.Database("LSREPConn");
        public PetaPoco.Database db {
            get { return _db; }
            set { _db = value; }
        } 
        public ApiControllerDB() : base()
        {
            
        }
    }
}