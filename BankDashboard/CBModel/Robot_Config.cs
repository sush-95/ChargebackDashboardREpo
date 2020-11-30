namespace BankDashboard.CBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Robot_Config
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }

        public DateTime? UpdatedTime { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? AddedTime { get; set; }

        public string AddedBy { get; set; }
    }
}
