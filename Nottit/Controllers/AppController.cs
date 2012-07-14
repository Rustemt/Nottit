using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Soshel.Controllers {
    public partial class AppController : Controller {

        public virtual ActionResult Html(string slug) {
            return View(slug);
        }
    }
}
