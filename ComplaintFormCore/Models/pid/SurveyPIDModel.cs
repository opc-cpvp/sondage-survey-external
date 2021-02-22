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
		public int? Institution { get; set; }

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
		/// Possible choices: []<br/>
		/// Survey question type: checkbox
		/// </summary>
		public List<string> WasNotPracticalToNotifyPCPrior { get; set; }

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
		/// Survey question type: radiogroup
		/// </summary>
		public string LegislativeAuthority { get; set; }

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
		/// Page: page_q_4_0<br/>
		/// Please indicate the disclosure recipient(s)<br/>
		/// Possible choices: [law_enforcement, family_member, named_representative, goc_institution, regulatory, gov_other_jurisdiction, media, public, other]<br/>
		/// Survey question type: checkbox
		/// </summary>
		public List<string> DisclosureRecipients { get; set; }

		/// <summary>
		/// Page: page_q_4_0_details<br/>
		/// Please indicate which family member(s)<br/>
		/// Possible choices: [spouse, parent, sibling, other]<br/>
		/// Required condition: {DisclosureRecipients} contains 'family_member'<br/>
		/// Survey question type: checkbox
		/// </summary>
		public List<string> DisclosureRecipients_FamilyMember { get; set; }

		/// <summary>
		/// Page: page_q_5_0<br/>
		/// Please provide the names of the individuals whose personal information was/...<br/>
		/// Possible choices: [directly, upload]<br/>
		/// Survey question type: radiogroup
		/// </summary>
		public string MultipleIndividualsAddOption { get; set; }

		/// <summary>
		/// Page: page_q_5_0<br/>
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
		/// Page: page_q_7_0<br/>
		/// Data elements disclosed<br/>
		/// Possible choices: [name, dob, home_address, phone_number, email, death, law_enforcement, medical, financial, interaction_goc, other]<br/>
		/// Survey question type: checkbox
		/// </summary>
		public List<string> DataElementsDisclosed { get; set; }

		/// <summary>
		/// Page: page_q_7_0_details<br/>
		/// Explain the medical data elements disclosed<br/>
		/// Required condition: {DataElementsDisclosed} contains 'medical'<br/>
		/// Survey question type: comment
		/// </summary>
		public string MedicalDisclosedExplanation { get; set; }

		/// <summary>
		/// Page: page_q_7_0_details<br/>
		/// Explain the financial data elements disclosed<br/>
		/// Required condition: {DataElementsDisclosed} contains 'financial'<br/>
		/// Survey question type: comment
		/// </summary>
		public string FinancialDisclosedExplanation { get; set; }

		/// <summary>
		/// Page: page_q_7_0_details<br/>
		/// Explain the interaction with Government of Canada data elements disclosed<br/>
		/// Required condition: {DataElementsDisclosed} contains 'interaction_goc'<br/>
		/// Survey question type: comment
		/// </summary>
		public string InteractionGOCDisclosedExplanation { get; set; }

		/// <summary>
		/// Page: page_q_7_0_details<br/>
		/// Explain the information related to death data elements disclosed<br/>
		/// Possible choices: [date, cause, reports_investigations, other]<br/>
		/// Required condition: {DataElementsDisclosed} contains 'death'<br/>
		/// Survey question type: checkbox
		/// </summary>
		public List<string> DeathDisclosedExplanation { get; set; }

		/// <summary>
		/// Page: page_q_7_0_details<br/>
		/// Explain the law enforcement data elements disclosed<br/>
		/// Possible choices: [charges, convictions, criminal_record, other]<br/>
		/// Required condition: {DataElementsDisclosed} contains 'law_enforcement'<br/>
		/// Survey question type: checkbox
		/// </summary>
		public List<string> InfoLawEnforcementDisclosedExplanation { get; set; }

		/// <summary>
		/// Page: page_q_8_0<br/>
		/// Please provide rationale for disclosure. Your answer should include the pur...<br/>
		/// Survey question type: comment
		/// </summary>
		public string RationalForDisclosure { get; set; }

		/// <summary>
		/// Page: page_q_9_0<br/>
		/// Has your institution notified the affected individual(s) of the disclosure?<br/>
		/// Survey question type: boolean
		/// </summary>
		public bool? HasInstitutionNotifiedIndOfDisclosure { get; set; }

		/// <summary>
		/// Page: page_q_10_0<br/>
		/// Upload supplementary documentation<br/>
		/// Survey question type: file
		/// </summary>
		public List<SurveyFile> FileSupplementaryDocumentations { get; set; }


		public List<DeathDisclosedExplanation_Other> DeathDisclosedExplanation_Other { get; set; }

		public List<DisclosureRecipients_FamilyMember_Other> DisclosureRecipients_FamilyMember_Other { get; set; }

		public List<DisclosureRecipients_goc_institution> DisclosureRecipients_goc_institution { get; set; }

		public List<DisclosureRecipients_Jurisdiction> DisclosureRecipients_Jurisdiction { get; set; }

		public List<DisclosureRecipients_law_enforcement> DisclosureRecipients_law_enforcement { get; set; }

		public List<DisclosureRecipients_Media> DisclosureRecipients_Media { get; set; }

		public List<DisclosureRecipients_named_representative> DisclosureRecipients_named_representative { get; set; }

		public List<DisclosureRecipients_Public> DisclosureRecipients_Public { get; set; }

		public List<DisclosureRecipients_Regulatory> DisclosureRecipients_Regulatory { get; set; }

		public List<InfoLawEnforcementDisclosedExplanation_Other> InfoLawEnforcementDisclosedExplanation_Other { get; set; }

		public List<MultipleIndividuals> MultipleIndividuals { get; set; }

		public List<OtherDisclosedExplanation> OtherDisclosedExplanation { get; set; }

	}
	public class DeathDisclosedExplanation_Other
	{
		/// <summary>
		/// Added<br/>
		/// Survey question type: text
		/// </summary>
		public string Item { get; set; }

	}
	public class DisclosureRecipients_FamilyMember_Other
	{
		/// <summary>
		/// Added<br/>
		/// Survey question type: text
		/// </summary>
		public string Item { get; set; }

	}
	public class DisclosureRecipients_goc_institution
	{
		/// <summary>
		/// Added<br/>
		/// Survey question type: text
		/// </summary>
		public string Item { get; set; }

	}
	public class DisclosureRecipients_Jurisdiction
	{
		/// <summary>
		/// Added<br/>
		/// Survey question type: text
		/// </summary>
		public string Item { get; set; }

	}
	public class DisclosureRecipients_law_enforcement
	{
		/// <summary>
		/// Added<br/>
		/// Survey question type: text
		/// </summary>
		public string Item { get; set; }

	}
	public class DisclosureRecipients_Media
	{
		/// <summary>
		/// Added<br/>
		/// Survey question type: text
		/// </summary>
		public string Item { get; set; }

	}
	public class DisclosureRecipients_named_representative
	{
		/// <summary>
		/// Added<br/>
		/// Survey question type: text
		/// </summary>
		public string Item { get; set; }

	}
	public class DisclosureRecipients_Public
	{
		/// <summary>
		/// Added<br/>
		/// Survey question type: text
		/// </summary>
		public string Item { get; set; }

	}
	public class DisclosureRecipients_Regulatory
	{
		/// <summary>
		/// Added<br/>
		/// Survey question type: text
		/// </summary>
		public string Item { get; set; }

	}
	public class InfoLawEnforcementDisclosedExplanation_Other
	{
		/// <summary>
		/// Added<br/>
		/// Survey question type: text
		/// </summary>
		public string Item { get; set; }

	}
	public class MultipleIndividuals
	{
		/// <summary>
		/// Name<br/>
		/// Survey question type: text
		/// </summary>
		public string Name { get; set; }

	}
	public class OtherDisclosedExplanation
	{
		/// <summary>
		/// Added<br/>
		/// Survey question type: text
		/// </summary>
		public string Item { get; set; }

	}
}
