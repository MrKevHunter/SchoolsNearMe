﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolMap.Net.Models;

namespace SchoolMap.Net.Controllers
{
    public class SchoolDetailController : BaseController
    {
        //
        // GET: /SchoolDetail/

        public ActionResult Detail(int id)
        {
            var school = RavenSession.Load<School>(id.ToString());
            return PartialView(school);
        }

    }
}