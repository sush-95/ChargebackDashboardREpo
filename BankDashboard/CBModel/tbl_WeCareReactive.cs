namespace BankDashboard.CBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_WeCareReactive
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string FeedbackID { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(50)]
        public string Priority { get; set; }

        [StringLength(50)]
        public string CustomerCommercialName { get; set; }

        [StringLength(50)]
        public string AssignedUserID { get; set; }

        [StringLength(50)]
        public string RegistrationDate { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string CreatedByID { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        [StringLength(50)]
        public string AccountNumber { get; set; }

        [StringLength(50)]
        public string AccountStatus { get; set; }

        [StringLength(50)]
        public string AccountType { get; set; }

        [StringLength(50)]
        public string AdditionalEmailID { get; set; }

        [StringLength(50)]
        public string AdditionalMobileNo { get; set; }

        [StringLength(50)]
        public string CommercialAddress { get; set; }

        [StringLength(50)]
        public string AmexCardNo { get; set; }

        [StringLength(50)]
        public string Amount { get; set; }

        [StringLength(50)]
        public string AOFNo { get; set; }

        [StringLength(50)]
        public string Area { get; set; }

        [StringLength(50)]
        public string BankingWith { get; set; }

        [StringLength(50)]
        public string BeneficiaryDeviceNumber { get; set; }

        [StringLength(50)]
        public string BeneficiaryAccountNo { get; set; }

        [StringLength(50)]
        public string BeneficiaryBillContractNumber { get; set; }

        [StringLength(50)]
        public string BeneficiaryDeviceSoftware { get; set; }

        [StringLength(50)]
        public string BillAccountNo { get; set; }

        [StringLength(50)]
        public string BillContractNo { get; set; }

        [StringLength(50)]
        public string CardsChannel { get; set; }

        [StringLength(50)]
        public string CardNo { get; set; }

        [StringLength(50)]
        public string CardType { get; set; }

        [StringLength(50)]
        public string CaseDispute { get; set; }

        [StringLength(50)]
        public string FeedbackType { get; set; }

        [StringLength(50)]
        public string Category { get; set; }

        [StringLength(50)]
        public string Channel { get; set; }

        [StringLength(50)]
        public string ChequeNo { get; set; }

        [StringLength(50)]
        public string CIFNo { get; set; }

        [StringLength(50)]
        public string ClosedByGroup { get; set; }

        [StringLength(50)]
        public string ClosedByUser { get; set; }

        [StringLength(50)]
        public string ClosureComments { get; set; }

        [StringLength(50)]
        public string ClosureDate { get; set; }

        [StringLength(50)]
        public string CommentsArabic { get; set; }

        [StringLength(50)]
        public string CommentsEnglish { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        [StringLength(50)]
        public string CustomerCategory { get; set; }

        [StringLength(50)]
        public string CustomerType { get; set; }

        [StringLength(50)]
        public string cxuserfillmandatoryfield { get; set; }

        [StringLength(50)]
        public string DeviceLocation { get; set; }

        [StringLength(50)]
        public string DeviceSoftware { get; set; }

        [StringLength(50)]
        public string DeviceType { get; set; }

        [StringLength(50)]
        public string DisputeForm { get; set; }

        [StringLength(50)]
        public string DocumentsReceived { get; set; }

        [StringLength(50)]
        public string IsDocumentsRequired { get; set; }

        [StringLength(50)]
        public string DraweeBank { get; set; }

        [StringLength(50)]
        public string EmailID { get; set; }

        [StringLength(50)]
        public string Escalate1OtherComment { get; set; }

        [StringLength(50)]
        public string Escalate1ToGroup { get; set; }

        [StringLength(50)]
        public string Escalate2OtherComment { get; set; }

        [StringLength(50)]
        public string Escalate2ToGroup { get; set; }

        [StringLength(50)]
        public string Escalate3OtherComment { get; set; }

        [StringLength(50)]
        public string Escalate3ToGroup { get; set; }

        [StringLength(50)]
        public string FinalizedComments { get; set; }

        [StringLength(50)]
        public string ClosureNotes { get; set; }

        [StringLength(50)]
        public string FTNo { get; set; }

        [StringLength(50)]
        public string GroupEmaiAddress { get; set; }

        [StringLength(50)]
        public string GSMNo { get; set; }

        [StringLength(50)]
        public string GuaranteeReferenceNo { get; set; }

        [StringLength(50)]
        public string Hold { get; set; }

        [StringLength(50)]
        public string HoldCount { get; set; }

        [StringLength(50)]
        public string HoldStatus { get; set; }

        [StringLength(50)]
        public string HoldUser { get; set; }

        [StringLength(50)]
        public string HoldUserGroup { get; set; }

        [StringLength(50)]
        public string HomeBranch { get; set; }

        [StringLength(50)]
        public string IDNo { get; set; }

        [StringLength(50)]
        public string IncidentBranch { get; set; }

        [StringLength(50)]
        public string IncidentBranchLocation { get; set; }

        [StringLength(50)]
        public string IncidentBranchRegion { get; set; }

        [StringLength(50)]
        public string IncidentDate { get; set; }

        [StringLength(50)]
        public string InternationalGuaranteeReferenceNo { get; set; }

        [StringLength(50)]
        public string IssuanceDate { get; set; }

        [StringLength(50)]
        public string Issue { get; set; }

        [StringLength(50)]
        public string LatestHoldDate { get; set; }

        [StringLength(50)]
        public string LCReferenceNo { get; set; }

        [StringLength(50)]
        public string LegalDocumentNumber { get; set; }

        [StringLength(50)]
        public string LegalDocumentName { get; set; }

        [StringLength(50)]
        public string ContactDetails { get; set; }

        [StringLength(50)]
        public string MobileHandsetModel { get; set; }

        [StringLength(50)]
        public string MobileNo { get; set; }

        [StringLength(50)]
        public string TerminalModel { get; set; }

        [StringLength(50)]
        public string ModeOfPayment { get; set; }

        [StringLength(50)]
        public string NarrationOfTransaction { get; set; }

        [StringLength(50)]
        public string NIDNo { get; set; }

        [StringLength(50)]
        public string NOCCLCApplicationNo { get; set; }

        [StringLength(50)]
        public string NonCustomerName { get; set; }

        [StringLength(50)]
        public string NoofDaysBeyondSLA { get; set; }

        [StringLength(50)]
        public string NoofSLADays { get; set; }

        [StringLength(50)]
        public string TerminalID { get; set; }

        [StringLength(50)]
        public string OnHoldNotes { get; set; }

        [StringLength(50)]
        public string OtherBankName { get; set; }

        [StringLength(50)]
        public string PaymentChannel { get; set; }

        [StringLength(50)]
        public string PolicyNo { get; set; }

        [StringLength(50)]
        public string PolicyType { get; set; }

        [StringLength(50)]
        public string PotentialDuplicateCase { get; set; }

        [StringLength(50)]
        public string ProcessMatrixID { get; set; }

        [StringLength(50)]
        public string ReasonofEscalation1 { get; set; }

        [StringLength(50)]
        public string ReasonofEscalation2 { get; set; }

        [StringLength(50)]
        public string ReasonofEscalation3 { get; set; }

        [StringLength(50)]
        public string ReasonofHold { get; set; }

        [StringLength(50)]
        public string Reasonofunhold { get; set; }

        [StringLength(50)]
        public string ReferenceNumber { get; set; }

        [StringLength(50)]
        public string RegisteredEmailID { get; set; }

        [StringLength(50)]
        public string RegisteredforMBanking { get; set; }

        [StringLength(50)]
        public string RegisteredMobileNo { get; set; }

        [StringLength(50)]
        public string Reopened { get; set; }

        [StringLength(50)]
        public string ReopenedDate { get; set; }

        [StringLength(50)]
        public string ReopenedReason { get; set; }

        [StringLength(50)]
        public string ReopeningChannel { get; set; }

        [StringLength(50)]
        public string ReopeningComments { get; set; }

        [StringLength(50)]
        public string RequestedPercentageRate { get; set; }

        [StringLength(50)]
        public string RerouteDate { get; set; }

        [StringLength(50)]
        public string RerouteOtherComment { get; set; }

        [StringLength(50)]
        public string RerouteReason { get; set; }

        [StringLength(50)]
        public string RerouteUser { get; set; }

        [StringLength(50)]
        public string RerouteUserGroup { get; set; }

        [StringLength(50)]
        public string ResolutionDate { get; set; }

        [StringLength(50)]
        public string ResolvedByGroup { get; set; }

        [StringLength(50)]
        public string ResolvedByUser { get; set; }

        [StringLength(50)]
        public string ResolvedNotes { get; set; }

        [StringLength(50)]
        public string RootCause { get; set; }

        [StringLength(50)]
        public string ServiceProvider { get; set; }

        [StringLength(50)]
        public string Severity { get; set; }

        [StringLength(50)]
        public string SindbadNumber { get; set; }

        [StringLength(50)]
        public string SLADate { get; set; }

        [StringLength(50)]
        public string SocialPlatform { get; set; }

        [StringLength(50)]
        public string Source { get; set; }

        [StringLength(50)]
        public string Stage { get; set; }

        [StringLength(50)]
        public string SubCategory { get; set; }

        [StringLength(50)]
        public string SuggestedAction { get; set; }

        [StringLength(50)]
        public string AlternateContactNo { get; set; }

        [StringLength(50)]
        public string MurshidID { get; set; }

        [StringLength(50)]
        public string POSTerminal { get; set; }

        [StringLength(50)]
        public string AssignedToGroup { get; set; }

        [StringLength(50)]
        public string Branch { get; set; }

        [StringLength(50)]
        public string DeviceNo { get; set; }

        [StringLength(50)]
        public string AccountCardnumber { get; set; }

        [StringLength(50)]
        public string SLAStatus { get; set; }

        [StringLength(50)]
        public string AssignedtoBranchRegion { get; set; }

        public DateTime? BotEntryTime { get; set; }

        public DateTime? FileScheduleDate { get; set; }

        [StringLength(50)]
        public string FileType { get; set; }
    }
}
