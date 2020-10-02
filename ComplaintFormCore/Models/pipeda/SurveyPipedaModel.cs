using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComplaintFormCore.Models.pipeda
{
    public class SurveyPipedaModel
    {
        /// <summary>
        /// Page: page_part_a_jurisdiction_province<br/>
        /// Question <br/>
        /// What province or territory did the incident or practice you are concerned a...<br/>
        /// </summary>
        public string ProvinceIncidence { get; set; }

        /// <summary>
        /// Page: page_part_a_jurisdiction_particulars<br/>
        /// Question <br/>
        /// Does your complaint relate to the handling of personal information outside ...<br/>
        /// Possible choices: [yes, no, not_sure]<br/>
        /// </summary>
        public string ComplaintAboutHandlingInformationOutsideProvince { get; set; }

        /// <summary>
        /// Page: page_part_a_jurisdiction_particulars<br/>
        /// Question <br/>
        /// Is your complaint against any of the following types of organizations?<br/>
        /// Possible choices: [yes, no]<br/>
        /// </summary>
        public string IsAgainstFwub { get; set; }

        /// <summary>
        /// Page: page_part_a_jurisdiction_particulars<br/>
        /// Question <br/>
        /// Did the { ProvinceIncidence } Privacy Commissioner specifically refer you t...<br/>
        /// Possible choices: [yes, no]<br/>
        /// </summary>
        public string DidOrganizationDirectComplaintToOpc { get; set; }

        /// <summary>
        /// Page: page_part_a_jurisdiction_provincial<br/>
        /// Question <br/>
        /// Is your complaint about the handling of personal information by any of the ...<br/>
        /// Possible choices: [yes, no]<br/>
        /// </summary>
        public string ComplaintAgainstHandlingOfInformation { get; set; }

        /// <summary>
        /// Page: page_part_a_jurisdiction_provincial<br/>
        /// Question <br/>
        /// Is your complaint about the handling of personal information by a health pr...<br/>
        /// Possible choices: [yes, no]<br/>
        /// </summary>
        public string HealthPractitioner { get; set; }

        /// <summary>
        /// Page: page_part_a_jurisdiction_provincial<br/>
        /// Question <br/>
        /// Is your complaint about the handling of personal information during or in r...<br/>
        /// Possible choices: [yes, no, not_applicable]<br/>
        /// </summary>
        public string IndependantPhysicalExam { get; set; }

        /// <summary>
        /// Page: page_part_a_customer_or_employee<br/>
        /// Question <br/>
        /// Are you submitting the complaint as a customer or as an employee of the org...<br/>
        /// Possible choices: [customer, employee, other]<br/>
        /// </summary>
        public string EmployeeOrCustomer { get; set; }

        /// <summary>
        /// Page: page_part_a_customer_or_employee<br/>
        /// Question <br/>
        /// Is your complaint against any of the following types of organizations?<br/>
        /// Possible choices: [yes, no]<br/>
        /// Required condition: {ProvinceIncidence} anyof [1,3,4,5,7,8,10]
        /// </summary>
        public string AgainstOrganizations { get; set; }

        /// <summary>
        /// Page: page_part_a_competence<br/>
        /// Question <br/>
        /// If your complaint is against any of the following types of organizations, p...<br/>
        /// Possible choices: [charity_non_profit, condominium_corporation, federal_government, first_nation, individual, journalism, political_party]<br/>
        /// </summary>
        public List<string> Question3Answers { get; set; }
    }
}
