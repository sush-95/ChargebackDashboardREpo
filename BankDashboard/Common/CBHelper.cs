using BankDashboard.CBModel;
using BankDashboard.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
            List<tbl_UnassignedTickets> list = objdata.GetCaseStatusDataWithFilter(DateTime.Today,DateTime.Today,"");
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
        public static List<string> CaseDataOnFilter(string startdate,string enddate)
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
        public static List<tbl_UnassignedTickets> CaseDataTableOnFilter(string startdate, string enddate,string filter)
        {
            DateTime Strtday = DateTime.Today, EndDt = DateTime.Today;
            if (!string.IsNullOrEmpty(startdate) && !string.IsNullOrEmpty(enddate))
            {
                Strtday = DateTime.Parse(startdate);
                EndDt = DateTime.Parse(enddate);
            }
            CaseStatisticsDataLayer objdata = new CaseStatisticsDataLayer();
            List<tbl_UnassignedTickets> list = objdata.GetCaseStatusDataWithFilter(Strtday, EndDt,filter);
            return list;
        }

        #endregion----------------------------------------------------------------------------------------------------

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