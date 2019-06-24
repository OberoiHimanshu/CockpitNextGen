using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cockpit_NextGenMVC.BAL;
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
        public string ReportName  { get; set; }
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
        public string SalesForce { get; set; }
        public string InstallationStatus { get; set; }
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

        public string FiltersApplied;

        public string Get_AllFiltersInfo()
        {
            FiltersApplied = string.Empty;
            bool FiltersUSed = false;

            if (Report != null && Report != "") { FiltersApplied += "Report = " + Report + ","; FiltersUSed = true; }
            if (Region != null && Region != "") { FiltersApplied += "Region = " + Region + ","; FiltersUSed = true; }
            if (Business != null && Business != "") { FiltersApplied += "Business = " + Business + ","; FiltersUSed = true; }
            if (Division != null && Division != "") { FiltersApplied += "Division = " + Division + ","; FiltersUSed = true; }
            if (Sorg != null && Sorg != "") { FiltersApplied += "Sorg = " + Sorg + ","; FiltersUSed = true; }
            if (PrimaryProduct != null && PrimaryProduct != "") { FiltersApplied += "PrimaryProduct = " + PrimaryProduct + ","; FiltersUSed = true; }
            if (PL != null && PL != "") { FiltersApplied += "PL = " + PL + ","; FiltersUSed = true; }
            if (BacklogStatus != null && BacklogStatus != "") { FiltersApplied += "BacklogStatus = " + BacklogStatus + ","; FiltersUSed = true; }
            if (CustomerPONumber != null && CustomerPONumber != "") { FiltersApplied += "CustomerPONumber = " + CustomerPONumber + ","; FiltersUSed = true; }
            if (CreatedBy != null && CreatedBy != "") { FiltersApplied += "CreatedBy = " + CreatedBy + ","; FiltersUSed = true; }
            //if (OrderOwner != null) { FiltersApplied += "OrderOwner = " + OrderOwner + ","; FiltersUSed = true;}
            if (SNIAging != null && SNIAging != "") { FiltersApplied += "SNIAging = " + SNIAging + ","; FiltersUSed = true; }
            if (SNIAgingBucket != null && SNIAgingBucket != "") { FiltersApplied += "SNIAgingBucket = " + SNIAgingBucket + ","; FiltersUSed = true; }
            if (InstallationStatus != null && InstallationStatus != "") { FiltersApplied += "Overall Installation Status = " + InstallationStatus + ","; FiltersUSed = true; }
            if (Aging != null && Aging != "") { FiltersApplied += "Aging = " + Aging + ","; FiltersUSed = true; }
            if (AgingBucket != null && AgingBucket != "") { FiltersApplied += "AgingBucket = " + AgingBucket + ","; FiltersUSed = true; }
            if (DollarBucket != null && DollarBucket != "") { FiltersApplied += "DollarBucket = " + DollarBucket + ","; FiltersUSed = true; }
            if (DBClosureStatus != null && DBClosureStatus != "") { FiltersApplied += "DBClosureStatus = " + DBClosureStatus + ","; FiltersUSed = true; }
            if (SNIClosureStatus != null && SNIClosureStatus != "") { FiltersApplied += "SNIClosureStatus = " + SNIClosureStatus + ","; FiltersUSed = true; }
            if (BacklogAmt != 0) { FiltersApplied += "BacklogAmt = " + BacklogAmt.ToString() + ","; FiltersUSed = true; }
            if (SoldToAccountID != null && SoldToAccountID != "") { FiltersApplied += "SoldToAccountID = " + SoldToAccountID + ","; FiltersUSed = true; }
            if (SoldToAccount != null && SoldToAccount != "") { FiltersApplied += "SoldToAccount = " + SoldToAccount + ","; FiltersUSed = true; }
            if (SoldToCountry != null && SoldToCountry != "") { FiltersApplied += "SoldToCountry = " + SoldToCountry + ","; FiltersUSed = true; }
            if (ShipToAccountID != null && ShipToAccountID != "") { FiltersApplied += "ShipToAccountID = " + ShipToAccountID + ","; FiltersUSed = true; }
            if (ShipToAccount != null && ShipToAccount != "") { FiltersApplied += "ShipToAccount = " + ShipToAccount + ","; FiltersUSed = true; }
            if (ShipToCountry != null && ShipToCountry != "") { FiltersApplied += "ShipToCountry = " + ShipToCountry + ","; FiltersUSed = true; }
            if (ZUAccountID != null && ZUAccountID != "") { FiltersApplied += "ZUAccountID = " + ZUAccountID + ","; FiltersUSed = true; }
            if (ZUAccount != null && ZUAccount != "") { FiltersApplied += "ZUAccount = " + ZUAccount + ","; FiltersUSed = true; }
            if (ZUCountry != null && ZUCountry != "") { FiltersApplied += "ZUCountry = " + ZUCountry + ","; FiltersUSed = true; }
            if (SalesRep != null && SalesRep != "") { FiltersApplied += "SalesRep = " + SalesRep + ","; FiltersUSed = true; }
            if (SalesForce != null && SalesForce != "") { FiltersApplied += "SalesForce = " + SalesForce + ","; FiltersUSed = true; }
            if (BTM != null && BTM != "") { FiltersApplied += "BTM = " + BTM + ","; FiltersUSed = true; }
            if (BTM_Manager != null && BTM_Manager != "") { FiltersApplied += "BTM_Manager = " + BTM_Manager + ","; FiltersUSed = true; }
            if (PaymentTerm != null && PaymentTerm != "") { FiltersApplied += "PaymentTerm = " + PaymentTerm + ","; FiltersUSed = true; }
            if (BillingBlock_Code != null && BillingBlock_Code != "") { FiltersApplied += "BillingBlock_Code = " + BillingBlock_Code + ","; FiltersUSed = true; }
            if (DeliveryBlock_Code != null && DeliveryBlock_Code != "") { FiltersApplied += "DeliveryBlock_Code = " + DeliveryBlock_Code + ","; FiltersUSed = true; }
            if (BillingBlock_HeaderText != null && BillingBlock_HeaderText != "") { FiltersApplied += "BillingBlock_HeaderText = " + BillingBlock_HeaderText + ","; FiltersUSed = true; }
            if (DeliveryBlock_HeaderText != null && DeliveryBlock_HeaderText != "") { FiltersApplied += "DeliveryBlock_HeaderText = " + DeliveryBlock_HeaderText + ","; FiltersUSed = true; }
            if (BillingBlock_ItemText != null && BillingBlock_ItemText != "") { FiltersApplied += "BillingBlock_ItemText = " + BillingBlock_ItemText + ","; FiltersUSed = true; }
            if (DeliveryBlock_ItemText != null && DeliveryBlock_ItemText != "") { FiltersApplied += "DeliveryBlock_ItemText = " + DeliveryBlock_ItemText + ","; FiltersUSed = true; }
            if (DeltaLoaddate != null && DeltaLoaddate != "") { FiltersApplied += "DeltaLoaddate = " + DeltaLoaddate + ","; FiltersUSed = true; }
            if (DeltaLoaddateBucket != null && DeltaLoaddateBucket != "") { FiltersApplied += "DeltaLoaddateBucket = " + DeltaLoaddateBucket + ","; FiltersUSed = true; }
            if (ClosuredaysDeltaFrom != 0) { FiltersApplied += "ClosuredaysDeltaFrom = " + ClosuredaysDeltaFrom.ToString() + ","; FiltersUSed = true; }
            if (ClosuredaysDeltaTo != 0) { FiltersApplied += "ClosuredaysDeltaTo = " + ClosuredaysDeltaTo.ToString() + ","; FiltersUSed = true; }
            //if (CockpitUI != null) { FiltersApplied += "CockpitUI = " + CockpitUI + ","; FiltersUSed = true;}
            //if (OwnerName != null && OwnerName != "") { FiltersApplied += "OwnerName = " + OwnerName + ","; FiltersUSed = true; }
            if (SalesOrder != null && SalesOrder != "") { FiltersApplied += "SalesOrder = " + SalesOrder + ","; FiltersUSed = true; }
            if (NLHD != null && NLHD != "") { FiltersApplied += "NLHD = " + NLHD + ","; FiltersUSed = true; }
            if (LoaDDate != null && LoaDDate != "") { FiltersApplied += "LoaDDate = " + LoaDDate + ","; FiltersUSed = true; }
            if (TrioLoaDDate != null && TrioLoaDDate != "") { FiltersApplied += "TrioLoaDDate = " + TrioLoaDDate + ","; FiltersUSed = true; }
            if (CRDD != null && CRDD != "") { FiltersApplied += "CRDD = " + CRDD + ","; FiltersUSed = true; }
            if (ExpReleaseDate != null && ExpReleaseDate != "") { FiltersApplied += "ExpReleaseDate = " + ExpReleaseDate + ","; FiltersUSed = true; }
            if (ReasonCode != null && ReasonCode != "") { FiltersApplied += "ReasonCode = " + ReasonCode + ","; FiltersUSed = true; }

            if (FiltersUSed != true) { FiltersApplied = ""; } 

            return FiltersApplied;
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
        public string SalesForce { get; set; }
        public string InstallationStatus { get; set; }
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