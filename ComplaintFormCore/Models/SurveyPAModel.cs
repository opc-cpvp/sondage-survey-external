
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComplaintFormCore.Models
{
    public class SurveyPAModel
    {
        public string FilingComplaintOnOwnBehalf { get; set; }

        public string WhichFederalGovernementInstitutionComplaintAgainst { get; set; }

        public string RaisedPrivacyToAtipCoordinator { get; set; }

        public List<string> NatureOfComplaint { get; set; }

        public string IsEmployee { get; set; }

        public string DateSentRequestsAccess { get; set; }

        public string WordingOfRequest { get; set; }

        public string MoreDetailsOfRequest { get; set; }

        public string DateOfFinalAnswer { get; set; }

        public string DidNoRecordExist { get; set; }

        public string PrivacyActSectionsApplied { get; set; }

        public string ItemsNotRecieved { get; set; }

        public string InstitutionAgreedRequestOnInformalBasis { get; set; }

        public string SummarizeYourConcernsAndAnyStepsTaken { get; set; }

        public string SummarizeYourComplaintAndAnyStepsTaken { get; set; }

        public string WhatWouldResolveYourComplaintDetailsPA { get; set; }

        public string SummarizeAttemptsToResolvePrivacyMatter { get; set; }

        public string AdditionalComments { get; set; }

        public string Complainant_HaveYouSubmittedBefore { get; set; }

        public string Reprensentative_title { get; set; }

        public string Reprensentative_FirstName { get; set; }

        public string Reprensentative_LastName { get; set; }

        public string Reprensentative_Email { get; set; }

        public string Reprensentative_MailingAddress { get; set; }

        public string Reprensentative_City { get; set; }

        public string Reprensentative_PostalCode { get; set; }

        public string Reprensentative_DayTimeNumber { get; set; }

        public string Complainant_title { get; set; }

        public string Complainant_FirstName { get; set; }

        public string Complainant_LastName { get; set; }

        public string Complainant_Email { get; set; }

        public string Complainant_MailingAddress { get; set; }

        public string Complainant_City { get; set; }

        public string Complainant_PostalCode { get; set; }

        public string Complainant_Country { get; set; }

        public string Complainant_ProvinceOrState { get; set; }

        public string Complainant_DayTimeNumber { get; set; }

        public string Complainant_NeedsDisabilityAccommodation { get; set; }

        public string Complainant_NeedsDisabilityAccommodation_details { get; set; }

        public string Documentation_type { get; set; }

        public List<SurveyFile> Documentation_file_upload { get; set; }

        public List<SurveyFile> Documentation_file_upload_rep { get; set; }

        public List<string> Confirmation_verification { get; set; }
    }

    [Serializable]
    public class SurveyFile 
    {
        public string name { get; set; }

        public string type { get; set; }

        public string content { get; set; }

        public long size { get; set; }
    }


    public enum NatureOfComplaintType
    {
        NatureOfComplaintDelay,
        NatureOfComplaintExtensionOfTime,
        NatureOfComplaintDenialOfAccess,
        NatureOfComplaintLanguage,
        NatureOfComplaintCorrection,
        NatureOfComplaintCollection,
        NatureOfComplaintUseAndDisclosure,
        NatureOfComplaintRetentionAndDisposal,
        NatureOfComplaintOther
    }
}
