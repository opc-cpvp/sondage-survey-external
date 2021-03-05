using System.Collections.Generic;
using FluentValidation;
using System.Linq;
using ComplaintFormCore.Resources;
using ComplaintFormCore.Web_Apis.Models;

namespace ComplaintFormCore.Models
{
	public partial class SurveyPIDModelValidator : AbstractValidator<SurveyPIDModel>
	{
		public SurveyPIDModelValidator(SharedLocalizer _localizer)
		{
			ValidatorOptions.Global.CascadeMode = CascadeMode.Continue;

			// IsLongSurvey (Page: page_intro)
			RuleFor(x => x.IsLongSurvey).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.IsLongSurvey).Must(x => new List<string> { "no", "yes" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

			// Institution (Page: page_q_1_0)
			RuleFor(x => x.Institution).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// HasNotificationApproved (Page: page_q_1_1)
			RuleFor(x => x.HasNotificationApproved).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// ApproverName (Page: page_q_1_1)
			RuleFor(x => x.ApproverName).NotEmpty().When(x => x.HasNotificationApproved == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.ApproverName).Length(0, 100).When(x => x.HasNotificationApproved == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// ApproverTitle (Page: page_q_1_1)
			RuleFor(x => x.ApproverTitle).NotEmpty().When(x => x.HasNotificationApproved == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.ApproverTitle).Length(0, 100).When(x => x.HasNotificationApproved == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// ContactName (Page: page_q_1_2)
			RuleFor(x => x.ContactName).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.ContactName).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// ContactEmail (Page: page_q_1_2)
			RuleFor(x => x.ContactEmail).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.ContactEmail).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
			RuleFor(x => x.ContactEmail).EmailAddress();

			// ReferenceFileNumber (Page: page_q_1_3)
			RuleFor(x => x.ReferenceFileNumber).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// TimingOfDisclosure (Page: page_q_2_0)
			RuleFor(x => x.TimingOfDisclosure).NotEmpty().When(x => x.IsLongSurvey == "yes").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.TimingOfDisclosure).Must(x => new List<string> { "already", "future" }.Contains(x)).When(x => x.IsLongSurvey == "yes").WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

			// DateOfDisclosure (Page: page_q_2_0_a)
			RuleFor(x => x.DateOfDisclosure).NotEmpty().When(x => x.IsLongSurvey == "yes" && x.TimingOfDisclosure == "already").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// DisclosurePriorToNotificationExplanation (Page: page_q_2_0_a)
			RuleFor(x => x.DisclosurePriorToNotificationExplanation).NotEmpty().When(x => x.IsLongSurvey == "yes" && x.TimingOfDisclosure == "already").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.DisclosurePriorToNotificationExplanation).Length(0, 2000).When(x => x.IsLongSurvey == "yes" && x.TimingOfDisclosure == "already").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// WasNotPracticalToNotifyPCPrior (Page: page_q_2_0_a)

			// AnticipatedDateOfDisclosureKnown (Page: page_q_2_0_b)
			RuleFor(x => x.AnticipatedDateOfDisclosureKnown).NotEmpty().When(x => x.IsLongSurvey == "yes" && x.TimingOfDisclosure == "future").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.AnticipatedDateOfDisclosureKnown).Must(x => new List<string> { "yes", "no" }.Contains(x)).When(x => x.IsLongSurvey == "yes" && x.TimingOfDisclosure == "future").WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

			// AnticipatedDateOfDisclosure (Page: page_q_2_0_b)
			RuleFor(x => x.AnticipatedDateOfDisclosure).NotEmpty().When(x => x.IsLongSurvey == "yes" && x.TimingOfDisclosure == "future" && x.AnticipatedDateOfDisclosureKnown == "yes").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// AnticipatedDateOfDisclosureText (Page: page_q_2_0_b)
			RuleFor(x => x.AnticipatedDateOfDisclosureText).NotEmpty().When(x => x.IsLongSurvey == "yes" && x.TimingOfDisclosure == "future" && x.AnticipatedDateOfDisclosureKnown == "no").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.AnticipatedDateOfDisclosureText).Length(0, 2000).When(x => x.IsLongSurvey == "yes" && x.TimingOfDisclosure == "future" && x.AnticipatedDateOfDisclosureKnown == "no").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// LegislativeAuthority (Page: page_q_3_0)
			RuleFor(x => x.LegislativeAuthority).NotEmpty().When(x => x.IsLongSurvey == "yes").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// OtherOptionsForDisclosure (Page: page_q_3_1)
			RuleFor(x => x.OtherOptionsForDisclosure).NotEmpty().When(x => x.IsLongSurvey == "yes").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.OtherOptionsForDisclosure).Must(x => new List<string> { "yes", "no", "not_apply" }.Contains(x)).When(x => x.IsLongSurvey == "yes").WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

			// OtherOptionsForDisclosureAdditonalInfo (Page: page_q_3_1)
			RuleFor(x => x.OtherOptionsForDisclosureAdditonalInfo).Length(0, 2000).When(x => x.IsLongSurvey == "yes").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// DisclosureRecipients (Page: page_q_4_0)
			RuleFor(x => x.DisclosureRecipients).NotEmpty().When(x => x.IsLongSurvey == "yes").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleForEach(x => x.DisclosureRecipients).Must(x => new List<string> { "law_enforcement", "family_member", "named_representative", "goc_institution", "regulatory", "gov_other_jurisdiction", "media", "public", "other" }.Contains(x)).When(x => x.IsLongSurvey == "yes").WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

			// DisclosureRecipients_law_enforcement (Page: page_q_4_0_details)
			RuleFor(x => x.DisclosureRecipients_law_enforcement).NotEmpty().When(x => x.IsLongSurvey == "yes" && x.DisclosureRecipients.Count == 1 && x.DisclosureRecipients.Contains("law_enforcement")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// DisclosureRecipients_FamilyMember (Page: page_q_4_0_details)
			RuleFor(x => x.DisclosureRecipients_FamilyMember).NotEmpty().When(x => x.IsLongSurvey == "yes" && x.DisclosureRecipients.Contains("family_member")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleForEach(x => x.DisclosureRecipients_FamilyMember).Must(x => new List<string> { "spouse", "parent", "sibling", "other" }.Contains(x)).When(x => x.IsLongSurvey == "yes" && x.DisclosureRecipients.Contains("family_member")).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

			// DisclosureRecipients_FamilyMember_Other (Page: page_q_4_0_details)
			RuleFor(x => x.DisclosureRecipients_FamilyMember_Other).NotEmpty().When(x => x.IsLongSurvey == "yes" && x.DisclosureRecipients.Contains("family_member") && x.DisclosureRecipients_FamilyMember.Contains("other")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// DisclosureRecipients_named_representative (Page: page_q_4_0_details)
			RuleFor(x => x.DisclosureRecipients_named_representative).NotEmpty().When(x => x.IsLongSurvey == "yes" && x.DisclosureRecipients.Contains("named_representative")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.DisclosureRecipients_named_representative).Length(0, 100).When(x => x.IsLongSurvey == "yes" && x.DisclosureRecipients.Contains("named_representative")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// DisclosureRecipients_goc_institution (Page: page_q_4_0_details)
			RuleFor(x => x.DisclosureRecipients_goc_institution).NotEmpty().When(x => x.IsLongSurvey == "yes" && x.DisclosureRecipients.Contains("goc_institution")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// DisclosureRecipients_Regulatory (Page: page_q_4_0_details)
			RuleFor(x => x.DisclosureRecipients_Regulatory).NotEmpty().When(x => x.IsLongSurvey == "yes" && x.DisclosureRecipients.Contains("regulatory")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// DisclosureRecipients_Jurisdiction (Page: page_q_4_0_details)
			RuleFor(x => x.DisclosureRecipients_Jurisdiction).NotEmpty().When(x => x.IsLongSurvey == "yes" && x.DisclosureRecipients.Contains("gov_other_jurisdiction")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// DisclosureRecipients_Media (Page: page_q_4_0_details)
			RuleFor(x => x.DisclosureRecipients_Media).NotEmpty().When(x => x.IsLongSurvey == "yes" && x.DisclosureRecipients.Contains("media")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// DisclosureRecipients_Public (Page: page_q_4_0_details)
			RuleFor(x => x.DisclosureRecipients_Public).NotEmpty().When(x => x.IsLongSurvey == "yes" && x.DisclosureRecipients.Contains("public")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.DisclosureRecipients_Public).Length(0, 100).When(x => x.IsLongSurvey == "yes" && x.DisclosureRecipients.Contains("public")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// DisclosureRecipients_Other (Page: page_q_4_0_details)
			RuleFor(x => x.DisclosureRecipients_Other).NotEmpty().When(x => x.IsLongSurvey == "yes" && x.DisclosureRecipients.Contains("other")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// MultipleIndividualsAddOption (Page: page_q_5_0)
			RuleFor(x => x.MultipleIndividualsAddOption).NotEmpty().When(x => x.IsLongSurvey == "yes").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.MultipleIndividualsAddOption).Must(x => new List<string> { "directly", "upload" }.Contains(x)).When(x => x.IsLongSurvey == "yes").WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

			// MultipleIndividuals (Page: page_q_5_0)
			RuleFor(x => x.MultipleIndividuals).NotEmpty().When(x => x.IsLongSurvey == "yes" && x.MultipleIndividualsAddOption == "directly").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// FileMultipleIndividuals (Page: page_q_5_0)
			RuleFor(x => x.FileMultipleIndividuals).NotEmpty().When(x => x.IsLongSurvey == "yes" && x.MultipleIndividualsAddOption == "upload").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// DescriptionOfEvents (Page: page_q_6_0)
			RuleFor(x => x.DescriptionOfEvents).Length(0, 2000).When(x => x.IsLongSurvey == "yes").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// DataElementsDisclosed (Page: page_q_7_0)
			RuleFor(x => x.DataElementsDisclosed).NotEmpty().When(x => x.IsLongSurvey == "yes").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleForEach(x => x.DataElementsDisclosed).Must(x => new List<string> { "name", "dob", "home_address", "phone_number", "email", "death", "law_enforcement", "medical", "financial", "interaction_goc", "other" }.Contains(x)).When(x => x.IsLongSurvey == "yes").WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

			// MedicalDisclosedExplanation (Page: page_q_7_0_details)
			RuleFor(x => x.MedicalDisclosedExplanation).NotEmpty().When(x => x.IsLongSurvey == "yes" && x.DataElementsDisclosed.Contains("medical")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.MedicalDisclosedExplanation).Length(0, 2000).When(x => x.IsLongSurvey == "yes" && x.DataElementsDisclosed.Contains("medical")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// FinancialDisclosedExplanation (Page: page_q_7_0_details)
			RuleFor(x => x.FinancialDisclosedExplanation).NotEmpty().When(x => x.IsLongSurvey == "yes" && x.DataElementsDisclosed.Contains("financial")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.FinancialDisclosedExplanation).Length(0, 2000).When(x => x.IsLongSurvey == "yes" && x.DataElementsDisclosed.Contains("financial")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// InteractionGOCDisclosedExplanation (Page: page_q_7_0_details)
			RuleFor(x => x.InteractionGOCDisclosedExplanation).NotEmpty().When(x => x.IsLongSurvey == "yes" && x.DataElementsDisclosed.Contains("interaction_goc")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.InteractionGOCDisclosedExplanation).Length(0, 2000).When(x => x.IsLongSurvey == "yes" && x.DataElementsDisclosed.Contains("interaction_goc")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// DeathDisclosedExplanation (Page: page_q_7_0_details)
			RuleFor(x => x.DeathDisclosedExplanation).NotEmpty().When(x => x.IsLongSurvey == "yes" && x.DataElementsDisclosed.Contains("death")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleForEach(x => x.DeathDisclosedExplanation).Must(x => new List<string> { "date", "cause", "reports_investigations", "other" }.Contains(x)).When(x => x.IsLongSurvey == "yes" && x.DataElementsDisclosed.Contains("death")).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

			// DeathDisclosedExplanation_Other (Page: page_q_7_0_details)
			RuleFor(x => x.DeathDisclosedExplanation_Other).NotEmpty().When(x => x.IsLongSurvey == "yes" && x.DataElementsDisclosed.Contains("death") && x.DeathDisclosedExplanation.Contains("other")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// InfoLawEnforcementDisclosedExplanation (Page: page_q_7_0_details)
			RuleFor(x => x.InfoLawEnforcementDisclosedExplanation).NotEmpty().When(x => x.IsLongSurvey == "yes" && x.DataElementsDisclosed.Contains("law_enforcement")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleForEach(x => x.InfoLawEnforcementDisclosedExplanation).Must(x => new List<string> { "charges", "convictions", "criminal_record", "other" }.Contains(x)).When(x => x.IsLongSurvey == "yes" && x.DataElementsDisclosed.Contains("law_enforcement")).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

			// InfoLawEnforcementDisclosedExplanation_Other (Page: page_q_7_0_details)
			RuleFor(x => x.InfoLawEnforcementDisclosedExplanation_Other).NotEmpty().When(x => x.IsLongSurvey == "yes" && x.DataElementsDisclosed.Contains("law_enforcement") && x.InfoLawEnforcementDisclosedExplanation.Contains("other")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// OtherDisclosedExplanation (Page: page_q_7_0_details)
			RuleFor(x => x.OtherDisclosedExplanation).NotEmpty().When(x => x.IsLongSurvey == "yes" && x.DataElementsDisclosed.Contains("other")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// RationalForDisclosure (Page: page_q_8_0)
			RuleFor(x => x.RationalForDisclosure).NotEmpty().When(x => x.IsLongSurvey == "yes").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.RationalForDisclosure).Length(0, 2000).When(x => x.IsLongSurvey == "yes").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// HasInstitutionNotifiedIndOfDisclosure (Page: page_q_9_0)
			RuleFor(x => x.HasInstitutionNotifiedIndOfDisclosure).NotEmpty().When(x => x.IsLongSurvey == "yes").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// FileSupplementaryDocumentations (Page: page_q_10_0)

			// DeathDisclosedExplanation_Other
			RuleForEach(x => x.DeathDisclosedExplanation_Other).ChildRules(child => {
				// Item (Page: page_q_7_0_details)
				child.RuleFor(x => x.Item).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
			}).When(x => x.IsLongSurvey == "yes" && x.DataElementsDisclosed.Contains("death") && x.DeathDisclosedExplanation.Contains("other"));

			// DisclosureRecipients_FamilyMember_Other
			RuleForEach(x => x.DisclosureRecipients_FamilyMember_Other).ChildRules(child => {
				// Item (Page: page_q_4_0_details)
				child.RuleFor(x => x.Item).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
			}).When(x => x.IsLongSurvey == "yes" && x.DisclosureRecipients.Contains("family_member") && x.DisclosureRecipients_FamilyMember.Contains("other"));

			// DisclosureRecipients_goc_institution
			RuleForEach(x => x.DisclosureRecipients_goc_institution).ChildRules(child => {
				// Item (Page: page_q_4_0_details)
				child.RuleFor(x => x.Item).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
			}).When(x => x.IsLongSurvey == "yes" && x.DisclosureRecipients.Contains("goc_institution"));

			// DisclosureRecipients_Jurisdiction
			RuleForEach(x => x.DisclosureRecipients_Jurisdiction).ChildRules(child => {
				// Item (Page: page_q_4_0_details)
				child.RuleFor(x => x.Item).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
			}).When(x => x.IsLongSurvey == "yes" && x.DisclosureRecipients.Contains("gov_other_jurisdiction"));

			// DisclosureRecipients_law_enforcement
			RuleForEach(x => x.DisclosureRecipients_law_enforcement).ChildRules(child => {
				// Item (Page: page_q_4_0_details)
				child.RuleFor(x => x.Item).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
			}).When(x => x.IsLongSurvey == "yes" && x.DisclosureRecipients.Count == 1 && x.DisclosureRecipients.Contains("law_enforcement"));

			// DisclosureRecipients_Media
			RuleForEach(x => x.DisclosureRecipients_Media).ChildRules(child => {
				// Item (Page: page_q_4_0_details)
				child.RuleFor(x => x.Item).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
			}).When(x => x.IsLongSurvey == "yes" && x.DisclosureRecipients.Contains("media"));

			// DisclosureRecipients_Other
			RuleForEach(x => x.DisclosureRecipients_Other).ChildRules(child => {
				// Item (Page: page_q_4_0_details)
				child.RuleFor(x => x.Item).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
			}).When(x => x.IsLongSurvey == "yes" && x.DisclosureRecipients.Contains("other"));

			// DisclosureRecipients_Regulatory
			RuleForEach(x => x.DisclosureRecipients_Regulatory).ChildRules(child => {
				// Item (Page: page_q_4_0_details)
				child.RuleFor(x => x.Item).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
			}).When(x => x.IsLongSurvey == "yes" && x.DisclosureRecipients.Contains("regulatory"));

			// InfoLawEnforcementDisclosedExplanation_Other
			RuleForEach(x => x.InfoLawEnforcementDisclosedExplanation_Other).ChildRules(child => {
				// Item (Page: page_q_7_0_details)
				child.RuleFor(x => x.Item).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
			}).When(x => x.IsLongSurvey == "yes" && x.DataElementsDisclosed.Contains("law_enforcement") && x.InfoLawEnforcementDisclosedExplanation.Contains("other"));

			// MultipleIndividuals
			RuleForEach(x => x.MultipleIndividuals).ChildRules(child => {
				// Name (Page: page_q_5_0)
				child.RuleFor(x => x.Name).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
			}).When(x => x.IsLongSurvey == "yes" && x.MultipleIndividualsAddOption == "directly");

			// OtherDisclosedExplanation
			RuleForEach(x => x.OtherDisclosedExplanation).ChildRules(child => {
				// Item (Page: page_q_7_0_details)
				child.RuleFor(x => x.Item).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
			}).When(x => x.IsLongSurvey == "yes" && x.DataElementsDisclosed.Contains("other"));

		}
	}
}
