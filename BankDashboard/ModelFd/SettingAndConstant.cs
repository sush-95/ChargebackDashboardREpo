namespace BankDashboard.ModelFd
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SettingAndConstant
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }

        public int? EditFlag { get; set; }
    }
}
