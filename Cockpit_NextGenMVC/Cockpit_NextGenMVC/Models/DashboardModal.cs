using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cockpit_NextGenMVC.BAL;
using Cockpit_NextGenMVC.BAL_User_Mgmt;
using Cockpit_NextGenMVC.Controllers;

namespace Cockpit_NextGenMVC.Models
{
    public class DashboardModal
    {
        public string LastRefreshDate { get; set; }

        public string UI_Type { get; set; }

        public Tbl_Backlog_Summary BacklogSummary { get; set; }
        public IEnumerable<Backlogstats> BacklogStats { get; set; }

        public BCRSummary BCRSummary { get; set; }
        public IEnumerable<BCRstats> BCRStats { get; set; }

        public int TotalFollowups { get; set; }
        public Tbl_Followup_Summary[] FollowupSummary { get; set; }
        public Tbl_Followups[] SystemGeneratedFollowups { get; set; }

        public List<Model_Pie> Pie_Model;

        public Session_Filters oSessionFilters = new Session_Filters();

        public Level2_Summary Level2_Summary;
        public string Level2_Default_Report;

        public IEnumerable<Backlogstats> BCR_Level1_RawData { get; set; }
        public IEnumerable<Backlogstats> BCR_Level2_RawData { get; set; }

        public SelectList lstReportsMaster;

        public Tbl_Daily_Control_Reports_Summary[] My_BCRSummary;

        public DashboardModal BuildControlReportsSummaryforUI(DashboardModal oDashboardModal, Tbl_Daily_Control_Reports_Summary[] BCRSummary, VW_USERS oSessionUser)
        {
            oDashboardModal.BCRSummary.TotalPendingOrders = Convert.ToInt32(BCRSummary.Select(p => p.Pending_Comments).Sum());
            oDashboardModal.BCRSummary.TotalPendingSignOffs = Convert.ToInt32(BCRSummary.Select(p => p.Pending_Sign_Off).Sum());
            oDashboardModal.BCRSummary.TotalPendingReviews = Convert.ToInt32(BCRSummary.Select(p => p.Pending_Review).Sum());

            #region Bind Summary chart Data
            List<Model_Pie> oPieData = new List<Model_Pie>();

            Model_Pie oPie = new Model_Pie();
            oPie.category = "Pending Comments";
            oPie.value = oDashboardModal.BCRSummary.TotalPendingOrders;
            oPie.Color = "#9de219";
            oPieData.Add(oPie);

            oPie = new Model_Pie();
            oPie.category = "Pending Comments Review";
            oPie.value = oDashboardModal.BCRSummary.TotalPendingReviews;
            oPie.Color = "#90cc38";
            oPieData.Add(oPie);

            oPie = new Model_Pie();
            oPie.category = "Pending Sign-off";
            oPie.value = oDashboardModal.BCRSummary.TotalPendingSignOffs;
            oPie.Color = "#068c35";
            oPieData.Add(oPie);

            oDashboardModal.Pie_Model = oPieData;

            #endregion

            #region Binf Details chart Data
            string y_Axis = string.Empty;
            Level2_Summary oLevel2_Summary = new Level2_Summary();

            if (oSessionUser.ROLE_DESC == "CSR")
            {
                #region CSR Chart

                List<Backlogstats> listStats = new List<Backlogstats>();

                listStats = (from tbl in BCRSummary
                             group tbl by new { tbl.ReportName } into g
                             select new Backlogstats
                             {
                                 ReportName = g.Key.ReportName,
                                 PendingComments = (int)g.Sum(s => s.Pending_Comments),
                                 PendingReview = (int)g.Sum(s => s.Pending_Review),
                                 PendingSignOff = (int)g.Sum(s => s.Pending_Sign_Off)
                             }).ToList<Backlogstats>();

                //Build Details chart by Report Name
                List<Model_Bars> oDetailsResult = new List<Model_Bars>();
                string[] UniqueReports = listStats.Select(p => p.ReportName).Distinct().ToArray();
                double[] PendingComments = listStats.Select(p => p.PendingComments).ToArray();
                double[] PendingReview = listStats.Select(p => p.PendingReview).ToArray();
                double[] PendingSignOff = listStats.Select(p => p.PendingSignOff).ToArray();

                oLevel2_Summary.PendingComments = PendingComments;
                oLevel2_Summary.PendingReview = PendingReview;
                oLevel2_Summary.PendingSignOff = PendingSignOff;
                oLevel2_Summary.UniqueReports = UniqueReports;
                oDashboardModal.BCR_Level1_RawData = listStats;

                List<Backlogstats> UniqueReportsList = new List<Backlogstats>();

                UniqueReportsList = (from tbl in BCRSummary
                                     group tbl by tbl.ReportName into g
                                     select new Backlogstats { ReportName = g.Key }).ToList<Backlogstats>();
                try
                {
                    var DefaultReport = listStats[0].ReportName;
                    oDashboardModal.lstReportsMaster = new SelectList(UniqueReportsList, "ReportName", "ReportName", DefaultReport);
                }
                catch (Exception ex)
                {
                    UniqueReportsList.Add(new Backlogstats { ReportName = "Blank" });
                    oDashboardModal.lstReportsMaster = new SelectList(UniqueReportsList, "ReportName", "ReportName");
                }



                #endregion
            }
            else if (oSessionUser.ROLE_DESC == "Supervisor")
            {
                #region Supervisor Chart

                var DefaultReport = BCRSummary[0].ReportName;

                List<Backlogstats> listStats = new List<Backlogstats>();

                listStats = (from tbl in BCRSummary
                             group tbl by new { tbl.ReportName } into g
                             select new Backlogstats
                             {
                                 ReportName = g.Key.ReportName,
                                 PendingComments = (int)g.Sum(s => s.Pending_Comments),
                                 PendingReview = (int)g.Sum(s => s.Pending_Review),
                                 PendingSignOff = (int)g.Sum(s => s.Pending_Sign_Off)
                             }).ToList<Backlogstats>();


                //Build Details chart by Report Name
                List<Model_Bars> oDetailsResult = new List<Model_Bars>();
                string[] UniqueReports = listStats.Select(p => p.ReportName).Distinct().ToArray();
                double[] PendingComments = listStats.Select(p => p.PendingComments).ToArray();
                double[] PendingReview = listStats.Select(p => p.PendingReview).ToArray();
                double[] PendingSignOff = listStats.Select(p => p.PendingSignOff).ToArray();

                oLevel2_Summary.PendingComments = PendingComments;
                oLevel2_Summary.PendingReview = PendingReview;
                oLevel2_Summary.PendingSignOff = PendingSignOff;
                oLevel2_Summary.UniqueReports = UniqueReports;
                oDashboardModal.BCR_Level1_RawData = listStats;

                List<Backlogstats> UniqueReportsList = new List<Backlogstats>();

                UniqueReportsList = (from tbl in BCRSummary
                                     group tbl by tbl.ReportName into g
                                     select new Backlogstats { ReportName = g.Key }).ToList<Backlogstats>();

                oDashboardModal.lstReportsMaster = new SelectList(UniqueReportsList, "ReportName", "ReportName", DefaultReport);
                #endregion
            }
            else if (oSessionUser.ROLE_DESC == "BPA")
            {
                #region BPA/Regional Lead Chart

                List<Backlogstats> listStats = new List<Backlogstats>();

                listStats = (from tbl in BCRSummary
                             group tbl by new { tbl.ReportName } into g
                             select new Backlogstats
                             {
                                 ReportName = g.Key.ReportName,
                                 PendingComments = (int)g.Sum(s => s.Pending_Comments),
                                 PendingReview = (int)g.Sum(s => s.Pending_Review),
                                 PendingSignOff = (int)g.Sum(s => s.Pending_Sign_Off)
                             }).ToList<Backlogstats>();


                //Build Details chart by Report Name
                List<Model_Bars> oDetailsResult = new List<Model_Bars>();
                string[] UniqueReports = listStats.Select(p => p.ReportName).Distinct().ToArray();
                double[] PendingComments = listStats.Select(p => p.PendingComments).ToArray();
                double[] PendingReview = listStats.Select(p => p.PendingReview).ToArray();
                double[] PendingSignOff = listStats.Select(p => p.PendingSignOff).ToArray();

                oLevel2_Summary.PendingComments = PendingComments;
                oLevel2_Summary.PendingReview = PendingReview;
                oLevel2_Summary.PendingSignOff = PendingSignOff;
                oLevel2_Summary.UniqueReports = UniqueReports;

                List<Backlogstats> listStatsDetails = new List<Backlogstats>();

                listStatsDetails = (from tbl in BCRSummary
                                    group tbl by new { tbl.ReportName } into g
                                    select new Backlogstats
                                    {
                                        ReportName = g.Key.ReportName,
                                        PendingComments = (int)g.Sum(s => s.Pending_Comments),
                                        PendingReview = (int)g.Sum(s => s.Pending_Review),
                                        PendingSignOff = (int)g.Sum(s => s.Pending_Sign_Off)
                                    }).ToList<Backlogstats>();
                oDashboardModal.BCR_Level1_RawData = listStatsDetails;

                List<Backlogstats> listStats2 = new List<Backlogstats>();
                var DefaultReport = listStats[0].ReportName;

                oDashboardModal.Level2_Default_Report = DefaultReport;

                oDashboardModal.BacklogStats = listStats;
                List<Backlogstats> UniqueReportsList = new List<Backlogstats>();

                UniqueReportsList = (from tbl in BCRSummary
                                     group tbl by tbl.ReportName into g
                                     select new Backlogstats { ReportName = g.Key }).ToList<Backlogstats>();

                oDashboardModal.lstReportsMaster = new SelectList(UniqueReportsList, "ReportName", "ReportName", DefaultReport);

                #endregion
            }
            else
            {
                #region WW Lead Chart

                List<Backlogstats> listStats = new List<Backlogstats>();

                listStats = (from tbl in BCRSummary
                             group tbl by new { tbl.ReportName } into g
                             select new Backlogstats
                             {
                                 ReportName = g.Key.ReportName,
                                 PendingComments = (int)g.Sum(s => s.Pending_Comments),
                                 PendingReview = (int)g.Sum(s => s.Pending_Review),
                                 PendingSignOff = (int)g.Sum(s => s.Pending_Sign_Off)
                             }).ToList<Backlogstats>();


                //Build Details chart by Region Name
                List<Model_Bars> oDetailsResult = new List<Model_Bars>();
                string[] UniqueReports = listStats.Select(p => p.Region).Distinct().ToArray();
                double[] PendingComments = listStats.Select(p => p.PendingComments).ToArray();
                double[] PendingReview = listStats.Select(p => p.PendingReview).ToArray();
                double[] PendingSignOff = listStats.Select(p => p.PendingSignOff).ToArray();

                oLevel2_Summary.PendingComments = PendingComments;
                oLevel2_Summary.PendingReview = PendingReview;
                oLevel2_Summary.PendingSignOff = PendingSignOff;
                oLevel2_Summary.UniqueReports = UniqueReports;

                oDashboardModal.BCR_Level1_RawData = listStats;

                List<Backlogstats> listStats2 = new List<Backlogstats>();
                var DefaultReport = BCRSummary[0].ReportName;

                oDashboardModal.Level2_Default_Report = DefaultReport;

                oDashboardModal.BacklogStats = listStats;

                List<Backlogstats> UniqueReportsList = new List<Backlogstats>();

                UniqueReportsList = (from tbl in BCRSummary
                                     group tbl by tbl.ReportName into g
                                     select new Backlogstats { ReportName = g.Key }).ToList<Backlogstats>();

                oDashboardModal.lstReportsMaster = new SelectList(UniqueReportsList, "ReportName", "ReportName", DefaultReport);
                #endregion
            }
            oDashboardModal.Level2_Summary = oLevel2_Summary;
            #endregion

            return oDashboardModal;
        }
    }

    public class BCRstats
    {
        public string ReportName { get; set; }
        public double Percentorders { get; set; }
        public double Totalorders { get; set; }
        public double OrdersWorth { get; set; }
        public string Backcolor { get; set; }
        public string Unit { get; set; }

        public string Area { get; set; }
        public string ReportPath { get; set; }
    }

    public class Backlogstats
    {

        public string ReportName { get; set; }
        public string Region { get; set; }
        public string OrderOwner { get; set; }

        public double CurrentTotalorders { get; set; }
        public double Percentorders { get; set; }
        public double Totalorders { get; set; }
        public double OrdersWorth { get; set; }

        public int TotalSNI_Cnt { get; set; }
        public int TotalDB_Cnt { get; set; }
        public int TotalUnblocked_Cnt { get; set; }
        public int TotalOSBR_Cnt { get; set; }

        public double PendingComments { get; set; }
        public double PendingReview { get; set; }
        public double PendingSignOff { get; set; }

        public string Backcolor { get; set; }
        public string Unit { get; set; }

        public string Area { get; set; }
        public string ReportPath { get; set; }
    }

    public class BacklogSummary
    {
        public double TotalBacklog { get; set; }
        public double TotalSNI { get; set; }
        public double TotalDB { get; set; }
        public double TotalUnblocked { get; set; }

        public int TotalSNI_Cnt { get; set; }
        public int TotalDB_Cnt { get; set; }
        public int TotalUnblocked_Cnt { get; set; }
    }

    public class BCRSummary
    {
        public double TotalPendingOrders { get; set; }
        public double TotalPendingSignOffs { get; set; }
        public double TotalPendingReviews { get; set; }
        public double TotalBCRBacklog { get; set; }
    }

    public class Level2_Summary
    {
        public double[] PendingComments { get; set; }
        public double[] PendingReview { get; set; }
        public double[] PendingSignOff { get; set; }
        public string[] UniqueReports { get; set; }

        public double[] PendingComments2 { get; set; }
        public double[] PendingReview2 { get; set; }
        public double[] PendingSignOff2 { get; set; }
        public string[] UniqueReports2 { get; set; }
    }

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

        public string Get_AllFiltersInfo()
        {
            string FiltersApplied = string.Empty;

            if (Report != null) { FiltersApplied += "Report = " + Report + ","; }
            if (Region != null) { FiltersApplied += "Region = " + Region + ","; }
            if (Business != null) { FiltersApplied += "Business = " + Business + ","; }
            if (Division != null) { FiltersApplied += "Division = " + Division + ","; }
            if (Sorg != null) { FiltersApplied += "Sorg = " + Sorg + ","; }
            if (PrimaryProduct != null) { FiltersApplied += "PrimaryProduct = " + PrimaryProduct + ","; }
            if (PL != null) { FiltersApplied += "PL = " + PL + ","; }
            if (BacklogStatus != null) { FiltersApplied += "BacklogStatus = " + BacklogStatus + ","; }
            if (CustomerPONumber != null) { FiltersApplied += "CustomerPONumber = " + CustomerPONumber + ","; }
            if (CreatedBy != null) { FiltersApplied += "CreatedBy = " + CreatedBy + ","; }
            if (OrderOwner != null) { FiltersApplied += "OrderOwner = " + OrderOwner + ","; }
            if (SNIAging != null) { FiltersApplied += "SNIAging = " + SNIAging + ","; }
            if (SNIAgingBucket != null) { FiltersApplied += "SNIAgingBucket = " + SNIAgingBucket + ","; }
            if (Aging != null) { FiltersApplied += "Aging = " + Aging + ","; }
            if (AgingBucket != null) { FiltersApplied += "AgingBucket = " + AgingBucket + ","; }
            if (DollarBucket != null) { FiltersApplied += "DollarBucket = " + DollarBucket + ","; }
            if (DBClosureStatus != null) { FiltersApplied += "DBClosureStatus = " + DBClosureStatus + ","; }
            if (SNIClosureStatus != null) { FiltersApplied += "SNIClosureStatus = " + SNIClosureStatus + ","; }
            if (BacklogAmt != 0) { FiltersApplied += "BacklogAmt = " + BacklogAmt.ToString() + ","; }
            if (SoldToAccountID != null) { FiltersApplied += "SoldToAccountID = " + SoldToAccountID + ","; }
            if (SoldToAccount != null) { FiltersApplied += "SoldToAccount = " + SoldToAccount + ","; }
            if (SoldToCountry != null) { FiltersApplied += "SoldToCountry = " + SoldToCountry + ","; }
            if (ShipToAccountID != null) { FiltersApplied += "ShipToAccountID = " + ShipToAccountID + ","; }
            if (ShipToAccount != null) { FiltersApplied += "ShipToAccount = " + ShipToAccount + ","; }
            if (ShipToCountry != null) { FiltersApplied += "ShipToCountry = " + ShipToCountry + ","; }
            if (ZUAccountID != null) { FiltersApplied += "ZUAccountID = " + ZUAccountID + ","; }
            if (ZUAccount != null) { FiltersApplied += "ZUAccount = " + ZUAccount + ","; }
            if (ZUCountry != null) { FiltersApplied += "ZUCountry = " + ZUCountry + ","; }
            if (SalesRep != null) { FiltersApplied += "SalesRep = " + SalesRep + ","; }
            if (BTM != null) { FiltersApplied += "BTM = " + BTM + ","; }
            if (BTM_Manager != null) { FiltersApplied += "BTM_Manager = " + BTM_Manager + ","; }
            if (PaymentTerm != null) { FiltersApplied += "PaymentTerm = " + PaymentTerm + ","; }
            if (BillingBlock_Code != null) { FiltersApplied += "BillingBlock_Code = " + BillingBlock_Code + ","; }
            if (DeliveryBlock_Code != null) { FiltersApplied += "DeliveryBlock_Code = " + DeliveryBlock_Code + ","; }
            if (BillingBlock_HeaderText != null) { FiltersApplied += "BillingBlock_HeaderText = " + BillingBlock_HeaderText + ","; }
            if (DeliveryBlock_HeaderText != null) { FiltersApplied += "DeliveryBlock_HeaderText = " + DeliveryBlock_HeaderText + ","; }
            if (BillingBlock_ItemText != null) { FiltersApplied += "BillingBlock_ItemText = " + BillingBlock_ItemText + ","; }
            if (DeliveryBlock_ItemText != null) { FiltersApplied += "DeliveryBlock_ItemText = " + DeliveryBlock_ItemText + ","; }
            if (DeltaLoaddate != null) { FiltersApplied += "DeltaLoaddate = " + DeltaLoaddate + ","; }
            if (DeltaLoaddateBucket != null) { FiltersApplied += "DeltaLoaddateBucket = " + DeltaLoaddateBucket + ","; }
            if (ClosuredaysDeltaFrom != 0) { FiltersApplied += "ClosuredaysDeltaFrom = " + ClosuredaysDeltaFrom.ToString() + ","; }
            if (ClosuredaysDeltaTo != 0) { FiltersApplied += "ClosuredaysDeltaTo = " + ClosuredaysDeltaTo.ToString() + ","; }
            if (CockpitUI != null) { FiltersApplied += "CockpitUI = " + CockpitUI + ","; }
            if (OwnerName != null) { FiltersApplied += "OwnerName = " + OwnerName + ","; }
            if (SalesOrder != null) { FiltersApplied += "SalesOrder = " + SalesOrder + ","; }
            if (NLHD != null) { FiltersApplied += "NLHD = " + NLHD + ","; }
            if (LoaDDate != null) { FiltersApplied += "LoaDDate = " + LoaDDate + ","; }
            if (TrioLoaDDate != null) { FiltersApplied += "TrioLoaDDate = " + TrioLoaDDate + ","; }
            if (CRDD != null) { FiltersApplied += "CRDD = " + CRDD + ","; }
            if (ExpReleaseDate != null) { FiltersApplied += "ExpReleaseDate = " + ExpReleaseDate + ","; }
            if (ReasonCode != null) { FiltersApplied += "ReasonCode = " + ReasonCode + ","; }


            return FiltersApplied;
        }

        public BAL.Session_Filters Set_ServiceFilters()
        {
            BAL.Session_Filters oResult = new BAL.Session_Filters();

            oResult.Region = this.Region;
            oResult.Business = this.Business;
            oResult.Division = this.Division;
            oResult.Sorg = this.Sorg;
            oResult.PrimaryProduct = this.PrimaryProduct;
            oResult.PL = this.PL;
            oResult.BacklogStatus = this.BacklogStatus;
            oResult.CustomerPONumber = this.CustomerPONumber;
            oResult.CreatedBy = this.CreatedBy;
            oResult.OrderOwner = this.OrderOwner;
            oResult.SNIAging = this.SNIAging;
            oResult.SNIAgingBucket = this.SNIAgingBucket;
            oResult.Aging = this.Aging;
            oResult.AgingBucket = this.AgingBucket;
            oResult.DollarBucket = this.DollarBucket;
            oResult.DBClosureStatus = this.DBClosureStatus;
            oResult.SNIClosureStatus = this.SNIClosureStatus;
            oResult.BacklogAmt = this.BacklogAmt;
            oResult.SoldToAccountID = this.SoldToAccountID;
            oResult.SoldToAccount = this.SoldToAccount;
            oResult.SoldToCountry = this.SoldToCountry;
            oResult.ShipToAccountID = this.ShipToAccountID;
            oResult.ShipToAccount = this.ShipToAccount;
            oResult.ShipToCountry = this.ShipToCountry;
            oResult.ZUAccountID = this.ZUAccountID;
            oResult.ZUAccount = this.ZUAccount;
            oResult.ZUCountry = this.ZUCountry;
            oResult.SalesRep = this.SalesRep;
            oResult.BTM = this.BTM;
            oResult.BTM_Manager = this.BTM_Manager;
            oResult.PaymentTerm = this.PaymentTerm;
            oResult.BillingBlock_Code = this.BillingBlock_Code;
            oResult.DeliveryBlock_Code = this.DeliveryBlock_Code;
            oResult.BillingBlock_HeaderText = this.BillingBlock_HeaderText;
            oResult.DeliveryBlock_HeaderText = this.DeliveryBlock_HeaderText;
            oResult.BillingBlock_ItemText = this.BillingBlock_ItemText;
            oResult.DeliveryBlock_ItemText = this.DeliveryBlock_ItemText;
            oResult.DeltaLoaddate = this.DeltaLoaddate;
            oResult.DeltaLoaddateBucket = this.DeltaLoaddateBucket;
            oResult.ClosuredaysDeltaFrom = this.ClosuredaysDeltaFrom;
            oResult.ClosuredaysDeltaTo = this.ClosuredaysDeltaTo;
            oResult.CockpitUI = this.CockpitUI;
            oResult.OwnerName = this.OwnerName;
            oResult.SalesOrder = this.SalesOrder;
            oResult.NLHD = this.NLHD;
            oResult.LoaDDate = this.LoaDDate;
            oResult.TrioLoaDDate = this.TrioLoaDDate;
            oResult.CRDD = this.CRDD;
            oResult.ExpReleaseDate = this.ExpReleaseDate;
            oResult.ReasonCode = this.ReasonCode;

            return oResult;
        } 
    }

    public class Local_Filters
    {
        public string SalesOrder { get; set; }
        public string CustomerPONumber { get; set; }
        public string Sorg { get; set; }
        public string OwnerName { get; set; }
        public string Aging { get; set; }
        public string AgingBucket { get; set; }
        public string SNIAging { get; set; }
        public string SNIAgingBucket { get; set; }
        public string DBClosureStatus { get; set; }
        public string SNIClosureStatus { get; set; }
        public string SoldToAccount { get; set; }
        public string ZUAccount { get; set; }
        public string SalesRep { get; set; }
        public string PaymentTerm { get; set; }
        public string BillingBlock { get; set; }
        public string DeliveryBlock { get; set; }
        public string NLHD { get; set; }
        public string LoaDDate { get; set; }
        public string TrioLoaDDate { get; set; }
        public string DeltaLoaddateBucket { get; set; }
        public string CRDD { get; set; }
        public string ExpReleaseDate { get; set; }
        public string ReasonCode { get; set; }
    }

}