namespace BankDashboard.CBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_IssuingOutgoingVISA
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string DeliveryType { get; set; }

        [StringLength(50)]
        public string MemberCase { get; set; }

        [StringLength(20)]
        public string ROLCaseNo { get; set; }

        [StringLength(50)]
        public string CardAccountNumber { get; set; }

        [StringLength(20)]
        public string DisputeAmt { get; set; }

        [StringLength(20)]
        public string TranDate { get; set; }

        [StringLength(50)]
        public string AcquirerRefACQBINRRNSTAN { get; set; }

        [StringLength(100)]
        public string MerchantName { get; set; }

        [StringLength(50)]
        public string MerchCity { get; set; }

        [StringLength(50)]
        public string MerchState { get; set; }

        [StringLength(50)]
        public string NoOfAttach { get; set; }

        [StringLength(30)]
        public string AcquirerBID { get; set; }

        [StringLength(50)]
        public string AuthCode { get; set; }

        [StringLength(50)]
        public string CATInd { get; set; }

        [StringLength(50)]
        public string OriginalCPD { get; set; }

        [StringLength(50)]
        public string MessageType { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(50)]
        public string MOTOECI { get; set; }

        [StringLength(50)]
        public string MCC { get; set; }

        [StringLength(100)]
        public string MemberMessageText { get; set; }

        [StringLength(50)]
        public string MerchCntry { get; set; }

        [StringLength(30)]
        public string ReasonCode { get; set; }

        [StringLength(20)]
        public string ReimbAttr { get; set; }

        [StringLength(20)]
        public string RqstdPmtSvc { get; set; }

        [StringLength(20)]
        public string SpecCondInd { get; set; }

        [StringLength(50)]
        public string TranAmt { get; set; }

        [StringLength(20)]
        public string CurrCode { get; set; }

        [StringLength(20)]
        public string TranCode { get; set; }

        [StringLength(50)]
        public string TranID { get; set; }

        [StringLength(50)]
        public string AcqInstCntry { get; set; }

        [StringLength(10)]
        public string IntlFeeInd { get; set; }

        [StringLength(20)]
        public string FinancialCPD { get; set; }

        [StringLength(20)]
        public string CPDEst { get; set; }

        [StringLength(20)]
        public string DocType { get; set; }

        [StringLength(20)]
        public string ActionDate { get; set; }

        [StringLength(30)]
        public string CardAccptrID { get; set; }

        [StringLength(30)]
        public string TerminalID { get; set; }

        [StringLength(50)]
        public string NetworkID { get; set; }

        [StringLength(50)]
        public string DispCurrCode { get; set; }

        [StringLength(30)]
        public string TimeofDelivery { get; set; }

        [StringLength(50)]
        public string BQIFileName { get; set; }

        [StringLength(50)]
        public string FileSizeMB { get; set; }

        [StringLength(50)]
        public string BulkMailFileName { get; set; }

        [StringLength(50)]
        public string POSTermCap { get; set; }

        [StringLength(50)]
        public string AcceptanceAmount { get; set; }

        [StringLength(50)]
        public string AcceptanceAmountCurrCode { get; set; }

        [StringLength(20)]
        public string FloorLimitIndicator { get; set; }

        [StringLength(50)]
        public string SettlementFlag { get; set; }

        [StringLength(20)]
        public string PCASIndicator { get; set; }

        [StringLength(50)]
        public string POSEntryMode { get; set; }

        [StringLength(50)]
        public string MerchantZipPostalCode { get; set; }

        [StringLength(20)]
        public string FeeProgramIndicator { get; set; }

        [StringLength(50)]
        public string CRBExceptionFileIndicator { get; set; }

        [StringLength(50)]
        public string NationalReimbursementFee { get; set; }

        [StringLength(50)]
        public string PrePaidCardIndicator { get; set; }

        [StringLength(20)]
        public string MVV { get; set; }

        [StringLength(50)]
        public string AdditionalDataIndicator { get; set; }

        [StringLength(20)]
        public string CaseClass { get; set; }

        [StringLength(20)]
        public string ProductCardType { get; set; }

        [StringLength(20)]
        public string TRN { get; set; }

        [StringLength(20)]
        public string Token { get; set; }

        [StringLength(20)]
        public string DisputeCPD { get; set; }

        [StringLength(50)]
        public string DisputeCategory { get; set; }

        [StringLength(30)]
        public string DisputeCategoryCondition { get; set; }

        [StringLength(50)]
        public string RespType { get; set; }

        [StringLength(30)]
        public string RespCPD { get; set; }

        [StringLength(30)]
        public string DisputePreArbCPD { get; set; }

        [StringLength(30)]
        public string DisputePreArbRespCPD { get; set; }

        [StringLength(20)]
        public string ResponseDuration { get; set; }

        [StringLength(50)]
        public string ResponseTier { get; set; }

        [StringLength(30)]
        public string VROLFinancialID { get; set; }

        [StringLength(30)]
        public string FinancialInd { get; set; }

        [StringLength(50)]
        public string VROLBundleCase { get; set; }

        [StringLength(50)]
        public string InterFeeAmount { get; set; }

        [StringLength(50)]
        public string InterFeeDC { get; set; }

        [StringLength(50)]
        public string InterFeeCurrCode { get; set; }

        [StringLength(20)]
        public string DisputeStatus { get; set; }

        [StringLength(50)]
        public string DisputeStatusDescription { get; set; }

        [StringLength(20)]
        public string OnUsIndicator { get; set; }

        [StringLength(30)]
        public string Jurisdiction { get; set; }

        public int? BotId { get; set; }

        public DateTime? BotEntryTime { get; set; }

        [StringLength(50)]
        public string CaseSubmissionStatus { get; set; }

        [StringLength(50)]
        public string FileType { get; set; }

        public DateTime? FileScheduleDate { get; set; }
    }
}
