namespace BankDashboard.ModelFd
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MailConfig")]
    public partial class MailConfig
    {
        public int ID { get; set; }

        public string BodyFlag { get; set; }

        public string BCC { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}
