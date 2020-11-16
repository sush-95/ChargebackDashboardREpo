using BankDashboard.Common;
using BankDashboard.ModelFd;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace BankDashboard.Controllers
{
    public class FDController : Controller
    {
        // GET: FD
        public ActionResult OverView()
        {
            ViewBag.OverView = "active";
            try
            {
                ViewBag.couning = getCountingFigures();
                ViewBag.realtime = GetListofStatus(true);
                ViewBag.historical = GetListofStatus(false);
            }
            catch (Exception ex)
            { throw ex; }
            return View();
        }
        public FDViewModelClass.ChartInfo GetListofStatus(bool isRealtime)
        {
            DBFD db = new DBFD();
            FDViewModelClass.ChartInfo chartobj = new FDViewModelClass.ChartInfo();
            List<FDRequest> fdList = db.FDRequests.ToList();
            List<long> list = new List<long>();
            if (isRealtime)
            {
                FDRequest fdobj = db.FDRequests.OrderByDescending(x => x.BOTEntryDate).First();
                DateTime todate = Convert.ToDateTime(fdobj.BOTEntryDate);
                DateTime stdate = todate.AddHours(-24);
                chartobj.YTD = todate.ToString("dd-MMM");
                fdList = fdList.Where(x => x.BOTEntryDate <= todate && x.BOTEntryDate >= stdate).ToList();
                list.Add(fdList.Where(x => x.FDCreated.Replace(" ", "").Equals("ValidFD-Created")).ToList().Count());
                list.Add(fdList.Where(x => x.MailSent.Replace(" ", "").Equals("ValidFD-MailSent")).ToList().Count());
                list.Add(fdList.Where(x => x.SMSSent.Replace(" ", "").Equals("ValidFD-SMSCreated")).ToList().Count());
                list.Add(fdList.Where(x => x.MarkedStatusInMB.Replace(" ", "").Equals("ValidFD-StatusMarked")).ToList().Count());
                list.Add(4);
                chartobj.CartFigures = list;
            }
            else
            {
                FDRequest fdobj = fdList.OrderByDescending(x => x.BOTEntryDate).LastOrDefault();
                DateTime todate = Convert.ToDateTime(fdobj.BOTEntryDate);
                chartobj.YTD = todate.ToString("dd-MMM");
                list.Add(fdList.Where(x => x.FDCreated.Replace(" ", "").Equals("ValidFD-Created")).ToList().Count());
                list.Add(fdList.Where(x => x.MailSent.Replace(" ", "").Equals("ValidFD-MailSent")).ToList().Count());
                list.Add(fdList.Where(x => x.SMSSent.Replace(" ", "").Equals("ValidFD-SMSCreated")).ToList().Count());
                list.Add(fdList.Where(x => x.MarkedStatusInMB.Replace(" ", "").Equals("ValidFD-StatusMarked")).ToList().Count());
                list.Add(4);
                chartobj.CartFigures = list;
            }
            return chartobj;
        }
        public FDViewModelClass.CountingInfo getCountingFigures()
        {
            FDViewModelClass.CountingInfo obj = new FDViewModelClass.CountingInfo();
            DBFD db = new DBFD();
            List<BOT_Status> botstats = db.BOT_Status.Where(x => x.EndTime != null).ToList();
            List<FDRequest> Fdlist = db.FDRequests.ToList();
            obj.Requested = Fdlist.Count().ToString();
            obj.Processed = Fdlist.Where(x => x.FDCreated != null && x.MailSent != null && x.SMSSent != null && x.MarkedStatusInMB != null).ToList().Count().ToString();
            long totalbothour = 0;
            foreach (var item in botstats)
            {
                totalbothour += (Convert.ToDateTime(item.EndTime) - Convert.ToDateTime(item.StartTime)).Minutes;
            }
            long totalmanhour = Convert.ToInt64(obj.Processed) * 10;
            obj.Manhoursaved = Convert.ToDecimal((totalmanhour - totalbothour) / 60).ToString();
            obj.date = Convert.ToDateTime(Fdlist.Last().BOTEntryDate).ToString("dd-MMM");
            return obj;
        }

        #region---------------------------------Report-----------------------------------------------------
        public ActionResult Report(FDViewModelClass.ReportView filter, string Apply)
        {
            ViewBag.report = "active";
            try
            {
                using (DBFD db = new DBFD())
                {
                    if (Apply != null)
                    {
                        ViewBag.ReportList = FDHelper.GetFilteredLIst(filter);
                        ViewBag.filter = filter;
                    }
                    else
                    {
                        ViewBag.ReportList = db.FDRequests.ToList();
                    }
                    ViewBag.getPropertyList = FDHelper.getAllPropertyList();

                }
            }
            catch
            { throw; }
            return View();
        }
        public ActionResult GetExcel(string hfilter)
        {
            FDViewModelClass.ReportView filterobj = new FDViewModelClass.ReportView();
            try
            {
                List<FDRequest> list = new List<FDRequest>();
                if (!hfilter.Equals("null"))
                {
                    filterobj = JsonConvert.DeserializeObject<FDViewModelClass.ReportView>(hfilter);
                    list = FDHelper.GetFilteredLIst(filterobj);
                }
                else
                {
                    DBFD db = new DBFD();
                    list = db.FDRequests.ToList();
                }
                FormattoExcel(list, "Report_" + DateTime.Now.ToString("ddMMyyyyHHmmss"));
                TempData["Success"] = "Excel downloaded successfully..";
            }
            catch
            {
                TempData["Error"] = "Something went wrong..!";
            }
            return RedirectToAction("Report", new { filter = filterobj, Apply = "" });
        }

        void FormattoExcel(List<FDRequest> p, string sname)
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
            System.Web.HttpContext.Current.Response.Write("Customer Number");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Term Length");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");



            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Profit Payment Frequency");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Deposit Amount");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Request Date");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("FD Created");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Mail Sent");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Marked Status In MB");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("SMS Sent");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("BOT Entry Date");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Maturity Date");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("FDBooking Date");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("FD Account No");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            System.Web.HttpContext.Current.Response.Write("<Td>");
            System.Web.HttpContext.Current.Response.Write("<B>");
            System.Web.HttpContext.Current.Response.Write("Payment Method");
            System.Web.HttpContext.Current.Response.Write("</B>");
            System.Web.HttpContext.Current.Response.Write("</Td>");

            //System.Web.HttpContext.Current.Response.Write("<Td>");
            //System.Web.HttpContext.Current.Response.Write("<B>");
            //System.Web.HttpContext.Current.Response.Write("Status");
            //System.Web.HttpContext.Current.Response.Write("</B>");
            //System.Web.HttpContext.Current.Response.Write("</Td>");



            foreach (FDRequest pdata in p)
            {
                System.Web.HttpContext.Current.Response.Write("<TR>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.CustomerNumber);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.TermLength);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.ProfitPaymentFrequency);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.DepositAmount);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.RequestDate);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.FDCreated);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.MailSent);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.MarkedStatusInMB);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.SMSSent);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.BOTEntryDate);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.MaturityDate);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.FDBookingDate);

                System.Web.HttpContext.Current.Response.Write("</Td>");

                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.FDAccNo);

                System.Web.HttpContext.Current.Response.Write("</Td>");


                System.Web.HttpContext.Current.Response.Write("<Td>");

                System.Web.HttpContext.Current.Response.Write(pdata.PaymentMethod);

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

        #region--------------------------Robot Config---------------------------------------------
        public ActionResult RobotConfig()
        {
            ViewBag.rbtconfg = "active";
            try
            {
                DBFD db = new DBFD();
                ViewBag.RbConfigList = db.SettingAndConstants.Where(x => x.EditFlag == 1).ToList();
            }
            catch (Exception ex)
            {
                // TempData["Error"] = "Something went wrong..!" + ex.Message;
                throw ex;
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RobotConfig(SettingAndConstant robconfigObj)
        {
            try
            {
                using (DBFD db = new DBFD())
                {
                    SettingAndConstant editobj = db.SettingAndConstants.Where(x => x.ID == robconfigObj.ID).FirstOrDefault();
                    editobj.Name = robconfigObj.Name;
                    editobj.Value = robconfigObj.Value;
                    editobj.Description = robconfigObj.Description;
                    db.SaveChanges();
                    TempData["Success"] = "Data saved successfully.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Something went wrong..!" + ex.Message;
            }

            return RedirectToAction("RobotConfig");
        }
        #endregion--------------------------------------------------------------

        #region---------------------------SMS Config--------------------------------------------
        public ActionResult SMSConfig()
        {
            ViewBag.sms = "active";
            try
            {
                DBFD db = new DBFD();
                ViewBag.smsConfigList = db.SMSConfigs.ToList();
            }
            catch
            { }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SMSConfig(SMSConfig smsconfigObj)
        {
            try
            {
                using (DBFD db = new DBFD())
                {
                    SMSConfig editobj = db.SMSConfigs.Where(x => x.ID == smsconfigObj.ID).FirstOrDefault();
                    editobj.BusinessException = smsconfigObj.BusinessException;
                    editobj.Language = smsconfigObj.Language;
                    editobj.SMSBody = smsconfigObj.SMSBody;
                    db.SaveChanges();
                    TempData["Success"] = "Data saved successfully.";
                }
            }
            catch
            {
                TempData["Error"] = "Something went wrong..!";
            }

            return RedirectToAction("SMSConfig");
        }
        #endregion--------------------------------------------------------------


        #region---------------------------Email Config--------------------------------------------
        public ActionResult EmailConfig()
        {
            ViewBag.email = "active";
            try
            {
                DBFD db = new DBFD();
                ViewBag.emailConfigList = db.MailConfigs.ToList();
            }
            catch
            { }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmailConfig(MailConfig configobj)
        {
            try
            {
                using (DBFD db = new DBFD())
                {
                    MailConfig editobj = db.MailConfigs.Where(x => x.ID == configobj.ID).FirstOrDefault();
                    editobj.BCC = configobj.BCC;
                    editobj.Body = getMessagebody(int.Parse(editobj.BodyFlag), configobj.Body);
                    db.SaveChanges();
                    TempData["Success"] = "Data saved successfully.";
                }
            }
            catch
            {
                TempData["Error"] = "Something went wrong..!";
            }

            return RedirectToAction("EmailConfig");
        }

        public string getMessagebody(int id, string message)
        {
            string msgBdy = string.Empty;
            try
            {
                switch (id)
                {
                    case 0:
                        msgBdy = "<html> <body> <p>Dear Customer</p> <p id='p'>" + message.Trim() + "</p> <p>Thankyou, <br> <br>Sincerely,<br> Meethaq Islamic Banking<br><br><p style='color: red;'>Please do not reply to this email as this is a system generated message.<br>For any queries please email at Meethaq@bankmuscat.com</p> </body> </html>";
                        break;
                    case 2:
                        msgBdy = "<html> <body> <p>Dear Customer</p> <p id='p'>" + message.Trim() + "</p> <br>Sincerely,<br> Meethaq Islamic Banking <br><br><p style='color: red;'>Please do not reply to this email as this is a system generated message.<br> For any queries please email at Meethaq@bankmuscat.com</p> </body> </html>";
                        break;
                    default:
                        msgBdy = "<html><body><p>Dear Customer</p><br><p id='p'>" + message.Trim() + "</p><br><p>Sincerely, <br>Meethaq Islamic Banking <br>Please do not reply to this email as this is a system generated message. For any queries please email at Meethaq@bankmuscat.com </p>MB Team </p></body></html>";
                        break;
                }
            }
            catch { }
            return msgBdy;
        }
        #endregion--------------------------------------------------------------


        #region--------------------------TL Config---------------------------------------------
        public ActionResult TLConfig()
        {
            ViewBag.term = "active";
            try
            {
                DBFD db = new DBFD();
                ViewBag.TlList = db.TblTermLengths.ToList();
            }
            catch
            { }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TLConfig(TblTermLength configObj)
        {
            try
            {
                using (DBFD db = new DBFD())
                {
                    if (configObj.ID == 0)
                    {
                        db.TblTermLengths.Add(configObj);
                        db.SaveChanges();
                        TempData["Success"] = "Data saved successfully.";
                    }
                    else
                    {
                        TblTermLength editobj = db.TblTermLengths.Where(x => x.ID == configObj.ID).FirstOrDefault();
                        editobj.TermLength = configObj.TermLength;
                        editobj.ProfitPaymentFrequency = configObj.ProfitPaymentFrequency;
                        editobj.ProductCode = configObj.ProductCode;
                        db.SaveChanges();
                        TempData["Success"] = "Data saved successfully.";
                    }

                }
            }
            catch
            {
                TempData["Error"] = "Something went wrong..!";
            }

            return RedirectToAction("TLConfig");
        }
        #endregion--------------------------------------------------------------

        #region-------------------------------------Asset--------------------------------
        public ActionResult Asset()
        {
            ViewBag.asset = "active";
            try
            {
                DBFD db = new DBFD();
                ViewBag.list = db.AssetsConfigs.ToList();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Something went wrong..!" + ex.Message;
            }
            return View();
        }
        #endregion--------------------------------------------------------------------------
    }
}