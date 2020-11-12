using System.Collections.Generic;
using System;

namespace ComplaintFormCore.Models
{
    public class SurveyPipedaModel
    {
        /// <summary>
        /// Page: page_part_a_jurisdiction_province<br/>
        /// What province or territory did the incident or practice you are concerned a...<br/>
        /// Possible choices: [/api/Province?lang={locale}&addOther=true]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string ProvinceIncidence { get; set; }

        /// <summary>
        /// Page: page_part_a_jurisdiction_particulars<br/>
        /// Does your complaint relate to the handling of personal information outside ...<br/>
        /// Possible choices: [yes, no, not_sure]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string ComplaintAboutHandlingInformationOutsideProvince { get; set; }

        /// <summary>
        /// Page: page_part_a_jurisdiction_particulars<br/>
        /// Is your complaint against any of the following types of organizations?<br/>
        /// Possible choices: [yes, no]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string IsAgainstFwub { get; set; }

        /// <summary>
        /// Page: page_part_a_jurisdiction_particulars<br/>
        /// Did the { ProvinceIncidence } Privacy Commissioner specifically refer you t...<br/>
        /// Possible choices: [yes, no]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string DidOrganizationDirectComplaintToOpc { get; set; }

        /// <summary>
        /// Page: page_part_a_jurisdiction_provincial<br/>
        /// Is your complaint about the handling of personal information by any of the ...<br/>
        /// Possible choices: [yes, no]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string ComplaintAgainstHandlingOfInformation { get; set; }

        /// <summary>
        /// Page: page_part_a_jurisdiction_provincial<br/>
        /// Is your complaint about the handling of personal information by a health pr...<br/>
        /// Possible choices: [yes, no]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string HealthPractitioner { get; set; }

        /// <summary>
        /// Page: page_part_a_jurisdiction_provincial<br/>
        /// Is your complaint about the handling of personal information during or in r...<br/>
        /// Possible choices: [yes, no, not_applicable]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string IndependantPhysicalExam { get; set; }

        /// <summary>
        /// Page: page_part_a_customer_or_employee<br/>
        /// Are you submitting the complaint as a customer or as an employee of the org...<br/>
        /// Possible choices: [customer, employee, other]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string EmployeeOrCustomer { get; set; }

        /// <summary>
        /// Page: page_part_a_customer_or_employee<br/>
        /// Is your complaint against any of the following types of organizations?<br/>
        /// Possible choices: [yes, no]<br/>
        /// Required condition: {ProvinceIncidence} anyof [1,3,4,5,7,8,10]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string AgainstOrganizations { get; set; }

        /// <summary>
        /// Page: page_part_a_competence<br/>
        /// If your complaint is against any of the following types of organizations, p...<br/>
        /// Possible choices: [charity_non_profit, condominium_corporation, federal_government, first_nation, individual, journalism, political_party]<br/>
        /// Survey question type: checkbox
        /// </summary>
        public List<string> Question3Answers { get; set; }

        /// <summary>
        /// Page: page_part_b_another_body<br/>
        /// Have you filed a complaint about your concerns with another body (other tha...<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? SubmittedComplaintToOtherBody { get; set; }

        /// <summary>
        /// Page: page_part_b_status_other_complaint<br/>
        /// Is that complaint process still ongoing?<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? ComplaintStillOngoing { get; set; }

        /// <summary>
        /// Page: page_part_b_means_recourse<br/>
        /// If your complaint is against any of the following types of organizations, p...<br/>
        /// Possible choices: [collection_agency, insurance_company, landlord, lawyer, do_not_call_list, unsubscribe, phone_internet_provider, realtor, your_employer]<br/>
        /// Survey question type: checkbox
        /// </summary>
        public List<string> AgainstTypeOrganizations { get; set; }

        /// <summary>
        /// Page: page_part_b_privacy_officer<br/>
        /// Have you raised your concern in writing with the organization's contact for...<br/>
        /// Possible choices: [yes, no, not_sure]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string RaisedConcernToPrivacyOfficer { get; set; }

        /// <summary>
        /// Page: page_part_c_complaint_about_access<br/>
        /// Is your complaint about being denied access to your personal information?<br/>
        /// Possible choices: [yes, yes_and_other_concern, no]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string DeniedAccess { get; set; }

        /// <summary>
        /// Page: page_part_c_authorization_representative<br/>
        /// Are you filing this complaint on your own behalf (or for a minor child you ...<br/>
        /// Possible choices: [yourself, someone_else]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string FilingComplaintOnOwnBehalf { get; set; }

        /// <summary>
        /// Page: page_part_c_details_complaint<br/>
        /// Which organization is your complaint against? (maximum 5,000 characters)<br/>
        /// Survey question type: comment
        /// </summary>
        public string InstitutionComplaint { get; set; }

        /// <summary>
        /// Page: page_part_c_details_complaint<br/>
        /// What date did you send your written access request to the organization? If ...<br/>
        /// Survey question type: comment
        /// </summary>
        public string DateAccessRequests { get; set; }

        /// <summary>
        /// Page: page_part_c_details_complaint<br/>
        /// What exact address(es) did you send your written access request (postal, em...<br/>
        /// Survey question type: comment
        /// </summary>
        public string AddressesAccessRequest { get; set; }

        /// <summary>
        /// Page: page_part_c_details_complaint<br/>
        /// Please describe each item (if any) from your access request that you have n...<br/>
        /// Survey question type: comment
        /// </summary>
        public string ItemsNotReceived { get; set; }

        /// <summary>
        /// Page: page_part_c_details_complaint<br/>
        /// Summarize your complaint (maximum 5,000 characters) <br/>
        /// Survey question type: comment
        /// </summary>
        public string ComplaintSummary { get; set; }

        /// <summary>
        /// Page: page_part_c_details_complaint<br/>
        /// Please provide details for each complaint you have made to another body inc...<br/>
        /// Survey question type: comment
        /// </summary>
        public string ComplaintToOtherBodyDetails { get; set; }

        /// <summary>
        /// Page: page_part_c_details_complaint<br/>
        /// What would resolve your concerns? (maximum 5,000 characters)<br/>
        /// Survey question type: comment
        /// </summary>
        public string WhatWouldResolveYourComplaint { get; set; }

        /// <summary>
        /// Page: page_part_c_details_complaint<br/>
        /// Summarize your attempts to resolve the privacy matter with the organization...<br/>
        /// Survey question type: comment
        /// </summary>
        public string EffortsTakenToResolve { get; set; }

        /// <summary>
        /// Page: page_part_c_complaint_representative<br/>
        /// Have you submitted a complaint in the past with the Office of the Privacy C...<br/>
        /// Possible choices: [yes, no]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string Complainant_HaveYouSubmittedBeforeChoice { get; set; }

        /// <summary>
        /// Page: page_part_c_complaint_representative<br/>
        /// Preferred form of address<br/>
        /// Possible choices: [Mr., Mrs., Ms., Other]<br/>
        /// Survey question type: dropdown
        /// </summary>
        public string Complainant_FormOfAddress { get; set; }

        /// <summary>
        /// Page: page_part_c_complaint_representative<br/>
        /// First name<br/>
        /// Survey question type: text
        /// </summary>
        public string Complainant_FirstName { get; set; }

        /// <summary>
        /// Page: page_part_c_complaint_representative<br/>
        /// Last name<br/>
        /// Survey question type: text
        /// </summary>
        public string Complainant_LastName { get; set; }

        /// <summary>
        /// Page: page_part_c_complaint_representative<br/>
        /// E-mail address (yourname@domain.com)<br/>
        /// Survey question type: text
        /// </summary>
        public string Complainant_Email { get; set; }

        /// <summary>
        /// Page: page_part_c_complaint_representative<br/>
        /// Mailing address<br/>
        /// Survey question type: text
        /// </summary>
        public string Complainant_MailingAddress { get; set; }

        /// <summary>
        /// Page: page_part_c_complaint_representative<br/>
        /// City<br/>
        /// Survey question type: text
        /// </summary>
        public string Complainant_City { get; set; }

        /// <summary>
        /// Page: page_part_c_complaint_representative<br/>
        /// Country<br/>
        /// Possible choices: [/api/Country?lang={locale}]<br/>
        /// Survey question type: dropdown
        /// </summary>
        public string Complainant_Country { get; set; }

        /// <summary>
        /// Page: page_part_c_complaint_representative<br/>
        /// Province/Territory<br/>
        /// Possible choices: [/api/Province?lang={locale}]<br/>
        /// Required condition: {complainant_Country} = 'CA'<br/>
        /// Survey question type: dropdown
        /// </summary>
        public string Complainant_ProvinceOrState { get; set; }

        /// <summary>
        /// Page: page_part_c_complaint_representative<br/>
        /// Postal code<br/>
        /// Survey question type: text
        /// </summary>
        public string Complainant_PostalCode { get; set; }

        /// <summary>
        /// Page: page_part_c_complaint_representative<br/>
        /// Daytime telephone number (123-456-7890)<br/>
        /// Survey question type: text
        /// </summary>
        public string Complainant_DayTimeNumber { get; set; }

        /// <summary>
        /// Page: page_part_c_complaint_representative<br/>
        /// Extension (digits only)<br/>
        /// Survey question type: text
        /// </summary>
        public string Complainant_DayTimeNumberExtension { get; set; }

        /// <summary>
        /// Page: page_part_c_complaint_representative<br/>
        /// Alternate telephone number (123-456-7890)<br/>
        /// Survey question type: text
        /// </summary>
        public string Complainant_AltTelephoneNumber { get; set; }

        /// <summary>
        /// Page: page_part_c_complaint_representative<br/>
        /// Extension (digits only)<br/>
        /// Survey question type: text
        /// </summary>
        public string Complainant_AltTelephoneNumberExtension { get; set; }

        /// <summary>
        /// Page: page_part_c_complaint_representative<br/>
        /// Preferred form of address<br/>
        /// Possible choices: [Mr., Mrs., Ms., Other]<br/>
        /// Survey question type: dropdown
        /// </summary>
        public string Reprensentative_FormOfAddress { get; set; }

        /// <summary>
        /// Page: page_part_c_complaint_representative<br/>
        /// First name<br/>
        /// Survey question type: text
        /// </summary>
        public string Reprensentative_FirstName { get; set; }

        /// <summary>
        /// Page: page_part_c_complaint_representative<br/>
        /// Last name<br/>
        /// Survey question type: text
        /// </summary>
        public string Reprensentative_LastName { get; set; }

        /// <summary>
        /// Page: page_part_c_complaint_representative<br/>
        /// E-mail address (yourname@domain.com)<br/>
        /// Survey question type: text
        /// </summary>
        public string Reprensentative_Email { get; set; }

        /// <summary>
        /// Page: page_part_c_complaint_representative<br/>
        /// Mailing address<br/>
        /// Survey question type: text
        /// </summary>
        public string Reprensentative_MailingAddress { get; set; }

        /// <summary>
        /// Page: page_part_c_complaint_representative<br/>
        /// City<br/>
        /// Survey question type: text
        /// </summary>
        public string Reprensentative_City { get; set; }

        /// <summary>
        /// Page: page_part_c_complaint_representative<br/>
        /// Country<br/>
        /// Possible choices: [/api/Country?lang={locale}]<br/>
        /// Survey question type: dropdown
        /// </summary>
        public string Reprensentative_Country { get; set; }

        /// <summary>
        /// Page: page_part_c_complaint_representative<br/>
        /// Province/Territory<br/>
        /// Possible choices: [/api/Province?lang={locale}]<br/>
        /// Required condition: {reprensentative_Country} = 'CA'<br/>
        /// Survey question type: dropdown
        /// </summary>
        public string Reprensentative_ProvinceOrState { get; set; }

        /// <summary>
        /// Page: page_part_c_complaint_representative<br/>
        /// Postal code<br/>
        /// Survey question type: text
        /// </summary>
        public string Reprensentative_PostalCode { get; set; }

        /// <summary>
        /// Page: page_part_c_complaint_representative<br/>
        /// Daytime telephone number (123-456-7890)<br/>
        /// Survey question type: text
        /// </summary>
        public string Reprensentative_DayTimeNumber { get; set; }

        /// <summary>
        /// Page: page_part_c_complaint_representative<br/>
        /// Extension (digits only)<br/>
        /// Survey question type: text
        /// </summary>
        public string Reprensentative_DayTimeNumberExtension { get; set; }

        /// <summary>
        /// Page: page_part_c_complaint_representative<br/>
        /// Alternate telephone number (123-456-7890)<br/>
        /// Survey question type: text
        /// </summary>
        public string Reprensentative_AltTelephoneNumber { get; set; }

        /// <summary>
        /// Page: page_part_c_complaint_representative<br/>
        /// Extension (digits only)<br/>
        /// Survey question type: text
        /// </summary>
        public string Reprensentative_AltTelephoneNumberExtension { get; set; }

        /// <summary>
        /// Page: page_part_c_complaint_representative<br/>
        /// We are committed to ensuring that clients with disabilities have equal acce...<br/>
        /// Possible choices: [yes, no]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string NeedsDisabilityAccommodationChoice { get; set; }

        /// <summary>
        /// Page: page_part_c_complaint_representative<br/>
        /// If yes, please describe what accommodation measures you are requesting and ...<br/>
        /// Required condition: {NeedsDisabilityAccommodationChoice} contains 'yes'<br/>
        /// Survey question type: comment
        /// </summary>
        public string DisabilityAccommodation { get; set; }

        /// <summary>
        /// Page: page_part_c_documentation<br/>
        /// Do you wish to upload your supporting documents or send them by mail?<br/>
        /// Possible choices: [upload, mail, both, none]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string DocumentationType { get; set; }

        /// <summary>
        /// Page: page_part_c_documentation<br/>
        /// Survey question type: file
        /// </summary>
        public List<SurveyFile> Documentation_file_upload_rep { get; set; }

        /// <summary>
        /// Page: page_part_c_documentation<br/>
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

