using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace Cockpit_NextGenMVC.Models
{
    public class SessionExpire : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["UserProfile"] == null)
            {
                filterContext.Result =
                new RedirectToRouteResult(new RouteValueDictionary    
                                                {   
                                                { "action", "UserSessionExpired" },   
                                                { "controller", "User" }
                                                });

                return;
            }
        }

    }

}