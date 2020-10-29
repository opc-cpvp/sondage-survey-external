using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComplaintFormCore.Models.pa
{
    public class SurveyPAModel
    {
        /// <summary>
        /// Page: page_preliminary_info_Authorization_for_Representative<br/>
        /// Are you filing this complaint on your own behalf (or for a minor child you ...<br/>
        /// Possible choices: [yourself, someone_else]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string FilingComplaintOnOwnBehalf { get; set; }

        /// <summary>
        /// Page: page_preliminary_info_Identify_institution<br/>
        /// Which federal government institution is your complaint againts?<br/>
        /// Possible choices: [/api/Institution/GetAll?lang={locale}]<br/>
        /// Survey question type: dropdown
        /// </summary>
        public string WhichFederalGovernementInstitutionComplaintAgainst { get; set; }

        /// <summary>
        /// Page: page_steps_taken_Writing_ATIP_Coordinator<br/>
        /// Have you raised your privacy concerns in writing with the institution's [](...<br/>
        /// Possible choices: [yes, no, not_sure]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string RaisedPrivacyToAtipCoordinator { get; set; }

        /// <summary>
        /// Page: page_details_Type_complaint<br/>
        /// Which of the following choices most accurately summarizes your complaint?<br/>
        /// Possible choices: [NatureOfComplaintDelay, NatureOfComplaintExtensionOfTime, NatureOfComplaintDenialOfAccess, NatureOfComplaintLanguage, NatureOfComplaintCorrection, NatureOfComplaintCollection, NatureOfComplaintUseAndDisclosure, NatureOfComplaintRetentionAndDisposal, NatureOfComplaintOther]<br/>
        /// Survey question type: checkbox
        /// </summary>
        public List<string> NatureOfComplaint { get; set; }

        /// <summary>
        /// Page: page_details_description_of_concerns<br/>
        /// Are you submitting the complaint as a member of the general public or as an...<br/>
        /// Possible choices: [general_public, employee_government]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string IsEmployeeChoice { get; set; }

        /// <summary>
        /// Page: page_details_description_of_concerns<br/>
        /// What date did you send your written access request to the institution? If y...<br/>
        /// Required condition: {NatureOfComplaint} anyof ['NatureOfComplaintDelay','NatureOfComplaintExtensionOfTime','NatureOfComplaintDenialOfAccess','NatureOfComplaintLanguage']<br/>
        /// Survey question type: comment
        /// </summary>
        public string DateSentRequests { get; set; }

        /// <summary>
        /// Page: page_details_description_of_concerns<br/>
        /// What was the wording of your request and the file number (or reference numb...<br/>
        /// Required condition: {NatureOfComplaint} anyof ['NatureOfComplaintDelay','NatureOfComplaintExtensionOfTime','NatureOfComplaintDenialOfAccess','NatureOfComplaintLanguage']<br/>
        /// Survey question type: comment
        /// </summary>
        public string WordingOfRequest { get; set; }

        /// <summary>
        /// Page: page_details_description_of_concerns<br/>
        /// If the institution asked you for more information or to provide a clarifica...<br/>
        /// Survey question type: comment
        /// </summary>
        public string MoreDetailsOfRequest { get; set; }

        /// <summary>
        /// Page: page_details_description_of_concerns<br/>
        /// What date did you receive a final response from the institution? (maximum 5...<br/>
        /// Required condition: {NatureOfComplaint} anyof ['NatureOfComplaintDelay','NatureOfComplaintExtensionOfTime','NatureOfComplaintDenialOfAccess','NatureOfComplaintLanguage']<br/>
        /// Survey question type: comment
        /// </summary>
        public string DateOfFinalAnswer { get; set; }

        /// <summary>
        /// Page: page_details_description_of_concerns<br/>
        /// Did the institution reply that no records exist?<br/>
        /// Possible choices: [yes, no]<br/>
        /// Required condition: {NatureOfComplaint} anyof ['NatureOfComplaintDelay','NatureOfComplaintExtensionOfTime','NatureOfComplaintDenialOfAccess','NatureOfComplaintLanguage']<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string DidNoRecordExistChoice { get; set; }

        /// <summary>
        /// Page: page_details_description_of_concerns<br/>
        /// What sections of the *Privacy Act* did the institution mention (if any) as ...<br/>
        /// Survey question type: comment
        /// </summary>
        public string PrivacyActSectionsApplied { get; set; }

        /// <summary>
        /// Page: page_details_description_of_concerns<br/>
        /// Please describe each item (if any) from your access request that you have n...<br/>
        /// Survey question type: comment
        /// </summary>
        public string ItemsNotRecieved { get; set; }

        /// <summary>
        /// Page: page_details_description_of_concerns<br/>
        /// Did the institution agree to process your request on an informal basis?<br/>
        /// Possible choices: [yes, no, not_sure]<br/>
        /// Required condition: {NatureOfComplaint} anyof ['NatureOfComplaintDelay','NatureOfComplaintExtensionOfTime','NatureOfComplaintDenialOfAccess','NatureOfComplaintLanguage']<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string InstitutionAgreedRequestOnInformalBasis { get; set; }

        /// <summary>
        /// Page: page_details_description_of_concerns<br/>
        /// You indicated that in addition to concerns about your access request, you h...<br/>
        /// Required condition: ({NatureOfComplaint} anyof ['NatureOfComplaintCorrection','NatureOfComplaintCollection','NatureOfComplaintUseAndDisclosure','NatureOfComplaintRetentionAndDisposal', 'NatureOfComplaintOther']) and ({NatureOfComplaint} anyof ['NatureOfComplaintDelay','NatureOfComplaintExtensionOfTime','NatureOfComplaintDenialOfAccess','NatureOfComplaintLanguage'])<br/>
        /// Survey question type: comment
        /// </summary>
        public string SummarizeYourConcernsAndAnyStepsTaken { get; set; }

        /// <summary>
        /// Page: page_details_description_of_concerns<br/>
        /// Summarize your complaint and any steps you have taken to try to resolve it ...<br/>
        /// Required condition: ({NatureOfComplaint} anyof ['NatureOfComplaintCorrection','NatureOfComplaintCollection','NatureOfComplaintUseAndDisclosure','NatureOfComplaintRetentionAndDisposal', 'NatureOfComplaintOther']) and ({NatureOfComplaint} anyof ['NatureOfComplaintDelay','NatureOfComplaintExtensionOfTime','NatureOfComplaintDenialOfAccess','NatureOfComplaintLanguage'])<br/>
        /// Survey question type: comment
        /// </summary>
        public string SummarizeYourComplaintAndAnyStepsTaken { get; set; }

        /// <summary>
        /// Page: page_details_description_of_concerns<br/>
        /// What would resolve your concerns? (maximum 5,000 characters)<br/>
        /// Survey question type: comment
        /// </summary>
        public string WhatWouldResolveYourComplaint { get; set; }

        /// <summary>
        /// Page: page_details_description_of_concerns<br/>
        /// Summarize your attempts to resolve the privacy matter with the organization...<br/>
        /// Survey question type: comment
        /// </summary>
        public string SummarizeAttemptsToResolvePrivacyMatter { get; set; }

        /// <summary>
        /// Page: page_details_description_of_concerns<br/>
        /// Additional comments / special Instructions (maximum 5,000 characters)<br/>
        /// Survey question type: comment
        /// </summary>
        public string AdditionalComments { get; set; }

        /// <summary>
        /// Page: page_Complainant_representative<br/>
        /// Have you submitted a complaint in the past with the Office of the Privacy C...<br/>
        /// Possible choices: [yes, no]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string Complainant_HaveYouSubmittedBeforeChoice { get; set; }

        /// <summary>
        /// Page: page_Complainant_representative<br/>
        /// Preferred form of address<br/>
        /// Possible choices: [Mr., Mrs., Ms., Other]<br/>
        /// Survey question type: dropdown
        /// </summary>
        public string Complainant_FormOfAddress { get; set; }

        /// <summary>
        /// Page: page_Complainant_representative<br/>
        /// First name<br/>
        /// Survey question type: text
        /// </summary>
        public string Complainant_FirstName { get; set; }

        /// <summary>
        /// Page: page_Complainant_representative<br/>
        /// Last name<br/>
        /// Survey question type: text
        /// </summary>
        public string Complainant_LastName { get; set; }

        /// <summary>
        /// Page: page_Complainant_representative<br/>
        /// E-mail address (yourname@domain.com)<br/>
        /// Survey question type: text
        /// </summary>
        public string Complainant_Email { get; set; }

        /// <summary>
        /// Page: page_Complainant_representative<br/>
        /// Mailing address<br/>
        /// Survey question type: text
        /// </summary>
        public string Complainant_MailingAddress { get; set; }

        /// <summary>
        /// Page: page_Complainant_representative<br/>
        /// City<br/>
        /// Survey question type: text
        /// </summary>
        public string Complainant_City { get; set; }

        /// <summary>
        /// Page: page_Complainant_representative<br/>
        /// Country<br/>
        /// Possible choices: [/api/Country?lang={locale}]<br/>
        /// Survey question type: dropdown
        /// </summary>
        public string Complainant_Country { get; set; }

        /// <summary>
        /// Page: page_Complainant_representative<br/>
        /// Province/Territory<br/>
        /// Possible choices: [/api/Province?lang={locale}]<br/>
        /// Required condition: {Complainant_Country} = 'CA'<br/>
        /// Survey question type: dropdown
        /// </summary>
        public string Complainant_ProvinceOrState { get; set; }

        /// <summary>
        /// Page: page_Complainant_representative<br/>
        /// Postal code<br/>
        /// Survey question type: text
        /// </summary>
        public string Complainant_PostalCode { get; set; }

        /// <summary>
        /// Page: page_Complainant_representative<br/>
        /// Daytime telephone number (123-456-7890)<br/>
        /// Survey question type: text
        /// </summary>
        public string Complainant_DayTimeNumber { get; set; }

        /// <summary>
        /// Page: page_Complainant_representative<br/>
        /// Extension (digits only)<br/>
        /// Survey question type: text
        /// </summary>
        public string Complainant_DayTimeNumberExtension { get; set; }

        /// <summary>
        /// Page: page_Complainant_representative<br/>
        /// Alternate telephone number (123-456-7890)<br/>
        /// Survey question type: text
        /// </summary>
        public string Complainant_AltTelephoneNumber { get; set; }

        /// <summary>
        /// Page: page_Complainant_representative<br/>
        /// Extension (digits only)<br/>
        /// Survey question type: text
        /// </summary>
        public string Complainant_AltTelephoneNumberExtension { get; set; }

        /// <summary>
        /// Page: page_Complainant_representative<br/>
        /// Preferred form of address<br/>
        /// Possible choices: [Mr., Mrs., Ms., Other]<br/>
        /// Survey question type: dropdown
        /// </summary>
        public string Reprensentative_FormOfAddress { get; set; }

        /// <summary>
        /// Page: page_Complainant_representative<br/>
        /// First name<br/>
        /// Survey question type: text
        /// </summary>
        public string Reprensentative_FirstName { get; set; }

        /// <summary>
        /// Page: page_Complainant_representative<br/>
        /// Last name<br/>
        /// Survey question type: text
        /// </summary>
        public string Reprensentative_LastName { get; set; }

        /// <summary>
        /// Page: page_Complainant_representative<br/>
        /// E-mail address (yourname@domain.com)<br/>
        /// Survey question type: text
        /// </summary>
        public string Reprensentative_Email { get; set; }

        /// <summary>
        /// Page: page_Complainant_representative<br/>
        /// Mailing address<br/>
        /// Survey question type: text
        /// </summary>
        public string Reprensentative_MailingAddress { get; set; }

        /// <summary>
        /// Page: page_Complainant_representative<br/>
        /// City<br/>
        /// Survey question type: text
        /// </summary>
        public string Reprensentative_City { get; set; }

        /// <summary>
        /// Page: page_Complainant_representative<br/>
        /// Country<br/>
        /// Possible choices: [/api/Country?lang={locale}]<br/>
        /// Survey question type: dropdown
        /// </summary>
        public string Reprensentative_Country { get; set; }

        /// <summary>
        /// Page: page_Complainant_representative<br/>
        /// Province/Territory<br/>
        /// Possible choices: [/api/Province?lang={locale}]<br/>
        /// Required condition: {Reprensentative_Country} = 'CA'<br/>
        /// Survey question type: dropdown
        /// </summary>
        public string Reprensentative_ProvinceOrState { get; set; }

        /// <summary>
        /// Page: page_Complainant_representative<br/>
        /// Postal code<br/>
        /// Survey question type: text
        /// </summary>
        public string Reprensentative_PostalCode { get; set; }

        /// <summary>
        /// Page: page_Complainant_representative<br/>
        /// Daytime telephone number (123-456-7890)<br/>
        /// Survey question type: text
        /// </summary>
        public string Reprensentative_DayTimeNumber { get; set; }

        /// <summary>
        /// Page: page_Complainant_representative<br/>
        /// Extension (digits only)<br/>
        /// Survey question type: text
        /// </summary>
        public string Reprensentative_DayTimeNumberExtension { get; set; }

        /// <summary>
        /// Page: page_Complainant_representative<br/>
        /// Alternate telephone number (123-456-7890)<br/>
        /// Survey question type: text
        /// </summary>
        public string Reprensentative_AltTelephoneNumber { get; set; }

        /// <summary>
        /// Page: page_Complainant_representative<br/>
        /// Extension (digits only)<br/>
        /// Survey question type: text
        /// </summary>
        public string Reprensentative_AltTelephoneNumberExtension { get; set; }

        /// <summary>
        /// Page: page_Complainant_representative<br/>
        /// We are committed to ensuring that clients with disabilities have equal acce...<br/>
        /// Possible choices: [yes, no]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string NeedsDisabilityAccommodationChoice { get; set; }

        /// <summary>
        /// Page: page_Complainant_representative<br/>
        /// If yes, please describe what accommodation measures you are requesting and ...<br/>
        /// Required condition: {NeedsDisabilityAccommodationChoice} contains 'yes'<br/>
        /// Survey question type: comment
        /// </summary>
        public string DisabilityAccommodation { get; set; }

        /// <summary>
        /// Page: page_documentation<br/>
        /// Do you wish to upload your supporting documents or send them by mail?<br/>
        /// Possible choices: [upload, mail, both, none]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string Documentation_type { get; set; }

        /// <summary>
        /// Page: page_documentation<br/>
        /// Survey question type: file
        /// </summary>
        public List<SurveyFile> Documentation_file_upload_rep { get; set; }

        /// <summary>
        /// Page: page_documentation<br/>
        /// Survey question type: file
        /// </summary>
        public List<SurveyFile> Documentation_file_upload { get; set; }

        /// <summary>
        /// Page: <br/>
        /// Please certify that the information you have given on this form is, to the ...<br/>
        /// Possible choices: [yes]<br/>
        /// Survey question type: checkbox
        /// </summary>
        public List<string> InformationIsTrue { get; set; }
    }
}
