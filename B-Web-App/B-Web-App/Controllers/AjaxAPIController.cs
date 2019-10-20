using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using B_Web_App.Models;

namespace B_Web_App.Controllers
{
    public class AjaxAPIController : ApiController
    {
        [Route("api/AjaxAPI/AjaxMethod")]
        [HttpPost]
        public User_Model AjaxMethod(User_Model User)
        {
            User.EMAIL = "himanshu.oberoi@stryker.com";
            return User;
        }
    }
}
