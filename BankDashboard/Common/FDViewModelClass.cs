using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankDashboard.Common
{
    public class FDViewModelClass
    {
        public class ReportView
        {
            public int ID { get; set; }

            public string CustomerNumber { get; set; }

            public string TermLength { get; set; }

            public string ProfitPaymentFrequency { get; set; }

            public string DepositAmount { get; set; }

            public string RequestDate { get; set; }

            public string FDCreated { get; set; }

            public string MailSent { get; set; }

            public string MarkedStatusInMB { get; set; }

            public string SMSSent { get; set; }

            public DateTime? BOTEntryDate { get; set; }

            public string MaturityDate { get; set; }

            public string FDBookingDate { get; set; }

            public string FDAccNo { get; set; }

            public string PaymentMethod { get; set; }

            public string Status { get; set; }

            public string FromDate { get; set; }

            public string ToDate { get; set; }


        }
        public class ListOFproperty
        {
            public List<string> ListofTermLength { get; set; }

            public List<string> ListOfMailsent { get; set; }

            public List<string> ListOfSMSSent { get; set; }

            public List<string> ListofFDCreated { get; set; }

            public List<string> ListMarkedStatusInMB { get; set; }
        }
        public class ChartInfo
        {
            public string YTD { get; set; }
            public List<long> CartFigures { get; set; }

        }
        public class CountingInfo
        {
            public string  Requested{ get; set; }
            public string Processed { get; set; }
            public string Manhoursaved { get; set; }
            public string date { get; set; }

        }


    }
    public class Constants
    {
        readonly public static string SaveStatus = "0000";
        readonly public static string ApproveSaveStatus = "00001";
        readonly public static string ModifyStatus = "0005";
        readonly public static string ApproveModifyStatus = "0006";

        public static class UserGroups
        {
            readonly public static string Admin = "RPA-IT Support Team";
            readonly public static string Checker = "RPA-Recon Checker";
            readonly public static string Maker = "RPA-Recon Maker";
            readonly public static string ParameterManager = "Parameter Management Team";
            readonly public static string ApplicationUserManagement = "Application User Management";
        }
    }
}