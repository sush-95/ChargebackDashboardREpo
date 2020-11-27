﻿using BankDashboard.CBModel;
using BankDashboard.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static BankDashboard.Common.ViewModelClass;

namespace BankDashboard.Controllers
{
    public class CBController : Controller
    {
        // GET: CB
        #region-------------------------Case Statistics----------------------------------------------
        public ActionResult Index()
        {
            ViewBag.Dashboard = "show";
            ViewBag.botstat = "active";
            BOtStatModel obj = new BOtStatModel();
            obj.castStatFigures = CBHelper.CaseForToday();
            obj.RoutingPortalFigures = CBHelper.GetRoutingPortalForToday();
            obj.CaseReadyForAction = CBHelper.GetCaseReadyForAction();
            ViewBag.casestat = obj;
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
                    }
                    else if (getval == "3")
                    {
                        obj.CaseReadyForAction = new List<string>() { "24", "35", "56", "21", "76" };
                    }
                    ViewBag.casestat = obj;
                }
            }
            catch { throw; }
            return View();
        }

        public ActionResult GetFilterData(string Flag, string Todate, string Fromdate, string Filter)
        {
            BOtStatModel obj = new BOtStatModel();
            obj.flag = Flag;
            if (Flag == "1")
            {
                obj.castStatFigures = CBHelper.CaseDataOnFilter(Fromdate, Todate);
                TempData["list"] = CBHelper.CaseDataTableOnFilter(Fromdate, Todate, Filter);
                TempData["filter"] = new CaseFilter() { Fromdate = Fromdate, Todate = Todate, Flag = Flag, Filter = Filter };
            }
            else if (Flag == "2")
            {
                obj.RoutingPortalFigures = CBHelper.GetRoutingPortalForToday();
            }
            else if (Flag == "3")
            {
                obj.CaseReadyForAction = new List<string>() { "24", "35", "56", "21", "76" };
            }
            TempData["caseObj"] = obj;
            return RedirectToAction("CaseViewFrom", new { getval = Flag });
        }
        public List<string> getpercentagefigure(List<string> casestatfigure)
        {
            List<string> list = new List<string>();
            long sum = 0;
            foreach (string item in casestatfigure)
            {
                sum += long.Parse(item);
            }
            foreach (string item in casestatfigure)
            {
                list.Add(((long.Parse(item) * 100 / sum)).ToString());
            }
            casestatfigure.AddRange(list);
            return casestatfigure;
        }
        #endregion-----------------------------------------------------------------------------------------
        #region-------------------------WeCare----------------------------------------------
        public ActionResult WC()
        {
            ViewBag.Dashboard = "show";
            ViewBag.wcstat = "active";
            try
            {
                WCStatModel obj = new WCStatModel();
                obj.WCCaseStatus = getpercentagefigure(new List<string>() { "23", "34", "54" });
                obj.Itypes = IssueTypeFigure();
                ViewBag.WCObj = obj;
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Somthing went wrong.." + ex.Message;
            }
            return View();
        }
        public IssueType IssueTypeFigure(int issue = 0)
        {

            List<string> name = new List<string>(), data = new List<string>(), backcolor = new List<string>(), bordercolor = new List<string>();
            IssueType obj = new IssueType();
            obj.Issuetypes = new List<string>() { "Amount not closed", "Amount Debited more than Once", "Card Captured", "Credit Card claims not Refunded",
                "Incorrect Amount Debited", "Partial amount received", "Payment to wrong School", "Request for Transfer Confirmation" };
            obj.Issuetypesfigures = new List<string>() { "23", "30", "42", "70", "53", "21", "65", "43" };
            if (issue == 0 && obj.Issuetypesfigures.Count > 5)
            {
                for (int i = 0; i < 5; i++)
                {
                    name.Add(obj.Issuetypes[i]);
                    data.Add(obj.Issuetypesfigures[i]);
                    backcolor.Add("rgba(241, 46, 35, 0.5)");
                    bordercolor.Add("rgba(241, 46, 35)");
                }
                obj.Issuetypes = name;
                obj.Issuetypesfigures = data;
                obj.IssuetypesBackColor = backcolor;
                obj.IssuetypesBordercolor = bordercolor;
            }
            else
            {
                for (int i = 0; i < obj.Issuetypesfigures.Count; i++)
                {
                    backcolor.Add("rgba(241, 46, 35, 0.5)");
                    bordercolor.Add("rgba(241, 46, 35)");
                }
                obj.IssuetypesBackColor = backcolor;
                obj.IssuetypesBordercolor = bordercolor;
            }
            return obj;
        }
        public ActionResult WCViewFrom(string getval)
        {
            ViewBag.Dashboard = "show";
            ViewBag.wcstat = "active";
            try
            {
                WCStatModel obj = new WCStatModel();
                obj.flag = getval;
                if (TempData["WCObj"] != null)
                {
                    ViewBag.casestat = TempData["WCObj"];
                    ViewBag.list = TempData["list"];
                }
                else
                {
                    if (getval == "1")
                    {
                        obj.WCCaseStatus = getpercentagefigure(new List<string>() { "23", "34", "54" });
                        // ViewBag.list = CBHelper.CaseTableDataForToday();
                    }
                    else if (getval == "2")
                    {
                        obj.Itypes = IssueTypeFigure(1);
                    }
                    ViewBag.WCstat = obj;
                }
            }
            catch { }
            return View();
        }
        #endregion------------------------------------------------------------------------------

        #region-------------------------Case History----------------------------------------------
        public ActionResult CaseHistory(FilterClass filter, string find)
        {
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
        public ActionResult MatchedFinTransaction(FilterClass filter, string find)
        {
            ViewBag.Report = "show";
            ViewBag.matchedTran = "active";
            try
            {
                CBDB db = new CBDB();
                if (find != null)
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

        #endregion------------------------------------------------------------------------------

        #region-------------------------Unmatched Financial Transaction----------------------------------------------
        public ActionResult UnmatchedFinTransaction(FilterClass filter, string find)
        {
            ViewBag.UnmatchedTran = "active";
            ViewBag.Report = "show";
            try
            {
                CBDB db = new CBDB();
                if (find != null)
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

        #endregion------------------------------------------------------------------------------
    }
}