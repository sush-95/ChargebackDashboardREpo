using BankDashboard.LogFile;
using BankDashboard.CBModel;
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
      
        #region----------------------------------------machine check------------------------------

        public static bool CheckMachine(string Machine)
        {
            if ((String.IsNullOrEmpty(Machine)))
                WriteToLogFile.writeMessage("Machine Name Received null");


            bool Check = false;
            try
            {
                //WriteToLogFile.writeMessage("Check Machine [Started]");

                using (CBDB db = new CBDB())
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
        public static Tbl_User_Detail GetUser(string uname, string pwd)
        {
            using (CBDB db = new CBDB())
            {
                var obj = db.Tbl_User_Detail.Where(x => x.UserName.Equals(uname) && x.Password.Equals(pwd)).FirstOrDefault();
                return obj;
            }

        }
        public static void SaveUser(Tbl_User_Detail obj)
        {
            //string DashboardURL = Convert.ToString(ConfigurationManager.AppSettings["DashboardURL"]);

            using (CBDB db = new CBDB())
            {
                var Check = db.Tbl_User_Detail.Where(x => x.UserName.Equals(obj.UserName)).FirstOrDefault();
                if (Check == null)
                {
                    db.Tbl_User_Detail.Add(obj);
                    db.SaveChanges();
                }
            }
        }
        public static void MachineLogout(string MName)
        {
            try
            {
                // WriteToLogFile.writeMessage("MachineLogout Method [Started]");
                using (CBDB db = new CBDB())
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