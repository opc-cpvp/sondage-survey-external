using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ComplaintFormCore.Resources;
using System.Security.Cryptography.X509Certificates;
using ComplaintFormCore.Web_Apis.Models;

namespace ComplaintFormCore.Models.pia
{
    public class SurveyPIAToolModelValidator: AbstractValidator<SurveyPIAToolModel>
    {
        public SurveyPIAToolModelValidator(SharedLocalizer _localizer)
        {
            //this.CascadeMode = CascadeMode.Continue;
            ValidatorOptions.Global.CascadeMode = CascadeMode.Continue;

            //  NOTES:
            //  _localizer.GetLocalizedStringResource("FieldIsOverCharacterLimit")
            //  _localizer.GetLocalizedStringResource("AtLeastOneItemMustBeSelected")
            //  _localizer.GetLocalizedStringResource("EmailInvalid")
            //  _localizer.GetLocalizedStringResource("PhoneNumberInvalid")
            //  _localizer.GetLocalizedStringResource("PostalCodeInvalid")

            //  SAMPLES
            //  RuleFor(x => x.ContactATIPQ16).Must(x => new List<string> { "receive_email", "no_email", "conduct_pia" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("ItemNotValid"));
            //  RuleFor(x => x.ProgamHasMajorChanges).NotEmpty().When(y => y.IsNewprogram == false /*gfsdgf*/); //ItemNotValid
            //  RuleFor(x => x.HasLegalAuthority).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            //////////////////////////////////////////////////////////////////////////////////////

            // HasLegalAuthority
            RuleFor(x => x.HasLegalAuthority).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // RelevantLegislationPolicies
            RuleFor(x => x.RelevantLegislationPolicies).NotEmpty().When(x => x.HasLegalAuthority).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.RelevantLegislationPolicies).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // ProgramName
            RuleFor(x => x.ProgramName).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ProgramName).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // IsNewprogram
            RuleFor(x => x.IsNewprogram).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // ProgamHasMajorChanges
            RuleFor(x => x.ProgamHasMajorChanges).NotEmpty().When(x => !x.IsNewprogram).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // IsProgamContractedOut
            RuleFor(x => x.IsProgamContractedOut).NotEmpty().When(x => !x.IsNewprogram).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // IsProgramInvolvePersonalInformation
            RuleFor(x => x.IsProgramInvolvePersonalInformation).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // ContactATIPQ16
            RuleFor(x => x.ContactATIPQ16).NotEmpty().When(x => x.IsProgramInvolvePersonalInformation == false).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ContactATIPQ16).Must(x => new List<string> { "receive_email", "no_email", "conduct_pia", }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // UserEmailAddress
            RuleFor(x => x.UserEmailAddress).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.UserEmailAddress).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
            RuleFor(x => x.UserEmailAddress).EmailAddress();

            // PersonalInfoUsedFor
            RuleFor(x => x.PersonalInfoUsedFor).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.PersonalInfoUsedFor).Must(x => new List<string> { "admin_purpose", "non_admin_purpose", }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // ContactATIPQ18
            RuleFor(x => x.ContactATIPQ18).NotEmpty().When(x => x.PersonalInfoUsedFor == "non_admin_purpose").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ContactATIPQ18).Must(x => new List<string> { "receive_email", "no_email", "conduct_pia", }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // SubjectOfPIA
            RuleFor(x => x.SubjectOfPIA).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.SubjectOfPIA).Must(x => new List<string> { "program_activity", "other", }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // ContactATIPQ110
            RuleFor(x => x.ContactATIPQ110).NotEmpty().When(x => x.SubjectOfPIA == "other").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ContactATIPQ110).Must(x => new List<string> { "receive_email", "no_email", "conduct_pia", }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // SingleOrMultiInstitutionPIA
            RuleFor(x => x.SingleOrMultiInstitutionPIA).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.SingleOrMultiInstitutionPIA).Must(x => new List<string> { "single", "multi", "single_related", }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // RelatedPIANameInstitution
            RuleFor(x => x.RelatedPIANameInstitution).NotEmpty().When(x => x.SingleOrMultiInstitutionPIA == "single_related").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.RelatedPIANameInstitution).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // RelatedPIANameProgram
            RuleFor(x => x.RelatedPIANameProgram).NotEmpty().When(x => x.SingleOrMultiInstitutionPIA == "single_related").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.RelatedPIANameProgram).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // RelatedPIADescription
            RuleFor(x => x.RelatedPIADescription).NotEmpty().When(x => x.SingleOrMultiInstitutionPIA == "single_related").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.RelatedPIADescription).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // BehalfSingleInstitution
            RuleFor(x => x.BehalfSingleInstitution).NotEmpty().When(x => x.SingleOrMultiInstitutionPIA == "single" || x.SingleOrMultiInstitutionPIA == "singlerelated").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // BehalfSingleInstitutionOther
            RuleFor(x => x.BehalfSingleInstitutionOther).NotEmpty().When(x => (x.SingleOrMultiInstitutionPIA == "single" || x.SingleOrMultiInstitutionPIA == "single_related") && x.BehalfSingleInstitution == Institution.OTHER_INSTITUTION_ID.ToString()).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.BehalfSingleInstitutionOther).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // BehalfMultipleInstitutionLead
            RuleFor(x => x.BehalfMultipleInstitutionLead).NotEmpty().When(x => x.SingleOrMultiInstitutionPIA == "multi").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // BehalfMultipleInstitutionLeadOther
            RuleFor(x => x.BehalfMultipleInstitutionLeadOther).NotEmpty().When(x => x.SingleOrMultiInstitutionPIA == "multi" && x.BehalfSingleInstitution == Institution.OTHER_INSTITUTION_ID.ToString()).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.BehalfMultipleInstitutionLeadOther).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // BehalfMultipleInstitutionOthers
            RuleFor(x => x.BehalfMultipleInstitutionOthers).NotEmpty().When(x => x.SingleOrMultiInstitutionPIA == "multi").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // HasLeadInstitutionConsultedOther
            RuleFor(x => x.HasLeadInstitutionConsultedOther).NotEmpty().When(x => x.SingleOrMultiInstitutionPIA == "multi").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.HasLeadInstitutionConsultedOther).Must(x => new List<string> { "yes", "no", }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // LeadInstitutionHasNotConsultedOtherReason
            RuleFor(x => x.LeadInstitutionHasNotConsultedOtherReason).NotEmpty().When(x => x.SingleOrMultiInstitutionPIA == "multi" && x.HasLeadInstitutionConsultedOther == "no").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.LeadInstitutionHasNotConsultedOtherReason).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // IsTreasuryBoardApproval
            RuleFor(x => x.IsTreasuryBoardApproval).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // HeadYourInstitutionFullname
            RuleFor(x => x.HeadYourInstitutionFullname).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.HeadYourInstitutionFullname).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // HeadYourInstitutionTitle
            RuleFor(x => x.HeadYourInstitutionTitle).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.HeadYourInstitutionTitle).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // HeadYourInstitutionSection
            RuleFor(x => x.HeadYourInstitutionSection).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.HeadYourInstitutionSection).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // HeadYourInstitutionEmail
            RuleFor(x => x.HeadYourInstitutionEmail).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.HeadYourInstitutionEmail).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
            RuleFor(x => x.HeadYourInstitutionEmail).EmailAddress();

            // SeniorOfficialFullname
            RuleFor(x => x.SeniorOfficialFullname).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.SeniorOfficialFullname).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // SeniorOfficialTitle
            RuleFor(x => x.SeniorOfficialTitle).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.SeniorOfficialTitle).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // SeniorOfficialSection
            RuleFor(x => x.SeniorOfficialSection).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.SeniorOfficialSection).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // SeniorOfficialEmail
            RuleFor(x => x.SeniorOfficialEmail).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.SeniorOfficialEmail).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
            RuleFor(x => x.SeniorOfficialEmail).EmailAddress();

            // PersonContact
            RuleFor(x => x.PersonContact).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.PersonContact).Must(x => new List<string> { }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // AnotherContactFullname
            RuleFor(x => x.AnotherContactFullname).NotEmpty().When(x => x.PersonContact == "another").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.AnotherContactFullname).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // AnotherContactTitle
            RuleFor(x => x.AnotherContactFullname).NotEmpty().When(x => x.PersonContact == "another").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.AnotherContactTitle).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // AnotherContactSection
            RuleFor(x => x.AnotherContactFullname).NotEmpty().When(x => x.PersonContact == "another").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.AnotherContactSection).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // AnotherContactEmail
            RuleFor(x => x.AnotherContactFullname).NotEmpty().When(x => x.PersonContact == "another").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.AnotherContactEmail).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
            RuleFor(x => x.AnotherContactEmail).EmailAddress();

            // NewOrUpdatedPIA
            RuleFor(x => x.NewOrUpdatedPIA).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.NewOrUpdatedPIA).Must(x => new List<string> { "new_pia", "update_pia", "new_pia_covers_already_submitted", }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // UpdatePIANumberAssigned
            RuleFor(x => x.UpdatePIANumberAssigned).NotEmpty().When(x => x.NewOrUpdatedPIA == "update_pia").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.UpdatePIANumberAssigned).Must(x => new List<string> { "update_pia_existing_reference_number", "update_pia_not_existing_reference_number", }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // UpdatePIAAllReferenceNumbersAssigned
            RuleFor(x => x.UpdatePIAAllReferenceNumbersAssigned).NotEmpty().When(x => x.NewOrUpdatedPIA == "update_pia" && x.UpdatePIANumberAssigned == "update_pia_existing_reference_number").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.UpdatePIAAllReferenceNumbersAssigned).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // DetailsPreviousSubmission
            RuleFor(x => x.DetailsPreviousSubmission).NotEmpty().When(x => x.NewOrUpdatedPIA == "update_pia").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.DetailsPreviousSubmission).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // NewPIANumberAssigned
            RuleFor(x => x.NewPIANumberAssigned).NotEmpty().When(x => x.NewOrUpdatedPIA == "new_pia_covers_already_submitted").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.NewPIANumberAssigned).Must(x => new List<string> { "new_pia_existing_reference_number", "new_pia_not_existing_reference_number", }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // NewPIAAllReferenceNumbersAssigned
            RuleFor(x => x.NewPIAAllReferenceNumbersAssigned).NotEmpty().When(x => x.NewOrUpdatedPIA == "new_pia_covers_already_submitted" && x.NewPIANumberAssigned == "new_pia_existing_reference_number").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.NewPIAAllReferenceNumbersAssigned).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // ProgramOverview
            RuleFor(x => x.ProgramOverview).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ProgramOverview).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // ParticipationOptions
            RuleFor(x => x.ParticipationOptions).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ParticipationOptions).Must(x => new List<string> { "participation_mandatory_and_automatic", "participation_not_mandatory_but_automatic", "participation_not_mandatory_but_can_be", "participation_voluntary", }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // OtherInitiatives
            RuleFor(x => x.OtherInitiatives).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // OtherInitiativesDescription
            RuleFor(x => x.OtherInitiativesDescription).NotEmpty().When(x => x.OtherInitiatives).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.OtherInitiativesDescription).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // DurationOptions
            RuleFor(x => x.DurationOptions).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.DurationOptions).Must(x => new List<string> { "pilot", "one_time", "short_term", "long_term", }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // DurationOptionsDescriptions
            RuleFor(x => x.DurationOptionsDescriptions).NotEmpty().When(x => x.DurationOptions == "long_term").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.DurationOptionsDescriptions).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // IsProgramRolledOutPhases
            RuleFor(x => x.IsProgramRolledOutPhases).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // AnticipatedStartDate
            RuleFor(x => x.AnticipatedStartDate).NotEmpty().When(x => x.IsAnticipatedStartDate).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.AnticipatedStartDate).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // IsAnticipatedEndDate
            RuleFor(x => x.IsAnticipatedEndDate).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // AnticipatedEndDate
            RuleFor(x => x.AnticipatedEndDate).NotEmpty().When(x => x.IsAnticipatedEndDate).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.AnticipatedEndDate).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // IsInvolveNewSoftware
            RuleFor(x => x.IsInvolveNewSoftware).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // InvolveNewSoftwareDescription
            RuleFor(x => x.InvolveNewSoftwareDescription).NotEmpty().When(x => x.IsInvolveNewSoftware).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.InvolveNewSoftwareDescription).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // DoesRequireModificationToIT
            RuleFor(x => x.DoesRequireModificationToIT).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // RequireModificationToITDescription
            RuleFor(x => x.RequireModificationToITDescription).NotEmpty().When(x => x.DoesRequireModificationToIT).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.RequireModificationToITDescription).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // HaschangesToBusinessRequirements
            RuleFor(x => x.HaschangesToBusinessRequirements).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // ChangesToBusinessRequirements
            RuleFor(x => x.ChangesToBusinessRequirements).NotEmpty().When(x => x.HaschangesToBusinessRequirements).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ChangesToBusinessRequirements).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // WillITLegacySystemRetained
            RuleFor(x => x.WillITLegacySystemRetained).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // AwarenessActivities
            RuleFor(x => x.AwarenessActivities).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.AwarenessActivities).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // ScopeThisPhase
            RuleFor(x => x.ScopeThisPhase).NotEmpty().When(x => x.IsProgramRolledOutPhases).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ScopeThisPhase).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // ScopeOtherPhases
            RuleFor(x => x.ScopeOtherPhases).NotEmpty().When(x => x.IsProgramRolledOutPhases).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ScopeOtherPhases).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // NotInScopePia
            RuleFor(x => x.NotInScopePia).NotEmpty().When(x => x.IsProgramRolledOutPhases).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.NotInScopePia).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // ScopePia
            RuleFor(x => x.ScopePia).NotEmpty().When(x => x.IsProgramRolledOutPhases).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ScopePia).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // NotInScope
            RuleFor(x => x.NotInScope).NotEmpty().When(x => x.IsProgramRolledOutPhases).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.NotInScope).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
        }
    }
}
