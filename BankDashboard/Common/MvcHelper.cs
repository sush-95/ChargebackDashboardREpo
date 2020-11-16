using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BankDashboard.Common;
using static BankDashboard.Common.ViewModelClass;
using System.Net.Mail;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using BankDashboard.LogFile;
using System.Web.UI.WebControls;

namespace BankDashboard.Common
{
    public class MvcHelper
    {
        //static tbl_UserDetail SessionObj = new tbl_UserDetail();
        //public MvcHelper(tbl_UserDetail user)
        //{
        //    SessionObj = user;
        //}

        //#region-------------------------------------------- UserProfilePage--------------------------------

        //public static List<tbl_GroupRoleMappingMaster> GetGroupRoleMappingMasterList()

        //{
        //    BNKModel db = new BNKModel();
        //    return db.tbl_GroupRoleMappingMaster.Where(x=>!x.GroupName.Trim().Contains(Constants.UserGroups.ApplicationUserManagement)).ToList();
        //}


        //public static tbl_GroupRoleMappingMaster GetGroupRoleMappingMaster(int id)
        //{
        //    using (BNKModel db = new BNKModel())
        //    {
        //        return db.tbl_GroupRoleMappingMaster.Where(x => x.ID == id).FirstOrDefault();
        //    }

        //}

        //public static void EditGroupRoleMappingMaster(tbl_GroupRoleMappingMaster obj, double id)
        //{
        //    using (BNKModel db = new BNKModel())
        //    {
        //        tbl_GroupRoleMappingMaster editobj = db.tbl_GroupRoleMappingMaster.Where(x => x.ID == id).FirstOrDefault();
        //        if (!string.IsNullOrEmpty(obj.ApproveStat) && obj.ApproveStat.Equals(Constants.ModifyStatus))
        //        {
        //            editobj.PageName = obj.PageName;

        //            editobj.UpdateBy = SessionObj.AccountName;

        //            editobj.ApproveStat = Constants.ApproveModifyStatus;

        //            editobj.UpdateTime = DateTime.Now;

        //            editobj.IsActive = true;

        //            db.SaveChanges();
        //        }
        //        else

        //        {
        //            editobj.PageName = obj.PageName;
        //            editobj.IsActive = false;
        //            editobj.UpdateBy = SessionObj.AccountName;
        //            editobj.ApproveStat = Constants.ModifyStatus;
        //            editobj.UpdateTime = DateTime.Now;
        //            db.SaveChanges();
        //        }

        //    }
        //}
        //#endregion











       
        //public static string GetGroupPages(string group)
        //{
        //    using (BNKModel db = new BNKModel())
        //    {               
        //        var obj = db.tbl_GroupRoleMappingMaster.Where(x => x.GroupName.Trim().Contains(group)&&x.IsActive==true).FirstOrDefault();
        //        if (obj != null)
        //        {
        //            return ((string.IsNullOrEmpty(obj.PageName)) ? "" : obj.PageName);
        //        }
        //        else
        //        {
        //            return "";
        //        }

        //    }
        //}










        #region-------------------------DecryptUsing----------------------------

        public static string DecryptToBytesUsingCBC(byte[] toDecrypt,string secretKey)
        {
            byte[] src = toDecrypt;
            byte[] dest = new byte[src.Length];
            //string secretKey = "PSVJQRk9QTEpNVU1DWUZCRVFGV1VVT0=";
            string initVec = "2314345645678765";
            using (var aes = new AesCryptoServiceProvider())
            {
                aes.BlockSize = 128;
                aes.KeySize = 128;
                aes.IV = Encoding.UTF8.GetBytes(initVec);
                aes.Key = Encoding.UTF8.GetBytes(secretKey);
                aes.Mode = CipherMode.CBC;
                //aes.Padding = PaddingMode.Zeros;
                aes.Padding = PaddingMode.PKCS7;
                // decryption
                using (ICryptoTransform decrypt = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    byte[] decryptedText = decrypt.TransformFinalBlock(src, 0, src.Length);
                    aes.Clear();
                    return Encoding.UTF8.GetString(decryptedText);
                }
            }
        }
        #endregion

        #region-------------------------Random Key Generation -----------

        public static string generateRandomKey(int length)
        {
            const string alphanumericCharacters =
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                "abcdefghijklmnopqrstuvwxyz" +
                "0123456789";
            return GetRandomString(length, alphanumericCharacters);
        }
        static string GetRandomString(int length, IEnumerable<char> characterSet)
        {
            //if (length < 0)
            //    throw new ArgumentException("length must not be negative", "length");
            //if (length > int.MaxValue / 8) // 250 million chars ought to be enough for anybody
            //    throw new ArgumentException("length is too big", "length");
            //if (characterSet == null)
            //    throw new ArgumentNullException("characterSet");

            var characterArray = characterSet.Distinct().ToArray();

            //if (characterArray.Length == 0)
            //    throw new ArgumentException("characterSet must not be empty", "characterSet");

            var bytes = new byte[length * 8];
            new RNGCryptoServiceProvider().GetBytes(bytes);
            var result = new char[length];
            for (int i = 0; i < length; i++)
            {
                ulong value = BitConverter.ToUInt64(bytes, i * 8);
                result[i] = characterArray[value % (uint)characterArray.Length];
            }
            return new string(result);
        }

        #endregion

        


        

        


        


        

        

        

        


        //#region----------------------------------------machine check------------------------------

        //public static bool CheckMachine(string Machine)
        //{
        //    if ((String.IsNullOrEmpty(Machine)))
        //        WriteToLogFile.writeMessage("Machine Name Received null");


        //    bool Check = false;
        //    try
        //    {
        //        //WriteToLogFile.writeMessage("Check Machine [Started]");
                
        //        using (BNKModel db = new BNKModel())
        //        {
        //            //WriteToLogFile.writeMessage("Connection With DB Created");
        //            var mobj = db.tbl_MachineInfo.Where(x => x.MachineName.Equals(Machine)).FirstOrDefault();

        //            if (mobj == null)
        //            {
        //                //WriteToLogFile.writeMessage("No machine is present with this name");
        //                //WriteToLogFile.writeMessage("Creating New Machine Entry in DB");
        //                tbl_MachineInfo obj = new tbl_MachineInfo();
        //                obj.MachineName = Machine;
        //                obj.CreatedTs = DateTime.Now;
        //                obj.IsActive = true;
        //                ///WriteToLogFile.writeMessage("Adding Entry in Table");
        //                db.tbl_MachineInfo.Add(obj);
        //               // WriteToLogFile.writeMessage("Entry Added Successfully in Table");
        //                db.SaveChanges();
        //                //WriteToLogFile.writeMessage("Changes Saved Successfully");
        //                Check = true;
        //            }
        //            else if (mobj.IsActive == false)
        //            {
        //               // WriteToLogFile.writeMessage("Machine Already present but with is asctive as false");
        //                mobj.IsActive = true;
        //                mobj.CreatedTs = DateTime.Now;
        //                //WriteToLogFile.writeMessage("Saving Changes to DB");
        //                db.SaveChanges();
        //                //WriteToLogFile.writeMessage("Changes Saved Successfully");
        //                Check = true;
        //            }
        //            else
        //            {
        //                //WriteToLogFile.writeMessage("Machine Already present and with is asctive as True");
        //                //WriteToLogFile.writeMessage("Checking Time Difference");
        //                int TimeDiff = Convert.ToInt32(ConfigurationManager.AppSettings["TimeDiff"].ToString());
        //                //WriteToLogFile.writeMessage("Time Diff = "+TimeDiff.ToString());
        //                int diff = (DateTime.Now - Convert.ToDateTime(mobj.CreatedTs)).Minutes;
        //                ///WriteToLogFile.writeMessage("diff in minutes "+diff.ToString());
        //                if (diff >= TimeDiff)
        //                {
        //                   // WriteToLogFile.writeMessage("Machine Time out reached to maximun updating is active to true");
        //                    mobj.IsActive = true;
        //                    mobj.CreatedTs = DateTime.Now;
        //                    //WriteToLogFile.writeMessage("Saving Changes to DB");
        //                    db.SaveChanges();
        //                    //WriteToLogFile.writeMessage("Changes Saved Successfully");
        //                    Check = true;
        //                }
        //                else
        //                {
        //                   // WriteToLogFile.writeMessage("Cannot activate machine because time out not reached");
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception Ex)
        //    {
        //        //throw Ex;
        //        WriteToLogFile.writeMessage("Exception Occured While processing this method exception msg = " +Ex.Message.ToString());
        //    }
        //    //WriteToLogFile.writeMessage("Returned Check Value "+Check.ToString());
        //    return Check;
        //}

        //public static void MachineLogout(string MName)
        //{
        //    try
        //    {
        //       // WriteToLogFile.writeMessage("MachineLogout Method [Started]");
        //        using (BNKModel db = new BNKModel())
        //        {
        //            var obj = db.tbl_MachineInfo.Where(x => x.MachineName.Equals(MName)).FirstOrDefault();
                    
        //            obj.IsActive = false;
        //            db.SaveChanges();
        //        }
        //       // WriteToLogFile.writeMessage("MachineLogout Method [Ended] successfully");
        //    }
        //    catch (Exception Ex)
        //    {
        //        //WriteToLogFile.writeMessage("MachineLogout Method [Ended] UNsuccessfully Error = " +Ex.Message.ToString());
        //        throw Ex;
        //    }
        //}
        //#endregion
    }
}