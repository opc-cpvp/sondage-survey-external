using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ComplaintFormCore.Exceptions;
using ComplaintFormCore.Models;
using ComplaintFormCore.Resources;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Localization;
using PhoneNumbers;

namespace ComplaintFormCore.Web_Apis
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PASurveyController : ControllerBase
    {
        private readonly SharedLocalizer _localizer;

        public PASurveyController(IStringLocalizerFactory factory)
        {
            _localizer = new SharedLocalizer(factory);
        }

        [HttpPost]
        public IActionResult Complete([FromBody] SurveyPAModel model, [FromQuery] string complaintId)
        {
            // Thread.Sleep(4000);
            OPCProblemDetails problem = _Validate(model, complaintId);

            if (problem.Status != 200 || problem.ErrorMessages.Count > 0)
            {
                return BadRequest(problem);
            }

            return Ok(new { ReferenceNumber = Guid.NewGuid().ToString() });
        }

        [HttpPost]
        public IActionResult Validate([FromBody] SurveyPAModel model, [FromQuery] string complaintId)
        {
            OPCProblemDetails problem = _Validate(model, complaintId);

            if (problem.Status != 200 || problem.ErrorMessages.Count > 0)
            {
                throw new Exception("glkdfjgldfghldhg");
                return BadRequest(problem);
            }

            return Ok();
        }

        [HttpPost]
        public IActionResult ValidateAttachments([FromBody] List<SurveyFile> documentation_file_upload, [FromQuery] string complaintId)
        {
            OPCProblemDetails problem = ValidateAttachmentsSize(documentation_file_upload, complaintId);

            if (problem != null)
            {
                return BadRequest(problem);
            }

            return Ok();
        }

        private OPCProblemDetails _Validate(SurveyPAModel model, string complaintId)
        {
            OPCProblemDetails problems = new OPCProblemDetails()
            {
                Status = 200
            };

            OPCProblemDetails preliminaryInfoValidation = ValidatePreliminaryInfo(model);
            problems.ErrorMessages.AddRange(preliminaryInfoValidation.ErrorMessages);

            OPCProblemDetails stepsTakenValidation = ValidateStepsTaken(model);
            problems.ErrorMessages.AddRange(stepsTakenValidation.ErrorMessages);

            OPCProblemDetails natureOfComplaintValidation = ValidateNatureOfComplaint(model);
            problems.ErrorMessages.AddRange(natureOfComplaintValidation.ErrorMessages);

            OPCProblemDetails descriptionConcernsValidation = ValidateDescriptionOfConcerns(model);
            problems.ErrorMessages.AddRange(descriptionConcernsValidation.ErrorMessages); //

            OPCProblemDetails complainantValidation = ValidateComplainant(model);
            problems.ErrorMessages.AddRange(complainantValidation.ErrorMessages);

            OPCProblemDetails representativeValidation = ValidateReprensentative(model);
            problems.ErrorMessages.AddRange(representativeValidation.ErrorMessages);

            OPCProblemDetails documentationValidation = ValidateDocumentations(model, complaintId);
            problems.ErrorMessages.AddRange(documentationValidation.ErrorMessages);

            if (model.InformationIsTrue == null || model.InformationIsTrue.Count == 0 || string.IsNullOrWhiteSpace(model.InformationIsTrue[0]) || model.InformationIsTrue[0] != "yes")
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringSubmission("PleaseCertify"));
            }

            if (problems.ErrorMessages.Count > 0)
            {
                problems.Status = 400;
            }

            return problems;
        }

        private OPCProblemDetails ValidatePreliminaryInfo(SurveyPAModel model)
        {
            //  Validation of Part A: Preliminary information

            OPCProblemDetails problems = new OPCProblemDetails();

            if (string.IsNullOrWhiteSpace(model.FilingComplaintOnOwnBehalf))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("AtLeastOneItemMustBeSelected") + " - " + _localizer.GetLocalizedStringResource("FilingComplaintOnOwnBehalf"));
            }

            //  This validation has been added by me (JF) to make sure the choices are valid
            if(model.FilingComplaintOnOwnBehalf != "yourself" && model.FilingComplaintOnOwnBehalf != "someone_else")
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("AtLeastOneItemMustBeSelected") + " - " + _localizer.GetLocalizedStringResource("FilingComplaintOnOwnBehalf"));
            }

            if (string.IsNullOrWhiteSpace(model.WhichFederalGovernementInstitutionComplaintAgainst))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsRequired") + " - " + _localizer.GetLocalizedStringResource("WhichFederalGovernementInstitutionComplaintAgainst"));
            }

            //  This validation has been added by me (JF) to make sure the institution value is an intitution id as int
            if (int.TryParse(model.WhichFederalGovernementInstitutionComplaintAgainst, out int intitutionId) == false)
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsRequired") + " - " + _localizer.GetLocalizedStringResource("WhichFederalGovernementInstitutionComplaintAgainst"));

                //  TODO: validate the institution id against a valid list, probably call the web service /api/Institution
            }

            return problems;
        }

        private OPCProblemDetails ValidateStepsTaken(SurveyPAModel model)
        {
            OPCProblemDetails problems = new OPCProblemDetails();

            if (string.IsNullOrWhiteSpace(model.RaisedPrivacyToAtipCoordinator))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("AtLeastOneItemMustBeSelected") + " - " + _localizer.GetLocalizedStringResource("RaisedPrivacyToAtipCoordinator"));
            }
            else if (model.RaisedPrivacyToAtipCoordinator != "yes" && model.RaisedPrivacyToAtipCoordinator != "no" && model.RaisedPrivacyToAtipCoordinator != "not_sure")
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("AtLeastOneItemMustBeSelected") + " - " + _localizer.GetLocalizedStringResource("RaisedPrivacyToAtipCoordinator"));
            }

            return problems;
        }

        private OPCProblemDetails ValidateNatureOfComplaint(SurveyPAModel model)
        {
            OPCProblemDetails problems = new OPCProblemDetails();

            if (model.NatureOfComplaint.Count == 0)
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("AtLeastOneItemMustBeSelected") + " - " + _localizer.GetLocalizedStringResource("WhichOneOfTheseChoicesSummarizesYourComplaint"));
            }

            return problems;
        }

        private OPCProblemDetails ValidateDescriptionOfConcerns(SurveyPAModel model)
        {
            // Validation of Part C: Details (description of concerns) - Privacy complaint form (federal institution)

            OPCProblemDetails problems = new OPCProblemDetails();

            if (string.IsNullOrWhiteSpace(model.IsEmployee))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("AtLeastOneItemMustBeSelected") + " - " + _localizer.GetLocalizedStringSubmission("IsEmployee"));
            }
            else if(model.IsEmployee != "general_public" && model.IsEmployee != "employee_government")
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("AtLeastOneItemMustBeSelected") + " - " + _localizer.GetLocalizedStringSubmission("IsEmployee"));
            }

            //  Getting the checkboxes selected from the NatureOfComplaint page (page 5)
            var isAnyFirstFourSelected = model.NatureOfComplaint.Where(x => x == NatureOfComplaintType.NatureOfComplaintDelay.ToString() || x == NatureOfComplaintType.NatureOfComplaintDenialOfAccess.ToString() || x == NatureOfComplaintType.NatureOfComplaintExtensionOfTime.ToString() || x == NatureOfComplaintType.NatureOfComplaintLanguage.ToString()).Any();

            var isAnyLastFiveSelected = model.NatureOfComplaint.Where(x => x == NatureOfComplaintType.NatureOfComplaintOther.ToString() || x == NatureOfComplaintType.NatureOfComplaintRetentionAndDisposal.ToString() || x == NatureOfComplaintType.NatureOfComplaintUseAndDisclosure.ToString() || x == NatureOfComplaintType.NatureOfComplaintCollection.ToString() || x == NatureOfComplaintType.NatureOfComplaintCorrection.ToString()).Any();

            if (isAnyFirstFourSelected)
            {
                if (string.IsNullOrWhiteSpace(model.DateSentRequests))
                {
                    problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsRequired") + " - " + _localizer.GetLocalizedStringSubmission("DateSentRequests"));
                }

                if (string.IsNullOrWhiteSpace(model.WordingOfRequest))
                {
                    problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsRequired") + " - " + _localizer.GetLocalizedStringSubmission("WordingOfRequest"));
                }

                if (string.IsNullOrWhiteSpace(model.DateOfFinalAnswer))
                {
                    problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsRequired") + " - " + _localizer.GetLocalizedStringSubmission("DateOfFinalAnswer"));
                }

                if (string.IsNullOrWhiteSpace(model.DidNoRecordExist))
                {
                    problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsRequired") + " - " + _localizer.GetLocalizedStringSubmission("DidNoRecordExist"));
                }

                if (string.IsNullOrWhiteSpace(model.InstitutionAgreedRequestOnInformalBasis))
                {
                    problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsRequired") + " - " + _localizer.GetLocalizedStringSubmission("InstitutionAgreedRequestOnInformalBasis"));
                }

                // Fields should be 5000 characters max
                if ((model.DateSentRequests ?? "").Length > 5000)
                {
                    problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsOverCharacterLimit") + " - " + _localizer.GetLocalizedStringSubmission("DateSentRequests"));
                }
                
                if ((model.WordingOfRequest ?? "").Length > 5000)
                {
                    problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsOverCharacterLimit") + " - " + _localizer.GetLocalizedStringSubmission("WordingOfRequest"));
                }

                if ((model.MoreDetailsOfRequest ?? "").Length > 5000)
                {
                    problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsOverCharacterLimit") + " - " + _localizer.GetLocalizedStringSubmission("MoreDetailsOfRequest"));
                }

                if ((model.DateOfFinalAnswer ?? "").Length > 5000)
                {
                    problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsOverCharacterLimit") + " - " + _localizer.GetLocalizedStringSubmission("DateOfFinalAnswer"));
                }

                if ((model.PrivacyActSectionsApplied ?? "").Length > 5000)
                {
                    problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsOverCharacterLimit") + " - " + _localizer.GetLocalizedStringSubmission("PrivacyActSectionsApplied"));
                }

                if ((model.ItemsNotRecieved ?? "").Length > 5000)
                {
                    problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsOverCharacterLimit") + " - " + _localizer.GetLocalizedStringSubmission("ItemsNotRecieved"));
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

            // Validate these remaining fields for all the types of nature of complaints
            if ((model.AdditionalComments ?? "").Length > 5000)
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsOverCharacterLimit") + " - " + _localizer.GetLocalizedStringSubmission("AdditionalComments"));
            }

            if (string.IsNullOrWhiteSpace(model.WhatWouldResolveYourComplaint))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsRequired") + " - " + _localizer.GetLocalizedStringSubmission("WhatWouldResolveYourComplaintDetailsPA"));
            }

            if ((model.WhatWouldResolveYourComplaint ?? "").Length > 5000)
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsOverCharacterLimit") + " - " + _localizer.GetLocalizedStringSubmission("WhatWouldResolveYourComplaintDetailsPA"));
            }

            if (string.IsNullOrWhiteSpace(model.SummarizeAttemptsToResolvePrivacyMatter))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsRequired") + " - " + _localizer.GetLocalizedStringSubmission("SummarizeAttemptsToResolvePrivacyMatter"));
            }
          
            if ((model.SummarizeAttemptsToResolvePrivacyMatter ?? "").Length > 5000)
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsOverCharacterLimit") + " - " + _localizer.GetLocalizedStringSubmission("SummarizeAttemptsToResolvePrivacyMatter"));
            }

            return problems;
        }

        private OPCProblemDetails ValidateComplainant(SurveyPAModel model)
        {
            OPCProblemDetails problems = new OPCProblemDetails();

            if (string.IsNullOrWhiteSpace(model.Complainant_FormOfAddress))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsRequired") + " - " + _localizer.GetLocalizedStringSubmission("ComplainantInfo") + "." + _localizer.GetLocalizedStringSubmission("FormOfAddress")); 
            }

            if (string.IsNullOrWhiteSpace(model.Complainant_FirstName))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsRequired") + " - " + _localizer.GetLocalizedStringSubmission("ComplainantInfo") + "." + _localizer.GetLocalizedStringSubmission("FirstName"));
            }
            else if (model.Complainant_FirstName.Length > 50)
            {
                problems.ErrorMessages.Add(string.Format(_localizer.GetLocalizedStringResource("FieldIsOverCharacterLimit"), 50) + " - " + _localizer.GetLocalizedStringSubmission("ComplainantInfo") + "." + _localizer.GetLocalizedStringSubmission("FirstName"));
            }

            if (string.IsNullOrWhiteSpace(model.Complainant_LastName))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsRequired") + " - " + _localizer.GetLocalizedStringSubmission("ComplainantInfo") + "." + _localizer.GetLocalizedStringSubmission("LastName"));
            }

            else if (model.Complainant_LastName.Length > 50)
            {
                problems.ErrorMessages.Add(string.Format(_localizer.GetLocalizedStringResource("FieldIsOverCharacterLimit"), 50) + " - " + _localizer.GetLocalizedStringSubmission("ComplainantInfo") + "." + _localizer.GetLocalizedStringSubmission("LastName"));
            }

            if (string.IsNullOrWhiteSpace(model.Complainant_Email))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsRequired") + " - " + _localizer.GetLocalizedStringSubmission("ComplainantInfo") + "." + _localizer.GetLocalizedStringSubmission("EmailAddress"));
            }
            else
            {
                if (!IsEmailValid(model.Complainant_Email))
                {
                    problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("EmailInvalid") + " - " + _localizer.GetLocalizedStringSubmission("ComplainantInfo") + "." + _localizer.GetLocalizedStringSubmission("EmailAddress"));
                }

                if (model.Complainant_Email.Length > 100)
                {
                    problems.ErrorMessages.Add(string.Format(_localizer.GetLocalizedStringResource("FieldIsOverCharacterLimit"), 100) + " - " + _localizer.GetLocalizedStringSubmission("ComplainantInfo") + "." + _localizer.GetLocalizedStringSubmission("EmailAddress"));
                }
            }

            if (string.IsNullOrWhiteSpace(model.Complainant_MailingAddress))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsRequired") + " - " + _localizer.GetLocalizedStringSubmission("ComplainantInfo") + "." + _localizer.GetLocalizedStringSubmission("MailAddress"));
            }

            if (string.IsNullOrWhiteSpace(model.Complainant_City))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsRequired") + " - " + _localizer.GetLocalizedStringSubmission("ComplainantInfo") + "." + _localizer.GetLocalizedStringSubmission("City"));
            }
            else if (model.Complainant_City.Length > 90)
            {
                problems.ErrorMessages.Add(string.Format(_localizer.GetLocalizedStringResource("FieldIsOverCharacterLimit"), 90) + " - " + _localizer.GetLocalizedStringSubmission("ComplainantInfo") + "." + _localizer.GetLocalizedStringSubmission("City"));
            }

            if (string.IsNullOrWhiteSpace(model.Complainant_Country))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsRequired") + " - " + _localizer.GetLocalizedStringSubmission("ComplainantInfo") + "." + _localizer.GetLocalizedStringSubmission("Country"));
            }

            if (model.Complainant_Country == "CA" && string.IsNullOrWhiteSpace(model.Complainant_ProvinceOrState))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsRequired") + " - " + _localizer.GetLocalizedStringSubmission("ComplainantInfo") + "." + _localizer.GetLocalizedStringSubmission("ProvinceState"));
            }

            if (string.IsNullOrWhiteSpace(model.Complainant_PostalCode))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsRequired") + " - " + _localizer.GetLocalizedStringSubmission("ComplainantInfo") + "." + _localizer.GetLocalizedStringSubmission("PostalCode"));
            }

            if (!IsUsorCanadianZipCode(model.Complainant_PostalCode, model.Complainant_Country))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("PostalCodeInvalid") + " - " + _localizer.GetLocalizedStringSubmission("ComplainantInfo") + "." + _localizer.GetLocalizedStringSubmission("PostalCode"));
            }

            // Phone number should be required only for the complainant if no representative and only the representative if there's a representative
            var isPhoneNumberRequired = model.FilingComplaintOnOwnBehalf == "yourself";

            if (isPhoneNumberRequired && string.IsNullOrWhiteSpace(model.Complainant_DayTimeNumber))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsRequired") + " - " + _localizer.GetLocalizedStringSubmission("ComplainantInfo") + "." + _localizer.GetLocalizedStringSubmission("DaytimePhone"));
            }

            if (!string.IsNullOrWhiteSpace(model.Complainant_DayTimeNumber))
            {
                if (!IsPhoneNumberValid(model.Complainant_DayTimeNumber, model.Complainant_Country))
                {
                    problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("PhoneNumberInvalid") + " - " + _localizer.GetLocalizedStringSubmission("ComplainantInfo") + "." + _localizer.GetLocalizedStringSubmission("DaytimePhone"));                }
            }

            if (!string.IsNullOrWhiteSpace(model.Complainant_AltTelephoneNumber))

            {
                if (!IsPhoneNumberValid(model.Complainant_AltTelephoneNumber, model.Complainant_Country))
                {
                    problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("PhoneNumberInvalid") + " - " + _localizer.GetLocalizedStringSubmission("ComplainantInfo") + "." + _localizer.GetLocalizedStringSubmission("AlternatePhone"));
                }
            }

            // Check if the phone number extensions contain digits only
            if (string.IsNullOrWhiteSpace(model.Complainant_DayTimeNumberExtension) == false && !model.Complainant_DayTimeNumberExtension.All(char.IsDigit))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("CanOnlyContainDigits") + " - " + _localizer.GetLocalizedStringSubmission("ComplainantInfo") + "." + _localizer.GetLocalizedStringSubmission("PhoneExtension"));
            }

            // Check if the phone number extensions contain digits only
            if (model.Complainant_AltTelephoneNumberExtension != null && !model.Complainant_AltTelephoneNumberExtension.All(char.IsDigit))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("CanOnlyContainDigits") + " - " + _localizer.GetLocalizedStringSubmission("ComplainantInfo") + "." + _localizer.GetLocalizedStringSubmission("PhoneExtension"));
            }

            if (string.IsNullOrWhiteSpace(model.NeedsDisabilityAccommodation))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsRequired") + " - " + _localizer.GetLocalizedStringSubmission("ComplainantInfo") + "." + _localizer.GetLocalizedStringResource("NeedsDisabilityAccommodation"));
            }
            else if(model.NeedsDisabilityAccommodation == "yes" && string.IsNullOrWhiteSpace(model.DisabilityAccommodation))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsRequired") + " - " + _localizer.GetLocalizedStringSubmission("ComplainantInfo") + "." + _localizer.GetLocalizedStringResource("DisabilityAccommodation"));
            }

            return problems;
        }

        private OPCProblemDetails ValidateReprensentative(SurveyPAModel model)
        {
            OPCProblemDetails problems = new OPCProblemDetails();

            if (string.IsNullOrWhiteSpace(model.Complainant_FormOfAddress))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsRequired") + " - " + _localizer.GetLocalizedStringSubmission("RepresentativeInfo") + "." + _localizer.GetLocalizedStringSubmission("FormOfAddress"));
            }

            if (string.IsNullOrWhiteSpace(model.Complainant_FirstName))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsRequired") + " - " + _localizer.GetLocalizedStringSubmission("RepresentativeInfo") + "." + _localizer.GetLocalizedStringSubmission("FirstName"));
            }
            else if (model.Complainant_FirstName.Length > 50)
            {
                problems.ErrorMessages.Add(string.Format(_localizer.GetLocalizedStringResource("FieldIsOverCharacterLimit"), 50) + " - " + _localizer.GetLocalizedStringSubmission("RepresentativeInfo") + "." + _localizer.GetLocalizedStringSubmission("FirstName"));
            }

            if (string.IsNullOrWhiteSpace(model.Complainant_LastName))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsRequired") + " - " + _localizer.GetLocalizedStringSubmission("RepresentativeInfo") + "." + _localizer.GetLocalizedStringSubmission("LastName"));
            }

            else if (model.Complainant_LastName.Length > 50)
            {
                problems.ErrorMessages.Add(string.Format(_localizer.GetLocalizedStringResource("FieldIsOverCharacterLimit"), 50) + " - " + _localizer.GetLocalizedStringSubmission("RepresentativeInfo") + "." + _localizer.GetLocalizedStringSubmission("LastName"));
            }

            if (string.IsNullOrWhiteSpace(model.Complainant_Email))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsRequired") + " - " + _localizer.GetLocalizedStringSubmission("RepresentativeInfo") + "." + _localizer.GetLocalizedStringSubmission("EmailAddress"));
            }
            else
            {
                if (!IsEmailValid(model.Complainant_Email))
                {
                    problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("EmailInvalid") + " - " + _localizer.GetLocalizedStringSubmission("RepresentativeInfo") + "." + _localizer.GetLocalizedStringSubmission("EmailAddress"));
                }

                if (model.Complainant_Email.Length > 100)
                {
                    problems.ErrorMessages.Add(string.Format(_localizer.GetLocalizedStringResource("FieldIsOverCharacterLimit"), 100) + " - " + _localizer.GetLocalizedStringSubmission("RepresentativeInfo") + "." + _localizer.GetLocalizedStringSubmission("EmailAddress"));
                }
            }

            if (string.IsNullOrWhiteSpace(model.Complainant_MailingAddress))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsRequired") + " - " + _localizer.GetLocalizedStringSubmission("RepresentativeInfo") + "." + _localizer.GetLocalizedStringSubmission("MailAddress"));
            }

            if (string.IsNullOrWhiteSpace(model.Complainant_City))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsRequired") + " - " + _localizer.GetLocalizedStringSubmission("RepresentativeInfo") + "." + _localizer.GetLocalizedStringSubmission("City"));
            }
            else if (model.Complainant_City.Length > 90)
            {
                problems.ErrorMessages.Add(string.Format(_localizer.GetLocalizedStringResource("FieldIsOverCharacterLimit"), 90) + " - " + _localizer.GetLocalizedStringSubmission("RepresentativeInfo") + "." + _localizer.GetLocalizedStringSubmission("City"));
            }

            if (string.IsNullOrWhiteSpace(model.Complainant_Country))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsRequired") + " - " + _localizer.GetLocalizedStringSubmission("RepresentativeInfo") + "." + _localizer.GetLocalizedStringSubmission("Country"));
            }

            if (model.Complainant_Country == "CA" && string.IsNullOrWhiteSpace(model.Complainant_ProvinceOrState))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsRequired") + " - " + _localizer.GetLocalizedStringSubmission("RepresentativeInfo") + "." + _localizer.GetLocalizedStringSubmission("ProvinceState"));
            }

            if (string.IsNullOrWhiteSpace(model.Complainant_PostalCode))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsRequired") + " - " + _localizer.GetLocalizedStringSubmission("RepresentativeInfo") + "." + _localizer.GetLocalizedStringSubmission("PostalCode"));
            }

            if (!IsUsorCanadianZipCode(model.Complainant_PostalCode, model.Complainant_Country))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("PostalCodeInvalid") + " - " + _localizer.GetLocalizedStringSubmission("RepresentativeInfo") + "." + _localizer.GetLocalizedStringSubmission("PostalCode"));
            }

            // Phone number should be required only for the complainant if no representative and only the representative if there's a representative
            var isPhoneNumberRequired = model.FilingComplaintOnOwnBehalf == "someone_else";

            if (isPhoneNumberRequired && string.IsNullOrWhiteSpace(model.Complainant_DayTimeNumber))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("FieldIsRequired") + " - " + _localizer.GetLocalizedStringSubmission("RepresentativeInfo") + "." + _localizer.GetLocalizedStringSubmission("DaytimePhone"));
            }

            if (!string.IsNullOrWhiteSpace(model.Complainant_DayTimeNumber))
            {
                if (!IsPhoneNumberValid(model.Complainant_DayTimeNumber, model.Complainant_Country))
                {
                    problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("PhoneNumberInvalid") + " - " + _localizer.GetLocalizedStringSubmission("RepresentativeInfo") + "." + _localizer.GetLocalizedStringSubmission("DaytimePhone"));
                }
            }

            if (!string.IsNullOrWhiteSpace(model.Complainant_AltTelephoneNumber))

            {
                if (!IsPhoneNumberValid(model.Complainant_AltTelephoneNumber, model.Complainant_Country))
                {
                    problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("PhoneNumberInvalid") + " - " + _localizer.GetLocalizedStringSubmission("RepresentativeInfo") + "." + _localizer.GetLocalizedStringSubmission("AlternatePhone"));
                }
            }

            // Check if the phone number extensions contain digits only
            if (string.IsNullOrWhiteSpace(model.Complainant_DayTimeNumberExtension) == false && !model.Complainant_DayTimeNumberExtension.All(char.IsDigit))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("CanOnlyContainDigits") + " - " + _localizer.GetLocalizedStringSubmission("RepresentativeInfo") + "." + _localizer.GetLocalizedStringSubmission("PhoneExtension"));
            }

            // Check if the phone number extensions contain digits only
            if (model.Complainant_AltTelephoneNumberExtension != null && !model.Complainant_AltTelephoneNumberExtension.All(char.IsDigit))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("CanOnlyContainDigits") + " - " + _localizer.GetLocalizedStringSubmission("RepresentativeInfo") + "." + _localizer.GetLocalizedStringSubmission("PhoneExtension"));
            }

            return problems;
        }

        private OPCProblemDetails ValidateDocumentations(SurveyPAModel model, string complaintId)
        {
            OPCProblemDetails problems = new OPCProblemDetails();

            if (string.IsNullOrWhiteSpace(model.Documentation_type))
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("AtLeastOneItemMustBeSelected") + " - " + _localizer.GetLocalizedStringSubmission("Documentation"));
            }
            else if (model.Documentation_type != "upload" && model.Documentation_type != "mail" && model.Documentation_type != "both" && model.Documentation_type != "none")
            {
                problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("AtLeastOneItemMustBeSelected") + " - " + _localizer.GetLocalizedStringSubmission("Documentation"));
            }
            else if (model.Documentation_type == "upload" || model.Documentation_type != "both")
            {
                List<SurveyFile> attachmentsToValidateForSize = new List<SurveyFile>();

                if (model.Documentation_file_upload == null || model.Documentation_file_upload.Count == 0)
                {
                    problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("AtLeastOneFileHasToBeUploaded") + " - " + _localizer.GetLocalizedStringSubmission("Attachments"));
                }
                else
                {
                    attachmentsToValidateForSize = model.Documentation_file_upload;
                }

                if (model.Documentation_file_upload_rep == null || model.Documentation_file_upload_rep.Count == 0)
                {
                    problems.ErrorMessages.Add(_localizer.GetLocalizedStringResource("AtLeastOneFileHasToBeUploaded") + " - " + _localizer.GetLocalizedStringSubmission("AuthzFormAttachments"));
                }
                else
                {
                    attachmentsToValidateForSize.AddRange(model.Documentation_file_upload_rep);
                }

                OPCProblemDetails attachmentSizeProblems = ValidateAttachmentsSize(attachmentsToValidateForSize, complaintId);

                if (attachmentSizeProblems != null)
                {
                    problems.ErrorMessages.AddRange(attachmentSizeProblems.ErrorMessages);
                }
            }

            return problems;
        }

        private OPCProblemDetails ValidateAttachmentsSize(List<SurveyFile> documentationFileUpload, string complaintId)
        {
            long totalSizes = 0;
            long multipleFileMaxSize = 26214400;

            var folderName = Path.Combine("FileUploads", complaintId);
            var folderpath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            DirectoryInfo directory = new DirectoryInfo(folderpath);

            foreach (var file in directory.GetFiles())
            {
                if (documentationFileUpload.Where(f => f.name == file.Name).Any())
                {
                    totalSizes += file.Length;
                }
            }

            if (totalSizes > multipleFileMaxSize)
            {
                return new OPCProblemDetails
                {
                    Detail = _localizer.GetLocalizedStringSubmission("SizeOfFilesExceeded"),
                    Status = 400,
                    Title = "",
                    ErrorMessages = new List<string>() { _localizer.GetLocalizedStringSubmission("SizeOfFilesExceeded") + " - " + _localizer.GetLocalizedStringSubmission("Attachments") }
                };
            }

            return null;
        }

        private static readonly Regex _emailRegex = new Regex(@"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$", RegexOptions.Compiled);
        private static readonly Regex _USZipRegEx = new Regex(@"^\d{5}(?:[-\s]\d{4})?$", RegexOptions.Compiled);
        private static readonly Regex _CAZipRegEx = new Regex(@"^([ABCEGHJKLMNPRSTVXY]\d[ABCEGHJKLMNPRSTVWXYZ])\ {0,1}(\d[ABCEGHJKLMNPRSTVWXYZ]\d)$", RegexOptions.Compiled);
        private static readonly Regex _phoneNumberRegex = new Regex(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", RegexOptions.Compiled);

        private static bool IsEmailValid(string email) => !string.IsNullOrWhiteSpace(email) && _emailRegex.Match(email).Success;

        private static bool IsPhoneNumberValid(string number, string country)
        {
            // Validate the phone number if the country is Canada or US
            if (country == "CA" || country == "US")
            {
                var util = PhoneNumberUtil.GetInstance();
                var phoneNumber = new PhoneNumber();
                try
                {
                    phoneNumber = util.Parse(number, country);
                }
                catch (NumberParseException e)
                {
                    // This is not a number, return thats its not good
                    return false;
                }
                var phoneNumberIsValid = util.IsValidNumber(phoneNumber);
                return phoneNumberIsValid;
            }

            return true;
        }

        private static bool IsUsorCanadianZipCode(string zipCode, string country)
        {
            if (string.IsNullOrWhiteSpace(zipCode))
            {
                return false;
            }

            // Canada or US? Check if valid
            // Other, check if its not empty
            switch (country)
            {
                case "CA":
                    return _CAZipRegEx.Match(zipCode.ToUpper()).Success;
                case "US":
                    return _USZipRegEx.Match(zipCode.ToUpper()).Success;
                default:
                    return !string.IsNullOrEmpty(zipCode);
            }
        }
    }
}
