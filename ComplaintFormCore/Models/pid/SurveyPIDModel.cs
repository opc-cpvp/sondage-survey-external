using System;
using System.Collections.Generic;

namespace ComplaintFormCore.Models
{
	public class SurveyPIDModel
	{
		public bool? IsLongSurvey { get; set; }
		public List<SurveyFile> SurveySummary { get; set; }
		public string ReferenceNumber { get; set; }
		public string Institution { get; set; }
		public bool? HasNotificationApproved { get; set; }
		public string ApproverName { get; set; }
		public string ApproverTitle { get; set; }
		public string ContactName { get; set; }
		public string ContactEmail { get; set; }
		public string ReferenceFileNumber { get; set; }
		public string TimingOfDisclosure { get; set; }
		public DateTime? DateOfDisclosure { get; set; }
		public string DisclosurePriorToNotificationExplanation { get; set; }
		public List<string> WasNotPracticalToNotifyPCPrior { get; set; }
		public string AnticipatedDateOfDisclosureKnown { get; set; }
		public DateTime? AnticipatedDateOfDisclosure { get; set; }
		public string AnticipatedDateOfDisclosureText { get; set; }
		public string LegislativeAuthority { get; set; }
		public string OtherOptionsForDisclosure { get; set; }
		public string OtherOptionsForDisclosureAdditonalInfo { get; set; }
		public List<string> DisclosureRecipients { get; set; }
		public List<DisclosureRecipients_law_enforcement> DisclosureRecipients_law_enforcement { get; set; }
		public List<string> DisclosureRecipients_FamilyMember { get; set; }
		public List<DisclosureRecipients_FamilyMember_Other> DisclosureRecipients_FamilyMember_Other { get; set; }
		public string DisclosureRecipients_named_representative { get; set; }
		public List<DisclosureRecipients_goc_institution> DisclosureRecipients_goc_institution { get; set; }
		public List<DisclosureRecipients_Regulatory> DisclosureRecipients_Regulatory { get; set; }
		public List<DisclosureRecipients_Jurisdiction> DisclosureRecipients_Jurisdiction { get; set; }
		public List<DisclosureRecipients_Media> DisclosureRecipients_Media { get; set; }
		public string DisclosureRecipients_Public { get; set; }
		public List<DisclosureRecipients_Other> DisclosureRecipients_Other { get; set; }
		public string MultipleIndividualsAddOption { get; set; }
		public List<MultipleIndividuals> MultipleIndividuals { get; set; }
		public List<SurveyFile> FileMultipleIndividuals { get; set; }
		public string DescriptionOfEvents { get; set; }
		public string RationalForDisclosure { get; set; }
		public bool? HasInstitutionNotifiedIndOfDisclosure { get; set; }
		public List<SurveyFile> FileSupplementaryDocumentations { get; set; }
	}

	public class DisclosureRecipients_law_enforcement
	{
		public string Item { get; set; }
	}

	public class DisclosureRecipients_FamilyMember_Other
	{
		public string Item { get; set; }
	}

	public class DisclosureRecipients_goc_institution
	{
		public string Item { get; set; }
	}

	public class DisclosureRecipients_Regulatory
	{
		public string Item { get; set; }
	}

	public class DisclosureRecipients_Jurisdiction
	{
		public string Item { get; set; }
	}

	public class DisclosureRecipients_Media
	{
		public string Item { get; set; }
	}

	public class DisclosureRecipients_Other
	{
		public string Item { get; set; }
	}

	public class MultipleIndividuals
	{
		public string Name { get; set; }
		public List<string> DataElementsDisclosed { get; set; }
		public string MedicalDisclosedExplanation { get; set; }
		public string FinancialDisclosedExplanation { get; set; }
		public string InteractionGOCDisclosedExplanation { get; set; }
		public List<string> DeathDisclosedExplanation { get; set; }
		public List<DeathDisclosedExplanation_Other> DeathDisclosedExplanation_Other { get; set; }
		public List<string> InfoLawEnforcementDisclosedExplanation { get; set; }
		public List<InfoLawEnforcementDisclosedExplanation_Other> InfoLawEnforcementDisclosedExplanation_Other { get; set; }
		public List<OtherDisclosedExplanation> OtherDisclosedExplanation { get; set; }
	}

	public class DeathDisclosedExplanation_Other
	{
		public string Item { get; set; }
	}

	public class InfoLawEnforcementDisclosedExplanation_Other
	{
		public string Item { get; set; }
	}

	public class OtherDisclosedExplanation
	{
		public string Item { get; set; }
	}

}
