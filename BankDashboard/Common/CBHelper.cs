using BankDashboard.CBModel;
using BankDashboard.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

        #region ---------------------------------Reconciliation Report-----------------------------------------------
        public static List<NonCustom_GLReconciliationTable> GetReconsiledReportFilterd(ViewModelClass.ReconciliationFilter filter)
        {
            List<NonCustom_GLReconciliationTable> list = new List<NonCustom_GLReconciliationTable>();
            try
            {
                CBDB db = new CBDB();
                list = db.NonCustom_GLReconciliationTable.SqlQuery(GetQueryForReconciliedData(filter)).ToList();
            }
            catch (Exception ex) { throw ex; }
            return list;
        }

        public static string GetQueryForReconciliedData(ViewModelClass.ReconciliationFilter filter)
        {

            string query = "Select * from NonCustom_GLReconciliationTable where ";
            int qrylength = query.Length;
            if (!string.IsNullOrEmpty(filter.CardNumber) && !string.IsNullOrEmpty(filter.MemberCaseNumber))
            {
                query = query + "MemberCase='"+ filter.MemberCaseNumber.Trim() + "' and CardNumber ='" +filter.CardNumber.Trim() +"' and" ;
            }
            else if (!string.IsNullOrEmpty(filter.MemberCaseNumber))
            {
                query = query + "MemberCase='" + filter.MemberCaseNumber.Trim() + "' and";
            }
            else if (!string.IsNullOrEmpty(filter.CardNumber))
            {
                query = query + "CardNumber='" + filter.CardNumber.Trim() + "' and";
            }
            query += " IsActive='1';";


            return query;
        }
        public static void ReconsiledReport()
        {
            try
            {
                CaseStatisticsDataLayer objdata = new CaseStatisticsDataLayer();
                List<string> Ids =  objdata.GetRecordIdForReconsiliation();
                
                string Ids_Str = string.Join(",", Ids);

                objdata.MarkReconciliationRecordsInActive_IdsList(Ids_Str);

            }
            catch (Exception ex) { throw ex; }
            
        }
        public static void AddRow_NonCustom_GLReconciliationTable(NonCustom_GLReconciliationTable obj)
        {
            NonCustom_GLReconciliationTable addObj = new NonCustom_GLReconciliationTable();
            if(obj!=null)
            {
                CBDB db = new CBDB();
                CaseStatisticsDataLayer objdata = new CaseStatisticsDataLayer();
                
                addObj.ValueDate = obj.ValueDate;
                addObj.PostDate = obj.PostDate;
                addObj.MemberCase = obj.MemberCase;
                addObj.Name = obj.Name;
                addObj.Reference = obj.CardNumber;
                addObj.Credit = obj.Credit;
                addObj.Debit = obj.Debit;
                
                addObj.IsActive = true;
                

                db.NonCustom_GLReconciliationTable.Add(addObj);
                db.SaveChanges();
            }

        }
        public static void MarkAllRecordsAsInActive()
        {
            try
            {                
                CaseStatisticsDataLayer objdata = new CaseStatisticsDataLayer();
                objdata.MarkAllReconciliationDataInActive();  
            }
            catch (Exception ex) { throw ex; }
        }
        public static void MarkRecordsAsInActive(string Id)
        {
            try
            {
                CaseStatisticsDataLayer objdata = new CaseStatisticsDataLayer();
                objdata.MarkReconciliationRecordInActive(Id);
            }
            catch (Exception ex) { throw ex; }
        }

        #endregion

        #region------------------------------------------AcceptedCaseClosureReportFilter------------------------------
        public static List<tbl_IssuingIncomingVISA> AcceptedCaseClosureReportFilter(ViewModelClass.ClosureReportFilter filter)
        {
            List<tbl_IssuingIncomingVISA> list = new List<tbl_IssuingIncomingVISA>();
            try
            {
                CBDB db = new CBDB();
                list = db.tbl_IssuingIncomingVISA.SqlQuery(GetQueryForAcceptedClosureReport(filter)).ToList();
            }
            catch (Exception ex) { throw ex; }
            return list;
        }
        public static List<tbl_IssuingIncomingVISA> AcceptedCaseClosureReportDefaultData()
        {
            List<tbl_IssuingIncomingVISA> list = new List<tbl_IssuingIncomingVISA>();
            try
            {
                CBDB db = new CBDB();
                list = db.tbl_IssuingIncomingVISA.SqlQuery("select * from tbl_IssuingIncomingVISA where CaseClosureStatus In ('open','close')").ToList();
            }
            catch (Exception ex) { throw ex; }
            return list;
        }


        public static string GetQueryForAcceptedClosureReport(ViewModelClass.ClosureReportFilter filter)
        {

            string query = "Select * from tbl_IssuingIncomingVISA where ";
            int qrylength = query.Length;
            if (filter.ROLCaseNumber != null && !string.IsNullOrEmpty(filter.ROLCaseNumber.Trim()))
            {
                query = query + "ROLCaseNumber='" + filter.ROLCaseNumber.Trim() + "'";
            }
            if (filter.FinancialCPD != null && !string.IsNullOrEmpty(filter.FinancialCPD.Trim()))
            {
                query = query + ((query.Length > qrylength + 3) ? " and FinancialCPD='" + filter.FinancialCPD.Trim() + "'" : " CIFNo='" + filter.FinancialCPD.Trim() + "'");
            }
            if(filter.Status != null && !string.IsNullOrEmpty(filter.Status.Trim()) && !filter.Status.Trim().Equals("0"))
            {
                query = query + ((query.Length > qrylength + 3) ? " and CaseClosureStatus='" + filter.Status.Trim() + "'" : " CaseClosureStatus='" + filter.Status.Trim() + "'");
            }
            if (filter.FromDate != null && (filter.FromDate != new DateTime(0001, 01, 01)))
            {

                query = query + ((query.Length > qrylength + 3) ? " and BotEntryTime >= '" + filter.FromDate.ToString("MM-dd-yyyy") + "'" : " BotEntryTime >= '" + filter.FromDate.ToString("MM-dd-yyyy") + "'");

            }
            if (filter.ToDate != null && (filter.ToDate != new DateTime(0001, 01, 01)))
            {

                query = query + ((query.Length > qrylength + 3) ? " and BotEntryTime >= '" + filter.ToDate.ToString("MM-dd-yyyy") + "'" : " BotEntryTime >= '" + filter.ToDate.ToString("MM-dd-yyyy") + "'");

            }
            if (qrylength == query.Length)
            {
                query = "select * from tbl_IssuingIncomingVISA where CaseClosureStatus In ('open','close')";
            }
            return query + ";";
        }
        #endregion

    }
}