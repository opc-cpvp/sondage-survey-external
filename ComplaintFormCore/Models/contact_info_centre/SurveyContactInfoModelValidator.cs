using System.Collections.Generic;
using FluentValidation;
using System.Linq;
using ComplaintFormCore.Resources;
using ComplaintFormCore.Web_Apis.Models;

namespace ComplaintFormCore.Models
{
	public partial class SurveyContactInfoModelValidator : AbstractValidator<SurveyContactInfoModel>
	{
		public SurveyContactInfoModelValidator(SharedLocalizer _localizer)
		{
			ValidatorOptions.Global.CascadeMode = CascadeMode.Continue;

			// FirstName (Page: page__start)
			RuleFor(x => x.FirstName).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// LastName (Page: page__start)
			RuleFor(x => x.LastName).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// PhoneNumber (Page: page__start)
			RuleFor(x => x.PhoneNumber).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// Email (Page: page__start)
			RuleFor(x => x.Email).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.Email).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
			RuleFor(x => x.Email).EmailAddress();

			// ProvinceOrState (Page: page__start)
			RuleFor(x => x.ProvinceOrState).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// ContactingUsAs (Page: page__start)
			RuleFor(x => x.ContactingUsAs).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.ContactingUsAs).Must(x => new List<string> { "individual", "federal_or_agency", "private_sector", "organization_school_hospital" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

			// QuestionsRelateTo (Page: page__start)
			RuleFor(x => x.QuestionsRelateTo).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.QuestionsRelateTo).Must(x => new List<string> { "individual", "federal_or_agency", "private_sector", "organization_school_hospital" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

			// TopicOfEnquiry (Page: page_topics)
			RuleForEach(x => x.TopicOfEnquiry).Must(x => new List<string> { "personal_information", "advertising_marketing", "air_travel", "authentication_identification", "behavioural_advertising", "biometrics", "businesses_collection_pi", "businesses_safeguarding_pi", "casl_spam", "cloud", "compliance", "consent", "driver_licence", "electronic_disclosure_tribunal_decisions", "complaint_pipeda", "genetic_privacy", "gps", "health_information", "identity_theft", "online_privacy", "jurisdiction", "landlords_tenants", "non_profit_sector", "OPC_role_mandate", "outsourcing", "personal_financial_information", "privacy_kids", "privacy_breaches", "impact_assessments", "privacy_policies", "safety_law_enforcement", "sin", "social_networking", "surveillance_monitoring", "technology_privacy", "workplace_privacy", "privacy_rights" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

			// QuestionToInformationCenterDetails (Page: page_question_details)
			RuleFor(x => x.QuestionToInformationCenterDetails).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.QuestionToInformationCenterDetails).Length(0, 2000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

		}
	}
}
