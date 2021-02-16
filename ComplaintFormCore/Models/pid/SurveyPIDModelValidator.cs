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

			// Institution (Page: page_q_1_0)
			RuleFor(x => x.Institution).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// HasNotificationApproved (Page: page_q_1_1)
			RuleFor(x => x.HasNotificationApproved).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// ApproverName (Page: page_q_1_1)
			RuleFor(x => x.ApproverName).NotEmpty().When(x => x.HasNotificationApproved == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.ApproverName).Length(0, 200).When(x => x.HasNotificationApproved == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// ApproverTitle (Page: page_q_1_1)
			RuleFor(x => x.ApproverTitle).NotEmpty().When(x => x.HasNotificationApproved == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.ApproverTitle).Length(0, 200).When(x => x.HasNotificationApproved == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// ContactName (Page: page_q_1_2)
			RuleFor(x => x.ContactName).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.ContactName).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// ContactEmail (Page: page_q_1_2)
			RuleFor(x => x.ContactEmail).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.ContactEmail).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
			RuleFor(x => x.ContactEmail).EmailAddress();

			// ReferenceFileNumber (Page: page_q_1_3)
			RuleFor(x => x.ReferenceFileNumber).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// TimingOfDisclosure (Page: page_q_2_0)
			RuleFor(x => x.TimingOfDisclosure).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.TimingOfDisclosure).Must(x => new List<string> { "already", "future" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

			// DateOfDisclosure (Page: page_q_2_0_a)
			RuleFor(x => x.DateOfDisclosure).NotEmpty().When(x => x.TimingOfDisclosure == "already").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// DisclosurePriorToNotificationExplanation (Page: page_q_2_0_a)
			RuleFor(x => x.DisclosurePriorToNotificationExplanation).NotEmpty().When(x => x.TimingOfDisclosure == "already").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.DisclosurePriorToNotificationExplanation).Length(0, 5000).When(x => x.TimingOfDisclosure == "already").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// WasNotPracticalToNotifyPCPrior (Page: page_q_2_0_a)
			RuleForEach(x => x.WasNotPracticalToNotifyPCPrior).Must(x => new List<string> { "not_practical" }.Contains(x)).When(x => x.TimingOfDisclosure == "already").WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

			// NotPracticalToNotifyOPCPriorAddiotnalInfo (Page: page_q_2_0_a)
			RuleFor(x => x.NotPracticalToNotifyOPCPriorAddiotnalInfo).Length(0, 5000).When(x => x.TimingOfDisclosure == "already" && x.WasNotPracticalToNotifyPCPrior != null && x.WasNotPracticalToNotifyPCPrior[0] == "not_practical").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// AnticipatedDateOfDisclosureKnown (Page: page_q_2_0_b)
			RuleFor(x => x.AnticipatedDateOfDisclosureKnown).NotEmpty().When(x => x.TimingOfDisclosure == "future").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.AnticipatedDateOfDisclosureKnown).Must(x => new List<string> { "yes", "no" }.Contains(x)).When(x => x.TimingOfDisclosure == "future").WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

			// AnticipatedDateOfDisclosure (Page: page_q_2_0_b)
			RuleFor(x => x.AnticipatedDateOfDisclosure).NotEmpty().When(x => x.TimingOfDisclosure == "future" && x.AnticipatedDateOfDisclosureKnown == "yes").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// AnticipatedDateOfDisclosureText (Page: page_q_2_0_b)
			RuleFor(x => x.AnticipatedDateOfDisclosureText).NotEmpty().When(x => x.TimingOfDisclosure == "future" && x.AnticipatedDateOfDisclosureKnown == "no").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.AnticipatedDateOfDisclosureText).Length(0, 5000).When(x => x.TimingOfDisclosure == "future" && x.AnticipatedDateOfDisclosureKnown == "no").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// LegislativeAuthority (Page: page_q_3_0)
			RuleFor(x => x.LegislativeAuthority).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// OtherOptionsForDisclosure (Page: page_q_3_1)
			RuleFor(x => x.OtherOptionsForDisclosure).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.OtherOptionsForDisclosure).Must(x => new List<string> { "yes", "no", "not_apply" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

			// OtherOptionsForDisclosureAdditonalInfo (Page: page_q_3_1)
			RuleFor(x => x.OtherOptionsForDisclosureAdditonalInfo).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// DisclosureRecipients (Page: page_q_4_0)
			RuleFor(x => x.DisclosureRecipients).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// DisclosingOfPIOneOrMultipleIndividual (Page: page_q_5_0)
			RuleFor(x => x.DisclosingOfPIOneOrMultipleIndividual).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.DisclosingOfPIOneOrMultipleIndividual).Must(x => new List<string> { "single", "multiple" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

			// OneIndividualPIDisclosedName (Page: page_q_5_1_a)
			RuleFor(x => x.OneIndividualPIDisclosedName).NotEmpty().When(x => x.DisclosingOfPIOneOrMultipleIndividual == "single").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.OneIndividualPIDisclosedName).Length(0, 200).When(x => x.DisclosingOfPIOneOrMultipleIndividual == "single").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// MultipleIndividualsAddOption (Page: page_q_5_1_b)
			RuleFor(x => x.MultipleIndividualsAddOption).NotEmpty().When(x => x.DisclosingOfPIOneOrMultipleIndividual == "multiple").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.MultipleIndividualsAddOption).Must(x => new List<string> { "directly", "upload" }.Contains(x)).When(x => x.DisclosingOfPIOneOrMultipleIndividual == "multiple").WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

			// MultipleIndividuals (Page: page_q_5_1_b)
			RuleFor(x => x.MultipleIndividuals).NotEmpty().When(x => x.DisclosingOfPIOneOrMultipleIndividual == "multiple" && x.MultipleIndividualsAddOption == "directly").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// FileMultipleIndividuals (Page: page_q_5_1_b)
			RuleFor(x => x.FileMultipleIndividuals).NotEmpty().When(x => x.DisclosingOfPIOneOrMultipleIndividual == "multiple" && x.MultipleIndividualsAddOption == "upload").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// DescriptionOfEvents (Page: page_q_6_0)
			RuleFor(x => x.DescriptionOfEvents).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// DataElementsDisclosed (Page: page_q_7_0)
			RuleFor(x => x.DataElementsDisclosed).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// MedicalDisclosedExplanation (Page: page_q_7_0)
			RuleFor(x => x.MedicalDisclosedExplanation).NotEmpty().When(x => x.DataElementsDisclosed.Contains("medical")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.MedicalDisclosedExplanation).Length(0, 5000).When(x => x.DataElementsDisclosed.Contains("medical")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// FinancialDisclosedExplanation (Page: page_q_7_0)
			RuleFor(x => x.FinancialDisclosedExplanation).NotEmpty().When(x => x.DataElementsDisclosed.Contains("financial")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.FinancialDisclosedExplanation).Length(0, 5000).When(x => x.DataElementsDisclosed.Contains("financial")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// InteractionGOCDisclosedExplanation (Page: page_q_7_0)
			RuleFor(x => x.InteractionGOCDisclosedExplanation).NotEmpty().When(x => x.DataElementsDisclosed.Contains("interaction_goc")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.InteractionGOCDisclosedExplanation).Length(0, 5000).When(x => x.DataElementsDisclosed.Contains("interaction_goc")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// DeathDisclosedExplanation (Page: page_q_7_0)
			RuleFor(x => x.DeathDisclosedExplanation).NotEmpty().When(x => x.DataElementsDisclosed.Contains("death")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// InfoLawEnforcementDisclosedExplanation (Page: page_q_7_0)
			RuleFor(x => x.InfoLawEnforcementDisclosedExplanation).NotEmpty().When(x => x.DataElementsDisclosed.Contains("law_enforcement")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// RationalForDisclosure (Page: page_q_8_0)
			RuleFor(x => x.RationalForDisclosure).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.RationalForDisclosure).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// HasInstitutionNotifiedIndOfDisclosure (Page: page_q_9_0)
			RuleFor(x => x.HasInstitutionNotifiedIndOfDisclosure).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.HasInstitutionNotifiedIndOfDisclosure).Must(x => new List<string> { "yes", "no" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

			// FileSupplementaryDocumentations (Page: page_q_10_0)

			// DisclosureRecipients
			RuleForEach(x => x.DisclosureRecipients).ChildRules(child => {
				// Recipient (Page: page_q_4_0)
				child.RuleFor(x => x.Recipient).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
				child.RuleFor(x => x.Recipient).Must(x => new List<string> { "law_enforcement", "family_member", "named_representative", "goc_institution", "regulatory", "gov_other_jurisdiction", "media", "public", "other" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));
			});

			RuleForEach(x => x.DisclosureRecipients).ChildRules(child => {
				// Info (Page: page_q_4_0)
				child.RuleFor(x => x.Info).Length(0, 5000).When(x => x.Recipient != "family_member").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
			});

			RuleForEach(x => x.DisclosureRecipients).ChildRules(child =>
			{
				// FamilyMember (Page: page_q_4_0)
				child.RuleFor(x => x.FamilyMember).NotEmpty().When(x => x.Recipient == "family_member").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			});

			// MultipleIndividuals
			RuleForEach(x => x.MultipleIndividuals).ChildRules(child => {
				// Name (Page: page_q_5_1_b)
				child.RuleFor(x => x.Name).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
			}).When(x => x.DisclosingOfPIOneOrMultipleIndividual == "multiple" && x.MultipleIndividualsAddOption == "directly");

		}
	}
}
