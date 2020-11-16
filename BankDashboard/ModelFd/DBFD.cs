namespace BankDashboard.ModelFd
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Configuration;
    using System.Security.Cryptography;
    using System.Text;

    public partial class DBFD : DbContext
    {
        public DBFD()
            : base(GetSqlConnection())
        {
        }

        public virtual DbSet<AssetsConfig> AssetsConfigs { get; set; }
        public virtual DbSet<BOT_Status> BOT_Status { get; set; }
        public virtual DbSet<FDRequest> FDRequests { get; set; }
        public virtual DbSet<MailConfig> MailConfigs { get; set; }
        public virtual DbSet<SettingAndConstant> SettingAndConstants { get; set; }
        public virtual DbSet<SMSConfig> SMSConfigs { get; set; }
        public virtual DbSet<tbl_MachineInfo> tbl_MachineInfo { get; set; }
        public virtual DbSet<Tbl_User_Detail> Tbl_User_Detail { get; set; }
        public virtual DbSet<TblTermLength> TblTermLengths { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
        public static string GetSqlConnection()
        {
            string x = ConfigurationManager.AppSettings["getstr"].ToString();
            byte[] inputArray = Convert.FromBase64String(x);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes("sblw-3hn8-sqoy19");
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            //string sty= UTF8Encoding.UTF8.GetString(resultArray);
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}
