
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComplaintFormCore.Models
{
    public class SurveyPAModel
    {
        /// <summary>
        /// Page 1: page_preliminary_info_Authorization_for_Representative
        /// </summary>
        public string FilingComplaintOnOwnBehalf { get; set; }

        /// <summary>
        /// Page 2: page_preliminary_info_Identify_institution
        /// </summary>
        public string WhichFederalGovernementInstitutionComplaintAgainst { get; set; }

        /// <summary>
        /// Page 3: page_steps_taken_Writing_ATIP_Coordinator
        /// </summary>
        public string RaisedPrivacyToAtipCoordinator { get; set; }

        /// <summary>
        /// Page 5: page_details_Type_complaint
        /// </summary>
        public List<string> NatureOfComplaint { get; set; }

        /// <summary>
        /// Page 6: page_details_description_of_concerns
        /// </summary>
        public string IsEmployee { get; set; }

        /// <summary>
        /// Page 6: page_details_description_of_concerns
        /// </summary>
        public string DateSentRequests { get; set; }

        /// <summary>
        /// Page 6: page_details_description_of_concerns
        /// </summary>
        public string WordingOfRequest { get; set; }

        /// <summary>
        /// Page 6: page_details_description_of_concerns
        /// </summary>
        public string MoreDetailsOfRequest { get; set; }

        /// <summary>
        /// Page 6: page_details_description_of_concerns
        /// </summary>
        public string DateOfFinalAnswer { get; set; }

        /// <summary>
        /// Page 6: page_details_description_of_concerns
        /// </summary>
        public string DidNoRecordExist { get; set; }

        /// <summary>
        /// Page 6: page_details_description_of_concerns
        /// </summary>
        public string PrivacyActSectionsApplied { get; set; }

        /// <summary>
        /// Page 6: page_details_description_of_concerns
        /// </summary>
        public string ItemsNotRecieved { get; set; }

        /// <summary>
        /// Page 6: page_details_description_of_concerns
        /// </summary>
        public string InstitutionAgreedRequestOnInformalBasis { get; set; }

        /// <summary>
        /// Page 6: page_details_description_of_concerns
        /// </summary>
        public string SummarizeYourConcernsAndAnyStepsTaken { get; set; }

        /// <summary>
        /// Page 6: page_details_description_of_concerns
        /// </summary>
        public string SummarizeYourComplaintAndAnyStepsTaken { get; set; }

        /// <summary>
        /// Page 6: page_details_description_of_concerns
        /// </summary>
        public string WhatWouldResolveYourComplaint { get; set; }

        /// <summary>
        /// Page 6: page_details_description_of_concerns
        /// </summary>
        public string SummarizeAttemptsToResolvePrivacyMatter { get; set; }

        /// <summary>
        /// Page 6: page_details_description_of_concerns
        /// </summary>
        public string AdditionalComments { get; set; }

        /// <summary>
        /// Page 7: page_complainant_representative
        /// </summary>
        public string Complainant_HaveYouSubmittedBefore { get; set; }

        public string Reprensentative_title { get; set; }

        public string Reprensentative_FirstName { get; set; }

        public string Reprensentative_LastName { get; set; }

        public string Reprensentative_Email { get; set; }

        public string Reprensentative_MailingAddress { get; set; }

        public string Reprensentative_City { get; set; }

        public string Reprensentative_PostalCode { get; set; }

        public string Reprensentative_DayTimeNumber { get; set; }

        public string Complainant_FormOfAddress { get; set; }

        public string Complainant_FirstName { get; set; }

        public string Complainant_LastName { get; set; }

        public string Complainant_Email { get; set; }

        public string Complainant_MailingAddress { get; set; }

        public string Complainant_City { get; set; }

        public string Complainant_PostalCode { get; set; }

        public string Complainant_Country { get; set; }

        public string Complainant_ProvinceOrState { get; set; }

        public string Complainant_DayTimeNumber { get; set; }

        public string Complainant_DayTimeNumberExtension { get; set; }

        public string Complainant_AltTelephoneNumber { get; set; }        

        public string Complainant_AltTelephoneNumberExtension { get; set; }

        public string NeedsDisabilityAccommodation { get; set; }

        public string DisabilityAccommodation { get; set; }

        public string Documentation_type { get; set; }

        public List<SurveyFile> Documentation_file_upload { get; set; }

        public List<SurveyFile> Documentation_file_upload_rep { get; set; }

        public List<string> InformationIsTrue { get; set; }
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
