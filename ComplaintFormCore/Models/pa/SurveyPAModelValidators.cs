using System.Collections.Generic;
using FluentValidation;
using ComplaintFormCore.Resources;
using ComplaintFormCore.Web_Apis.Models;
using System.Linq;

namespace ComplaintFormCore.Models.pa
{
    public partial class SurveyPAModelValidator : AbstractValidator<SurveyPAModel>
    {
        public SurveyPAModelValidator(SharedLocalizer _localizer)
        {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Continue;

            // FilingComplaintOnOwnBehalf (Page: page_preliminary_info_Authorization_for_Representative)
            RuleFor(x => x.FilingComplaintOnOwnBehalf).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.FilingComplaintOnOwnBehalf).Must(x => new List<string> { "yourself", "someone_else" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // WhichFederalGovernementInstitutionComplaintAgainst (Page: page_preliminary_info_Identify_institution)
            RuleFor(x => x.WhichFederalGovernementInstitutionComplaintAgainst).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // RaisedPrivacyToAtipCoordinator (Page: page_steps_taken_Writing_ATIP_Coordinator)
            RuleFor(x => x.RaisedPrivacyToAtipCoordinator).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.RaisedPrivacyToAtipCoordinator).Must(x => new List<string> { "yes", "no", "not_sure" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // NatureOfComplaint (Page: page_details_Type_complaint)
            RuleFor(x => x.NatureOfComplaint).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleForEach(x => x.NatureOfComplaint).Must(x => new List<string> { "NatureOfComplaintDelay", "NatureOfComplaintExtensionOfTime", "NatureOfComplaintDenialOfAccess", "NatureOfComplaintLanguage", "NatureOfComplaintCorrection", "NatureOfComplaintCollection", "NatureOfComplaintUseAndDisclosure", "NatureOfComplaintRetentionAndDisposal", "NatureOfComplaintOther" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // IsEmployee (Page: page_details_description_of_concerns)
            RuleFor(x => x.IsEmployee).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.IsEmployee).Must(x => new List<string> { "general_public", "employee_government" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // DateSentRequests (Page: page_details_description_of_concerns)
            RuleFor(x => x.DateSentRequests).NotEmpty().When(x => NatureOfComplaintIsAnyFirstFourSelected(x.NatureOfComplaint)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.DateSentRequests).Length(0, 5000).When(x => NatureOfComplaintIsAnyFirstFourSelected(x.NatureOfComplaint)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // WordingOfRequest (Page: page_details_description_of_concerns)
            RuleFor(x => x.WordingOfRequest).NotEmpty().When(x => NatureOfComplaintIsAnyFirstFourSelected(x.NatureOfComplaint)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.WordingOfRequest).Length(0, 5000).When(x => NatureOfComplaintIsAnyFirstFourSelected(x.NatureOfComplaint)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // MoreDetailsOfRequest (Page: page_details_description_of_concerns)
            RuleFor(x => x.MoreDetailsOfRequest).Length(0, 5000).When(x => NatureOfComplaintIsAnyFirstFourSelected(x.NatureOfComplaint)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // DateOfFinalAnswer (Page: page_details_description_of_concerns)
            RuleFor(x => x.DateOfFinalAnswer).NotEmpty().When(x => NatureOfComplaintIsAnyFirstFourSelected(x.NatureOfComplaint)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.DateOfFinalAnswer).Length(0, 5000).When(x => NatureOfComplaintIsAnyFirstFourSelected(x.NatureOfComplaint)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // DidNoRecordExist (Page: page_details_description_of_concerns)
            RuleFor(x => x.DidNoRecordExist).NotEmpty().When(x => NatureOfComplaintIsAnyFirstFourSelected(x.NatureOfComplaint)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.DidNoRecordExist).Must(x => new List<string> { "yes", "no" }.Contains(x)).When(x => NatureOfComplaintIsAnyFirstFourSelected(x.NatureOfComplaint)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // PrivacyActSectionsApplied (Page: page_details_description_of_concerns)
            RuleFor(x => x.PrivacyActSectionsApplied).Length(0, 5000).When(x => NatureOfComplaintIsAnyFirstFourSelected(x.NatureOfComplaint)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // ItemsNotRecieved (Page: page_details_description_of_concerns)
            RuleFor(x => x.ItemsNotRecieved).Length(0, 5000).When(x => NatureOfComplaintIsAnyFirstFourSelected(x.NatureOfComplaint)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // InstitutionAgreedRequestOnInformalBasis (Page: page_details_description_of_concerns)
            RuleFor(x => x.InstitutionAgreedRequestOnInformalBasis).NotEmpty().When(x => NatureOfComplaintIsAnyFirstFourSelected(x.NatureOfComplaint)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.InstitutionAgreedRequestOnInformalBasis).Must(x => new List<string> { "yes", "no", "not_sure" }.Contains(x)).When(x => NatureOfComplaintIsAnyFirstFourSelected(x.NatureOfComplaint)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // SummarizeYourConcernsAndAnyStepsTaken (Page: page_details_description_of_concerns)
            RuleFor(x => x.SummarizeYourConcernsAndAnyStepsTaken).NotEmpty().When(x => NatureOfComplaintIsAnyFirstFourSelected(x.NatureOfComplaint) && NatureOfComplaintIsAnyLastFiveSelected(x.NatureOfComplaint)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.SummarizeYourConcernsAndAnyStepsTaken).Length(0, 5000).When(x => NatureOfComplaintIsAnyFirstFourSelected(x.NatureOfComplaint) && NatureOfComplaintIsAnyLastFiveSelected(x.NatureOfComplaint)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // SummarizeYourComplaintAndAnyStepsTaken (Page: page_details_description_of_concerns)
            RuleFor(x => x.SummarizeYourComplaintAndAnyStepsTaken).NotEmpty().When(x => !NatureOfComplaintIsAnyFirstFourSelected(x.NatureOfComplaint) && NatureOfComplaintIsAnyLastFiveSelected(x.NatureOfComplaint)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.SummarizeYourComplaintAndAnyStepsTaken).Length(0, 5000).When(x => !NatureOfComplaintIsAnyFirstFourSelected(x.NatureOfComplaint) && NatureOfComplaintIsAnyLastFiveSelected(x.NatureOfComplaint)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // WhatWouldResolveYourComplaint (Page: page_details_description_of_concerns)
            RuleFor(x => x.WhatWouldResolveYourComplaint).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.WhatWouldResolveYourComplaint).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // SummarizeAttemptsToResolvePrivacyMatter (Page: page_details_description_of_concerns)
            RuleFor(x => x.SummarizeAttemptsToResolvePrivacyMatter).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.SummarizeAttemptsToResolvePrivacyMatter).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // AdditionalComments (Page: page_details_description_of_concerns)
            RuleFor(x => x.AdditionalComments).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Complainant_HaveYouSubmittedBefore (Page: page_Complainant_representative)
            RuleFor(x => x.Complainant_HaveYouSubmittedBefore).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Complainant_HaveYouSubmittedBefore).Must(x => new List<string> { "yes", "no" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // Complainant_FormOfAddress (Page: page_Complainant_representative)
            RuleFor(x => x.Complainant_FormOfAddress).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Complainant_FormOfAddress).Must(x => new List<string> { "Mr.", "Mrs.", "Ms.", "Other" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // Complainant_FirstName (Page: page_Complainant_representative)
            RuleFor(x => x.Complainant_FirstName).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Complainant_FirstName).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Complainant_LastName (Page: page_Complainant_representative)
            RuleFor(x => x.Complainant_LastName).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Complainant_LastName).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Complainant_Email (Page: page_Complainant_representative)
            RuleFor(x => x.Complainant_Email).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Complainant_Email).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
            RuleFor(x => x.Complainant_Email).EmailAddress();

            // Complainant_MailingAddress (Page: page_Complainant_representative)
            RuleFor(x => x.Complainant_MailingAddress).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Complainant_MailingAddress).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Complainant_City (Page: page_Complainant_representative)
            RuleFor(x => x.Complainant_City).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Complainant_City).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Complainant_Country (Page: page_Complainant_representative)
            RuleFor(x => x.Complainant_Country).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // Complainant_ProvinceOrState (Page: page_Complainant_representative)
            RuleFor(x => x.Complainant_ProvinceOrState).NotEmpty().When(x => x.Complainant_Country == "CA").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // Complainant_PostalCode (Page: page_Complainant_representative)
            RuleFor(x => x.Complainant_PostalCode).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Complainant_PostalCode).Length(0, 7).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Complainant_DayTimeNumber (Page: page_Complainant_representative)
            RuleFor(x => x.Complainant_DayTimeNumber).NotEmpty().When(x => x.FilingComplaintOnOwnBehalf == "yourself").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Complainant_DayTimeNumber).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Complainant_DayTimeNumberExtension (Page: page_Complainant_representative)

            // Complainant_AltTelephoneNumber (Page: page_Complainant_representative)
            RuleFor(x => x.Complainant_AltTelephoneNumber).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Complainant_AltTelephoneNumberExtension (Page: page_Complainant_representative)

            // Reprensentative_FormOfAddress (Page: page_Complainant_representative)
            RuleFor(x => x.Reprensentative_FormOfAddress).NotEmpty().When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Reprensentative_FormOfAddress).Must(x => new List<string> { "Mr.", "Mrs.", "Ms.", "Other" }.Contains(x)).When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // Reprensentative_FirstName (Page: page_Complainant_representative)
            RuleFor(x => x.Reprensentative_FirstName).NotEmpty().When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Reprensentative_FirstName).Length(0, 200).When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Reprensentative_LastName (Page: page_Complainant_representative)
            RuleFor(x => x.Reprensentative_LastName).NotEmpty().When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Reprensentative_LastName).Length(0, 200).When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Reprensentative_Email (Page: page_Complainant_representative)
            RuleFor(x => x.Reprensentative_Email).NotEmpty().When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Reprensentative_Email).Length(0, 200).When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
            RuleFor(x => x.Reprensentative_Email).EmailAddress();

            // Reprensentative_MailingAddress (Page: page_Complainant_representative)
            RuleFor(x => x.Reprensentative_MailingAddress).NotEmpty().When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Reprensentative_MailingAddress).Length(0, 200).When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Reprensentative_City (Page: page_Complainant_representative)
            RuleFor(x => x.Reprensentative_City).NotEmpty().When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Reprensentative_City).Length(0, 200).When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Reprensentative_Country (Page: page_Complainant_representative)
            RuleFor(x => x.Reprensentative_Country).NotEmpty().When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // Reprensentative_ProvinceOrState (Page: page_Complainant_representative)
            RuleFor(x => x.Reprensentative_ProvinceOrState).NotEmpty().When(x => x.FilingComplaintOnOwnBehalf == "someone_else" && x.Reprensentative_Country == "CA").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // Reprensentative_PostalCode (Page: page_Complainant_representative)
            RuleFor(x => x.Reprensentative_PostalCode).NotEmpty().When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Reprensentative_PostalCode).Length(0, 7).When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Reprensentative_DayTimeNumber (Page: page_Complainant_representative)
            RuleFor(x => x.Reprensentative_DayTimeNumber).NotEmpty().When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Reprensentative_DayTimeNumber).Length(0, 200).When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Reprensentative_DayTimeNumberExtension (Page: page_Complainant_representative)

            // Reprensentative_AltTelephoneNumber (Page: page_Complainant_representative)
            RuleFor(x => x.Reprensentative_AltTelephoneNumber).Length(0, 200).When(x => x.FilingComplaintOnOwnBehalf == "someone_else").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Reprensentative_AltTelephoneNumberExtension (Page: page_Complainant_representative)

            // NeedsDisabilityAccommodation (Page: page_Complainant_representative)
            RuleFor(x => x.NeedsDisabilityAccommodation).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.NeedsDisabilityAccommodation).Must(x => new List<string> { "yes", "no" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // DisabilityAccommodation (Page: page_Complainant_representative)
            RuleFor(x => x.DisabilityAccommodation).NotEmpty().When(x => x.NeedsDisabilityAccommodation == "yes").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.DisabilityAccommodation).Length(0, 5000).When(x => x.NeedsDisabilityAccommodation == "yes").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Documentation_type (Page: page_documentation)
            RuleFor(x => x.Documentation_type).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Documentation_type).Must(x => new List<string> { "upload", "mail", "both", "none" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // Documentation_file_upload_rep (Page: page_documentation)
            RuleFor(x => x.Documentation_file_upload_rep).NotEmpty().When(x => x.FilingComplaintOnOwnBehalf == "someone_else" && (new List<string>() { x.Documentation_type }.Intersect(new List<string>() { "upload", "both" }).Any())).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // Documentation_file_upload (Page: page_documentation)
            RuleFor(x => x.Documentation_file_upload).NotEmpty().When(x => new List<string>() { x.Documentation_type }.Intersect(new List<string>() { "upload", "both" }).Any()).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // InformationIsTrue (Page: )
            RuleFor(x => x.InformationIsTrue).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleForEach(x => x.InformationIsTrue).Must(x => new List<string> { "yes" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

        }

        private bool NatureOfComplaintIsAnyFirstFourSelected(List<string> natureOfComplaintSelectedItems)
        {
            if (natureOfComplaintSelectedItems.Contains("NatureOfComplaintDelay") || natureOfComplaintSelectedItems.Contains("NatureOfComplaintExtensionOfTime") || natureOfComplaintSelectedItems.Contains("NatureOfComplaintDenialOfAccess") || natureOfComplaintSelectedItems.Contains("NatureOfComplaintLanguage"))
            {
                return true;
            }

            return false;
        }

        private bool NatureOfComplaintIsAnyLastFiveSelected(List<string> natureOfComplaintSelectedItems)
        {
            if (natureOfComplaintSelectedItems.Contains("NatureOfComplaintCorrection") || natureOfComplaintSelectedItems.Contains("NatureOfComplaintCollection") || natureOfComplaintSelectedItems.Contains("NatureOfComplaintUseAndDisclosure") || natureOfComplaintSelectedItems.Contains("NatureOfComplaintRetentionAndDisposal") || natureOfComplaintSelectedItems.Contains("NatureOfComplaintOther"))
            {
                return true;
            }

            return false;
        }
    }
}
