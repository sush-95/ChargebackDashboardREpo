namespace BankDashboard.ModelFd
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SMSConfig")]
    public partial class SMSConfig
    {
        public int ID { get; set; }

        public string BusinessException { get; set; }

        public string Language { get; set; }

        public string SMSBody { get; set; }
    }
}
