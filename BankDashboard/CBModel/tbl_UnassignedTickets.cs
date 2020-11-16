namespace BankDashboard.CBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_UnassignedTickets
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FeedbackId { get; set; }

        [StringLength(50)]
        public string CustomerName { get; set; }

        [StringLength(50)]
        public string CIFNo { get; set; }

        [StringLength(50)]
        public string CardNumber { get; set; }

        [StringLength(50)]
        public string AccountNumber { get; set; }

        public string Comments { get; set; }

        public DateTime? IncidentDate { get; set; }

        [StringLength(50)]
        public string Amount { get; set; }

        [StringLength(50)]
        public string BankingWith { get; set; }

        [StringLength(50)]
        public string Category { get; set; }

        [StringLength(50)]
        public string SubCategory { get; set; }

        [StringLength(50)]
        public string Issue { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public DateTime? BotDataEntryTime { get; set; }

        [StringLength(50)]
        public string BotRemarks { get; set; }

        [StringLength(50)]
        public string Status { get; set; }
    }
}
