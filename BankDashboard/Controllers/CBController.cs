using BankDashboard.CBModel;
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
                TempData["list"] = CBHelper.getRoutingPortalTable(Fromdate, Todate, Filter);
                TempData["filter"] = new CaseFilter() { Fromdate = Fromdate, Todate = Todate, Flag = Flag, Filter = Filter };
            }
            else if (Flag == "3")
            {
                obj.CaseReadyForAction = new List<string>() { "24", "35", "56", "21", "76" };
                TempData["list"] = CBHelper.GetCaseReadyTable();
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
        public ActionResult GetFilterDataWC(string Flag, string Todate, string Fromdate, string Filter)
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
        #endregion------------------------------------------------------------------------------

        #region---------------------------SLA-------------------------------------------
        public ActionResult SLA(SLAFilter obj, string find)
        {
            ViewBag.Dashboard = "show";
            ViewBag.SLAStat = "active";
            CBDB db = new CBDB();
            try
            {
                if (find != null)
                {
                    ViewBag.list = CBHelper.GetSla(obj);
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
        #endregion-----------------------------------------------------------------------

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


        #endregion--------------------------------------------------------------------------------------------------

        #region-------------------------------------Closure Report----------------------------------------
        public ActionResult ClosureReport(ClosureReportFilter filter, string find)
        {
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
            try
            {
                ViewBag.userlist = CBHelper.GetUsersForProfile();
            }
            catch{ }
            return View();
        }
        [HttpPost]
        public ActionResult UserManagement(string ID,string pages)
        {
            try
            {
                CBHelper.SaveUserPages(int.Parse(ID),pages);
                TempData["Success"] = "Data saved successfully.";
            }
            catch { TempData["Error"] = "Something went wrong."; }
            return RedirectToAction("UserManagement");
        }
        #endregion---------------------------------------------------------------------------------------

    }
}