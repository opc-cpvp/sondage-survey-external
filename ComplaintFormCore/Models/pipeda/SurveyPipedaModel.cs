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

    }
}

