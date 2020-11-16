namespace BankDashboard.ModelFd
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AssetsConfig")]
    public partial class AssetsConfig
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Asset { get; set; }

        public string OrchestratorAssetFolder { get; set; }
    }
}
