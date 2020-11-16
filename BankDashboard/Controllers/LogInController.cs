using BankDashboard.Common;


using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BankDashboard.LogFile;
using System.Text.RegularExpressions;
using BankDashboard.ModelFd;

namespace BankDashboard.Controllers
{
    public class LogInController : Controller
    {
        // GET: LogIn
        public ActionResult LogIn()
        {

            string ErrorMsg = string.Empty; string Action = string.Empty; string cntrlr = string.Empty, groupname = string.Empty;
            if (Session["User"] != null)
            {
                var userSession = ((Tbl_User_Detail)Session["User"]);

                return RedirectToAction("OverView", "FD");
            }
            else
            {
                Session["LoginKey"] = MvcHelper.generateRandomKey(31).ToUpper() + "=";
                TempData["keyData"] = Session["LoginKey"];
            }
            return View();
        }
        [HttpPost]
        public ActionResult LogIn(string uname, string pwd)
        {
            if (string.IsNullOrEmpty(pwd))
                return RedirectToAction("LogIn", "LogIn");
            if (Session["LoginKey"] == null)
            {

            }

            pwd = pwd.Trim();
            bool logincheck = false;
            string key = Session["LoginKey"].ToString();

            pwd = MvcHelper.DecryptToBytesUsingCBC(Convert.FromBase64String(pwd), key);


            pwd = pwd.Trim();


            string ErrorMsg = string.Empty; string Action = string.Empty; string cntrlr = string.Empty, groupname = string.Empty;

            Tbl_User_Detail user = new Tbl_User_Detail();
           
            //ADManager AdObj = new ADManager();
            //logincheck = AdObj.ChcekLogin(uname, pwd, ref groupname);


            user = FDHelper.GetUser(uname, pwd);

            if (user != null)
            {
                logincheck = true;
                groupname = user.Usergroup;
            }

            if (logincheck)
            {
                if (!string.IsNullOrEmpty(groupname))
                {
                    user = new Tbl_User_Detail()
                    {
                        UserName = uname,
                        Usergroup = groupname,
                    };
                     // user.GroupPages = MvcHelper.GetGroupPages(groupname);


                    bool check = FDHelper.CheckMachine(user.UserName.Trim().ToString());


                    if (check)
                    {
                        Session["USerName"] = user.UserName.Trim().ToString();
                        FDHelper.SaveUser(user);
                        Action = "OverView"; cntrlr = "FD";
                        //WriteToLogFile.writeMessage("Machine is available User will be Redirected to -- Action = index cntrlr = Dashboard");
                    }
                    else
                    {
                        //WriteToLogFile.writeMessage("Sorry...!!! Multiple user login is not allowed.");
                        ErrorMsg = "Sorry...!!! Multiple user login is not allowed.";
                    }
                }
                else
                {
                    //WriteToLogFile.writeMessage("User type is not authorized for the dashboard...!");

                    ErrorMsg = "User type is not authorized for the dashboard...!";
                }
            }
            else
            {
                //WriteToLogFile.writeMessage("Wrong credential...!");
                ErrorMsg = "Wrong credential...!";
            }

            if (!string.IsNullOrEmpty(ErrorMsg))
            {
                //WriteToLogFile.writeMessage("Error msg" +ErrorMsg +"Redirecting to [GET] login page");
                TempData["invalidmsg"] = ErrorMsg;

                return RedirectToAction("LogIn");
            }
            else
            {
                Session["User"] = user;
                //WriteToLogFile.writeMessage("[HttpPost]Login  Ended");
                //WriteToLogFile.writeMessage("Redirecting to -- Action = +"+Action.ToString() +" cntrlr = " + cntrlr.ToString());


                return RedirectToAction(Action, cntrlr);
            }
        }

        public JsonResult EncryptPassword(string pwd)
        {

            //WriteToLogFile.writeMessage("EncryptedPassword [Started]");

            string EncryptKey = Convert.ToString(ConfigurationManager.AppSettings["DecryptKey"]);

            //WriteToLogFile.writeMessage("EncryptKey = "+ EncryptKey.ToString() );

            string encrypteddata = FDHelper.CodeEncrypt(pwd.Trim(), EncryptKey.Trim());

            //WriteToLogFile.writeMessage("encrypteddata = " + encrypteddata.ToString());

            //WriteToLogFile.writeMessage("EncryptedPassword [Ended]");

            return Json(encrypteddata, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Logout()
        {
            ////UserTransactionData utobj = new UserTransactionData();
            ////utobj.MachineLogout(Session["Machine"].ToString());
            //WriteToLogFile.writeMessage("Logout [Started] " +"Machine id ="+ ((Session["USerName"] == null)?"null": Session["USerName"].ToString()));

            ////Session["USerName"]
            ////if (Session["Machine"] != null)
            if (Session["USerName"] != null)
            {
                FDHelper.MachineLogout(Session["USerName"].ToString());
            }
            Session.Abandon();
            try
            {
                //WriteToLogFile.writeMessage("set ASP.NET_SessionID to null");
                Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
                //WriteToLogFile.writeMessage("Successfully Generated New ID");
            }
            catch (Exception e)
            {
                //WriteToLogFile.writeMessage("Error Occured While Generating New ID ERROR = " + e.Message.ToString());

            }
            //WriteToLogFile.writeMessage("Logout [Ended]");
            return RedirectToAction("LogIn");
        }
    }
}