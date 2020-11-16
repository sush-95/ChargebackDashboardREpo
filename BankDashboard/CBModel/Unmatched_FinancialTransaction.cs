namespace BankDashboard.CBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Unmatched_FinancialTransaction
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string MemberCase { get; set; }

        [Required]
        public string Status { get; set; }

        public DateTime? FileScheduleDate { get; set; }

        public DateTime? BotEntryTime { get; set; }
    }
}
