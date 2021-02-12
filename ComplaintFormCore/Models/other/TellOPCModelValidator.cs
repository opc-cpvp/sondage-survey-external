using System.Collections.Generic;
using FluentValidation;
using System.Linq;
using ComplaintFormCore.Resources;
using ComplaintFormCore.Web_Apis.Models;

namespace ComplaintFormCore.Models
{
	public partial class SurveyTellOPCModelValidator : AbstractValidator<SurveyTellOPCModel>
	{
		public SurveyTellOPCModelValidator(SharedLocalizer _localizer)
		{
			ValidatorOptions.Global.CascadeMode = CascadeMode.Continue;

			// PrivacyConcern (Page: page_main)
			RuleFor(x => x.PrivacyConcern).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.PrivacyConcern).Length(0, 4096).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// Complainant_FirstName (Page: page_main)
			RuleFor(x => x.Complainant_FirstName).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// Complainant_LastName (Page: page_main)
			RuleFor(x => x.Complainant_LastName).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// Complainant_Email (Page: page_main)
			RuleFor(x => x.Complainant_Email).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
			RuleFor(x => x.Complainant_Email).EmailAddress()
			;

			// Complainant_PhoneNumber (Page: page_main)
			RuleFor(x => x.Complainant_PhoneNumber).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

		}
	}
}

