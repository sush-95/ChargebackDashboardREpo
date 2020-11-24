using BankDashboard.CBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankDashboard.Common
{
    public class CBHelper
    {
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