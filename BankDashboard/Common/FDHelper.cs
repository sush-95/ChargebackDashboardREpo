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

      
        public static Tbl_User_Detail GetUser(string uname, string pwd)
        {
            using (CBDB db = new CBDB())
            {
                var obj = db.Tbl_User_Detail.Where(x => x.UserName.Equals(uname) && x.Password.Equals(pwd)).FirstOrDefault();
                return obj;
            }

        }
        public static void SaveUser(ref Tbl_User_Detail obj)
        {
            string user = obj.UserName;
            Tbl_User_Detail tbl = new Tbl_User_Detail();
            using (CBDB db = new CBDB())
            {
                tbl = db.Tbl_User_Detail.Where(x => x.UserName.ToLower().Equals(user.ToLower())).FirstOrDefault();
                if (tbl == null)
                {
                    if (obj.Usergroup.ToLower().Contains("admin"))
                    {
                        obj.GroupPages = "CaseStat ,WCStat ,SLA ,CaseHistory ,CaseClosure ,MtchedTran ,UnmtchedTran ,Recon,RobotConfig";
                    }
                    else
                    {
                        obj.GroupPages = "CaseStat ,WCStat ,SLA ,CaseHistory ,CaseClosure ,MtchedTran ,UnmtchedTran ,Recon";
                    }
                    db.Tbl_User_Detail.Add(obj);
                    db.SaveChanges();
                    tbl = obj;
                }
            }
            obj = tbl;
        }
        public static string GetPageName(string pages)
        {
            string pagename = "Index";
            try
            {
                if (!string.IsNullOrEmpty(pages))
                {
                    if (pages.Contains("CaseStat"))
                    {
                        pagename = "Index";
                    }
                    else
                    {
                        string[] arr = pages.Split(',');
                        arr = arr.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                        switch (arr[0].Trim())
                        {
                            case "CaseStat":
                                pagename = "Index";
                                break;
                            case "WCStat":
                                pagename = "WC";
                                break;
                            case "SLA":
                                pagename = "SLA";
                                break;
                            case "CaseHistory":
                                pagename = "CaseHistory";
                                break;
                            case "CaseClosure":
                                pagename = "ClosureReports";
                                break;
                            case "MtchedTran":
                                pagename = "MatchedFinTransaction";
                                break;
                            case "UnmtchedTran":
                                pagename = "UnmatchedFinTransaction";
                                break;
                            case "tlconfig":
                                pagename = "TLConfig";
                                break;
                            case "Recon":
                                pagename = "ReconsiliationReport";
                                break;
                            default:
                                pagename = "Index";
                                break;
                        }
                    }
                }
            }
            catch { pagename = "Index"; }
            return pagename;

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