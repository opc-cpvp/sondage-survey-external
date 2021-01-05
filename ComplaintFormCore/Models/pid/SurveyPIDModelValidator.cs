using ComplaintFormCore.Resources;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComplaintFormCore.Models
{
	public class SurveyPIDModelValidator : AbstractValidator<SurveyPIDModel>
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
			RuleForEach(x => x.OtherOptionsForDisclosure).Must(x => new List<string> { "yes", "no", "not_apply" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

			// OtherOptionsForDisclosureAdditonalInfo (Page: page_q_3_1)
			RuleFor(x => x.OtherOptionsForDisclosureAdditonalInfo).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
		}
	}
}
