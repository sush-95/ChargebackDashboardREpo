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
        public static List<tbl_WeCareReactive> GetCaseHistoryFilterd(ViewModelClass.FilterClass filter)
        {
            List<tbl_WeCareReactive> list = new List<tbl_WeCareReactive>();
            try
            {
                CBDB db = new CBDB();
                if (filter.FeedbackID != null && filter.Fromdate != null && filter.Todate != null)
                {
                    DateTime fdt = Convert.ToDateTime(filter.Fromdate), tdt = Convert.ToDateTime(filter.Todate);
                    list = db.tbl_WeCareReactive.Where(x => x.FeedbackID.Equals(filter.FeedbackID) && x.BotEntryTime >= fdt && x.BotEntryTime <= tdt).ToList();
                }
                else if (filter.Fromdate != null && filter.Todate != null)
                {
                    DateTime fdt = Convert.ToDateTime(filter.Fromdate), tdt = Convert.ToDateTime(filter.Todate);
                    list = db.tbl_WeCareReactive.Where(x => x.BotEntryTime >= fdt && x.BotEntryTime <= tdt).ToList();
                }
                else if (filter.FeedbackID != null)
                {
                    list = db.tbl_WeCareReactive.Where(x => x.FeedbackID.Equals(filter.FeedbackID)).ToList();
                }
                else
                {
                    list = db.tbl_WeCareReactive.ToList();
                }
            }
            catch (Exception ex) { throw ex; }
            return list;
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
                    list = db.Matched_FinancialTransaction.Where(x =>x.BotEntryTime >= fdt && x.BotEntryTime <= tdt).ToList();
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