namespace BankDashboard.CBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_MasterCardAcquiring
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string ClaimID { get; set; }

        [StringLength(50)]
        public string RequestID { get; set; }

        [StringLength(50)]
        public string AccountNumber { get; set; }

        [StringLength(50)]
        public string AcquirerReferenceNumber { get; set; }

        [StringLength(50)]
        public string TransactionAmount { get; set; }

        [StringLength(50)]
        public string CurrencyCode { get; set; }

        [StringLength(50)]
        public string ReasonCode { get; set; }

        [StringLength(50)]
        public string AcquirerResponseCode { get; set; }

        [StringLength(50)]
        public string IssuerResponseCode { get; set; }

        [StringLength(50)]
        public string IssuerICA { get; set; }

        [StringLength(50)]
        public string AcquirerICA { get; set; }

        [StringLength(50)]
        public string RejectReasonCode { get; set; }

        [StringLength(50)]
        public string ImageReviewDecision { get; set; }

        public DateTime? ClearingDate { get; set; }

        [StringLength(50)]
        public string ClaimStatus { get; set; }

        public DateTime? FileScheduleDate { get; set; }

        public DateTime? BotEntryTime { get; set; }
    }
}
