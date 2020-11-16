using BankDashboard.CBModel;
using BankDashboard.Common;
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
        public ActionResult Index()
        {
            ViewBag.botstat = "active";
            return View();
        }
        #region-------------------------WeCare----------------------------------------------
        public ActionResult WC()
        {
            ViewBag.wcstat = "active";
            try
            {

            }
            catch(Exception ex)
            {
                TempData["Error"] = "Somthing went wrong.."+ex.Message;
            }
            return View();
        }
        #endregion------------------------------------------------------------------------------

        #region-------------------------Case History----------------------------------------------
        public ActionResult CaseHistory(FilterClass filter,string find)
        {
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
                    ViewBag.list = db.tbl_WeCareReactive.ToList();
                }            
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Somthing went wrong.." + ex.Message;
            }
            return View();
        }

        #endregion-------------------------------------------------------------------------------------------------

        #region-------------------------Matched Financial Transaction----------------------------------------------
        public ActionResult MatchedFinTransaction(FilterClass filter, string find)
        {
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