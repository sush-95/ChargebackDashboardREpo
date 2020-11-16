namespace BankDashboard.CBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_AuthCode
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string FeedbackId { get; set; }

        [StringLength(50)]
        public string AuthCode { get; set; }

        [StringLength(50)]
        public string CardNumber { get; set; }

        [StringLength(50)]
        public string RetrievalRefNo { get; set; }

        [StringLength(50)]
        public string TransactionAmount { get; set; }

        [StringLength(50)]
        public string TranId { get; set; }

        [Column("ARN Number")]
        [StringLength(50)]
        public string ARN_Number { get; set; }

        [Column("RRN Number")]
        [StringLength(50)]
        public string RRN_Number { get; set; }

        [Column("Routing Channel")]
        [StringLength(50)]
        public string Routing_Channel { get; set; }

        [StringLength(50)]
        public string Amount { get; set; }

        [StringLength(50)]
        public string Issuer { get; set; }

        [StringLength(50)]
        public string Acquirer { get; set; }

        [Column("MCC Value")]
        [StringLength(50)]
        public string MCC_Value { get; set; }

        [Column("VROL Case No")]
        [StringLength(50)]
        public string VROL_Case_No { get; set; }

        [Column("Card Type")]
        [StringLength(50)]
        public string Card_Type { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        public string Exception { get; set; }

        [Column("Bot Remark")]
        [StringLength(50)]
        public string Bot_Remark { get; set; }

        [Column("Bot Process StartTime")]
        public DateTime? Bot_Process_StartTime { get; set; }

        [Column("Bot Process EndTime")]
        public DateTime? Bot_Process_EndTime { get; set; }

        [Column("Bot EntryTime")]
        public DateTime? Bot_EntryTime { get; set; }

        [Column("Bot UpdateTime")]
        public DateTime? Bot_UpdateTime { get; set; }
    }
}
