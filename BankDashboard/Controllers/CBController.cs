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
            obj.castStatFigures = getpercentagefigure(new List<string>(){ "34", "34", "45", "9" });
            obj.RoutingPortalFigures = new List<string>() { "24", "35", "56", "21", "76" };
            ViewBag.casestat = obj;
            return View();
        }
        public List<string> getpercentagefigure(List<string> casestatfigure)
        {
            List<string> list =new List<string>();
            long sum = 0;
            foreach(string item in casestatfigure)
            {
                sum += long.Parse(item);
            }
            foreach(string item in casestatfigure)
            {
                list.Add(((long.Parse(item) * 100 / sum) ).ToString());
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
               
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Somthing went wrong.." + ex.Message;
            }
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