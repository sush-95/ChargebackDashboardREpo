namespace BankDashboard.CBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_AuthCodeTransactionInfo
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string AuthCode { get; set; }

        [StringLength(50)]
        public string AmountLcy { get; set; }

        [StringLength(50)]
        public string ValueDate { get; set; }

        [StringLength(50)]
        public string TransReference { get; set; }

        [StringLength(50)]
        public string SystemId { get; set; }

        [StringLength(50)]
        public string NetworkIdent { get; set; }

        public DateTime? BotEntryTime { get; set; }
    }
}
