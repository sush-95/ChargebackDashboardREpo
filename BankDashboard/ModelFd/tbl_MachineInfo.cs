namespace BankDashboard.ModelFd
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_MachineInfo
    {
        public long ID { get; set; }

        [Required]
        public string MachineName { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedTs { get; set; }
    }
}
