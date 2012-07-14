using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Nottit.Models;

namespace Nottit.Controllers {

    public abstract class BaseController : ApiController {
        protected NottitDb Db { get; private set; }
        protected LoginManager LoginManager { get; private set; }

        protected override void Dispose(bool disposing) {
            if (Db != null && disposing) {
                Db.Dispose();
            }
            base.Dispose(disposing);
        }

        protected BaseController() {
            Db = new NottitDb();
            LoginManager = new LoginManager(Db);
        }
    }
}
