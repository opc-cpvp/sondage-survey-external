using System.Collections.Generic;
using FluentValidation;
using ComplaintFormCore.Resources;
using ComplaintFormCore.Web_Apis.Models;

namespace ComplaintFormCore.Models
{
    public  class SurveyPIAToolModelValidator : AbstractValidator<SurveyPIAToolModel>
    {
        public SurveyPIAToolModelValidator(SharedLocalizer _localizer)
        {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Continue;

            // HasLegalAuthority (Page: page_before_begin_q_0_1)
            RuleFor(x => x.HasLegalAuthority).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // RelevantLegislationPolicies (Page: page_before_begin_q_0_2)
            RuleFor(x => x.RelevantLegislationPolicies).NotEmpty().When(x => x.HasLegalAuthority == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.RelevantLegislationPolicies).Length(0, 5000).When(x => x.HasLegalAuthority == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // ProgramName (Page: page_step_1_a)
            RuleFor(x => x.ProgramName).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ProgramName).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // IsNewprogram (Page: page_step_1_a)
            RuleFor(x => x.IsNewprogram).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // ProgamHasMajorChanges (Page: page_step_1_b)
            RuleFor(x => x.ProgamHasMajorChanges).NotEmpty().When(x => x.IsNewprogram == false).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // IsProgamContractedOut (Page: page_step_1_b)
            RuleFor(x => x.IsProgamContractedOut).NotEmpty().When(x => x.IsNewprogram == false).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // IsProgramInvolvePersonalInformation (Page: page_step_1_q_1_5)
            RuleFor(x => x.IsProgramInvolvePersonalInformation).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // ContactATIPQ16 (Page: page_step_1_q_1_6)
            RuleFor(x => x.ContactATIPQ16).NotEmpty().When(x => x.IsProgramInvolvePersonalInformation == false).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ContactATIPQ16).Must(x => new List<string> { "receive_email", "no_email", "conduct_pia" }.Contains(x)).When(x => x.IsProgramInvolvePersonalInformation == false).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // UserEmailAddress16 (Page: page_step_1_q_1_6)
            RuleFor(x => x.UserEmailAddress16).NotEmpty().When(x => x.IsProgramInvolvePersonalInformation == false && x.ContactATIPQ16 == "receive_email").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.UserEmailAddress16).Length(0, 100).When(x => x.IsProgramInvolvePersonalInformation == false && x.ContactATIPQ16 == "receive_email").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
            RuleFor(x => x.UserEmailAddress16).EmailAddress();

            // PersonalInfoUsedFor (Page: page_step_1_q_1_7)
            RuleFor(x => x.PersonalInfoUsedFor).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.PersonalInfoUsedFor).Must(x => new List<string> { "admin_purpose", "non_admin_purpose" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // ContactATIPQ18 (Page: page_step_1_q_1_8)
            RuleFor(x => x.ContactATIPQ18).NotEmpty().When(x => x.PersonalInfoUsedFor == "non_admin_purpose").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ContactATIPQ18).Must(x => new List<string> { "receive_email", "no_email", "conduct_pia" }.Contains(x)).When(x => x.PersonalInfoUsedFor == "non_admin_purpose").WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // UserEmailAddress18 (Page: page_step_1_q_1_8)
            RuleFor(x => x.UserEmailAddress18).NotEmpty().When(x => x.PersonalInfoUsedFor == "non_admin_purpose" && x.ContactATIPQ18 == "receive_email").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.UserEmailAddress18).Length(0, 100).When(x => x.PersonalInfoUsedFor == "non_admin_purpose" && x.ContactATIPQ18 == "receive_email").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
            RuleFor(x => x.UserEmailAddress18).EmailAddress();

            // SubjectOfPIA (Page: page_step_1_q_1_9)
            RuleFor(x => x.SubjectOfPIA).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.SubjectOfPIA).Must(x => new List<string> { "program_activity", "other" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // ContactATIPQ110 (Page: page_step_1_q_1_10)
            RuleFor(x => x.ContactATIPQ110).NotEmpty().When(x => x.SubjectOfPIA == "other").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ContactATIPQ110).Must(x => new List<string> { "receive_email", "no_email", "conduct_pia" }.Contains(x)).When(x => x.SubjectOfPIA == "other").WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // UserEmailAddress110 (Page: page_step_1_q_1_10)
            RuleFor(x => x.UserEmailAddress110).NotEmpty().When(x => x.SubjectOfPIA == "other" && x.ContactATIPQ110 == "receive_email").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.UserEmailAddress110).Length(0, 100).When(x => x.SubjectOfPIA == "other" && x.ContactATIPQ110 == "receive_email").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
            RuleFor(x => x.UserEmailAddress110).EmailAddress();

            // SingleOrMultiInstitutionPIA (Page: page_step_2_1_q_2_1_1)
            RuleFor(x => x.SingleOrMultiInstitutionPIA).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.SingleOrMultiInstitutionPIA).Must(x => new List<string> { "single", "multi", "single_related" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // RelatedPIANameInstitution (Page: page_step_2_1_q_2_1_1)
            RuleFor(x => x.RelatedPIANameInstitution).NotEmpty().When(x => x.SingleOrMultiInstitutionPIA == "single_related").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.RelatedPIANameInstitution).Length(0, 100).When(x => x.SingleOrMultiInstitutionPIA == "single_related").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // RelatedPIANameProgram (Page: page_step_2_1_q_2_1_1)
            RuleFor(x => x.RelatedPIANameProgram).NotEmpty().When(x => x.SingleOrMultiInstitutionPIA == "single_related").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.RelatedPIANameProgram).Length(0, 100).When(x => x.SingleOrMultiInstitutionPIA == "single_related").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // RelatedPIADescription (Page: page_step_2_1_q_2_1_1)
            RuleFor(x => x.RelatedPIADescription).NotEmpty().When(x => x.SingleOrMultiInstitutionPIA == "single_related").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.RelatedPIADescription).Length(0, 100).When(x => x.SingleOrMultiInstitutionPIA == "single_related").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // BehalfSingleInstitution (Page: page_step_2_1_q_2_1_2a)
            RuleFor(x => x.BehalfSingleInstitution).NotEmpty().When(x => x.SingleOrMultiInstitutionPIA == "single" || x.SingleOrMultiInstitutionPIA == "singlerelated").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // BehalfSingleInstitutionOther (Page: page_step_2_1_q_2_1_2a)
            RuleFor(x => x.BehalfSingleInstitutionOther).NotEmpty().When(x => x.BehalfSingleInstitution == 9999.ToString()).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.BehalfSingleInstitutionOther).Length(0, 100).When(x => x.BehalfSingleInstitution == 9999.ToString()).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // BehalfMultipleInstitutionLead (Page: page_step_2_1_q_2_1_2b)
            RuleFor(x => x.BehalfMultipleInstitutionLead).NotEmpty().When(x => x.SingleOrMultiInstitutionPIA == "multi").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // BehalfMultipleInstitutionLeadOther (Page: page_step_2_1_q_2_1_2b)
            RuleFor(x => x.BehalfMultipleInstitutionLeadOther).NotEmpty().When(x => x.SingleOrMultiInstitutionPIA == "multi" && x.BehalfMultipleInstitutionLead == 9999.ToString()).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.BehalfMultipleInstitutionLeadOther).Length(0, 100).When(x => x.SingleOrMultiInstitutionPIA == "multi" && x.BehalfMultipleInstitutionLead == 9999.ToString()).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // BehalfMultipleInstitutionOthers (Page: page_step_2_1_q_2_1_2b)
            RuleFor(x => x.BehalfMultipleInstitutionOthers).NotEmpty().When(x => x.SingleOrMultiInstitutionPIA == "multi").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // HasLeadInstitutionConsultedOther (Page: page_step_2_1_q_2_1_3)
            RuleFor(x => x.HasLeadInstitutionConsultedOther).NotEmpty().When(x => x.SingleOrMultiInstitutionPIA == "multi").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.HasLeadInstitutionConsultedOther).Must(x => new List<string> { "yes", "no" }.Contains(x)).When(x => x.SingleOrMultiInstitutionPIA == "multi").WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // LeadInstitutionHasNotConsultedOtherReason (Page: page_step_2_1_q_2_1_3)
            RuleFor(x => x.LeadInstitutionHasNotConsultedOtherReason).NotEmpty().When(x => x.SingleOrMultiInstitutionPIA == "multi" && x.HasLeadInstitutionConsultedOther == "no").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.LeadInstitutionHasNotConsultedOtherReason).Length(0, 5000).When(x => x.SingleOrMultiInstitutionPIA == "multi" && x.HasLeadInstitutionConsultedOther == "no").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // IsTreasuryBoardApproval (Page: page_step_2_1_q_2_1_4)
            RuleFor(x => x.IsTreasuryBoardApproval).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // HeadYourInstitutionFullname (Page: page_step_2_1_q_2_1_5)
            RuleFor(x => x.HeadYourInstitutionFullname).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.HeadYourInstitutionFullname).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // HeadYourInstitutionTitle (Page: page_step_2_1_q_2_1_5)
            RuleFor(x => x.HeadYourInstitutionTitle).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.HeadYourInstitutionTitle).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // HeadYourInstitutionSection (Page: page_step_2_1_q_2_1_5)
            RuleFor(x => x.HeadYourInstitutionSection).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.HeadYourInstitutionSection).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // HeadYourInstitutionEmail (Page: page_step_2_1_q_2_1_5)
            RuleFor(x => x.HeadYourInstitutionEmail).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.HeadYourInstitutionEmail).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
            RuleFor(x => x.HeadYourInstitutionEmail).EmailAddress();

            // BehalfMultipleInstitutionOthers (Page: page_step_2_1_q_2_1_6)

            // SeniorOfficialFullname (Page: page_step_2_1_q_2_1_7)
            RuleFor(x => x.SeniorOfficialFullname).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.SeniorOfficialFullname).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // SeniorOfficialTitle (Page: page_step_2_1_q_2_1_7)
            RuleFor(x => x.SeniorOfficialTitle).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.SeniorOfficialTitle).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // SeniorOfficialSection (Page: page_step_2_1_q_2_1_7)
            RuleFor(x => x.SeniorOfficialSection).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.SeniorOfficialSection).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // SeniorOfficialEmail (Page: page_step_2_1_q_2_1_7)
            RuleFor(x => x.SeniorOfficialEmail).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.SeniorOfficialEmail).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
            RuleFor(x => x.SeniorOfficialEmail).EmailAddress();

            // BehalfMultipleInstitutionOthers (Page: page_step_2_1_q_2_1_8)

            // PersonContact (Page: page_step_2_1_q_2_1_9)
            RuleFor(x => x.PersonContact).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // AnotherContactFullname (Page: page_step_2_1_q_2_1_9)
            RuleFor(x => x.AnotherContactFullname).NotEmpty().When(x => x.PersonContact == "another").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.AnotherContactFullname).Length(0, 100).When(x => x.PersonContact == "another").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // AnotherContactTitle (Page: page_step_2_1_q_2_1_9)
            RuleFor(x => x.AnotherContactTitle).NotEmpty().When(x => x.PersonContact == "another").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.AnotherContactTitle).Length(0, 100).When(x => x.PersonContact == "another").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // AnotherContactSection (Page: page_step_2_1_q_2_1_9)
            RuleFor(x => x.AnotherContactSection).NotEmpty().When(x => x.PersonContact == "another").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.AnotherContactSection).Length(0, 100).When(x => x.PersonContact == "another").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // AnotherContactEmail (Page: page_step_2_1_q_2_1_9)
            RuleFor(x => x.AnotherContactEmail).NotEmpty().When(x => x.PersonContact == "another").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.AnotherContactEmail).Length(0, 100).When(x => x.PersonContact == "another").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
            RuleFor(x => x.AnotherContactEmail).EmailAddress();

            // NewOrUpdatedPIA (Page: page_step_2_1_q_2_1_10)
            RuleFor(x => x.NewOrUpdatedPIA).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.NewOrUpdatedPIA).Must(x => new List<string> { "new_pia", "update_pia", "new_pia_covers_already_submitted" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // UpdatePIANumberAssigned (Page: page_step_2_1_q_2_1_11_a)
            RuleFor(x => x.UpdatePIANumberAssigned).NotEmpty().When(x => x.NewOrUpdatedPIA == "update_pia").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.UpdatePIANumberAssigned).Must(x => new List<string> { "update_pia_existing_reference_number", "update_pia_not_existing_reference_number" }.Contains(x)).When(x => x.NewOrUpdatedPIA == "update_pia").WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // UpdatePIAAllReferenceNumbersAssigned (Page: page_step_2_1_q_2_1_11_a)
            RuleFor(x => x.UpdatePIAAllReferenceNumbersAssigned).NotEmpty().When(x => x.NewOrUpdatedPIA == "update_pia" && x.UpdatePIANumberAssigned == "update_pia_existing_reference_number").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.UpdatePIAAllReferenceNumbersAssigned).Length(0, 100).When(x => x.NewOrUpdatedPIA == "update_pia" && x.UpdatePIANumberAssigned == "update_pia_existing_reference_number").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // DetailsPreviousSubmission (Page: page_step_2_1_q_2_1_11_b)
            RuleFor(x => x.DetailsPreviousSubmission).NotEmpty().When(x => x.NewOrUpdatedPIA == "update_pia").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.DetailsPreviousSubmission).Length(0, 5000).When(x => x.NewOrUpdatedPIA == "update_pia").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // NewPIANumberAssigned (Page: page_step_2_1_q_2_1_12)
            RuleFor(x => x.NewPIANumberAssigned).NotEmpty().When(x => x.NewOrUpdatedPIA == "new_pia_covers_already_submitted").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.NewPIANumberAssigned).Must(x => new List<string> { "new_pia_existing_reference_number", "new_pia_not_existing_reference_number" }.Contains(x)).When(x => x.NewOrUpdatedPIA == "new_pia_covers_already_submitted").WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // NewPIAAllReferenceNumbersAssigned (Page: page_step_2_1_q_2_1_12)
            RuleFor(x => x.NewPIAAllReferenceNumbersAssigned).NotEmpty().When(x => x.NewOrUpdatedPIA == "new_pia_covers_already_submitted" && x.NewPIANumberAssigned == "new_pia_existing_reference_number").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.NewPIAAllReferenceNumbersAssigned).Length(0, 100).When(x => x.NewOrUpdatedPIA == "new_pia_covers_already_submitted" && x.NewPIANumberAssigned == "new_pia_existing_reference_number").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // ProgramOverview (Page: page_step_2_2_q_2_2_1)
            RuleFor(x => x.ProgramOverview).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ProgramOverview).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // ParticipationOptions (Page: page_step_2_2_q_2_2_2)
            RuleFor(x => x.ParticipationOptions).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ParticipationOptions).Must(x => new List<string> { "participation_mandatory_and_automatic", "participation_not_mandatory_but_automatic", "participation_not_mandatory_but_can_be", "participation_voluntary" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // OtherInitiatives (Page: page_step_2_2_q_2_2_3)
            RuleFor(x => x.OtherInitiatives).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // OtherInitiativesDescription (Page: page_step_2_2_q_2_2_3)
            RuleFor(x => x.OtherInitiativesDescription).NotEmpty().When(x => x.OtherInitiatives == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.OtherInitiativesDescription).Length(0, 5000).When(x => x.OtherInitiatives == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // DurationOptions (Page: page_step_2_2_q_2_2_4)
            RuleFor(x => x.DurationOptions).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.DurationOptions).Must(x => new List<string> { "pilot", "one_time", "short_term", "long_term" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // DurationOptionsDescriptions (Page: page_step_2_2_q_2_2_4)
            RuleFor(x => x.DurationOptionsDescriptions).NotEmpty().When(x => x.DurationOptions == "long_term").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.DurationOptionsDescriptions).Length(0, 5000).When(x => x.DurationOptions == "long_term").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // IsProgramRolledOutPhases (Page: page_step_2_2_q_2_2_5)
            RuleFor(x => x.IsProgramRolledOutPhases).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // IsAnticipatedStartDate (Page: page_step_2_2_q_2_2_6)
            RuleFor(x => x.IsAnticipatedStartDate).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // AnticipatedStartDate (Page: page_step_2_2_q_2_2_6)
            RuleFor(x => x.AnticipatedStartDate).NotEmpty().When(x => x.IsAnticipatedStartDate == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.AnticipatedStartDate).Length(0, 5000).When(x => x.IsAnticipatedStartDate == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // IsAnticipatedEndDate (Page: page_step_2_2_q_2_2_7)
            RuleFor(x => x.IsAnticipatedEndDate).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // AnticipatedEndDate (Page: page_step_2_2_q_2_2_7)
            RuleFor(x => x.AnticipatedEndDate).NotEmpty().When(x => x.IsAnticipatedEndDate == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.AnticipatedEndDate).Length(0, 5000).When(x => x.IsAnticipatedEndDate == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // IsInvolveNewSoftware (Page: page_step_2_2_q_2_2_8)
            RuleFor(x => x.IsInvolveNewSoftware).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // InvolveNewSoftwareDescription (Page: page_step_2_2_q_2_2_8)
            RuleFor(x => x.InvolveNewSoftwareDescription).NotEmpty().When(x => x.IsInvolveNewSoftware == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.InvolveNewSoftwareDescription).Length(0, 5000).When(x => x.IsInvolveNewSoftware == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // DoesRequireModificationToIT (Page: page_step_2_2_q_2_2_10)
            RuleFor(x => x.DoesRequireModificationToIT).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // RequireModificationToITDescription (Page: page_step_2_2_q_2_2_10)
            RuleFor(x => x.RequireModificationToITDescription).NotEmpty().When(x => x.DoesRequireModificationToIT == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.RequireModificationToITDescription).Length(0, 5000).When(x => x.DoesRequireModificationToIT == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // HaschangesToBusinessRequirements (Page: page_step_2_2_q_2_2_12)
            RuleFor(x => x.HaschangesToBusinessRequirements).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // ChangesToBusinessRequirements (Page: page_step_2_2_q_2_2_12)
            RuleFor(x => x.ChangesToBusinessRequirements).NotEmpty().When(x => x.HaschangesToBusinessRequirements == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ChangesToBusinessRequirements).Length(0, 5000).When(x => x.HaschangesToBusinessRequirements == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // WillITLegacySystemRetained (Page: page_step_2_2_q_2_2_13)
            RuleFor(x => x.WillITLegacySystemRetained).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // AwarenessActivities (Page: page_step_2_2_q_2_2_14)
            RuleFor(x => x.AwarenessActivities).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.AwarenessActivities).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // ScopeThisPhase (Page: page_step_2_2_q_2_2_15)
            RuleFor(x => x.ScopeThisPhase).NotEmpty().When(x => x.IsProgramRolledOutPhases == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ScopeThisPhase).Length(0, 5000).When(x => x.IsProgramRolledOutPhases == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // ScopeOtherPhases (Page: page_step_2_2_q_2_2_15)
            RuleFor(x => x.ScopeOtherPhases).NotEmpty().When(x => x.IsProgramRolledOutPhases == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ScopeOtherPhases).Length(0, 5000).When(x => x.IsProgramRolledOutPhases == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // NotInScopePia (Page: page_step_2_2_q_2_2_15)
            RuleFor(x => x.NotInScopePia).NotEmpty().When(x => x.IsProgramRolledOutPhases == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.NotInScopePia).Length(0, 5000).When(x => x.IsProgramRolledOutPhases == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // ScopePia (Page: page_step_2_2_q_2_2_15)
            RuleFor(x => x.ScopePia).NotEmpty().When(x => x.IsProgramRolledOutPhases == false).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ScopePia).Length(0, 5000).When(x => x.IsProgramRolledOutPhases == false).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // NotInScope (Page: page_step_2_2_q_2_2_15)
            RuleFor(x => x.NotInScope).NotEmpty().When(x => x.IsProgramRolledOutPhases == false).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.NotInScope).Length(0, 5000).When(x => x.IsProgramRolledOutPhases == false).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // WhyProgramNecessary (Page: page_step_3_1_q_3_1_1_q_3_1_2)
            RuleFor(x => x.WhyProgramNecessary).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // DoesMeasureEffectiveness (Page: page_step_3_1_q_3_1_1_q_3_1_2)

            // HowMeasureEffectiveness (Page: page_step_3_1_q_3_1_3)
            RuleFor(x => x.HowMeasureEffectiveness).Length(0, 5000).When(x => x.DoesMeasureEffectiveness == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // WhyNotMeasureEffectiveness (Page: page_step_3_1_q_3_1_3)
            RuleFor(x => x.WhyNotMeasureEffectiveness).Length(0, 5000).When(x => x.DoesMeasureEffectiveness == false).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // AssessProportionalityExplanation (Page: page_step_3_1_q_3_1_4)
            RuleFor(x => x.AssessProportionalityExplanation).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // DidConsiderLessIntrusiveSolution (Page: page_step_3_1_q_3_1_5)

            // DescriptionConsiderLessIntrusiveSolution (Page: page_step_3_1_q_3_1_6)
            RuleFor(x => x.DescriptionConsiderLessIntrusiveSolution).Length(0, 5000).When(x => x.DidConsiderLessIntrusiveSolution == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // WhyNotConsiderLessIntrusiveSolution (Page: page_step_3_1_q_3_1_6)
            RuleFor(x => x.WhyNotConsiderLessIntrusiveSolution).Length(0, 5000).When(x => x.DidConsiderLessIntrusiveSolution == false).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // ContactATIPQ317 (Page: page_step_3_1_q_3_1_7)
            RuleFor(x => x.ContactATIPQ317).Must(x => new List<string> { "receive_email", "conduct_pia" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // UserEmailAddress317 (Page: page_step_3_1_q_3_1_7)
            RuleFor(x => x.UserEmailAddress317).NotEmpty().When(x => x.ContactATIPQ317 == "receive_email").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.UserEmailAddress317).Length(0, 100).When(x => x.ContactATIPQ317 == "receive_email").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
            RuleFor(x => x.UserEmailAddress317).EmailAddress();

            // IsHeadYourInstitutionResponsibleWithPA (Page: page_step_3_2_q_3_2_1)

            // ResponsibleComplianceWithPA (Page: page_step_3_2_q_3_2_1)
            RuleFor(x => x.ResponsibleComplianceWithPA).Length(0, 5000).When(x => x.IsHeadYourInstitutionResponsibleWithPA == false).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // ListPoliciesAndProcedures (Page: page_step_3_2_q_3_2_3)
            RuleFor(x => x.ListPoliciesAndProcedures).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // DoesStaffReceivedTraining (Page: page_step_3_2_q_3_2_4_q_3_2_5)

            // StaffReceivedTrainingDescription (Page: page_step_3_2_q_3_2_4_q_3_2_5)
            RuleFor(x => x.StaffReceivedTrainingDescription).Length(0, 5000).When(x => x.DoesStaffReceivedTraining == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // ProcessHandlingPrivacyComplaint (Page: page_step_3_2_q_3_2_6)
            RuleFor(x => x.ProcessHandlingPrivacyComplaint).Must(x => new List<string> { "yes_in_place", "yes_not_established", "no" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // ProcessHandlingPrivacyComplaintDescription (Page: page_step_3_2_q_3_2_6)
            RuleFor(x => x.ProcessHandlingPrivacyComplaintDescription).Length(0, 5000).When(_ => true/* TODO: {ProcessHandlingPrivacyComplaint} anyof ['yes_in_place', 'yes_not_established'] */).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // HasDataMinimization (Page: page_step_3_3_1)
            RuleFor(x => x.HasDataMinimization).Must(x => new List<string> { "yes", "not_yet_planned", "no" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // DataMinimizationDetails (Page: page_step_3_3_1)
            RuleFor(x => x.DataMinimizationDetails).Length(0, 5000).When(x => x.HasDataMinimization == "yes").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // DataMinimizationPlannedDetails (Page: page_step_3_3_1)
            RuleFor(x => x.DataMinimizationPlannedDetails).Length(0, 5000).When(x => x.HasDataMinimization == "not_yet_planned").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // PersonalInformationCategory (Page: page_step_3_3_3)

            // IsContextualSensitivities (Page: page_step_3_3_4)

            // ContextualSensitivitiesDetails (Page: page_step_3_3_4)
            RuleFor(x => x.ContextualSensitivitiesDetails).Length(0, 5000).When(x => x.IsContextualSensitivities == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // IsInformationPhysicalFormat (Page: page_step_3_3_5)

            // InformationPhysicalFormatDescription (Page: page_step_3_3_5)
            RuleFor(x => x.InformationPhysicalFormatDescription).Length(0, 5000).When(x => x.IsInformationPhysicalFormat == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // IsInformationPhysicalConvertedCopy (Page: page_step_3_3_5)

            // InformationPhysicalConvertedCopyDescription (Page: page_step_3_3_5)
            RuleFor(x => x.InformationPhysicalConvertedCopyDescription).Length(0, 5000).When(x => x.IsInformationPhysicalFormat == true && x.IsInformationPhysicalConvertedCopy == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // IsPersonalInformationElectronicFormat (Page: page_step_3_3_6)

            // PersonalInformationElectronicFormatDescription (Page: page_step_3_3_6)
            RuleFor(x => x.PersonalInformationElectronicFormatDescription).Length(0, 5000).When(x => x.IsPersonalInformationElectronicFormat == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // IsInformationElectronicConvertedCopy (Page: page_step_3_3_6)

            // IsInformationElectronicConvertedCopyDescription (Page: page_step_3_3_6)
            RuleFor(x => x.IsInformationElectronicConvertedCopyDescription).Length(0, 5000).When(x => x.IsPersonalInformationElectronicFormat == true && x.IsInformationElectronicConvertedCopy == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // IsThereCollectNotIntended (Page: page_step_3_3_8a)

            // DoesHavePoliciesProcedures (Page: page_step_3_3_8a)

            // HavePoliciesProceduresDescription (Page: page_step_3_3_8a)
            RuleFor(x => x.HavePoliciesProceduresDescription).Length(0, 5000).When(x => x.DoesHavePoliciesProcedures == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            RuleForEach(x => x.BehalfMultipleInstitutionOthers).SetValidator(new BehalfMultipleInstitutionOthersValidator(_localizer));
            RuleForEach(x => x.PersonalInformationCategory).SetValidator(new PersonalInformationCategoryValidator(_localizer));
        }
    }
    public partial class BehalfMultipleInstitutionOthersValidator : AbstractValidator<BehalfMultipleInstitutionOthers>
    {
        public BehalfMultipleInstitutionOthersValidator(SharedLocalizer _localizer)
        {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Continue;

            // BehalfMultipleInstitutionOther

            // OtherInstitutionHeadFullname
            RuleFor(x => x.OtherInstitutionHeadFullname).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.OtherInstitutionHeadFullname).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // OtherInstitutionHeadTitle
            RuleFor(x => x.OtherInstitutionHeadTitle).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.OtherInstitutionHeadTitle).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // OtherInstitutionSection
            RuleFor(x => x.OtherInstitutionSection).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.OtherInstitutionSection).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // OtherInstitutionEmail
            RuleFor(x => x.OtherInstitutionEmail).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.OtherInstitutionEmail).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
            RuleFor(x => x.OtherInstitutionEmail).EmailAddress();

            // SeniorOfficialOtherFullname
            RuleFor(x => x.SeniorOfficialOtherFullname).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.SeniorOfficialOtherFullname).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // SeniorOfficialOtherTitle
            RuleFor(x => x.SeniorOfficialOtherTitle).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.SeniorOfficialOtherTitle).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // SeniorOfficialOtherSection
            RuleFor(x => x.SeniorOfficialOtherSection).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.SeniorOfficialOtherSection).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // SeniorOfficialOtherEmail
            RuleFor(x => x.SeniorOfficialOtherEmail).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.SeniorOfficialOtherEmail).Length(0, 100).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
            RuleFor(x => x.SeniorOfficialOtherEmail).EmailAddress();

        }
    }
    public partial class PersonalInformationCategoryValidator : AbstractValidator<PersonalInformationCategory>
    {
        public PersonalInformationCategoryValidator(SharedLocalizer _localizer)
        {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Continue;

            // Category
            RuleFor(x => x.Category).Must(x => new List<string> { "name", "address", "contact_information", "from_social_media", "personal_website", "d_o_b", "current_age", "sex_gender", "physical_attribute", "marital_status", "family", "racial_identity", "ethnic_origin", "city_state_country", "citizenship", "opinions", "affiliations_or_association", "religious", "criminal", "government_issued_id", "employer", "business", "travel", "recorded_files", "transactions", "financial", "medical", "parental_guardian", "substitute_decision_maker", "allegations", "bodily_samples", "physical_biometrics", "behavioural_biometrics", "other" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // SupplementaryInformation
            RuleFor(x => x.SupplementaryInformation).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.SupplementaryInformation).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // PersonalInformationElement
            RuleFor(x => x.PersonalInformationElement).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

        }
    }
}

