namespace BankDashboard.CBModel
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Configuration;
    using System.Text;
    using System.Security.Cryptography;

    public partial class CBDB : DbContext
    {
        public CBDB()
            : base(GetSqlConnection())
        {
        }

        public virtual DbSet<Custom_GLReconciliationTable> Custom_GLReconciliationTable { get; set; }
        public virtual DbSet<Matched_FinancialTransaction> Matched_FinancialTransaction { get; set; }
        public virtual DbSet<NonCustom_GLReconciliationTable> NonCustom_GLReconciliationTable { get; set; }
        public virtual DbSet<tbl_AuthCode> tbl_AuthCode { get; set; }
        public virtual DbSet<tbl_AuthCodeTransactionInfo> tbl_AuthCodeTransactionInfo { get; set; }
        public virtual DbSet<tbl_IssuingIncomingVISA> tbl_IssuingIncomingVISA { get; set; }
        public virtual DbSet<tbl_IssuingOutgoingVISA> tbl_IssuingOutgoingVISA { get; set; }
        public virtual DbSet<tbl_MasterCardAcquiring> tbl_MasterCardAcquiring { get; set; }
        public virtual DbSet<tbl_MasterCardIssuing> tbl_MasterCardIssuing { get; set; }
        public virtual DbSet<tbl_UnassignedTickets> tbl_UnassignedTickets { get; set; }
        public virtual DbSet<tbl_VisaAcquiringIncoming> tbl_VisaAcquiringIncoming { get; set; }
        public virtual DbSet<tbl_VisaAcquiringOutgoing> tbl_VisaAcquiringOutgoing { get; set; }
        public virtual DbSet<tbl_WeCareReactive> tbl_WeCareReactive { get; set; }
        public virtual DbSet<Unmatched_FinancialTransaction> Unmatched_FinancialTransaction { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Matched_FinancialTransaction>()
                .Property(e => e.Debit)
                .IsFixedLength();

            modelBuilder.Entity<Matched_FinancialTransaction>()
                .Property(e => e.Credit)
                .IsFixedLength();

            modelBuilder.Entity<Matched_FinancialTransaction>()
                .Property(e => e.Status)
                .IsFixedLength();
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
