using ComplaintFormCore.Resources;
using FluentValidation;
using System.Collections.Generic;

namespace ComplaintFormCore.Models.pipeda
{
    public class SurveyPipedaModelValidator : AbstractValidator<SurveyPipedaModel>
    {
        public SurveyPipedaModelValidator(SharedLocalizer _localizer)
        {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Continue;

            // ProvinceIncidence (Page: page_part_a_jurisdiction_province)
            RuleFor(x => x.ProvinceIncidence).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ProvinceIncidence).Must(x => new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // ComplaintAboutHandlingInformationOutsideProvince (Page: page_part_a_jurisdiction_particulars)
            RuleFor(x => x.ComplaintAboutHandlingInformationOutsideProvince).NotEmpty().When(x => new List<string> { "2", "6", "9" }.Contains(x.ProvinceIncidence)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ComplaintAboutHandlingInformationOutsideProvince).Must(x => new List<string> { "yes", "no", "not_sure" }.Contains(x)).When(x => new List<string> { "2", "6", "9" }.Contains(x.ProvinceIncidence)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // IsAgainstFwub (Page: page_part_a_jurisdiction_particulars)
            RuleFor(x => x.IsAgainstFwub).NotEmpty().When(x => new List<string> { "2", "6", "9" }.Contains(x.ProvinceIncidence)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.IsAgainstFwub).Must(x => new List<string> { "yes", "no" }.Contains(x)).When(x => new List<string> { "2", "6", "9" }.Contains(x.ProvinceIncidence)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // DidOrganizationDirectComplaintToOpc (Page: page_part_a_jurisdiction_particulars)
            RuleFor(x => x.DidOrganizationDirectComplaintToOpc).NotEmpty().When(x => new List<string> { "2", "6", "9" }.Contains(x.ProvinceIncidence)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.DidOrganizationDirectComplaintToOpc).Must(x => new List<string> { "yes", "no" }.Contains(x)).When(x => new List<string> { "2", "6", "9" }.Contains(x.ProvinceIncidence)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // ComplaintAgainstHandlingOfInformation (Page: page_part_a_jurisdiction_provincial)
            RuleFor(x => x.ComplaintAgainstHandlingOfInformation).NotEmpty().When(x => new List<string> { "1", "3", "4", "5", "7", "8", "10" }.Contains(x.ProvinceIncidence)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ComplaintAgainstHandlingOfInformation).Must(x => new List<string> { "yes", "no" }.Contains(x)).When(x => new List<string> { "1", "3", "4", "5", "7", "8", "10" }.Contains(x.ProvinceIncidence)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // HealthPractitioner (Page: page_part_a_jurisdiction_provincial)
            RuleFor(x => x.HealthPractitioner).NotEmpty().When(x => new List<string> { "1", "3", "4", "5", "7", "8", "10" }.Contains(x.ProvinceIncidence)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.HealthPractitioner).Must(x => new List<string> { "yes", "no" }.Contains(x)).When(x => new List<string> { "1", "3", "4", "5", "7", "8", "10" }.Contains(x.ProvinceIncidence)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // IndependantPhysicalExam (Page: page_part_a_jurisdiction_provincial)
            RuleFor(x => x.IndependantPhysicalExam).NotEmpty().When(x => new List<string> { "1", "3", "4", "5", "7", "8", "10" }.Contains(x.ProvinceIncidence)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.IndependantPhysicalExam).Must(x => new List<string> { "yes", "no", "not_applicable" }.Contains(x)).When(x => new List<string> { "1", "3", "4", "5", "7", "8", "10" }.Contains(x.ProvinceIncidence)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // EmployeeOrCustomer (Page: page_part_a_customer_or_employee)
            RuleFor(x => x.EmployeeOrCustomer).NotEmpty().When(x => int.TryParse(x.ProvinceIncidence, out int val) && val <= 10).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.EmployeeOrCustomer).Must(x => new List<string> { "customer", "employee", "other" }.Contains(x)).When(x => int.TryParse(x.ProvinceIncidence, out int val) && val <= 10).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // AgainstOrganizations (Page: page_part_a_customer_or_employee)
            RuleFor(x => x.AgainstOrganizations).NotEmpty().When(x => int.TryParse(x.ProvinceIncidence, out int val) && val <= 10 && new List<string> { "1", "3", "4", "5", "7", "8", "10" }.Contains(x.ProvinceIncidence)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.AgainstOrganizations).Must(x => new List<string> { "yes", "no" }.Contains(x)).When(x => new List<string> { "1", "3", "4", "5", "7", "8", "10" }.Contains(x.ProvinceIncidence)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // Question3Answers (Page: page_part_a_competence)
            RuleForEach(x => x.Question3Answers).Must(x => new List<string> { "charity_non_profit", "condominium_corporation", "federal_government", "first_nation", "individual", "journalism", "political_party" }.Contains(x)).When(x => int.TryParse(x.ProvinceIncidence, out int val) && val <= 10).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // SubmittedComplaintToOtherBody (Page: page_part_b_another_body)
            RuleFor(x => x.SubmittedComplaintToOtherBody).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // ComplaintStillOngoing (Page: page_part_b_status_other_complaint)
            RuleFor(x => x.ComplaintStillOngoing).NotEmpty().When(x => x.SubmittedComplaintToOtherBody == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // AgainstTypeOrganizations (Page: page_part_b_means_recourse)
            RuleForEach(x => x.AgainstTypeOrganizations).Must(x => new List<string> { "collection_agency", "insurance_company", "landlord", "lawyer", "do_not_call_list", "unsubscribe", "phone_internet_provider", "realtor", "your_employer" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // RaisedConcernToPrivacyOfficer (Page: page_part_b_privacy_officer)
            RuleFor(x => x.RaisedConcernToPrivacyOfficer).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.RaisedConcernToPrivacyOfficer).Must(x => new List<string> { "yes", "no", "not_sure" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // DeniedAccess (Page: page_part_c_complaint_about_access)
            RuleFor(x => x.DeniedAccess).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.DeniedAccess).Must(x => new List<string> { "yes", "yes_and_other_concern", "no" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // FilingComplaintOnOwnBehalf (Page: page_part_c_authorization_representative)
            RuleFor(x => x.FilingComplaintOnOwnBehalf).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.FilingComplaintOnOwnBehalf).Must(x => new List<string> { "yourself", "someone_else" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // InstitutionComplaint (Page: page_part_c_details_complaint)
            RuleFor(x => x.InstitutionComplaint).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.InstitutionComplaint).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // DateAccessRequests (Page: page_part_c_details_complaint)
            RuleFor(x => x.DateAccessRequests).NotEmpty().When(x => x.DeniedAccess == "yes_and_other_concern" || x.DeniedAccess == "yes").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.DateAccessRequests).Length(0, 5000).When(x => x.DeniedAccess == "yes_and_other_concern" || x.DeniedAccess == "yes").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // AddressesAccessRequest (Page: page_part_c_details_complaint)
            RuleFor(x => x.AddressesAccessRequest).NotEmpty().When(x => x.DeniedAccess == "yes_and_other_concern" || x.DeniedAccess == "yes").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.AddressesAccessRequest).Length(0, 5000).When(x => x.DeniedAccess == "yes_and_other_concern" || x.DeniedAccess == "yes").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // ItemsNotReceived (Page: page_part_c_details_complaint)
            RuleFor(x => x.ItemsNotReceived).NotEmpty().When(x => x.DeniedAccess == "yes_and_other_concern" || x.DeniedAccess == "yes").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ItemsNotReceived).Length(0, 5000).When(x => x.DeniedAccess == "yes_and_other_concern" || x.DeniedAccess == "yes").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // ComplaintSummary (Page: page_part_c_details_complaint)
            RuleFor(x => x.ComplaintSummary).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ComplaintSummary).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // ComplaintToOtherBodyDetails (Page: page_part_c_details_complaint)
            RuleFor(x => x.ComplaintToOtherBodyDetails).NotEmpty().When(x => x.SubmittedComplaintToOtherBody == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ComplaintToOtherBodyDetails).Length(0, 5000).When(x => x.SubmittedComplaintToOtherBody == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // WhatWouldResolveYourComplaint (Page: page_part_c_details_complaint)
            RuleFor(x => x.WhatWouldResolveYourComplaint).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.WhatWouldResolveYourComplaint).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // EffortsTakenToResolve (Page: page_part_c_details_complaint)
            RuleFor(x => x.EffortsTakenToResolve).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.EffortsTakenToResolve).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Complainant_HaveYouSubmittedBeforeChoice (Page: page_part_c_complaint_representative)
            RuleFor(x => x.Complainant_HaveYouSubmittedBeforeChoice).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Complainant_HaveYouSubmittedBeforeChoice).Must(x => new List<string> { "yes", "no" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // Complainant_FormOfAddress (Page: page_part_c_complaint_representative)
            RuleFor(x => x.Complainant_FormOfAddress).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Complainant_FormOfAddress).Must(x => new List<string> { "Mr.", "Mrs.", "Ms.", "Other" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // Complainant_FirstName (Page: page_part_c_complaint_representative)
            RuleFor(x => x.Complainant_FirstName).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Complainant_FirstName).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Complainant_LastName (Page: page_part_c_complaint_representative)
            RuleFor(x => x.Complainant_LastName).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Complainant_LastName).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Complainant_Email (Page: page_part_c_complaint_representative)
            RuleFor(x => x.Complainant_Email).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Complainant_Email).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
            RuleFor(x => x.Complainant_Email).EmailAddress();

            // Complainant_MailingAddress (Page: page_part_c_complaint_representative)
            RuleFor(x => x.Complainant_MailingAddress).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Complainant_MailingAddress).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Complainant_City (Page: page_part_c_complaint_representative)
            RuleFor(x => x.Complainant_City).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Complainant_City).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // complainant_Country (Page: page_part_c_complaint_representative)
            RuleFor(x => x.Complainant_Country).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // Complainant_ProvinceOrState (Page: page_part_c_complaint_representative)
            RuleFor(x => x.Complainant_ProvinceOrState).NotEmpty().When(x => x.Complainant_Country == "CA").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // Complainant_PostalCode (Page: page_part_c_complaint_representative)
            RuleFor(x => x.Complainant_PostalCode).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Complainant_PostalCode).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Complainant_DayTimeNumber (Page: page_part_c_complaint_representative)
            RuleFor(x => x.Complainant_DayTimeNumber).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Complainant_DayTimeNumberExtension (Page: page_part_c_complaint_representative)
            RuleFor(x => x.Complainant_DayTimeNumberExtension).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Complainant_AltTelephoneNumber (Page: page_part_c_complaint_representative)
            RuleFor(x => x.Complainant_AltTelephoneNumber).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Complainant_AltTelephoneNumberExtension (Page: page_part_c_complaint_representative)
            RuleFor(x => x.Complainant_AltTelephoneNumberExtension).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Reprensentative_FormOfAddress (Page: page_part_c_complaint_representative)
            RuleFor(x => x.Reprensentative_FormOfAddress).NotEmpty().When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Reprensentative_FormOfAddress).Must(x => new List<string> { "Mr.", "Mrs.", "Ms.", "Other" }.Contains(x)).When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // Reprensentative_FirstName (Page: page_part_c_complaint_representative)
            RuleFor(x => x.Reprensentative_FirstName).NotEmpty().When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Reprensentative_FirstName).Length(0, 200).When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Reprensentative_LastName (Page: page_part_c_complaint_representative)
            RuleFor(x => x.Reprensentative_LastName).NotEmpty().When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Reprensentative_LastName).Length(0, 200).When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Reprensentative_Email (Page: page_part_c_complaint_representative)
            RuleFor(x => x.Reprensentative_Email).NotEmpty().When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Reprensentative_Email).Length(0, 200).When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
            RuleFor(x => x.Reprensentative_Email).EmailAddress();

            // Reprensentative_MailingAddress (Page: page_part_c_complaint_representative)
            RuleFor(x => x.Reprensentative_MailingAddress).NotEmpty().When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Reprensentative_MailingAddress).Length(0, 200).When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Reprensentative_City (Page: page_part_c_complaint_representative)
            RuleFor(x => x.Reprensentative_City).NotEmpty().When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Reprensentative_City).Length(0, 200).When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Reprensentative_Country (Page: page_part_c_complaint_representative)
            RuleFor(x => x.Reprensentative_Country).NotEmpty().When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // Reprensentative_ProvinceOrState (Page: page_part_c_complaint_representative)
            RuleFor(x => x.Reprensentative_ProvinceOrState).NotEmpty().When(x => x.FilingComplaintOnOwnBehalf == "someone_else" && x.Reprensentative_Country == "CA").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // Reprensentative_PostalCode (Page: page_part_c_complaint_representative)
            RuleFor(x => x.Reprensentative_PostalCode).NotEmpty().When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Reprensentative_PostalCode).Length(0, 200).When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Reprensentative_DayTimeNumber (Page: page_part_c_complaint_representative)
            RuleFor(x => x.Reprensentative_DayTimeNumber).NotEmpty().When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Reprensentative_DayTimeNumber).Length(0, 200).When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Reprensentative_DayTimeNumberExtension (Page: page_part_c_complaint_representative)
            RuleFor(x => x.Reprensentative_DayTimeNumberExtension).Length(0, 200).When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Reprensentative_AltTelephoneNumber (Page: page_part_c_complaint_representative)
            RuleFor(x => x.Reprensentative_AltTelephoneNumber).Length(0, 200).When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Reprensentative_AltTelephoneNumberExtension (Page: page_part_c_complaint_representative)
            RuleFor(x => x.Reprensentative_AltTelephoneNumberExtension).Length(0, 200).When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // NeedsDisabilityAccommodationChoice (Page: page_part_c_complaint_representative)
            RuleFor(x => x.NeedsDisabilityAccommodationChoice).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.NeedsDisabilityAccommodationChoice).Must(x => new List<string> { "yes", "no" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // DisabilityAccommodation (Page: page_part_c_complaint_representative)
            RuleFor(x => x.DisabilityAccommodation).NotEmpty().When(x => x.NeedsDisabilityAccommodationChoice == "yes").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.DisabilityAccommodation).Length(0, 5000).When(x => x.NeedsDisabilityAccommodationChoice == "yes").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // DocumentationType (Page: page_part_c_documentation)
            RuleFor(x => x.DocumentationType).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.DocumentationType).Must(x => new List<string> { "upload", "mail", "both", "none" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // Documentation_file_upload_rep (Page: page_part_c_documentation)
            RuleFor(x => x.Documentation_file_upload_rep).NotEmpty().When(x => x.FilingComplaintOnOwnBehalf == "someone_else" && (x.DocumentationType == "upload" || x.DocumentationType == "both")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // Documentation_file_upload (Page: page_part_c_documentation)
            RuleFor(x => x.Documentation_file_upload).NotEmpty().When(x => x.DocumentationType == "upload" || x.DocumentationType == "both").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // InformationIsTrue (Page: )
            RuleFor(x => x.InformationIsTrue).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleForEach(x => x.InformationIsTrue).Must(x => new List<string> { "yes" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

        }
    }
}
