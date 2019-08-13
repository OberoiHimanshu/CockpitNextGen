using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL_Service
{
    public class Session_Filters
    {
        public string Report { get; set; }
        public string Region { get; set; }
        public string Business { get; set; }
        public string Division { get; set; }
        public string Sorg { get; set; }
        public string PrimaryProduct { get; set; }
        public string PL { get; set; }
        public string BacklogStatus { get; set; }
        public string CustomerPONumber { get; set; }
        public string CreatedBy { get; set; }
        public string OrderOwner { get; set; }
        public string SNIAging { get; set; }
        public string SNIAgingBucket { get; set; }
        public string Aging { get; set; }
        public string AgingBucket { get; set; }
        public string DollarBucket { get; set; }
        public string DBClosureStatus { get; set; }
        public string SNIClosureStatus { get; set; }
        public double BacklogAmt { get; set; }
        public string SoldToAccountID { get; set; }
        public string SoldToAccount { get; set; }
        public string SoldToCountry { get; set; }
        public string ShipToAccountID { get; set; }
        public string ShipToAccount { get; set; }
        public string ShipToCountry { get; set; }
        public string ZUAccountID { get; set; }
        public string ZUAccount { get; set; }
        public string ZUCountry { get; set; }
        public string SalesRep { get; set; }
        public string BTM { get; set; }
        public string BTM_Manager { get; set; }
        public string PaymentTerm { get; set; }
        public string BillingBlock_Code { get; set; }
        public string DeliveryBlock_Code { get; set; }
        public string BillingBlock_HeaderText { get; set; }
        public string DeliveryBlock_HeaderText { get; set; }
        public string BillingBlock_ItemText { get; set; }
        public string DeliveryBlock_ItemText { get; set; }
        public string DeltaLoaddate { get; set; }
        public string DeltaLoaddateBucket { get; set; }
        public int ClosuredaysDeltaFrom { get; set; }
        public int ClosuredaysDeltaTo { get; set; }
        public string CockpitUI { get; set; }
        public string OwnerName { get; set; }
        public string SalesOrder { get; set; }
        public string NLHD { get; set; }
        public string LoaDDate { get; set; }
        public string TrioLoaDDate { get; set; }
        public string CRDD { get; set; }
        public string ExpReleaseDate { get; set; }
        public string ReasonCode { get; set; }
    }
}
