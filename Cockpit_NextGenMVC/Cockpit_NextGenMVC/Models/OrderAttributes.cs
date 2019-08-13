using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cockpit_NextGenMVC.BAL;

namespace Cockpit_NextGenMVC.Models
{
    public class OrderAttributes
    {
        public List<Tbl_Order_comments> Comments {get;set;}
        public List<Tbl_Order_Header_Details> Header { get; set; }
        public List<Tbl_Order_Items> Items { get; set; }
        public List<Tbl_Order_Partner_Details> Partner { get; set; }
        public List<Tbl_Order_Delivery_Info> Delivery { get; set; }
        public List<Tbl_Order_Block_Details> Block { get; set; }

        public List<Tbl_Followups> OpenFollowup { get; set; }
        public List<Tbl_Followups> ClosedFollowup { get; set; }
    }
}