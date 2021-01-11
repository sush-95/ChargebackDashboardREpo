using BankDashboard.CBModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BankDashboard.DataAccessLayer
{
    public class CaseStatisticsDataLayer
    {
        string connectionString = GetSqlConnection();
        public List<string> GetCaseStatusData(DateTime fromdate, DateTime todate)
        {
            List<string> CountList = new List<string>();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("UDSP_DASHBOARD_Get_CaseStatus_TicketCount_tblUnassigned", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Param_FromDate", fromdate);
                    cmd.Parameters.AddWithValue("@Param_ToDate", todate);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                CountList.Add(dr["Processed_Count"].ToString());
                                CountList.Add(dr["BusinessRuleException_Count"].ToString());
                                CountList.Add(dr["ApplicationException_Count"].ToString());
                                CountList.Add(dr["Unprocessed_Count"].ToString());
                            }
                        }
                    }
                }
            }
            catch { }
            return CountList;
        }
        public List<tbl_UnassignedTickets> GetCaseStatusDataWithFilter(DateTime fromdate, DateTime todate, string filter)
        {
            List<tbl_UnassignedTickets> tbl = new List<tbl_UnassignedTickets>();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("UDSP_DASHBOARD_Get_CaseStatus_TicketType_tblUnassigned", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Param_FromDate", fromdate);
                    cmd.Parameters.AddWithValue("@Param_ToDate", todate);
                    cmd.Parameters.AddWithValue("@Param_TicketStatus", filter);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                tbl_UnassignedTickets obj = new tbl_UnassignedTickets();
                                obj.FeedbackId = dr["FeedbackId"].ToString();
                                obj.Issue = dr["Issue"].ToString();
                                obj.Status = dr["Status"].ToString();
                                obj.BotRemarks= dr["BotRemarks"].ToString();
                                obj.BotDataEntryTime =Convert.ToDateTime(dr["BotDataEntryTime"].ToString());
                                tbl.Add(obj);
                            }
                        }
                    }
                }
            }
            catch { }
            return tbl;
        }

        public List<string> GetRoutingPortalGraph(DateTime fromdate, DateTime todate)
        {
            List<string> CountList = new List<string>();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("UDSP_DASHBOARD_Get_RoutingPortal_Stats_tblAuthCode", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Param_FromDate", fromdate);
                    cmd.Parameters.AddWithValue("@Param_ToDate", todate);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {                              
                                CountList.Add(dr["MasterCard_Count"].ToString());
                                CountList.Add(dr["Visa_Count"].ToString());
                                CountList.Add(dr["Omanet_Count"].ToString());
                                CountList.Add(dr["Onus_Count"].ToString());
                                CountList.Add(dr["Posecom_Count"].ToString());
                            }
                        }
                    }
                }
            }
            catch { }
            return CountList;
        }
        //public List<string> GetRoutingPortalTable(DateTime fromdate, DateTime todate,string Filter)
        //{
        //    List<tbl_UnassignedTickets> tbl = new List<tbl_UnassignedTickets>();
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(connectionString))
        //        {
        //            DataSet ds = new DataSet();
        //            SqlCommand cmd = new SqlCommand("UDSP_DASHBOARD_Get_CaseStatus_TicketType_tblUnassigned", con);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@Param_FromDate", fromdate);
        //            cmd.Parameters.AddWithValue("@Param_ToDate", todate);
        //            cmd.Parameters.AddWithValue("@Param_TicketStatus", filter);
        //            SqlDataAdapter da = new SqlDataAdapter(cmd);
        //            da.Fill(ds);
        //            if (ds != null)
        //            {
        //                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //                {
        //                    foreach (DataRow dr in ds.Tables[0].Rows)
        //                    {
        //                        tbl_UnassignedTickets obj = new tbl_UnassignedTickets();
        //                        obj.FeedbackId = dr["FeedbackId"].ToString();
        //                        obj.Issue = dr["Issue"].ToString();
        //                        obj.Status = dr["Status"].ToString();
        //                        obj.BotRemarks = dr["BotRemarks"].ToString();
        //                        obj.BotDataEntryTime = Convert.ToDateTime(dr["BotDataEntryTime"].ToString());
        //                        tbl.Add(obj);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch { }
        //    return tbl;
        //}

        public List<string> GetCaseReadyForAction()
        {
            List<string> CountList = new List<string>();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("UDSP_DASHBOARD_Get_CaseReadyForAction_Count_tblAuthCode", con);
                    cmd.CommandType = CommandType.StoredProcedure;                   
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                CountList.Add(dr["Cases Ready For Action"].ToString());                              
                            }
                        }
                    }
                }
            }
            catch { }
            return CountList;
        }

        public static string GetSqlConnection()
        {
            var x = System.Configuration.ConfigurationManager.AppSettings["getstr"];
            byte[] inputArray = Convert.FromBase64String(x);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes("sblw-3hn8-sqoy19");
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        public void MarkAllReconciliationDataInActive()
        {

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("UDSP_DASHBOARD_InActiveAllData_NonCustom_GLReconciliationTable", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e) { throw e; }

        }

        public void MarkReconciliationRecordInActive(string Id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("UDSP_DASHBOARD_InActiveRecord_NonCustom_GLReconciliationTable", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Param_RecordId", Id);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e) { throw e; }

        }
        public List<string> GetRecordIdForReconsiliation()
        {
            List<string> RecordIds = new List<string>();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("UDSP_DASHBOARD_GetRecordIdForRconsile", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            RecordIds.Add(ds.Tables[0].Rows[0]["idDebit"].ToString());
                            RecordIds.Add(ds.Tables[0].Rows[0]["idCredit"].ToString());
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                string idDebit = dr["idDebit"].ToString();
                                string idCredit = dr["idCredit"].ToString();

                                if (!RecordIds.Contains(idCredit) && !RecordIds.Contains(idDebit))
                                {
                                    RecordIds.Add(idDebit);
                                    RecordIds.Add(idCredit);
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception e) { throw e; }

            return RecordIds;

        }

        public void MarkReconciliationRecordsInActive_IdsList(string Ids)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("UDSP_DASHBOARD_InActiveRecords_IDsList_NonCustom_GLReconciliationTable", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@param_IdsList", Ids);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e) { throw e; }

        }

        public void UpdateMachineTime(string machine)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("UDSP_DASHBOARD_Update_MachineInfo", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Param_MachineName", machine);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);                   
                }
            }
            catch { }
        }
        public Dictionary<string, string> GetDebitCreditAmountAfterReconciliation()
        {
            Dictionary<string, string> DebitCreditInfo = new Dictionary<string, string>();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("UDSP_DASHBOARD_Get_ReconciliationDifference", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                DebitCreditInfo.Add("DifferenceDebitCredit", ((-1)*Convert.ToInt64(dr["DifferenceDebitCredit"])).ToString());
                                DebitCreditInfo.Add("Debit", dr["Debit"].ToString());
                                DebitCreditInfo.Add("Credit", dr["Credit"].ToString());
                                break;
                            }
                        }
                    }
                }
            }
            catch { }
            return DebitCreditInfo;
        }

    }
}