using System.Collections.Generic;
using FluentValidation;
using ComplaintFormCore.Resources;
using ComplaintFormCore.Web_Apis;

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

            // OrganizationCity (Page: page_organization)
            RuleFor(x => x.OrganizationCity).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.OrganizationCity).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // OrganizationStreet (Page: page_organization)
            RuleFor(x => x.OrganizationStreet).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.OrganizationStreet).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // OrganizationProvince (Page: page_organization)
            RuleFor(x => x.OrganizationProvince).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

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

            // ContactStreetAdress (Page: page_organization)
            RuleFor(x => x.ContactStreetAdress).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ContactStreetAdress).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // ContactCity (Page: page_organization)
            RuleFor(x => x.ContactCity).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ContactCity).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // ContactCountry (Page: page_organization)
            RuleFor(x => x.ContactCountry).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // ContactProvince (Page: page_organization)
            RuleFor(x => x.ContactProvince).NotEmpty().When(x => x.ContactCountry == "CA").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // ContactOtherProvince (Page: page_organization)
            RuleFor(x => x.ContactOtherProvince).NotEmpty().When(x => x.ContactProvince == Province.OTHER_PROVINCE_ID.ToString()).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ContactOtherProvince).Length(0, 200).When(x => x.ContactProvince == Province.OTHER_PROVINCE_ID.ToString()).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // ContactPostalCode (Page: page_organization)
            RuleFor(x => x.ContactPostalCode).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ContactPostalCode).Length(0, 10).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // NumberOfInvidualAffected (Page: page_breach_description_affected)
            RuleFor(x => x.NumberOfInvidualAffected).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

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

            // BreachCircumstanceDescription (Page: page_breach_description_circumstances)
            RuleFor(x => x.BreachCircumstanceDescription).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.BreachCircumstanceDescription).Length(0, 2000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // SecuritySafeguardsDescription (Page: page_breach_descriptionsecurity)
            RuleFor(x => x.SecuritySafeguardsDescription).Length(0, 2000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

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

            // NotificationDescription (Page: page_notification_description)
            RuleFor(x => x.NotificationDescription).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.NotificationDescription).Length(0, 2000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // StepsReduceRisksDescription (Page: page_risk_mitigation_steps)
            RuleFor(x => x.StepsReduceRisksDescription).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.StepsReduceRisksDescription).Length(0, 2000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // OrganizationDescription (Page: page_risk_mitigation_organization)
            RuleFor(x => x.OrganizationDescription).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // StepsReduceRisksFuture (Page: page_risk_mitigation)
            RuleFor(x => x.StepsReduceRisksFuture).Length(0, 2000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            RuleForEach(x => x.OrganizationDescription).SetValidator(new OrganizationDescriptionValidator(_localizer));

        }
    }
    public partial class OrganizationDescriptionValidator : AbstractValidator<OrganizationDescription>
    {
        public OrganizationDescriptionValidator(SharedLocalizer _localizer)
        {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Continue;

            // Name
            RuleFor(x => x.Name).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // DateNotified

        }
    }
}

