using System.Collections.Generic;
using System;

namespace ComplaintFormCore.Models
{
	public class SurveyPIDModel
	{
		/// <summary>
		/// Page: page_q_1_0<br/>
		/// Institution name<br/>
		/// Possible choices: [/api/Institution/GetAll?lang={locale}]<br/>
		/// Survey question type: dropdown
		/// </summary>
		public string Institution { get; set; }

		/// <summary>
		/// Page: page_q_1_1<br/>
		/// Before you proceed. Has this notification been formally approved in accorda...<br/>
		/// Survey question type: boolean
		/// </summary>
		public bool? HasNotificationApproved { get; set; }

		/// <summary>
		/// Page: page_q_1_1<br/>
		/// Please provide the name of the official responsible for approving this noti...<br/>
		/// Required condition: {HasNotificationApproved} = true<br/>
		/// Survey question type: text
		/// </summary>
		public string ApproverName { get; set; }

		/// <summary>
		/// Page: page_q_1_1<br/>
		/// Please provide the position title of the official responsible for approving...<br/>
		/// Required condition: {HasNotificationApproved} = true<br/>
		/// Survey question type: text
		/// </summary>
		public string ApproverTitle { get; set; }

		/// <summary>
		/// Page: page_q_1_2<br/>
		/// Name:<br/>
		/// Survey question type: text
		/// </summary>
		public string ContactName { get; set; }

		/// <summary>
		/// Page: page_q_1_2<br/>
		/// Email address:<br/>
		/// Survey question type: text (email)
		/// </summary>
		public string ContactEmail { get; set; }

		/// <summary>
		/// Page: page_q_1_3<br/>
		/// If applicable, please provide your institutional reference or file number<br/>
		/// Survey question type: text
		/// </summary>
		public string ReferenceFileNumber { get; set; }

		/// <summary>
		/// Page: page_q_2_0<br/>
		/// Please provide information about the timing of the disclosure<br/>
		/// Possible choices: [already, future]<br/>
		/// Survey question type: radiogroup
		/// </summary>
		public string TimingOfDisclosure { get; set; }

		/// <summary>
		/// Page: page_q_2_0_a<br/>
		/// Date of Disclosure<br/>
		/// Survey question type: datepicker (date)
		/// </summary>
		public DateTime? DateOfDisclosure { get; set; }

		/// <summary>
		/// Page: page_q_2_0_a<br/>
		/// Please explain why the disclosure occurred prior to notification<br/>
		/// Survey question type: comment
		/// </summary>
		public string DisclosurePriorToNotificationExplanation { get; set; }

		/// <summary>
		/// Page: page_q_2_0_a<br/>
		/// Notification to the Privacy Commissioner<br/>
		/// Possible choices: [not_practical]<br/>
		/// Survey question type: checkbox
		/// </summary>
		public List<string> WasNotPracticalToNotifyPCPrior { get; set; }

		/// <summary>
		/// Page: page_q_2_0_a<br/>
		/// Survey question type: comment
		/// </summary>
		public string NotPracticalToNotifyOPCPriorAddiotnalInfo { get; set; }

		/// <summary>
		/// Page: page_q_2_0_b<br/>
		/// Anticipated Date of Disclosure<br/>
		/// Possible choices: [yes, no]<br/>
		/// Survey question type: radiogroup
		/// </summary>
		public string AnticipatedDateOfDisclosureKnown { get; set; }

		/// <summary>
		/// Page: page_q_2_0_b<br/>
		/// Anticipated Date of Disclosure<br/>
		/// Required condition: {AnticipatedDateOfDisclosureKnown} contains 'yes'<br/>
		/// Survey question type: datepicker (date)
		/// </summary>
		public DateTime? AnticipatedDateOfDisclosure { get; set; }

		/// <summary>
		/// Page: page_q_2_0_b<br/>
		/// If you do not know the exact date, please use the open text box to enter an...<br/>
		/// Required condition: {AnticipatedDateOfDisclosureKnown} contains 'no'<br/>
		/// Survey question type: comment
		/// </summary>
		public string AnticipatedDateOfDisclosureText { get; set; }

		/// <summary>
		/// Page: page_q_3_0<br/>
		/// Please indicate under what legislative authority the disclosure was/will be...<br/>
		/// Possible choices: [public, individual, desda]<br/>
		/// Survey question type: checkbox
		/// </summary>
		public List<string> LegislativeAuthority { get; set; }

		/// <summary>
		/// Page: page_q_3_1<br/>
		/// Has the institution considered other options for this disclosure, such as s...<br/>
		/// Possible choices: [yes, no, not_apply]<br/>
		/// Survey question type: radiogroup
		/// </summary>
		public string OtherOptionsForDisclosure { get; set; }

		/// <summary>
		/// Page: page_q_3_1<br/>
		/// Additional information <br/>
		/// Survey question type: comment
		/// </summary>
		public string OtherOptionsForDisclosureAdditonalInfo { get; set; }

		/// <summary>
		/// Page: page_q_5_0<br/>
		/// Is the institution disclosing the personal information of one individual, o...<br/>
		/// Possible choices: [single, multiple]<br/>
		/// Survey question type: radiogroup
		/// </summary>
		public string DisclosingOfPIOneOrMultipleIndividual { get; set; }

		/// <summary>
		/// Page: page_q_5_1_a<br/>
		/// Please provide the name of the individual whose personal information was/wi...<br/>
		/// Survey question type: text
		/// </summary>
		public string OneIndividualPIDisclosedName { get; set; }

		/// <summary>
		/// Page: page_q_5_1_b<br/>
		/// Please provide the names of the individuals whose personal information was/...<br/>
		/// Possible choices: [directly, upload]<br/>
		/// Survey question type: radiogroup
		/// </summary>
		public string MultipleIndividualsAddOption { get; set; }

		/// <summary>
		/// Page: page_q_5_1_b<br/>
		/// Required condition: {MultipleIndividualsAddOption} contains 'upload'<br/>
		/// Survey question type: file
		/// </summary>
		public List<SurveyFile> FileMultipleIndividuals { get; set; }

		/// <summary>
		/// Page: page_q_6_0<br/>
		/// Description of events that lead to the disclosure<br/>
		/// Survey question type: comment
		/// </summary>
		public string DescriptionOfEvents { get; set; }

		/// <summary>
		/// Page: page_q_8_0<br/>
		/// Please provide rationale for disclosure. Your answer should include the pur...<br/>
		/// Survey question type: comment
		/// </summary>
		public string RationalForDisclosure { get; set; }

		/// <summary>
		/// Page: page_q_9_0<br/>
		/// Has your institution notified the affected individual(s) of the disclosure?<br/>
		/// Possible choices: [yes, no]<br/>
		/// Survey question type: radiogroup
		/// </summary>
		public string HasInstitutionNotifiedIndOfDisclosure { get; set; }

		/// <summary>
		/// Page: page_q_10_0<br/>
		/// Upload supplementary documentation<br/>
		/// Survey question type: file
		/// </summary>
		public List<SurveyFile> FileSupplementaryDocumentations { get; set; }


		public List<DataElementsDisclosed> DataElementsDisclosed { get; set; }

		public List<DisclosureRecipients> DisclosureRecipients { get; set; }

		public List<MultipleIndividuals> MultipleIndividuals { get; set; }

	}
	public class DataElementsDisclosed
	{
		/// <summary>
		/// Elements<br/>
		/// Possible choices: [name, dob, home_address, phone_number, email, death, law_enforcement, medical, financial, interaction_goc, other]<br/>
		/// Survey question type: dropdown
		/// </summary>
		public string DataElement { get; set; }

		/// <summary>
		/// Explain<br/>
		/// Required condition: {panel.DataElement} anyof ['medical','financial','interaction_goc','other']<br/>
		/// Survey question type: comment
		/// </summary>
		public string Info { get; set; }

		/// <summary>
		/// Information related to death<br/>
		/// Possible choices: [date, cause, reports_investigations]<br/>
		/// Survey question type: checkbox
		/// </summary>
		public List<string> InfoDeath { get; set; }

		/// <summary>
		/// Information related to law enforcement<br/>
		/// Possible choices: [charges, convictions, criminal_record]<br/>
		/// Survey question type: checkbox
		/// </summary>
		public List<string> InfoLawEnforcement { get; set; }

	}
	public class DisclosureRecipients
	{
		/// <summary>
		/// Recipients<br/>
		/// Possible choices: [law_enforcement, family_member, named_representative, goc_institution, regulatory, gov_other_jurisdiction, media, public, other]<br/>
		/// Survey question type: dropdown
		/// </summary>
		public string Recipient { get; set; }

		/// <summary>
		/// Explain<br/>
		/// Survey question type: comment
		/// </summary>
		public string Info { get; set; }

		/// <summary>
		/// Family Member<br/>
		/// Possible choices: [spouse, parent, sibling]<br/>
		/// Survey question type: checkbox
		/// </summary>
		public List<string> FamilyMember { get; set; }

	}
	public class MultipleIndividuals
	{
		/// <summary>
		/// Name<br/>
		/// Survey question type: text
		/// </summary>
		public string Name { get; set; }

	}
}
