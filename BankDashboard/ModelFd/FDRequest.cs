namespace BankDashboard.ModelFd
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FDRequest
    {
        public long ID { get; set; }

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

        public string RequestStatus { get; set; }
    }
}
