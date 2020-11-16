namespace BankDashboard.CBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Matched_FinancialTransaction
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string ValueDate { get; set; }

        [StringLength(50)]
        public string PostDate { get; set; }

        [StringLength(50)]
        public string MemberCase { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Reference { get; set; }

        [StringLength(50)]
        public string CardNumber { get; set; }

        [StringLength(10)]
        public string Debit { get; set; }

        [StringLength(10)]
        public string Credit { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        public DateTime? FileScheduleDate { get; set; }

        public DateTime? BotEntryTime { get; set; }
    }
}
