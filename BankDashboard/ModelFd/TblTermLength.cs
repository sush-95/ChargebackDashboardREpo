namespace BankDashboard.ModelFd
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TblTermLength")]
    public partial class TblTermLength
    {
        public int ID { get; set; }

        public string TermLength { get; set; }

        public string ProfitPaymentFrequency { get; set; }

        public string ProductCode { get; set; }
    }
}
