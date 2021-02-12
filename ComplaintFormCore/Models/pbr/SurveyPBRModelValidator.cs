using System.Collections.Generic;
using FluentValidation;
using System.Linq;
using ComplaintFormCore.Resources;
using ComplaintFormCore.Web_Apis.Models;

namespace ComplaintFormCore.Models
{
	public partial class SurveyPBRModelValidator : AbstractValidator<SurveyPBRModel>
	{
		public SurveyPBRModelValidator(SharedLocalizer _localizer)
		{
			ValidatorOptions.Global.CascadeMode = CascadeMode.Continue;

			// OrganizationName (Page: page_organization)
			RuleFor(x => x.OrganizationName).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.OrganizationName).Length(0, 250).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// OrganizationAddress (Page: page_organization)
			RuleFor(x => x.OrganizationAddress).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.OrganizationAddress).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// OrganizationCity (Page: page_organization)
			RuleFor(x => x.OrganizationCity).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.OrganizationCity).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// OrganizationProvince (Page: page_organization)
			RuleFor(x => x.OrganizationProvince).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// OrganizationProvinceOther (Page: page_organization)
			RuleFor(x => x.OrganizationProvinceOther).NotEmpty().When(x => x.OrganizationProvince == 14).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.OrganizationProvinceOther).Length(0, 200).When(x => x.OrganizationProvince == 14).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// OrganizationCountry (Page: page_organization)
			RuleFor(x => x.OrganizationCountry).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// OrganizationPostalCode (Page: page_organization)
			RuleFor(x => x.OrganizationPostalCode).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.OrganizationPostalCode).Length(0, 10).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// RepresentativeType (Page: page_organization)
			RuleFor(x => x.RepresentativeType).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.RepresentativeType).Must(x => new List<string> { "internal_representative", "external_representative" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

			// ContactName (Page: page_organization)
			RuleFor(x => x.ContactName).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.ContactName).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// ContactTitle (Page: page_organization)
			RuleFor(x => x.ContactTitle).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.ContactTitle).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// ContactPhoneWithCountryCode (Page: page_organization)
			RuleFor(x => x.ContactPhoneWithCountryCode).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.ContactPhoneWithCountryCode).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// ContactPhoneExtension (Page: page_organization)

			// ContactEmail (Page: page_organization)
			RuleFor(x => x.ContactEmail).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.ContactEmail).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
			RuleFor(x => x.ContactEmail).EmailAddress();

			// ContactAddress (Page: page_organization)
			RuleFor(x => x.ContactAddress).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.ContactAddress).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// ContactCity (Page: page_organization)
			RuleFor(x => x.ContactCity).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.ContactCity).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// ContactProvince (Page: page_organization)
			RuleFor(x => x.ContactProvince).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// ContactProvinceOther (Page: page_organization)
			RuleFor(x => x.ContactProvinceOther).NotEmpty().When(x => x.ContactProvince == 14).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.ContactProvinceOther).Length(0, 200).When(x => x.ContactProvince == 14).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// ContactCountry (Page: page_organization)
			RuleFor(x => x.ContactCountry).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// ContactPostalCode (Page: page_organization)
			RuleFor(x => x.ContactPostalCode).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.ContactPostalCode).Length(0, 10).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// NumberOfInvidualKnown (Page: page_breach_description_affected)
			RuleFor(x => x.NumberOfInvidualKnown).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// NumberOfInvidualAffected (Page: page_breach_description_affected)
			RuleFor(x => x.NumberOfInvidualAffected).NotEmpty().When(x => x.NumberOfInvidualKnown == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// NumberOfCanadiansAffected (Page: page_breach_description_affected)

			// NumberAffectedComment (Page: page_breach_description_affected)
			RuleFor(x => x.NumberAffectedComment).Length(0, 2000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// DateBreachOccurrenceStart (Page: page_breach_description_dates)
			RuleFor(x => x.DateBreachOccurrenceStart).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// DateBreachOccurrenceEnd (Page: page_breach_description_dates)

			// BreachOccurrenceComment (Page: page_breach_description_dates)
			RuleFor(x => x.BreachOccurrenceComment).Length(0, 2000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// TypeOfBreach (Page: page_breach_description_type)
			RuleFor(x => x.TypeOfBreach).Must(x => new List<string> { "accidental_disclosure", "loss_physical_devices", "theft_physical_devices", "unauthorized_access", "other" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

			// TypeOfBreachOther (Page: page_breach_description_type)
			RuleFor(x => x.TypeOfBreachOther).NotEmpty().When(x => x.TypeOfBreach == "other").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.TypeOfBreachOther).Length(0, 2000).When(x => x.TypeOfBreach == "other").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// BreachCircumstanceDescription (Page: page_breach_description_circumstances)
			RuleFor(x => x.BreachCircumstanceDescription).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.BreachCircumstanceDescription).Length(0, 2000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// SecuritySafeguardsDescription (Page: page_breach_descriptionsecurity)
			RuleFor(x => x.SecuritySafeguardsDescription).Length(0, 2000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// PersonalInformation (Page: page_breach_description_subject)
			RuleFor(x => x.PersonalInformation).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleForEach(x => x.PersonalInformation).Must(x => new List<string> { "name", "phone_number", "email_address", "account_number", "social_insurance_number" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

			// SubjectOfBreach (Page: page_breach_description_subject)
			RuleFor(x => x.SubjectOfBreach).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.SubjectOfBreach).Length(0, 2000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// IsAffectedNotified (Page: page_notification_steps)
			RuleFor(x => x.IsAffectedNotified).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// DateNotificationBegan (Page: page_notification_steps)
			RuleFor(x => x.DateNotificationBegan).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// DateNotificationCompleted (Page: page_notification_steps)

			// MethodOfNotification (Page: page_notification_steps)
			RuleFor(x => x.MethodOfNotification).Must(x => new List<string> { "directly", "indirectly", "directly_and_indirectly", "not_notified" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

			// IndirectlyNotificationDescription (Page: page_notification_steps)
			RuleFor(x => x.IndirectlyNotificationDescription).NotEmpty().When(x => x.MethodOfNotification == "indirectly").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.IndirectlyNotificationDescription).Length(0, 2000).When(x => x.MethodOfNotification == "indirectly").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// NotificationType (Page: page_notification_description)
			RuleFor(x => x.NotificationType).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleForEach(x => x.NotificationType).Must(x => new List<string> { "letter", "mail", "telephone", "newspaper" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

			// NotificationDescription (Page: page_notification_description)
			RuleFor(x => x.NotificationDescription).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.NotificationDescription).Length(0, 2000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// StepsReduceRisksDescription (Page: page_risk_mitigation_steps)
			RuleFor(x => x.StepsReduceRisksDescription).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
			RuleFor(x => x.StepsReduceRisksDescription).Length(0, 2000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// OrganizationNotified (Page: page_risk_mitigation_organization)
			RuleFor(x => x.OrganizationNotified).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// OrganizationDescription (Page: page_risk_mitigation_organization)
			RuleFor(x => x.OrganizationDescription).NotEmpty().When(x => x.OrganizationNotified == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

			// StepsReduceRisksFuture (Page: page_risk_mitigation)
			RuleFor(x => x.StepsReduceRisksFuture).Length(0, 2000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

			// OrganizationDescription
			RuleForEach(x => x.OrganizationDescription).ChildRules(child => {
				// Name (Page: page_risk_mitigation_organization)
				child.RuleFor(x => x.Name).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
			}).When(x => x.OrganizationNotified == true);

		}
	}
}

