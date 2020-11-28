using BankDashboard.CBModel;
using BankDashboard.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static BankDashboard.Common.ViewModelClass;

namespace BankDashboard.Common
{
    public class CBHelper
    {
        #region------------------------------------Case Statistics--------------------------------------------------
        public static List<string> CaseForToday()
        {
            CaseStatisticsDataLayer objdata = new CaseStatisticsDataLayer();
            List<string> cList = objdata.GetCaseStatusData(DateTime.Today, DateTime.Today);
            return getpercentagefigure(cList);
        }
        public static List<tbl_UnassignedTickets> CaseTableDataForToday()
        {
            CaseStatisticsDataLayer objdata = new CaseStatisticsDataLayer();
            List<tbl_UnassignedTickets> list = objdata.GetCaseStatusDataWithFilter(DateTime.Today, DateTime.Today, "");
            return list;
        }
        public static List<string> getpercentagefigure(List<string> casestatfigure)
        {
            List<string> list = new List<string>();
            long sum = 0;
            foreach (string item in casestatfigure)
            {
                sum += long.Parse(item);
            }
            foreach (string item in casestatfigure)
            {
                if (sum == 0) { list.Add("0"); }
                else
                {
                    list.Add(((long.Parse(item) * 100 / sum)).ToString());
                }
            }
            casestatfigure.AddRange(list);
            return casestatfigure;
        }
        public static List<string> CaseDataOnFilter(string startdate, string enddate)
        {
            DateTime Strtday = DateTime.Today, EndDt = DateTime.Today;
            if (!string.IsNullOrEmpty(startdate) && !string.IsNullOrEmpty(enddate))
            {
                Strtday = DateTime.Parse(startdate);
                EndDt = DateTime.Parse(enddate);
            }
            CaseStatisticsDataLayer objdata = new CaseStatisticsDataLayer();
            List<string> cList = objdata.GetCaseStatusData(Strtday, EndDt);
            return getpercentagefigure(cList);
        }
        public static List<tbl_UnassignedTickets> CaseDataTableOnFilter(string startdate, string enddate, string filter)
        {
            DateTime Strtday = DateTime.Today, EndDt = DateTime.Today;
            if (!string.IsNullOrEmpty(startdate) && !string.IsNullOrEmpty(enddate))
            {
                Strtday = DateTime.Parse(startdate);
                EndDt = DateTime.Parse(enddate);
            }
            CaseStatisticsDataLayer objdata = new CaseStatisticsDataLayer();
            List<tbl_UnassignedTickets> list = objdata.GetCaseStatusDataWithFilter(Strtday, EndDt, filter);
            return list;
        }
        public static List<string> GetRoutingPortalForToday()
        {
            CaseStatisticsDataLayer objdata = new CaseStatisticsDataLayer();
            List<string> cList = objdata.GetRoutingPortalGraph(DateTime.Today, DateTime.Today);
            return cList;
        }
        public static List<string> RoutingPortaOnFilter(string startdate, string enddate)
        {
            DateTime Strtday = DateTime.Today, EndDt = DateTime.Today;
            if (!string.IsNullOrEmpty(startdate) && !string.IsNullOrEmpty(enddate))
            {
                Strtday = DateTime.Parse(startdate);
                EndDt = DateTime.Parse(enddate);
            }
            CaseStatisticsDataLayer objdata = new CaseStatisticsDataLayer();
            List<string> cList = objdata.GetRoutingPortalGraph(Strtday, EndDt);
            return cList;
        }

        public static List<tbl_AuthCode> getRoutingPortalTable(string fromdate = "", string todate = "", string filter = "")
        {
            CBDB db = new CBDB();
            List<tbl_AuthCode> list = db.tbl_AuthCode.SqlQuery(GetQueryForTblAuthCode(fromdate, todate, filter)).ToList();
            return list;
        }
        public static string GetQueryForTblAuthCode(string fromdate = "", string todate = "", string filter = "")
        {
            string query = "SELECT [Id] ,[FeedbackId] ,[AuthCode] ,[CardNumber] ,[RetrievalRefNo] ,[TransactionAmount] ,[TranId] ,[ARN Number] as ARN_Number ,[RRN Number]as RRN_Number ,[Routing Channel] as Routing_Channel ,[Amount] ,[Issuer] ,[Acquirer] ,[MCC Value] as MCC_Value ,[VROL Case No] as VROL_Case_No ,[Card Type] as Card_Type ,[Status] ,[Exception] ,[Bot Remark] AS Bot_Remark ,[Bot Process StartTime] as Bot_Process_StartTime ,[Bot Process EndTime] as Bot_Process_EndTime ,[Bot EntryTime] as Bot_EntryTime ,[Bot UpdateTime] as Bot_UpdateTime FROM [Chargeback].[dbo].[tbl_AuthCode] where  ";
            int qrylength = query.Length;
            if (!string.IsNullOrEmpty(filter))
            {
                query = query + "[Routing Channel]='" + filter.Trim() + "'";
            }
            if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(todate))
            {
                DateTime fdate = Convert.ToDateTime(fromdate), Todate = Convert.ToDateTime(todate).AddHours(23);
                query = query + ((query.Length > qrylength + 3) ? " and [Bot EntryTime] between '" + fdate.Date.ToString() + "' and '" + Todate.Date.ToString() + "'" :
                    "  [Bot EntryTime] between '" + fdate.Date.ToString() + "' and '" + Todate.Date.ToString() + "'");
            }
            if (qrylength == query.Length)
            {
                query = "SELECT TOP (1000) [Id] ,[FeedbackId] ,[AuthCode] ,[CardNumber] ,[RetrievalRefNo] ,[TransactionAmount] ,[TranId] ,[ARN Number] as ARN_Number ,[RRN Number]as RRN_Number ,[Routing Channel] as Routing_Channel ,[Amount] ,[Issuer] ,[Acquirer] ,[MCC Value] as MCC_Value ,[VROL Case No] as VROL_Case_No ,[Card Type] as Card_Type ,[Status] ,[Exception] ,[Bot Remark] AS Bot_Remark ,[Bot Process StartTime] as Bot_Process_StartTime ,[Bot Process EndTime] as Bot_Process_EndTime ,[Bot EntryTime] as Bot_EntryTime ,[Bot UpdateTime] as Bot_UpdateTime FROM [Chargeback].[dbo].[tbl_AuthCode];";
            }
            return query;
        }
        public static List<string> GetCaseReadyForAction()
        {
            CaseStatisticsDataLayer objdata = new CaseStatisticsDataLayer();
            List<string> cList = objdata.GetCaseReadyForAction();
            return cList;
        }
        public static List<tbl_UnassignedTickets> GetCaseReadyTable()
        {
            CBDB db = new CBDB();
            List<tbl_UnassignedTickets> list = db.tbl_UnassignedTickets.SqlQuery("select * from tbl_UnassignedTickets where Status in ('processed','businessruleexception');").ToList();
            return list;
        }
        #endregion----------------------------------------------------------------------------------------------------

        #region--------------------------------Wc Statisstics-------------------------------------------
        public static List<string> WCScaseStatusFigure(ref List<tbl_WeCareReactive> tblWC, string startdate = "", string enddate = "", string Filter = "")
        {
            CBDB db = new CBDB();
            List<tbl_WeCareReactive> wclist = new List<tbl_WeCareReactive>();
            List<string> wccount = new List<string>();
            string daterange = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(startdate) && !string.IsNullOrEmpty(enddate))
                {
                    DateTime stdt = Convert.ToDateTime(startdate).Date, Tdt = Convert.ToDateTime(enddate).Date;
                    wclist = db.tbl_WeCareReactive.Where(x => x.BotEntryTime >= stdt && x.BotEntryTime <= Tdt).ToList();
                    daterange = "from " + startdate + " to" + enddate;
                }
                else
                {
                    wclist = db.tbl_WeCareReactive.ToList();
                    var wc = wclist.OrderByDescending(x => x.BotEntryTime).LastOrDefault();
                    if (wc != null && wc.BotEntryTime != null)
                    {
                        daterange = Convert.ToDateTime(wc.BotEntryTime).ToString("dd-MMM-yyyy");
                    }

                    daterange = "from " + daterange + " till date.";
                }
                wccount.Add(wclist.Where(x => x.Stage.Contains("Closed") || x.Stage.Contains("Resolved")).ToList().Count.ToString());
                wccount.Add(wclist.Where(x => x.Stage.Contains("In Progress") && x.AssignedUserID != null).ToList().Count.ToString());
                wccount.Add(wclist.Where(x => x.Stage.Contains("In Progress") && x.AssignedUserID == null).ToList().Count.ToString());
                wccount = getpercentagefigure(wccount);
                wccount.Add(daterange);
                //-----------------------------------------------Table----------------------------------------------------------------
                if (!string.IsNullOrEmpty(Filter))
                {
                    if (Filter.Equals("Closed"))
                    {
                        wclist = wclist.Where(x => x.Stage.Contains("Closed") || x.Stage.Contains("Resolved")).ToList();
                    }
                    else if (Filter.Equals("In Progress"))
                    {
                        wclist = wclist.Where(x => x.Stage.Contains("In Progress") && x.AssignedUserID != null).ToList();
                    }
                    else
                    {
                        wclist = wclist.Where(x => x.Stage.Contains("In Progress") && x.AssignedUserID == null).ToList();
                    }
                }
                tblWC = wclist;
            }
            catch { }
            return wccount;
        }
        public static IssueType GetListOfIssueTypes(string startdate = "", string enddate = "", string Filter = "", int issue = 0)
        {
            CBDB db = new CBDB();
            IssueType objI = new IssueType();
            List<tbl_WeCareReactive> wclist = db.tbl_WeCareReactive.ToList();
            try
            {
                if (!string.IsNullOrEmpty(startdate) && !string.IsNullOrEmpty(enddate))
                {
                    DateTime stdt = Convert.ToDateTime(startdate).Date, Tdt = Convert.ToDateTime(enddate).Date;
                    wclist = db.tbl_WeCareReactive.Where(x => x.BotEntryTime >= stdt && x.BotEntryTime <= Tdt).ToList();
                    objI.datelbl = "from " + startdate + " to " + enddate;
                }
                else
                {
                    wclist = db.tbl_WeCareReactive.ToList();
                    objI.datelbl = "for " + DateTime.Today.AddDays(-1).ToString("dd-MMM-yyyy");
                }
                if (!string.IsNullOrEmpty(Filter))
                {
                    wclist = wclist.Where(x => x.Issue.Equals(Filter)).ToList();
                }
                List<string> issues = wclist.Select(x => x.Issue).Distinct().ToList();
                objI.Issuetypes = issues;
                objI.Issuetypesfigures = new List<string>();
                foreach (string item in objI.Issuetypes)
                {
                    objI.Issuetypesfigures.Add(wclist.Where(x => x.Issue.Equals(item)).ToList().Count.ToString());
                }
                objI = GetColorOnIssueType(objI, issue);
            }
            catch {}
            return objI;
        }
        public static IssueType GetColorOnIssueType(IssueType obj, int issue)
        {
            List<string> name = new List<string>(), data = new List<string>(), backcolor = new List<string>(), bordercolor = new List<string>();
            if (issue == 0 && obj.Issuetypesfigures.Count > 5)
            {
                for (int i = 0; i < 5; i++)
                {
                    name.Add(obj.Issuetypes[i]);
                    data.Add(obj.Issuetypesfigures[i]);
                    backcolor.Add("rgba(241, 46, 35, 0.5)");
                    bordercolor.Add("rgba(241, 46, 35)");
                }
                obj.Issuetypes = name;
                obj.Issuetypesfigures = data;
                obj.IssuetypesBackColor = backcolor;
                obj.IssuetypesBordercolor = bordercolor;
            }
            else
            {
                for (int i = 0; i < obj.Issuetypesfigures.Count; i++)
                {
                    backcolor.Add("rgba(241, 46, 35, 0.5)");
                    bordercolor.Add("rgba(241, 46, 35)");
                }
                obj.IssuetypesBackColor = backcolor;
                obj.IssuetypesBordercolor = bordercolor;
            }
            return obj;
        }
        #endregion---------------------------------------------------------------------------------------

        #region-------------------------------Case History--------------------------------------------------------------------
        public static List<tbl_UnassignedTickets> GetCaseHistoryFilterd(ViewModelClass.FilterClass filter)
        {
            List<tbl_UnassignedTickets> list = new List<tbl_UnassignedTickets>();
            try
            {
                CBDB db = new CBDB();
                list = db.tbl_UnassignedTickets.SqlQuery(GetQueryForTblUnassigned(filter)).ToList();
            }
            catch (Exception ex) { throw ex; }
            return list.OrderByDescending(x => x.BotDataEntryTime).ToList();
        }
        public static string GetQueryForTblUnassigned(ViewModelClass.FilterClass filter)
        {

            string query = "Select * from tbl_UnassignedTickets where ";
            int qrylength = query.Length;
            if (filter.FeedbackID != null && !string.IsNullOrEmpty(filter.FeedbackID.Trim()))
            {
                query = query + "FeedbackId='" + filter.FeedbackID.Trim() + "'";
            }
            if (filter.CIFNo != null && !string.IsNullOrEmpty(filter.CIFNo.Trim()))
            {
                query = query + ((query.Length > qrylength + 3) ? " and CIFNo='" + filter.CIFNo.Trim() + "'" : " CIFNo='" + filter.CIFNo.Trim() + "'");
            }
            if (filter.Fromdate != null && !string.IsNullOrEmpty(filter.Fromdate) && filter.Todate != null && !string.IsNullOrEmpty(filter.Todate))
            {
                DateTime fdate = Convert.ToDateTime(filter.Fromdate), todate = Convert.ToDateTime(filter.Todate).AddHours(23);
                query = query + ((query.Length > qrylength + 3) ? " and BotDataEntryTime between '" + fdate.ToString() + "' and '" + todate.ToString() + "'" :
                    "  BotDataEntryTime between '" + fdate.ToString() + "' and '" + todate.ToString() + "'");
            }
            if (qrylength == query.Length)
            {
                query = "Select * from tbl_UnassignedTickets";
            }
            return query + ";";
        }
        #endregion-----------------------------------------------------------------------------------------------------

        #region------------------------------------------Matched Transaction--------------------------------------------
        public static List<Matched_FinancialTransaction> GetMatchedTransaction(ViewModelClass.FilterClass filter)
        {
            List<Matched_FinancialTransaction> list = new List<Matched_FinancialTransaction>();
            try
            {
                CBDB db = new CBDB();
                if (filter.FeedbackID != null && filter.Fromdate != null && filter.Todate != null)
                {
                    DateTime fdt = Convert.ToDateTime(filter.Fromdate), tdt = Convert.ToDateTime(filter.Todate);
                    list = db.Matched_FinancialTransaction.Where(x => x.BotEntryTime >= fdt && x.BotEntryTime <= tdt).ToList();
                }
                else
                {
                    list = db.Matched_FinancialTransaction.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
        #endregion------------------------------------------------------------------------------------------------------
        #region------------------------------------------UnMatched Transaction--------------------------------------------
        public static List<Unmatched_FinancialTransaction> GetUnMatchedTransaction(ViewModelClass.FilterClass filter)
        {
            List<Unmatched_FinancialTransaction> list = new List<Unmatched_FinancialTransaction>();
            try
            {
                CBDB db = new CBDB();
                if (filter.FeedbackID != null && filter.Fromdate != null && filter.Todate != null)
                {
                    DateTime fdt = Convert.ToDateTime(filter.Fromdate), tdt = Convert.ToDateTime(filter.Todate);
                    list = db.Unmatched_FinancialTransaction.Where(x => x.BotEntryTime >= fdt && x.BotEntryTime <= tdt).ToList();
                }
                else
                {
                    list = db.Unmatched_FinancialTransaction.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
        #endregion------------------------------------------------------------------------------------------------------
    }
}