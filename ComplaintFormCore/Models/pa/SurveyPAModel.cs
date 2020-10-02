
using ComplaintFormCore.Helpers;
using ComplaintFormCore.Resources;
using Microsoft.Extensions.Localization;
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ComplaintFormCore.Models
{
    public class SurveyPAModel : IValidatableObject
    {
        //private readonly IStringLocalizer<SurveyPAModel> _localizer;
        //public SurveyPAModel(IStringLocalizer<SurveyPAModel> localizer)
        //{
        //    _localizer = localizer;
        //}

        /// <summary>
        /// Page 1: page_preliminary_info_Authorization_for_Representative
        /// </summary>
        [Required]
        public string FilingComplaintOnOwnBehalf { get; set; }

        [Required]
        public bool? IsOnOwnBehalf
        {
            get
            {
                if(FilingComplaintOnOwnBehalf == "yourself")
                {
                    return true;
                }
                else if(FilingComplaintOnOwnBehalf == "someone_else")
                {
                    return false;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Page 2: page_preliminary_info_Identify_institution
        /// </summary>
        [Required]
        public string WhichFederalGovernementInstitutionComplaintAgainst { get; set; }

        [Required]
        public int? InstitutionId
        {
            get
            {
                if(int.TryParse(WhichFederalGovernementInstitutionComplaintAgainst, out int institutionId))
                {
                    return institutionId;
                }

                return null;
            }
        }

        /// <summary>
        /// Page 3: page_steps_taken_Writing_ATIP_Coordinator
        /// </summary>
        [Required]
        public string RaisedPrivacyToAtipCoordinator { get; set; }

        /// <summary>
        /// Page 5: page_details_Type_complaint
        /// </summary>
        [Required]
        public List<string> NatureOfComplaint { get; set; }

        /// <summary>
        /// Page 6: page_details_description_of_concerns
        /// </summary>
        [Required]
        public string IsEmployeeChoice { get; set; }

        [Required]
        public bool? IsEmployee
        {
            get
            {
                if(IsEmployeeChoice == "employee_government")
                {
                    return true;
                }
                else if (IsEmployeeChoice == "general_public")
                {
                    return false;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Page 6: page_details_description_of_concerns
        /// </summary>
        [StringLength(5000)]
        public string DateSentRequests { get; set; }

        /// <summary>
        /// Page 6: page_details_description_of_concerns
        /// </summary>
        [StringLength(5000)]
        public string WordingOfRequest { get; set; }

        /// <summary>
        /// Page 6: page_details_description_of_concerns
        /// </summary>
        [StringLength(5000)]
        public string MoreDetailsOfRequest { get; set; }

        /// <summary>
        /// Page 6: page_details_description_of_concerns
        /// </summary>
        [StringLength(5000)]
        public string DateOfFinalAnswer { get; set; }

        /// <summary>
        /// Page 6: page_details_description_of_concerns
        /// </summary>
        public string DidNoRecordExistChoice { get; set; }

        public bool? DidNoRecordExist
        {
            get
            {
                if (DidNoRecordExistChoice == "yes")
                {
                    return true;
                }
                else if (DidNoRecordExistChoice == "no")
                {
                    return false;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Page 6: page_details_description_of_concerns
        /// </summary>
        [StringLength(5000)]
        public string PrivacyActSectionsApplied { get; set; }

        /// <summary>
        /// Page 6: page_details_description_of_concerns
        /// </summary>
        [StringLength(5000)]
        public string ItemsNotRecieved { get; set; }

        /// <summary>
        /// Page 6: page_details_description_of_concerns
        /// </summary>
        public string InstitutionAgreedRequestOnInformalBasis { get; set; }

        /// <summary>
        /// Page 6: page_details_description_of_concerns
        /// </summary>
        [StringLength(5000)]
        public string SummarizeYourConcernsAndAnyStepsTaken { get; set; }

        /// <summary>
        /// Page 6: page_details_description_of_concerns
        /// </summary>
        [StringLength(5000)]
        public string SummarizeYourComplaintAndAnyStepsTaken { get; set; }

        /// <summary>
        /// Page 6: page_details_description_of_concerns
        /// </summary>
        [StringLength(5000)]
        [Required]
        public string WhatWouldResolveYourComplaint { get; set; }

        /// <summary>
        /// Page 6: page_details_description_of_concerns
        /// </summary>
        [StringLength(5000)]
        [Required]
        public string SummarizeAttemptsToResolvePrivacyMatter { get; set; }

        /// <summary>
        /// Page 6: page_details_description_of_concerns
        /// </summary>
        [StringLength(5000)]
        public string AdditionalComments { get; set; }

        /// <summary>
        /// Page 7: page_complainant_representative
        /// </summary>
        [Required]
        public string Complainant_HaveYouSubmittedBeforeChoice { get; set; }

        [Required]
        public bool? Complainant_HaveYouSubmittedBefore
        {
            get
            {
                if (Complainant_HaveYouSubmittedBeforeChoice == "yes")
                {
                    return true;
                }
                else if (Complainant_HaveYouSubmittedBeforeChoice == "no")
                {
                    return false;
                }
                else
                {
                    return null;
                }
            }
        }

        [RequiredIf(nameof(IsOnOwnBehalf), false, "This field is required")]
        public string Reprensentative_FormOfAddress { get; set; }

        [StringLength(50, ErrorMessage = "This field is over the 50 characters limit.")]
        [RequiredIf(nameof(IsOnOwnBehalf), false, "This field is required")]
        public string Reprensentative_FirstName { get; set; }

        [StringLength(50, ErrorMessage = "This field is over the 50 characters limit.")]
        [RequiredIf(nameof(IsOnOwnBehalf), false, "This field is required")]
        public string Reprensentative_LastName { get; set; }

        [StringLength(100, ErrorMessage = "This field is over the 100 characters limit.")]
        [RequiredIf(nameof(IsOnOwnBehalf), false, "This field is required")]
        public string Reprensentative_Email { get; set; }

        [RequiredIf(nameof(IsOnOwnBehalf), false, "This field is required")]
        public string Reprensentative_MailingAddress { get; set; }

        [RequiredIf(nameof(IsOnOwnBehalf), false, "This field is required")]
        public string Reprensentative_City { get; set; }

        [RequiredIf(nameof(IsOnOwnBehalf), false, "This field is required")]
        public string Reprensentative_Country { get; set; }

        public string Reprensentative_ProvinceOrState { get; set; }

        [RequiredIf(nameof(IsOnOwnBehalf), false, "This field is required")]
        public string Reprensentative_PostalCode { get; set; }

        public string Reprensentative_DayTimeNumber { get; set; }

        public string Reprensentative_DayTimeNumberExtension { get; set; }

        public string Reprensentative_AltTelephoneNumber { get; set; }

        public string Reprensentative_AltTelephoneNumberExtension { get; set; }

        [Required]
        public string Complainant_FormOfAddress { get; set; }

        [StringLength(50, ErrorMessage = "This field is over the 50 characters limit.")]
        [Required]
        public string Complainant_FirstName { get; set; }

        [StringLength(50, ErrorMessage = "This field is over the 50 characters limit.")]
        [Required]
        public string Complainant_LastName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "This field is over the 100 characters limit.")]
        public string Complainant_Email { get; set; }

        [Required]
        public string Complainant_MailingAddress { get; set; }

        [Required]
        [StringLength(90, ErrorMessage = "This field is over the 90 characters limit.")]
        public string Complainant_City { get; set; }

        [Required]
        public string Complainant_PostalCode { get; set; }

        [Required]
        public string Complainant_Country { get; set; }

        public string Complainant_ProvinceOrState { get; set; }

        public string Complainant_DayTimeNumber { get; set; }

        public string Complainant_DayTimeNumberExtension { get; set; }

        public string Complainant_AltTelephoneNumber { get; set; }

        public string Complainant_AltTelephoneNumberExtension { get; set; }

        [Required]
        public string NeedsDisabilityAccommodationChoice { get; set; }

        [Required]
        public bool? NeedsDisabilityAccommodation
        {
            get
            {
                if (NeedsDisabilityAccommodationChoice == "yes")
                {
                    return true;
                }
                else if (NeedsDisabilityAccommodationChoice == "no")
                {
                    return false;
                }
                else
                {
                    return null;
                }
            }
        }

        [RequiredIf(nameof(NeedsDisabilityAccommodation), true, "This field is required")]
        public string DisabilityAccommodation { get; set; }

        [Required]
        public string Documentation_type { get; set; }

        public List<SurveyFile> Documentation_file_upload { get; set; }

        public List<SurveyFile> Documentation_file_upload_rep { get; set; }

        [Required]
        public List<string> InformationIsTrue { get; set; }

        [Required]
        public bool IsCertified
        {
            get
            {
                if(InformationIsTrue.Count > 0 && InformationIsTrue[0] == "yes")
                {
                    return true;
                }

                return false;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (RaisedPrivacyToAtipCoordinator != "yes" && RaisedPrivacyToAtipCoordinator != "no" && RaisedPrivacyToAtipCoordinator != "not_sure")
            {
                yield return new ValidationResult("This field is required.", new[] { nameof(RaisedPrivacyToAtipCoordinator) });
            }

            #region NatureOfComplaint validation

            //  Getting the checkboxes selected from the NatureOfComplaint page (page 5)
            var isAnyFirstFourSelected = NatureOfComplaint.Where(x => x == NatureOfComplaintType.NatureOfComplaintDelay.ToString() || x == NatureOfComplaintType.NatureOfComplaintDenialOfAccess.ToString() || x == NatureOfComplaintType.NatureOfComplaintExtensionOfTime.ToString() || x == NatureOfComplaintType.NatureOfComplaintLanguage.ToString()).Any();

            var isAnyLastFiveSelected = NatureOfComplaint.Where(x => x == NatureOfComplaintType.NatureOfComplaintOther.ToString() || x == NatureOfComplaintType.NatureOfComplaintRetentionAndDisposal.ToString() || x == NatureOfComplaintType.NatureOfComplaintUseAndDisclosure.ToString() || x == NatureOfComplaintType.NatureOfComplaintCollection.ToString() || x == NatureOfComplaintType.NatureOfComplaintCorrection.ToString()).Any();

            if (isAnyFirstFourSelected)
            {
                if (string.IsNullOrWhiteSpace(DateSentRequests))
                {
                    yield return new ValidationResult("This field is required.", new[] { nameof(DateSentRequests) });
                }

                if (string.IsNullOrWhiteSpace(WordingOfRequest))
                {
                    yield return new ValidationResult("This field is required.", new[] { nameof(WordingOfRequest) });
                }

                if (string.IsNullOrWhiteSpace(DateOfFinalAnswer))
                {
                    yield return new ValidationResult("This field is required.", new[] { nameof(DateOfFinalAnswer) });
                }

                if (DidNoRecordExist == null)
                {
                    yield return new ValidationResult("This field is required.", new[] { nameof(DidNoRecordExist) });
                }

                if (string.IsNullOrWhiteSpace(InstitutionAgreedRequestOnInformalBasis))
                {
                    yield return new ValidationResult("This field is required.", new[] { nameof(InstitutionAgreedRequestOnInformalBasis) });
                }
            }

            if (isAnyLastFiveSelected)
            {
                //  I AM NOT SURE I GOT THE LOGIC RIGHT

                //if (string.IsNullOrWhiteSpace(model.ComplaintSummary))
                //    yield return new ValidationResult(Resource.FieldIsRequired, new[] { nameof(OtherNatureOfComplaint) + "." + nameof(OtherNatureOfComplaint.ComplaintSummary) });

                //if ((OtherNatureOfComplaint.ComplaintSummary ?? "").Length > 5000)
                //    yield return new ValidationResult(string.Format(Resource.FieldIsOverCharacterLimit, 5000), new[] { nameof(OtherNatureOfComplaint) + "." + nameof(OtherNatureOfComplaint.ComplaintSummary) });
            }
            #endregion

            #region Complainant & Representative Validation

            foreach (var complainantErrror in GetErrorsForComplainant())
            {
                yield return complainantErrror;
            }

            if(IsOnOwnBehalf == false)
            {
                foreach (var representativeErrror in GetErrorsForRepresentative())
                {
                    yield return representativeErrror;
                }
            }

            #endregion

            #region Document validation

            if (Documentation_type != "upload" && Documentation_type != "mail" && Documentation_type != "both" && Documentation_type != "none")
            {
                yield return new ValidationResult("This field is required.", new[] { nameof(Documentation_type) });
            }

            if (Documentation_type == "upload" || Documentation_type == "both")
            {
                if (Documentation_file_upload.Count == 0 && Documentation_file_upload_rep.Count == 0)
                {
                    yield return new ValidationResult("At least one file has to be uploaded.", new[] { nameof(Documentation_file_upload) });
                }
            }
            #endregion

            if (IsCertified == false)
            {
                yield return new ValidationResult("This field is required.", new[] { nameof(IsCertified) });
            }
        }

        private IEnumerable<ValidationResult> GetErrorsForComplainant()
        {
            if (!ValidatorHelpers.IsEmailValid(Complainant_Email))
            {
                yield return new ValidationResult("Please enter a valid email address.", new[] { nameof(Complainant_Email) });
            }

            if (Complainant_Country == "CA" && string.IsNullOrWhiteSpace(Complainant_ProvinceOrState))
            {
                yield return new ValidationResult("This field is required.", new[] { Complainant_ProvinceOrState });
            }

            if (!ValidatorHelpers.IsValidZipCode(Complainant_PostalCode, Complainant_Country))
            {
                yield return new ValidationResult("Please enter a valid postal code.", new[] { Complainant_PostalCode });
            }

            // Phone number should be required only for the complainant if no representative and only the representative if there's a representative
            var isComplainantPhoneNumberRequired = (bool)IsOnOwnBehalf;

            if (isComplainantPhoneNumberRequired && string.IsNullOrWhiteSpace(Complainant_DayTimeNumber))
            {
                yield return new ValidationResult("This field is required.", new[] { Complainant_DayTimeNumber });
            }

            if (!string.IsNullOrWhiteSpace(Complainant_DayTimeNumber))
            {
                if (!ValidatorHelpers.IsPhoneNumberValid(Complainant_DayTimeNumber, Complainant_Country))
                {
                    yield return new ValidationResult("Please enter a valid phone number.", new[] { Complainant_DayTimeNumber });
                }
            }

            if (!string.IsNullOrWhiteSpace(Complainant_AltTelephoneNumber))
            {
                if (!ValidatorHelpers.IsPhoneNumberValid(Complainant_AltTelephoneNumber, Complainant_Country))
                {
                    yield return new ValidationResult("Please enter a valid phone number.", new[] { Complainant_AltTelephoneNumber });
                }
            }

            // Check if the phone number extensions contain digits only
            if (Complainant_DayTimeNumberExtension != null && !Complainant_DayTimeNumberExtension.All(char.IsDigit))
            {
                yield return new ValidationResult("This field should only contain digits", new[] { Complainant_DayTimeNumberExtension });
            }
            // Check if the phone number extensions contain digits only
            if (Complainant_AltTelephoneNumberExtension != null && !Complainant_AltTelephoneNumberExtension.All(char.IsDigit))
            {
                yield return new ValidationResult("This field should only contain digits", new[] { Complainant_AltTelephoneNumberExtension });
            }
        }

        private IEnumerable<ValidationResult> GetErrorsForRepresentative()
        {
            if (!ValidatorHelpers.IsEmailValid(Reprensentative_Email))
            {
                yield return new ValidationResult("Please enter a valid email address.", new[] { nameof(Reprensentative_Email) });
            }

            if (Reprensentative_Country == "CA" && string.IsNullOrWhiteSpace(Reprensentative_ProvinceOrState))
            {
                yield return new ValidationResult("This field is required.", new[] { Reprensentative_ProvinceOrState });
            }

            if (!ValidatorHelpers.IsValidZipCode(Reprensentative_PostalCode, Reprensentative_Country))
            {
                yield return new ValidationResult("Please enter a valid postal code.", new[] { Reprensentative_PostalCode });
            }

            // Phone number should be required only for the complainant if no representative and only the representative if there's a representative
            var isComplainantPhoneNumberRequired = (bool)!IsOnOwnBehalf;

            if (isComplainantPhoneNumberRequired && string.IsNullOrWhiteSpace(Reprensentative_DayTimeNumber))
            {
                yield return new ValidationResult("This field is required.", new[] { Reprensentative_DayTimeNumber });
            }

            if (!string.IsNullOrWhiteSpace(Reprensentative_DayTimeNumber))
            {
                if (!ValidatorHelpers.IsPhoneNumberValid(Reprensentative_DayTimeNumber, Reprensentative_Country))
                {
                    yield return new ValidationResult("Please enter a valid phone number.", new[] { Reprensentative_DayTimeNumber });
                }
            }

            if (!string.IsNullOrWhiteSpace(Reprensentative_AltTelephoneNumber))
            {
                if (!ValidatorHelpers.IsPhoneNumberValid(Reprensentative_AltTelephoneNumber, Reprensentative_Country))
                {
                    yield return new ValidationResult("Please enter a valid phone number.", new[] { Reprensentative_AltTelephoneNumber });
                }
            }

            // Check if the phone number extensions contain digits only
            if (Reprensentative_DayTimeNumberExtension != null && !Reprensentative_DayTimeNumberExtension.All(char.IsDigit))
            {
                yield return new ValidationResult("This field should only contain digits", new[] { Reprensentative_DayTimeNumberExtension });
            }
            // Check if the phone number extensions contain digits only
            if (Reprensentative_AltTelephoneNumberExtension != null && !Reprensentative_AltTelephoneNumberExtension.All(char.IsDigit))
            {
                yield return new ValidationResult("This field should only contain digits", new[] { Reprensentative_AltTelephoneNumberExtension });
            }
        }
    }

    public enum NatureOfComplaintType
    {
        NatureOfComplaintDelay,
        NatureOfComplaintExtensionOfTime,
        NatureOfComplaintDenialOfAccess,
        NatureOfComplaintLanguage,
        NatureOfComplaintCorrection,
        NatureOfComplaintCollection,
        NatureOfComplaintUseAndDisclosure,
        NatureOfComplaintRetentionAndDisposal,
        NatureOfComplaintOther
    }
}
