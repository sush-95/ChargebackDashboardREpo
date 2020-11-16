using BankDashboard.LogFile;
using BankDashboard.ModelFd;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using static BankDashboard.Common.FDViewModelClass;

namespace BankDashboard.Common
{
    public class FDHelper
    {
        public static ListOFproperty getAllPropertyList()
        {
            ListOFproperty obj = new ListOFproperty();
            try
            {
                DBFD db = new DBFD();
                var FdList = db.FDRequests.ToList();
                obj.ListofFDCreated = FdList.Select(x => x.FDCreated).Distinct().ToList();
                obj.ListOfMailsent = FdList.Select(x => x.MailSent).Distinct().ToList();
                obj.ListOfSMSSent = FdList.Select(x => x.SMSSent).Distinct().ToList();
                obj.ListMarkedStatusInMB = FdList.Select(x => x.MarkedStatusInMB).Distinct().ToList();
                obj.ListofTermLength = FdList.Select(x => x.TermLength).Distinct().ToList();
            }
            catch { }
            return obj;
        }
        public static List<FDRequest> GetFilteredLIst(ReportView filter)
        {
            List<FDRequest> newfdlist = new List<FDRequest>();
            try
            {
                DBFD db = new DBFD();
                newfdlist = db.FDRequests.SqlQuery(GetQuery(filter)).ToList();
            }
            catch (Exception ex) { }
            return newfdlist;
        }
        public static string GetQuery(ReportView filter)
        {
            string query = "select * from FDRequests where ";
            int qrylength = query.Length;
            if (filter.CustomerNumber != null && !string.IsNullOrEmpty(filter.CustomerNumber.Trim()))
            {
                query = query + "CustomerNumber like '%" + filter.CustomerNumber.Trim() + "%'";
            }
            if (filter.TermLength != null && !string.IsNullOrEmpty(filter.TermLength.Trim()))
            {
                query = query + ((query.Length > qrylength + 3) ? " and TermLength='" + filter.TermLength.Trim() + "'" : " TermLength='" + filter.TermLength.Trim() + "'");
            }
            if (filter.RequestDate != null && !string.IsNullOrEmpty(filter.RequestDate.Trim()))
            {
                string date = Convert.ToDateTime(filter.RequestDate).ToString("yyyy-MM-dd");
                query = query + ((query.Length > qrylength + 3) ? " and RequestDate like '%" + date + "%'" : " RequestDate like '%" + date + "%'");
            }
            if (filter.FDCreated != null && !string.IsNullOrEmpty(filter.FDCreated.Trim()))
            {
                query = query + ((query.Length > qrylength + 3) ? " and FDCreated='" + filter.FDCreated.Trim() + "'" : "  FDCreated='" + filter.FDCreated + "'");
            }
            if (filter.MailSent != null && !string.IsNullOrEmpty(filter.MailSent.Trim()))
            {
                query = query + ((query.Length > qrylength + 3) ? " and MailSent='" + filter.MailSent.Trim() + "'" : " MailSent='" + filter.MailSent + "'");
            }
            if (filter.SMSSent != null && filter.SMSSent != null && !string.IsNullOrEmpty(filter.SMSSent.Trim()))
            {
                query = query + ((query.Length > qrylength + 3) ? " and SMSSent='" + filter.SMSSent.Trim() + "'" : " SMSSent='" + filter.SMSSent + "'");
            }
            if (filter.MarkedStatusInMB != null && !string.IsNullOrEmpty(filter.MarkedStatusInMB.Trim()))
            {
                query = query + ((query.Length > qrylength + 3) ? " and MarkedStatusInMB='" + filter.MarkedStatusInMB.Trim() + "'" : " MarkedStatusInMB='" + filter.MarkedStatusInMB.Trim() + "'");
            }
            if (filter.FromDate != null && !string.IsNullOrEmpty(filter.FromDate) && filter.ToDate != null && !string.IsNullOrEmpty(filter.ToDate))
            {
                DateTime fdate = Convert.ToDateTime(filter.FromDate), todate = Convert.ToDateTime(filter.ToDate).AddHours(23);
                query = query + ((query.Length > qrylength + 3) ? " and BOTEntryDate between '" + fdate.ToString() + "' and '" + todate.ToString() + "'" :
                    "  BOTEntryDate between '" + fdate.ToString() + "' and '" + todate.ToString() + "'");
            }
            return query + ";";
        }


        public static Tbl_User_Detail GetUser(string uname, string pwd)
        {
            using (DBFD db = new DBFD())
            {
                var obj = db.Tbl_User_Detail.Where(x => x.UserName.Equals(uname) && x.Password.Equals(pwd)).FirstOrDefault();
              
                return obj;
            }

        }
        public static void SaveUser(Tbl_User_Detail obj)
        {
            //string DashboardURL = Convert.ToString(ConfigurationManager.AppSettings["DashboardURL"]);

            using (DBFD db = new DBFD())
            {
                var Check = db.Tbl_User_Detail.Where(x => x.UserName.Equals(obj.UserName)).FirstOrDefault();
                if (Check == null)
                {
                    db.Tbl_User_Detail.Add(obj);
                    db.SaveChanges();
                }
            }
        }
        #region----------------------------------------machine check------------------------------

        public static bool CheckMachine(string Machine)
        {
            if ((String.IsNullOrEmpty(Machine)))
                WriteToLogFile.writeMessage("Machine Name Received null");


            bool Check = false;
            try
            {
                //WriteToLogFile.writeMessage("Check Machine [Started]");

                using (DBFD db = new DBFD())
                {
                    //WriteToLogFile.writeMessage("Connection With DB Created");
                    var mobj = db.tbl_MachineInfo.Where(x => x.MachineName.Equals(Machine)).FirstOrDefault();

                    if (mobj == null)
                    {
                        //WriteToLogFile.writeMessage("No machine is present with this name");
                        //WriteToLogFile.writeMessage("Creating New Machine Entry in DB");
                        tbl_MachineInfo obj = new tbl_MachineInfo();
                        obj.MachineName = Machine;
                        obj.CreatedTs = DateTime.Now;
                        obj.IsActive = true;
                        ///WriteToLogFile.writeMessage("Adding Entry in Table");
                        db.tbl_MachineInfo.Add(obj);
                        // WriteToLogFile.writeMessage("Entry Added Successfully in Table");
                        db.SaveChanges();
                        //WriteToLogFile.writeMessage("Changes Saved Successfully");
                        Check = true;
                    }
                    else if (mobj.IsActive == false)
                    {
                        // WriteToLogFile.writeMessage("Machine Already present but with is asctive as false");
                        mobj.IsActive = true;
                        mobj.CreatedTs = DateTime.Now;
                        //WriteToLogFile.writeMessage("Saving Changes to DB");
                        db.SaveChanges();
                        //WriteToLogFile.writeMessage("Changes Saved Successfully");
                        Check = true;
                    }
                    else
                    {
                        //WriteToLogFile.writeMessage("Machine Already present and with is asctive as True");
                        //WriteToLogFile.writeMessage("Checking Time Difference");
                        int TimeDiff = Convert.ToInt32(ConfigurationManager.AppSettings["TimeDiff"].ToString());
                        //WriteToLogFile.writeMessage("Time Diff = "+TimeDiff.ToString());
                        int diff = (DateTime.Now - Convert.ToDateTime(mobj.CreatedTs)).Minutes;
                        ///WriteToLogFile.writeMessage("diff in minutes "+diff.ToString());
                        if (diff >= TimeDiff)
                        {
                            // WriteToLogFile.writeMessage("Machine Time out reached to maximun updating is active to true");
                            mobj.IsActive = true;
                            mobj.CreatedTs = DateTime.Now;
                            //WriteToLogFile.writeMessage("Saving Changes to DB");
                            db.SaveChanges();
                            //WriteToLogFile.writeMessage("Changes Saved Successfully");
                            Check = true;
                        }
                        else
                        {
                            // WriteToLogFile.writeMessage("Cannot activate machine because time out not reached");
                        }
                    }
                }

            }
            catch (Exception Ex)
            {
                //throw Ex;
               // WriteToLogFile.writeMessage("Exception Occured While processing this method exception msg = " + Ex.Message.ToString());
            }
            //WriteToLogFile.writeMessage("Returned Check Value "+Check.ToString());
            return Check;
        }

        public static void MachineLogout(string MName)
        {
            try
            {
                // WriteToLogFile.writeMessage("MachineLogout Method [Started]");
                using (DBFD db = new DBFD())
                {
                    var obj = db.tbl_MachineInfo.Where(x => x.MachineName.Equals(MName)).FirstOrDefault();

                    obj.IsActive = false;
                    db.SaveChanges();
                }
                // WriteToLogFile.writeMessage("MachineLogout Method [Ended] successfully");
            }
            catch (Exception Ex)
            {
                //WriteToLogFile.writeMessage("MachineLogout Method [Ended] UNsuccessfully Error = " +Ex.Message.ToString());
                throw Ex;
            }
        }
        #endregion

        public static string CodeEncrypt(string str, string key)
        {
            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(str);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
    }
}