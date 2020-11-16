namespace BankDashboard.CBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_MasterCardIssuing
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string ClaimID { get; set; }

        [StringLength(50)]
        public string ChargebackReferenceNumber { get; set; }

        [StringLength(50)]
        public string ChargebackID { get; set; }

        [StringLength(50)]
        public string AccountNumber { get; set; }

        [StringLength(50)]
        public string AcquirerReferenceNumber { get; set; }

        [StringLength(50)]
        public string ChargebackAmount { get; set; }

        [StringLength(50)]
        public string CurrencyCode { get; set; }

        [StringLength(50)]
        public string IssuerICA { get; set; }

        [StringLength(50)]
        public string AcquirerICA { get; set; }

        [StringLength(50)]
        public string ReasonCode { get; set; }

        public DateTime? ClearingDate { get; set; }

        [StringLength(50)]
        public string ClaimStatus { get; set; }

        public DateTime? FraudNotificationDate { get; set; }

        [StringLength(50)]
        public string Reversal { get; set; }

        public DateTime? FileScheduleDate { get; set; }

        public DateTime? BotEntryTime { get; set; }
    }
}
