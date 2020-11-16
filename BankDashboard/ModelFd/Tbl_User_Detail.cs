namespace BankDashboard.ModelFd
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_User_Detail
    {
        public int ID { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Usergroup { get; set; }

        public string GroupPages { get; set; }
    }
}
