using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cockpit_NextGenMVC.Models;
using Cockpit_NextGenMVC.BAL;
using Cockpit_NextGenMVC.BAL_User_Mgmt;

namespace Cockpit_NextGenMVC.Models
{
    public class SessionGenerics
    {
        public DashboardModal oDashboardModel { get; set; }
        public VW_USERS oSessionUser { get; set; }
        public string  CockpitUI { get; set; }
        public string ReportGroup { get; set; }
        public string currentMainReport { get; set; }
    }
}