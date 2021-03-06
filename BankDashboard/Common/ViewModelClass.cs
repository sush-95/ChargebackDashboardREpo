﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankDashboard.Common
{
    public class ViewModelClass
    {
        
      
        public class selectPages
        {
            public string PageName { get; set; }
            public string groupname { get; set; }
            public string groupdesc { get; set; }
        }
        public class FilterClass
        {
            public string Fromdate { get; set; }
            public string Todate { get; set; }
            public string FeedbackID { get; set; }
            public string CIFNo { get; set; }
        }

        public class BOtStatModel
        {
            public List<string> castStatFigures { get; set; }
            public List<string> RoutingPortalFigures { get; set; }
            public List<string> CaseReadyForAction { get; set; }
            public string flag { get; set; }
        }
        public class WCStatModel
        {
            public List<string> WCCaseStatus{ get; set; }
            public IssueType Itypes { get; set; }
            public string flag { get; set; }
        }
        public class IssueType
        {
            public List<string> Issuetypes { get; set; }
            public List<string> Issuetypesfigures { get; set; }
            public List<string> IssuetypesBackColor { get; set; }
            public List<string> IssuetypesBordercolor { get; set; }
            public string datelbl { get; set; }
        }
        public class CaseFilter
        {
            public string Fromdate { get; set; }
            public string Todate { get; set; }
            public string Filter { get; set; }
            public string Flag { get; set; }
        }
        public class SLAFilter
        {
            public string SLADays { get; set; }
            public string CloseToSla { get; set; }
            public string SlACount { get; set; }
            public string Filter { get; set; }
        }
        public class ReconciliationFilter
        {
            public string CardNumber { get; set; }
            public string MemberCaseNumber { get; set; }
        }
        public class ClosureReportFilter
        {
            public DateTime FromDate { get; set; }
            public DateTime ToDate { get; set; }
            public string ROLCaseNumber { get; set; }
            public string Status { get; set; }
            public string FinancialCPD { get; set; }

        }
    }
}