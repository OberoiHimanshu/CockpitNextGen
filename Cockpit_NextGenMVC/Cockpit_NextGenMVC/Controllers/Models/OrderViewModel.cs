using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cockpit_NextGenMVC.Models
{
    public class OrderViewModel
    {
        public double? Sales_Ord
        {
            get;
            set;
        }

        public string Backlog_Status { get; set; }

        public string Order_Date
        {
            get;
            set;
        }

        public double? Backlog
        {
            get;
            set;
        }

        public string Created_By
        {
            get;
            set;
        }

        public string SOrg
        {
            get;
            set;
        }

        public string Sold_To_Customer_Name
        {
            get;
            set;
        }

        public string PT
        {
            get;
            set;
        }

        public string Ship_Pt
        {
            get;
            set;
        }
    }
}