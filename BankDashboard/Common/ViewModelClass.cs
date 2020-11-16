using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankDashboard.Common
{
    public class ViewModelClass
    {
        
      
        public class selectPages
        {
            public string PageName { get; set; }
            public string groupname { get; set; }
            public string groupdesc { get; set; }
        }
        public class FilterClass
        {
            public string Fromdate { get; set; }
            public string Todate { get; set; }
            public string FeedbackID { get; set; }
        }
        //public class ViewUserDetail
        //{
        //    public string UserName { get; set; }
        //    public string GroupName { get; set; }
        //    public string GroupPages { get; set; }
        //}
    }
}