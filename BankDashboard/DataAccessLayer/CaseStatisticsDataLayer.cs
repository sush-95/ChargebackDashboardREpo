﻿using BankDashboard.CBModel;
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
    }
}