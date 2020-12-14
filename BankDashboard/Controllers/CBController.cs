using BankDashboard.CBModel;
using BankDashboard.Common;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Media.Animation;
using static BankDashboard.Common.ViewModelClass;

namespace BankDashboard.Controllers
{
    public class CBController : Controller
    {
        // GET: CB
        #region-------------------------Case Statistics----------------------------------------------
        public ActionResult Index()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("LogIn", "LogIn");
            }
            if (string.IsNullOrEmpty(((Tbl_User_Detail)Session["User"]).GroupPages) || !((Tbl_User_Detail)Session["User"]).GroupPages.Contains("CaseStat"))
            {
                return RedirectToAction("Errorpage", "CB");
            }
            try
            {
                ViewBag.Dashboard = "show";
                ViewBag.botstat = "active";
                BOtStatModel obj = new BOtStatModel();
                obj.castStatFigures = CBHelper.CaseForToday();
                obj.RoutingPortalFigures = CBHelper.GetRoutingPortalForToday();
                obj.CaseReadyForAction = CBHelper.GetCaseReadyForAction();
                ViewBag.casestat = obj;
            }
            catch { }
            return View();
        }

        public ActionResult CaseViewFrom(string getval)
        {
            ViewBag.Dashboard = "show";
            ViewBag.botstat = "active";
            try
            {
                BOtStatModel obj = new BOtStatModel();
                obj.flag = getval;
                if (TempData["caseObj"] != null)
                {
                    ViewBag.casestat = TempData["caseObj"];
                    ViewBag.list = TempData["list"];
                }
                else
                {
                    if (getval == "1")
                    {
                        obj.castStatFigures = CBHelper.CaseForToday();
                        ViewBag.list = CBHelper.CaseTableDataForToday();
                    }
                    else if (getval == "2")
                    {
                        obj.RoutingPortalFigures = CBHelper.GetRoutingPortalForToday();
                        ViewBag.list = CBHelper.getRoutingPortalTable();
                    }
                    else if (getval == "3")
                    {
                        obj.CaseReadyForAction = new List<string>() { "24", "35", "56", "21", "76" };
                        ViewBag.list = CBHelper.GetCaseReadyTable();
                    }
                    ViewBag.casestat = obj;
                }
            }
            catch { throw; }
            return View();
        }

        public ActionResult GetFilterData(string Flag, string Todate, string Fromdate, string Filter, string Excel)
        {
            BOtStatModel obj = new BOtStatModel();
            obj.flag = Flag;
            if (Flag == "1")
            {
                obj.castStatFigures = CBHelper.CaseDataOnFilter(Fromdate, Todate);
                List<tbl_UnassignedTickets> tabList = CBHelper.CaseDataTableOnFilter(Fromdate, Todate, Filter);
                TempData["list"] = tabList;
                TempData["filter"] = new CaseFilter() { Fromdate = Fromdate, Todate = Todate, Flag = Flag, Filter = Filter };
                if (Excel != null)
                {
                    FormattoExcelForcasestat(tabList, "Casestatusreport_" + DateTime.Now.ToString("ddMMyyyyhhmmss"));
                }
            }
            else if (Flag == "2")
            {
                obj.RoutingPortalFigures = CBHelper.GetRoutingPortalForToday();
                List<tbl_AuthCode> tblauth = CBHelper.getRoutingPortalTable(Fromdate, Todate, Filter);
                TempData["list"] = tblauth;
                TempData["filter"] = new CaseFilter() { Fromdate = Fromdate, Todate = Todate, Flag = Flag, Filter = Filter };
                if (Excel != null)
                {
                    FormattoExcelForRoutingPortal(tblauth, "RoutingportalReport_" + DateTime.Now.ToString("ddMMyyyyhhmmss"));
                }
            }
            else if (Flag == "3")
            {
                obj.CaseReadyForAction = new List<string>() { "24", "35", "56", "21", "76" };
                List<tbl_UnassignedTickets> tabList = CBHelper.GetCaseReadyTable();
                TempData["list"] = CBHelper.GetCaseReadyTable();
                if (Excel != null)
                {
                    FormattoExcelFaorcaseReadyForAction(tabList, "Casereadyforactionreport_" + DateTime.Now.ToString("ddMMyyyyhhmmss"));
                }
            }
            TempData["caseObj"] = obj;
            return RedirectToAction("CaseViewFrom", new { getval = Flag });
        }

        void FormattoExcelForcasestat(List<tbl_UnassignedTickets> p, string sname)
        {
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ClearContent();
            System.Web.HttpContext.Current.Response.ClearHeaders();
            System.Web.HttpContext.Current.Response.Buffer = true;
            System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            System.Web.HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + sname + ".xls"); /*" + sname + "*/

            System.Web.HttpContext.Current.Response.Charset = "utf-8";
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            //sets font
            System.Web.HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            System.Web.HttpContext.Current.Response.Write("<BR><BR><BR>");
            System.Web.HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " + "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
              "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");


            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Feedback Id");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Issue Type");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Status");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Reason");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Bot Entry Time");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");
            System.Web.HttpContext.Current.Response.Write("</Tr>");



            foreach (var pdata in p)
            {
                System.Web.HttpContext.Current.Response.Write("<TR>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.FeedbackId);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.Issue);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.Status);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.BotRemarks);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.BotDataEntryTime);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("</TR>");

            }
            System.Web.HttpContext.Current.Response.Write("</Table>");
            System.Web.HttpContext.Current.Response.Write("</font>");
            System.Web.HttpContext.Current.Response.Flush();
            System.Web.HttpContext.Current.Response.End();
            System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        void FormattoExcelForRoutingPortal(List<tbl_AuthCode> p, string sname)
        {
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ClearContent();
            System.Web.HttpContext.Current.Response.ClearHeaders();
            System.Web.HttpContext.Current.Response.Buffer = true;
            System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            System.Web.HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + sname + ".xls"); /*" + sname + "*/

            System.Web.HttpContext.Current.Response.Charset = "utf-8";
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            //sets font
            System.Web.HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            System.Web.HttpContext.Current.Response.Write("<BR><BR><BR>");
            System.Web.HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " + "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
              "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");


            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Feedback Id");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Auth Code");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Amount");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Routing Portal");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Card Number");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Status");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("</Tr>");



            foreach (var pdata in p)
            {
                System.Web.HttpContext.Current.Response.Write("<TR>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.FeedbackId);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.AuthCode);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.Status);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.Amount);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.Routing_Channel);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.CardNumber);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.Status);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("</TR>");

            }
            System.Web.HttpContext.Current.Response.Write("</Table>");
            System.Web.HttpContext.Current.Response.Write("</font>");
            System.Web.HttpContext.Current.Response.Flush();
            System.Web.HttpContext.Current.Response.End();
            System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        void FormattoExcelFaorcaseReadyForAction(List<tbl_UnassignedTickets> p, string sname)
        {
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ClearContent();
            System.Web.HttpContext.Current.Response.ClearHeaders();
            System.Web.HttpContext.Current.Response.Buffer = true;
            System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            System.Web.HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + sname + ".xls"); /*" + sname + "*/

            System.Web.HttpContext.Current.Response.Charset = "utf-8";
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            //sets font
            System.Web.HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            System.Web.HttpContext.Current.Response.Write("<BR><BR><BR>");
            System.Web.HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " + "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
              "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");


            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Feedback Id");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("CIF No.");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Customer Name");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Registration Date");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Card Number");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Status");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("</Tr>");



            foreach (var pdata in p)
            {
                System.Web.HttpContext.Current.Response.Write("<TR>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.FeedbackId);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.CIFNo);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.CustomerName);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.IncidentDate);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.CardNumber);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.Status);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("</TR>");

            }
            System.Web.HttpContext.Current.Response.Write("</Table>");
            System.Web.HttpContext.Current.Response.Write("</font>");
            System.Web.HttpContext.Current.Response.Flush();
            System.Web.HttpContext.Current.Response.End();
            System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        #endregion-----------------------------------------------------------------------------------------

        #region-------------------------WeCare----------------------------------------------
        public ActionResult WC()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("LogIn", "LogIn");
            }
            if (string.IsNullOrEmpty(((Tbl_User_Detail)Session["User"]).GroupPages) || !((Tbl_User_Detail)Session["User"]).GroupPages.Contains("WCStat"))
            {
                return RedirectToAction("Errorpage", "CB");
            }
            ViewBag.Dashboard = "show";
            ViewBag.wcarestat = "active";
            try
            {
                WCStatModel obj = new WCStatModel();
                List<tbl_WeCareReactive> tblWC = new List<tbl_WeCareReactive>();
                obj.WCCaseStatus = CBHelper.WCScaseStatusFigure(ref tblWC);
                obj.Itypes = CBHelper.GetListOfIssueTypes("", "", "");
                ViewBag.WCObj = obj;
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Somthing went wrong.." + ex.Message;
            }
            return View();
        }

        public ActionResult WCViewFrom(string getval)
        {
            ViewBag.Dashboard = "show";
            ViewBag.wcarestat = "active";
            try
            {
                WCStatModel obj = new WCStatModel();
                obj.flag = getval;
                if (TempData["WCObj"] != null)
                {
                    ViewBag.WCstat = TempData["WCObj"];
                    ViewBag.list = TempData["list"];
                }
                else
                {
                    if (getval == "1")
                    {
                        List<tbl_WeCareReactive> tblWC = new List<tbl_WeCareReactive>();
                        obj.WCCaseStatus = CBHelper.WCScaseStatusFigure(ref tblWC);
                        ViewBag.list = tblWC;
                    }
                    else if (getval == "2")
                    {
                        obj.Itypes = CBHelper.GetListOfIssueTypes("", "", "", 1);
                        CBDB db = new CBDB();
                        TempData["issuelist"] = db.tbl_WeCareReactive.Select(x => x.Issue).Distinct().ToList();
                    }
                    ViewBag.WCstat = obj;
                }
            }
            catch (Exception ex) { }
            return View();
        }

        public ActionResult GetFilterDataWC(string Flag, string Todate, string Fromdate, string Filter, string Excel)
        {
            try
            {
                WCStatModel obj = new WCStatModel();
                obj.flag = Flag;
                if (Flag == "1")
                {
                    List<tbl_WeCareReactive> tblWC = new List<tbl_WeCareReactive>();
                    obj.WCCaseStatus = CBHelper.WCScaseStatusFigure(ref tblWC, Fromdate, Todate, Filter);
                    TempData["list"] = tblWC;
                    TempData["filter"] = new CaseFilter() { Fromdate = Fromdate, Todate = Todate, Flag = Flag, Filter = Filter };
                    if (Excel != null)
                    {
                        FormattoExcelForWCCaseStat(tblWC, "WecareCaseStat_" + DateTime.Now.ToString("ddMMyyyyhhmmss"));
                    }
                }
                else if (Flag == "2")
                {
                    obj.Itypes = CBHelper.GetListOfIssueTypes(Fromdate, Todate, Filter, 1);
                    TempData["list"] = CBHelper.getRoutingPortalTable(Fromdate, Todate, Filter);
                    TempData["filter"] = new CaseFilter() { Fromdate = Fromdate, Todate = Todate, Flag = Flag, Filter = Filter };
                    CBDB db = new CBDB();
                    TempData["issuelist"] = db.tbl_WeCareReactive.Select(x => x.Issue).Distinct().ToList();
                }
                TempData["WCObj"] = obj;

            }
            catch { }
            return RedirectToAction("WCViewFrom", new { getval = Flag });
        }
        void FormattoExcelForWCCaseStat(List<tbl_WeCareReactive> p, string sname)
        {
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ClearContent();
            System.Web.HttpContext.Current.Response.ClearHeaders();
            System.Web.HttpContext.Current.Response.Buffer = true;
            System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            System.Web.HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + sname + ".xls"); /*" + sname + "*/

            System.Web.HttpContext.Current.Response.Charset = "utf-8";
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            //sets font
            System.Web.HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            System.Web.HttpContext.Current.Response.Write("<BR><BR><BR>");
            System.Web.HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " + "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
              "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");


            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Feedback Id");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Assigned UserID");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Registration Date");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Resolution Date");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Stage");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");
            System.Web.HttpContext.Current.Response.Write("</Tr>");



            foreach (var pdata in p)
            {
                System.Web.HttpContext.Current.Response.Write("<TR>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.FeedbackID);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.AssignedUserID);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.RegistrationDate);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.ResolutionDate);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.Stage);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("</TR>");

            }
            System.Web.HttpContext.Current.Response.Write("</Table>");
            System.Web.HttpContext.Current.Response.Write("</font>");
            System.Web.HttpContext.Current.Response.Flush();
            System.Web.HttpContext.Current.Response.End();
            System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        #endregion------------------------------------------------------------------------------

        #region---------------------------SLA-------------------------------------------
        public ActionResult SLA(SLAFilter obj, string find, string Excel)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("LogIn", "LogIn");
            }
            if (string.IsNullOrEmpty(((Tbl_User_Detail)Session["User"]).GroupPages) || !((Tbl_User_Detail)Session["User"]).GroupPages.Contains("SLA"))
            {
                return RedirectToAction("Errorpage", "CB");
            }
            ViewBag.Dashboard = "show";
            ViewBag.SLAStat = "active";
            CBDB db = new CBDB();
            try
            {
                if (find != null)
                {
                    ViewBag.list = CBHelper.GetSla(obj);
                }
                else if (Excel != null)
                {
                    List<tbl_WeCareReactive> wcList = CBHelper.GetSla(obj);
                    ViewBag.list = wcList;
                    FormattoExcelForSLA(wcList, "SLAReport_" + DateTime.Now.ToString("ddMMyyyyhhmmss"), obj);
                }
                else
                {
                    ViewBag.list = db.tbl_WeCareReactive.ToList();
                    obj = new SLAFilter() { SLADays = "75", CloseToSla = "10", SlACount = "", Filter = "" };

                }
                ViewBag.filterobj = obj;
                ViewBag.userlist = db.tbl_WeCareReactive.Select(x => x.AssignedUserID).Distinct().ToList();
            }
            catch { }
            return View();
        }
        void FormattoExcelForSLA(List<tbl_WeCareReactive> p, string sname, SLAFilter obj)
        {
            long sladays = 0, closetosla = 0, datediff = 0; DateTime today = DateTime.Today, datex = new DateTime(); int flag = 0;
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ClearContent();
            System.Web.HttpContext.Current.Response.ClearHeaders();
            System.Web.HttpContext.Current.Response.Buffer = true;
            System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            System.Web.HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + sname + ".xls"); /*" + sname + "*/

            System.Web.HttpContext.Current.Response.Charset = "utf-8";
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            //sets font
            System.Web.HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            System.Web.HttpContext.Current.Response.Write("<BR><BR><BR>");
            System.Web.HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " + "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
              "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");


            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Feedback Id");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Registration Date");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Assigned UserId");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("SLA Date");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Incident Date");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Days Left");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("</Tr>");

            sladays = string.IsNullOrEmpty(obj.SLADays.Trim()) ? 0 : long.Parse(obj.SLADays.Trim());
            closetosla = string.IsNullOrEmpty(obj.CloseToSla.Trim()) ? 0 : long.Parse(obj.CloseToSla.Trim());
            foreach (var pdata in p)
            {
                flag = 0;
                datex = Convert.ToDateTime(pdata.RegistrationDate).AddDays(sladays);
                if (datex < today && pdata.Stage.Equals("In Progress") || pdata.Stage.ToLower().Equals("InProgress".ToLower()))
                {
                    flag = 1;
                    datediff = (datex - today).Days;
                }
                else if (datex >= today && datex <= today.AddDays(closetosla) &&
                               pdata.Stage.Equals("In Progress") || pdata.Stage.ToLower().Equals("InProgress".ToLower()))
                {
                    flag = 2;
                    datediff = (datex - today).Days;
                }
                if (flag != 0)
                {
                    System.Web.HttpContext.Current.Response.Write("<TR>");

                    System.Web.HttpContext.Current.Response.Write("<Td>");

                    System.Web.HttpContext.Current.Response.Write(pdata.FeedbackID);

                    System.Web.HttpContext.Current.Response.Write("</Td>");

                    System.Web.HttpContext.Current.Response.Write("<Td>");

                    System.Web.HttpContext.Current.Response.Write(pdata.RegistrationDate);

                    System.Web.HttpContext.Current.Response.Write("</Td>");

                    System.Web.HttpContext.Current.Response.Write("<Td>");

                    System.Web.HttpContext.Current.Response.Write(pdata.AssignedUserID);

                    System.Web.HttpContext.Current.Response.Write("</Td>");

                    System.Web.HttpContext.Current.Response.Write("<Td>");

                    System.Web.HttpContext.Current.Response.Write(Convert.ToDateTime(pdata.RegistrationDate).AddDays(sladays));

                    System.Web.HttpContext.Current.Response.Write("</Td>");

                    System.Web.HttpContext.Current.Response.Write("<Td>");

                    System.Web.HttpContext.Current.Response.Write(pdata.IncidentDate);

                    System.Web.HttpContext.Current.Response.Write("</Td>");
                    if (flag == 1)
                    {
                        System.Web.HttpContext.Current.Response.Write("<Td style='color: red'>");
                    }
                    else
                    {
                        System.Web.HttpContext.Current.Response.Write("<Td style='color: green'>");
                    }

                    System.Web.HttpContext.Current.Response.Write(datediff);

                    System.Web.HttpContext.Current.Response.Write("</Td>");

                    System.Web.HttpContext.Current.Response.Write("</TR>");
                }
            }
            System.Web.HttpContext.Current.Response.Write("</Table>");
            System.Web.HttpContext.Current.Response.Write("</font>");
            System.Web.HttpContext.Current.Response.Flush();
            System.Web.HttpContext.Current.Response.End();
            System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        #endregion-----------------------------------------------------------------------

        #region-------------------------Case History----------------------------------------------
        public ActionResult CaseHistory(FilterClass filter, string find)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("LogIn", "LogIn");
            }
            if (string.IsNullOrEmpty(((Tbl_User_Detail)Session["User"]).GroupPages) || !((Tbl_User_Detail)Session["User"]).GroupPages.Contains("CaseHistory"))
            {
                return RedirectToAction("Errorpage", "CB");
            }
            ViewBag.Report = "show";
            ViewBag.casehistrory = "active";
            try
            {
                CBDB db = new CBDB();
                if (find != null)
                {
                    ViewBag.list = CBHelper.GetCaseHistoryFilterd(filter);
                    ViewBag.filter = filter;
                }
                else
                {
                    ViewBag.list = db.tbl_UnassignedTickets.ToList().OrderByDescending(x => x.BotDataEntryTime);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Somthing went wrong.." + ex.Message;
            }
            return View();
        }
        [HttpPost]
        public JsonResult AuthCode(string feedback)
        {
            List<tbl_AuthCode> list = new List<tbl_AuthCode>();
            try
            {
                CBDB db = new CBDB();
                list = db.tbl_AuthCode.Where(x => x.FeedbackId.Equals(feedback.Trim())).ToList();
            }
            catch { }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetExcel(string hfilter)
        {
            ViewModelClass.FilterClass filterobj = new ViewModelClass.FilterClass();
            try
            {
                List<tbl_UnassignedTickets> list = new List<tbl_UnassignedTickets>();
                if (!hfilter.Equals("null"))
                {
                    filterobj = JsonConvert.DeserializeObject<ViewModelClass.FilterClass>(hfilter);
                    list = CBHelper.GetCaseHistoryFilterd(filterobj);
                }
                else
                {
                    CBDB db = new CBDB();
                    list = db.tbl_UnassignedTickets.ToList();
                }
                FormattoExcel(list, "Report_" + DateTime.Now.ToString("ddMMyyyyHHmmss"));
            }
            catch
            {
                TempData["Error"] = "Something went wrong..!";
            }
            return RedirectToAction("Report", new { filter = filterobj, Apply = "" });
        }
        void FormattoExcel(List<tbl_UnassignedTickets> p, string sname)
        {


            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ClearContent();
            System.Web.HttpContext.Current.Response.ClearHeaders();
            System.Web.HttpContext.Current.Response.Buffer = true;
            System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            System.Web.HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + sname + ".xls"); /*" + sname + "*/

            System.Web.HttpContext.Current.Response.Charset = "utf-8";
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            //sets font
            System.Web.HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            System.Web.HttpContext.Current.Response.Write("<BR><BR><BR>");
            System.Web.HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " + "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
              "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");


            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Feedback Id");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Customer Name");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("CIF No");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Card Number");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Account Number");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Comments");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Incident Date");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Amount");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Banking With");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Category");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Sub Category");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Issue");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Bot Remarks");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Status");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("</Tr>");



            foreach (tbl_UnassignedTickets pdata in p)
            {
                System.Web.HttpContext.Current.Response.Write("<TR>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.FeedbackId);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.CustomerName);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.CIFNo);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.CardNumber);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.AccountNumber);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.Comments);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.IncidentDate);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.Amount);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.BankingWith);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.Category);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.SubCategory);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.Issue);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.BotRemarks);

                System.Web.HttpContext.Current.Response.Write("</Td>");


                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.Status);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("</TR>");

            }
            System.Web.HttpContext.Current.Response.Write("</Table>");
            System.Web.HttpContext.Current.Response.Write("</font>");
            System.Web.HttpContext.Current.Response.Flush();
            System.Web.HttpContext.Current.Response.End();
            System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        #endregion-------------------------------------------------------------------------------------------------

        #region-------------------------Matched Financial Transaction----------------------------------------------
        public ActionResult MatchedFinTransaction(FilterClass filter, string find, string excel)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("LogIn", "LogIn");
            }
            if (string.IsNullOrEmpty(((Tbl_User_Detail)Session["User"]).GroupPages) || !((Tbl_User_Detail)Session["User"]).GroupPages.Contains("MtchedTran"))
            {
                return RedirectToAction("Errorpage", "CB");
            }
            ViewBag.Report = "show";
            ViewBag.matchedTran = "active";
            try
            {
                CBDB db = new CBDB();
                if (excel != null)
                {
                    FormattoExcelForMathcedTran(CBHelper.GetMatchedTransaction(filter),"MatchedFinancialTransaction_" + DateTime.Now.ToString("ddMMyyyyhhmmss"));
                }
                else if (find != null)
                {
                    ViewBag.list = CBHelper.GetMatchedTransaction(filter);
                    ViewBag.filter = filter;
                }
                else
                {
                    ViewBag.list = db.Matched_FinancialTransaction.ToList();
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Somthing went wrong.." + ex.Message;
            }
            return View();
        }
        void FormattoExcelForMathcedTran(List<Matched_FinancialTransaction> p, string sname)
        {
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ClearContent();
            System.Web.HttpContext.Current.Response.ClearHeaders();
            System.Web.HttpContext.Current.Response.Buffer = true;
            System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            System.Web.HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + sname + ".xls"); /*" + sname + "*/

            System.Web.HttpContext.Current.Response.Charset = "utf-8";
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            //sets font
            System.Web.HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            System.Web.HttpContext.Current.Response.Write("<BR><BR><BR>");
            System.Web.HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " + "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
              "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");


            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Post Date");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Member Case");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Name");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Reference");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Card Number");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Debit");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Credit");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Status");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");            

            System.Web.HttpContext.Current.Response.Write("</TR>");


            foreach (var pdata in p)
            {
                System.Web.HttpContext.Current.Response.Write("<TR>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.PostDate);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.MemberCase);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.Name);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.Reference);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.CardNumber);

                System.Web.HttpContext.Current.Response.Write("</Td>");
                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.Debit);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.Credit);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.Status);

                System.Web.HttpContext.Current.Response.Write("</Td>");
               

                System.Web.HttpContext.Current.Response.Write("</TR>");

            }
            System.Web.HttpContext.Current.Response.Write("</Table>");
            System.Web.HttpContext.Current.Response.Write("</font>");
            System.Web.HttpContext.Current.Response.Flush();
            System.Web.HttpContext.Current.Response.End();
            System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        #endregion------------------------------------------------------------------------------

        #region-------------------------Unmatched Financial Transaction----------------------------------------------
        public ActionResult UnmatchedFinTransaction(FilterClass filter, string find, string excel)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("LogIn", "LogIn");
            }
            if (string.IsNullOrEmpty(((Tbl_User_Detail)Session["User"]).GroupPages) || !((Tbl_User_Detail)Session["User"]).GroupPages.Contains("UnmtchedTran"))
            {
                return RedirectToAction("Errorpage", "CB");
            }
            ViewBag.UnmatchedTran = "active";
            ViewBag.Report = "show";
            try
            {
                CBDB db = new CBDB();
                if (excel != null)
                {
                  FormattoExcelForUnMathcedTran(CBHelper.GetUnMatchedTransaction(filter), "UnmatchedFinancialTransaction_" + DateTime.Now.ToString("ddMMyyyyhhmmss"));
                }
                else if (find != null)
                {
                    ViewBag.list = CBHelper.GetUnMatchedTransaction(filter);
                    ViewBag.filter = filter;
                }
                else
                {
                    ViewBag.list = db.Unmatched_FinancialTransaction.ToList();
                }

            }
            catch (Exception ex)
            {
                TempData["Error"] = "Somthing went wrong.." + ex.Message;
            }
            return View();
        }
        void FormattoExcelForUnMathcedTran(List<Unmatched_FinancialTransaction> p, string sname)
        {
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ClearContent();
            System.Web.HttpContext.Current.Response.ClearHeaders();
            System.Web.HttpContext.Current.Response.Buffer = true;
            System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            System.Web.HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + sname + ".xls"); /*" + sname + "*/

            System.Web.HttpContext.Current.Response.Charset = "utf-8";
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            //sets font
            System.Web.HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            System.Web.HttpContext.Current.Response.Write("<BR><BR><BR>");
            System.Web.HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " + "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
              "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
    

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Member Case");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("File Schedule Date");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Status");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Bot Entry Date");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");            

            System.Web.HttpContext.Current.Response.Write("</TR>");


            foreach (var pdata in p)
            {
                System.Web.HttpContext.Current.Response.Write("<TR>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.MemberCase);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.FileScheduleDate);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.Status);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.BotEntryTime);

                System.Web.HttpContext.Current.Response.Write("</Td>");                


                System.Web.HttpContext.Current.Response.Write("</TR>");

            }
            System.Web.HttpContext.Current.Response.Write("</Table>");
            System.Web.HttpContext.Current.Response.Write("</font>");
            System.Web.HttpContext.Current.Response.Flush();
            System.Web.HttpContext.Current.Response.End();
            System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        #endregion------------------------------------------------------------------------------

        #region---------------------------------------Reconciliation----------------------------------------------


        public ActionResult AddRowReconsiliationTable(NonCustom_GLReconciliationTable obj, string submit)
        {
            ViewBag.Report = "show";
            ViewBag.Reconsiliation = "active";
            if (submit != null && obj != null)
            {
                CBHelper.AddRow_NonCustom_GLReconciliationTable(obj);
                return RedirectToAction("ReconsiliationReport");
            }
            return View();

        }
        public ActionResult ReconsiliationReport(ReconciliationFilter filter, string find, string removeAll, string remove)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("LogIn", "LogIn");
            }
            if (string.IsNullOrEmpty(((Tbl_User_Detail)Session["User"]).GroupPages) || !((Tbl_User_Detail)Session["User"]).GroupPages.Contains("Recon"))
            {
                return RedirectToAction("Errorpage", "CB");
            }
            ViewBag.Report = "show";
            ViewBag.Reconsiliation = "active";
            try
            {
                CBDB db = new CBDB();
                if (find != null)
                {
                    ViewBag.list = CBHelper.GetReconsiledReportFilterd(filter);
                    ViewBag.filter = filter;
                }
                else if (removeAll != null)
                {

                    CBHelper.MarkAllRecordsAsInActive();
                    ViewBag.list = CBHelper.GetReconsiledReportFilterd(new ReconciliationFilter { CardNumber = "", MemberCaseNumber = "" });
                }
                else if (remove != null)
                {
                    CBHelper.MarkRecordsAsInActive(remove);
                    ViewBag.list = CBHelper.GetReconsiledReportFilterd(new ReconciliationFilter { CardNumber = "", MemberCaseNumber = "" });
                }
                else
                {
                    CBHelper.ReconsiledReport();
                    ViewBag.list = CBHelper.GetReconsiledReportFilterd(new ReconciliationFilter { CardNumber = "", MemberCaseNumber = "" });
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Somthing went wrong.." + ex.Message;
            }
            return View();

        }
        public ActionResult GetExcel_ReconciliationReport(string hfilter)
        {
            ViewModelClass.ReconciliationFilter filterobj = new ViewModelClass.ReconciliationFilter();
            try
            {
                List<NonCustom_GLReconciliationTable> list = new List<NonCustom_GLReconciliationTable>();
                if (!hfilter.Equals("null"))
                {
                    filterobj = JsonConvert.DeserializeObject<ViewModelClass.ReconciliationFilter>(hfilter);
                    list = CBHelper.GetReconsiledReportFilterd(filterobj).ToList();
                }
                else
                {

                    list = CBHelper.GetReconsiledReportFilterd(new ReconciliationFilter { CardNumber = "", MemberCaseNumber = "" }).ToList();
                }
                FormattoExcel_ReconsiliationReport(list, "ReconsiliationReport_" + DateTime.Now.ToString("ddMMyyyyHHmmss"));
            }
            catch
            {
                TempData["Error"] = "Something went wrong..!";
            }
            return RedirectToAction("ReconsiliationReport", new { filter = filterobj });
            //return RedirectToAction("Report", new { filter = filterobj, Apply = "" });
        }
        void FormattoExcel_ReconsiliationReport(List<NonCustom_GLReconciliationTable> p, string sname)
        {


            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ClearContent();
            System.Web.HttpContext.Current.Response.ClearHeaders();
            System.Web.HttpContext.Current.Response.Buffer = true;
            System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            System.Web.HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + sname + ".xls"); /*" + sname + "*/

            System.Web.HttpContext.Current.Response.Charset = "utf-8";
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            //sets font
            System.Web.HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            System.Web.HttpContext.Current.Response.Write("<BR><BR><BR>");
            System.Web.HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " + "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
              "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");


            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Post Date");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Member Case");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Card Number");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Debit");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Credit");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Year");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Comments");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");



            System.Web.HttpContext.Current.Response.Write("</Tr>");



            foreach (NonCustom_GLReconciliationTable pdata in p)
            {
                System.Web.HttpContext.Current.Response.Write("<TR>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.PostDate);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.MemberCase);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.CardNumber);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.Debit);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.Credit);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.Year);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.Comments);

                System.Web.HttpContext.Current.Response.Write("</Td>");


                System.Web.HttpContext.Current.Response.Write("</TR>");

            }
            System.Web.HttpContext.Current.Response.Write("</Table>");
            System.Web.HttpContext.Current.Response.Write("</font>");
            System.Web.HttpContext.Current.Response.Flush();
            System.Web.HttpContext.Current.Response.End();
            System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        public ActionResult UploadExcelFile(HttpPostedFileBase[] UploadedFiles, string SubmitFile)
        {
            string FileUploadMessage,SaveFolderLocation;
            if (Session["User"] == null)
            {
                return RedirectToAction("LogIn", "LogIn");
            }
            if (string.IsNullOrEmpty(((Tbl_User_Detail)Session["User"]).GroupPages) || !((Tbl_User_Detail)Session["User"]).GroupPages.Contains("Recon"))
            {
                return RedirectToAction("Errorpage", "CB");
            }
            ViewBag.Report = "show";
            ViewBag.Reconsiliation = "active";

            if ((UploadedFiles != null))
            {
                string filenamePrefix = "Reconsiliation_UserUpload";
                if (!string.IsNullOrEmpty(SubmitFile))
                {
                    filenamePrefix = ConfigurationManager.AppSettings["ReconsiliationFileNamePrefix"].ToString();
                }
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["SaveFolderLocationReconcilliationFile"].ToString()))
                    SaveFolderLocation = string.Concat(@"~/", ConfigurationManager.AppSettings["SaveFolderLocationReconcilliationFile"].ToString());
                else
                    SaveFolderLocation = @"~/ImagesScreens/";

                if (UploadedFiles[0].FileName.EndsWith(".XLSX") || UploadedFiles[0].FileName.EndsWith(".xlsx"))
                {
                    string fileSavedname = filenamePrefix + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_.xlsx";
                    
                    string filepath = Path.Combine(Server.MapPath(SaveFolderLocation), fileSavedname);
                    UploadedFiles[0].SaveAs(filepath);
                    int status = READExcel(filepath, out FileUploadMessage);
                    if(status == 1)
                    {
                        TempData["SuccessMessage"] = FileUploadMessage;
                    }
                    else if(status == 0)
                    {
                        TempData["ErrorMessage"] = FileUploadMessage;
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Select .xlsx files only!";
                }
            }            
            return RedirectToAction("ReconsiliationReport");
        }
        public static int READExcel(string path, out string returnMessage)
        {
            int returnStatus =1;
            returnMessage = "Data Uploaded Successfully";
            CBDB db = new CBDB();
            //Instance reference for Excel Application
            Microsoft.Office.Interop.Excel.Application objXL = null;
            //Workbook refrence
            Microsoft.Office.Interop.Excel.Workbook objWB = null;
            DataSet ds = new DataSet();
            try
            {
                //Instancing Excel using COM services
                objXL = new Microsoft.Office.Interop.Excel.Application();
                //Adding WorkBook
                objWB = objXL.Workbooks.Open(path);
                foreach (Microsoft.Office.Interop.Excel.Worksheet objSHT in objWB.Worksheets)
                {
                    int rows = objSHT.UsedRange.Rows.Count;
                    int cols = objSHT.UsedRange.Columns.Count;
                    DataTable dt = new DataTable();
                    int noofrow = 1;
                    int dateColIndex = 0, descriptionColIndex = 0,  referenceColIndex = 0, debitColIndex = 0, creditColIndex = 0, cardNumberColIndex =0;
                    int yearColIndex = 0, commentsColIndex=0;
                    //If 1st Row Contains unique Headers for datatable include this part else remove it
                    //Start
                    for (int c = 1; c <= cols; c++)
                    {
                        string colname = objSHT.Cells[1, c].Text;
                        //dt.Columns.Add(colname);
                        noofrow = 2;

                        if (dateColIndex <= 0)
                            dateColIndex = (!string.IsNullOrWhiteSpace(colname) && (colname.ToLower().ToString().Equals("date"))) ? c : -1;

                        if (descriptionColIndex <= 0)
                            descriptionColIndex = (!string.IsNullOrWhiteSpace(colname) && (colname.ToLower().ToString().Equals("description"))) ? c : -1;

                        if (referenceColIndex <= 0)
                            referenceColIndex = (!string.IsNullOrWhiteSpace(colname) && (colname.ToLower().ToString().Equals("reference"))) ? c : -1;

                        if (debitColIndex <= 0)
                            debitColIndex = (!string.IsNullOrWhiteSpace(colname) && (colname.ToLower().ToString().Equals("debit"))) ? c : -1;

                        if (creditColIndex <= 0)
                            creditColIndex = (!string.IsNullOrWhiteSpace(colname) && (colname.ToLower().ToString().Equals("credit"))) ? c : -1;

                        if (cardNumberColIndex <= 0)
                            cardNumberColIndex = (!string.IsNullOrWhiteSpace(colname) && (colname.ToLower().ToString().Equals("cardnumber"))) ? c : -1;

                        if (yearColIndex <= 0)
                            yearColIndex = (!string.IsNullOrWhiteSpace(colname) && (colname.ToLower().ToString().Equals("year"))) ? c : -1;

                        if (commentsColIndex <= 0)
                            commentsColIndex = (!string.IsNullOrWhiteSpace(colname) && (colname.ToLower().ToString().Equals("comments"))) ? c : -1;

                    }
                    //END
                    if((debitColIndex > 0 || creditColIndex > 0) && cardNumberColIndex > 0)
                    {
                        for (int r = noofrow; r <= rows; r++)
                        {
                            NonCustom_GLReconciliationTable obj = new NonCustom_GLReconciliationTable();
                            if (dateColIndex > 0 && !string.IsNullOrEmpty(objSHT.Cells[r, dateColIndex].Text))
                            {
                                double d = double.Parse(objSHT.Cells[r, dateColIndex].Value2.ToString());
                                DateTime conv = DateTime.FromOADate(d);
                                obj.PostDate = conv.ToShortDateString();
                            }
                            if (descriptionColIndex > 0 && !string.IsNullOrEmpty(objSHT.Cells[r, descriptionColIndex].Text))
                                obj.MemberCase = objSHT.Cells[r, descriptionColIndex].Value2.ToString();

                            if (referenceColIndex > 0 && !string.IsNullOrEmpty(objSHT.Cells[r, referenceColIndex].Text))
                                obj.Reference = objSHT.Cells[r, referenceColIndex].Value2.ToString();

                            if (debitColIndex > 0 && !string.IsNullOrEmpty(objSHT.Cells[r, debitColIndex].Text))
                                obj.Debit = objSHT.Cells[r, debitColIndex].Value2.ToString();

                            if (creditColIndex > 0 && !string.IsNullOrEmpty(objSHT.Cells[r, creditColIndex].Text))
                                obj.Credit = objSHT.Cells[r, creditColIndex].Value2.ToString();

                            if (cardNumberColIndex > 0 && !string.IsNullOrEmpty(objSHT.Cells[r, cardNumberColIndex].Text))
                                obj.CardNumber = objSHT.Cells[r, cardNumberColIndex].Value2.ToString().Replace("-","");

                            if (yearColIndex > 0 && !string.IsNullOrEmpty(objSHT.Cells[r, yearColIndex].Text))
                                obj.Year = objSHT.Cells[r, yearColIndex].Value2.ToString();

                            if (commentsColIndex > 0 && !string.IsNullOrEmpty(objSHT.Cells[r, commentsColIndex].Text))
                                obj.Comments = objSHT.Cells[r, commentsColIndex].Value2.ToString();

                            obj.IsActive = true;
                            db.NonCustom_GLReconciliationTable.Add(obj);

                            //DataRow dr = dt.NewRow();
                            //for (int c = 1; c <= cols; c++)
                            //{
                            //    dr[c - 1] = objSHT.Cells[r, c].Text;
                            //}
                            //dt.Rows.Add(dr);
                        }
                        
                        db.SaveChanges();
                    }
                    else
                    {
                        returnMessage = "Missing required columns Debit, Credit or CardNumber";
                        returnStatus = 0;
                    }

                    //ds.Tables.Add(dt);
                }
                
                objWB.Close();
                objXL.Quit();
            }
            catch (Exception ex)
            {
                objWB.Saved = true;
                objWB.Close();
                objXL.Quit();
                returnMessage = "Error Occured While uploading excel - "+ex.Message;
                returnStatus = 0;
            }
            
            return returnStatus;
        }



        #endregion--------------------------------------------------------------------------------------------------

        #region-------------------------------------Closure Report----------------------------------------
        public ActionResult ClosureReport(ClosureReportFilter filter, string find)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("LogIn", "LogIn");
            }
            if (string.IsNullOrEmpty(((Tbl_User_Detail)Session["User"]).GroupPages) || !((Tbl_User_Detail)Session["User"]).GroupPages.Contains("CaseClosure"))
            {
                return RedirectToAction("Errorpage", "CB");
            }
            ViewBag.Report = "show";
            ViewBag.ClosureReport = "active";
            try
            {
                CBDB db = new CBDB();
                if (find != null && filter != null)
                {
                    ViewBag.list = CBHelper.AcceptedCaseClosureReportFilter(filter);
                    ViewBag.filter = filter;
                }
                else
                {
                    ViewBag.list = CBHelper.AcceptedCaseClosureReportDefaultData();
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Somthing went wrong.." + ex.Message;
            }


            return View();
        }
        public ActionResult GetExcel_AcceptedCaseClosureReport(string hfilter)
        {
            ViewModelClass.ClosureReportFilter filterobj = new ViewModelClass.ClosureReportFilter();
            try
            {
                List<tbl_IssuingIncomingVISA> list = new List<tbl_IssuingIncomingVISA>();
                if (!hfilter.Equals("null"))
                {
                    filterobj = JsonConvert.DeserializeObject<ViewModelClass.ClosureReportFilter>(hfilter);
                    list = CBHelper.AcceptedCaseClosureReportFilter(filterobj).ToList();
                }
                else
                {

                    list = CBHelper.AcceptedCaseClosureReportDefaultData().ToList();
                }
                FormattoExcel_AcceptedCaseClosureReport(list, "AcceptedCaseClosureReport_" + DateTime.Now.ToString("ddMMyyyyHHmmss"));
            }
            catch
            {
                TempData["Error"] = "Something went wrong..!";
            }
            return RedirectToAction("ReconsiliationReport", new { filter = filterobj });
            //return RedirectToAction("Report", new { filter = filterobj, Apply = "" });
        }
        void FormattoExcel_AcceptedCaseClosureReport(List<tbl_IssuingIncomingVISA> p, string sname)
        {


            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ClearContent();
            System.Web.HttpContext.Current.Response.ClearHeaders();
            System.Web.HttpContext.Current.Response.Buffer = true;
            System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            System.Web.HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + sname + ".xls"); /*" + sname + "*/

            System.Web.HttpContext.Current.Response.Charset = "utf-8";
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            //sets font
            System.Web.HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            System.Web.HttpContext.Current.Response.Write("<BR><BR><BR>");
            System.Web.HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " + "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
              "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");


            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Feedback Id");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("ROL Case No.");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Financial CPD");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("ARN Number");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Message Type");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Case Closure Report");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");





            System.Web.HttpContext.Current.Response.Write("</Tr>");



            foreach (tbl_IssuingIncomingVISA pdata in p)
            {
                System.Web.HttpContext.Current.Response.Write("<TR>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write("");

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.ROLCaseNo);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.FinancialCPD);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write("");

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.MessageType);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.CaseClosureStatus);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("</TR>");

            }
            System.Web.HttpContext.Current.Response.Write("</Table>");
            System.Web.HttpContext.Current.Response.Write("</font>");
            System.Web.HttpContext.Current.Response.Flush();
            System.Web.HttpContext.Current.Response.End();
            System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        #endregion----------------------------------------------------------------------------------------

        #region-----------------------------------User Management--------------------------------------
        public ActionResult UserManagement()
        {
            ViewBag.Dashboard = "show";
            ViewBag.userManagement = "active";
            try
            {
                ViewBag.userlist = CBHelper.GetUsersForProfile();
            }
            catch { }
            return View();
        }
        [HttpPost]
        public ActionResult UserManagement(string ID, string pages)
        {
            try
            {
                CBHelper.SaveUserPages(int.Parse(ID), pages);
                TempData["Success"] = "Data saved successfully.";
            }
            catch { TempData["Error"] = "Something went wrong."; }
            return RedirectToAction("UserManagement");
        }
        #endregion---------------------------------------------------------------------------------------

        public ActionResult Errorpage()
        {
            return View();
        }

        #region ------------------------------------Bot Config------------------------------------
        public ActionResult BotConfig()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("LogIn", "LogIn");
            }
            if (string.IsNullOrEmpty(((Tbl_User_Detail)Session["User"]).GroupPages) || !((Tbl_User_Detail)Session["User"]).GroupPages.Contains("RobotConfig"))
            {
                return RedirectToAction("Errorpage", "CB");
            }
            ViewBag.Report = "show";
            ViewBag.botconfig = "active";
            try
            {
                CBDB db = new CBDB();
                ViewBag.list = db.Robot_Config.ToList();
            }
            catch { }
            return View();
        }
        [HttpPost]
        public ActionResult BotConfig(Robot_Config filter)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("LogIn", "LogIn");
            }
            try
            {
                CBDB db = new CBDB();
                if (filter.Id != 0)//edit............
                {
                    var tabl = db.Robot_Config.Where(x => x.Id == filter.Id).FirstOrDefault();
                    tabl.Name = filter.Name;
                    tabl.Value = filter.Value;
                    tabl.Description = filter.Description;
                    tabl.UpdatedBy = (Session["User"] != null ? ((Tbl_User_Detail)Session["User"]).UserName : "");
                    tabl.UpdatedTime = DateTime.Now;
                    db.SaveChanges();
                }
                else
                {
                    filter.AddedBy = (Session["User"] != null ? ((Tbl_User_Detail)Session["User"]).UserName : "");
                    filter.AddedTime = DateTime.Now;
                    db.Robot_Config.Add(filter);
                    db.SaveChanges();
                }
                TempData["Success"] = "Data saved successfully..";
            }
            catch { }
            return RedirectToAction("BotConfig");
        }
        public ActionResult GetExcelForRobtConfig()
        {
            try
            {
                CBDB db = new CBDB();
                FormattoExcelForRobotConfiig(db.Robot_Config.ToList(), "RobotConfig_" + DateTime.Now.ToString("ddMMyyyyhhmmss"));
            }
            catch { TempData["Error"] = "Something went wrong.!"; }
            return RedirectToAction("BotConfig");
        }
        void FormattoExcelForRobotConfiig(List<Robot_Config> p, string sname)
        {
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ClearContent();
            System.Web.HttpContext.Current.Response.ClearHeaders();
            System.Web.HttpContext.Current.Response.Buffer = true;
            System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            System.Web.HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + sname + ".xls"); /*" + sname + "*/

            System.Web.HttpContext.Current.Response.Charset = "utf-8";
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            //sets font
            System.Web.HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            System.Web.HttpContext.Current.Response.Write("<BR><BR><BR>");
            System.Web.HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " + "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
              "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");


            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Name");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Value");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Desc");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");


            foreach (var pdata in p)
            {
                System.Web.HttpContext.Current.Response.Write("<TR>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.Name);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.Value);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.Description);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("</TR>");

            }
            System.Web.HttpContext.Current.Response.Write("</Table>");
            System.Web.HttpContext.Current.Response.Write("</font>");
            System.Web.HttpContext.Current.Response.Flush();
            System.Web.HttpContext.Current.Response.End();
            System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        #endregion------------------------------------------------------------------------------------
    }
}