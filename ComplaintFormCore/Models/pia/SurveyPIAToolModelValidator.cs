﻿using System.Collections.Generic;
using FluentValidation;
using ComplaintFormCore.Resources;
using ComplaintFormCore.Web_Apis.Models;

namespace ComplaintFormCore.Models
{
    public partial class SurveyPIAToolModelValidator : AbstractValidator<SurveyPIAToolModel>
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
            RuleFor(x => x.ProgramName).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

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
            RuleFor(x => x.UserEmailAddress16).Length(0, 200).When(x => x.IsProgramInvolvePersonalInformation == false && x.ContactATIPQ16 == "receive_email").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
            RuleFor(x => x.UserEmailAddress16).EmailAddress();

            // PersonalInfoUsedFor (Page: page_step_1_q_1_7)
            RuleFor(x => x.PersonalInfoUsedFor).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.PersonalInfoUsedFor).Must(x => new List<string> { "admin_purpose", "non_admin_purpose" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // ContactATIPQ18 (Page: page_step_1_q_1_8)
            RuleFor(x => x.ContactATIPQ18).NotEmpty().When(x => x.PersonalInfoUsedFor == "non_admin_purpose").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ContactATIPQ18).Must(x => new List<string> { "receive_email", "no_email", "conduct_pia" }.Contains(x)).When(x => x.PersonalInfoUsedFor == "non_admin_purpose").WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // UserEmailAddress18 (Page: page_step_1_q_1_8)
            RuleFor(x => x.UserEmailAddress18).NotEmpty().When(x => x.PersonalInfoUsedFor == "non_admin_purpose" && x.ContactATIPQ18 == "receive_email").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.UserEmailAddress18).Length(0, 200).When(x => x.PersonalInfoUsedFor == "non_admin_purpose" && x.ContactATIPQ18 == "receive_email").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
            RuleFor(x => x.UserEmailAddress18).EmailAddress();

            // SubjectOfPIA (Page: page_step_1_q_1_9)
            RuleFor(x => x.SubjectOfPIA).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.SubjectOfPIA).Must(x => new List<string> { "program_activity", "other" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // ContactATIPQ110 (Page: page_step_1_q_1_10)
            RuleFor(x => x.ContactATIPQ110).NotEmpty().When(x => x.SubjectOfPIA == "other").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ContactATIPQ110).Must(x => new List<string> { "receive_email", "no_email", "conduct_pia" }.Contains(x)).When(x => x.SubjectOfPIA == "other").WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // UserEmailAddress110 (Page: page_step_1_q_1_10)
            RuleFor(x => x.UserEmailAddress110).NotEmpty().When(x => x.SubjectOfPIA == "other" && x.ContactATIPQ110 == "receive_email").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.UserEmailAddress110).Length(0, 200).When(x => x.SubjectOfPIA == "other" && x.ContactATIPQ110 == "receive_email").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
            RuleFor(x => x.UserEmailAddress110).EmailAddress();

            // SingleOrMultiInstitutionPIA (Page: page_step_2_1_q_2_1_1)
            RuleFor(x => x.SingleOrMultiInstitutionPIA).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.SingleOrMultiInstitutionPIA).Must(x => new List<string> { "single", "multi", "single_related" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // RelatedPIANameInstitution (Page: page_step_2_1_q_2_1_1)
            RuleFor(x => x.RelatedPIANameInstitution).NotEmpty().When(x => x.SingleOrMultiInstitutionPIA == "single_related").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.RelatedPIANameInstitution).Length(0, 200).When(x => x.SingleOrMultiInstitutionPIA == "single_related").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // RelatedPIANameProgram (Page: page_step_2_1_q_2_1_1)
            RuleFor(x => x.RelatedPIANameProgram).NotEmpty().When(x => x.SingleOrMultiInstitutionPIA == "single_related").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.RelatedPIANameProgram).Length(0, 200).When(x => x.SingleOrMultiInstitutionPIA == "single_related").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // RelatedPIADescription (Page: page_step_2_1_q_2_1_1)
            RuleFor(x => x.RelatedPIADescription).NotEmpty().When(x => x.SingleOrMultiInstitutionPIA == "single_related").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.RelatedPIADescription).Length(0, 200).When(x => x.SingleOrMultiInstitutionPIA == "single_related").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // BehalfSingleInstitution (Page: page_step_2_1_q_2_1_2a)
            RuleFor(x => x.BehalfSingleInstitution).NotEmpty().When(x => x.SingleOrMultiInstitutionPIA == "single" || x.SingleOrMultiInstitutionPIA == "single_related").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // BehalfSingleInstitutionOther (Page: page_step_2_1_q_2_1_2a)
            RuleFor(x => x.BehalfSingleInstitutionOther).NotEmpty().When(x => (x.SingleOrMultiInstitutionPIA == "single" || x.SingleOrMultiInstitutionPIA == "single_related") && x.BehalfSingleInstitution == Institution.OTHER_INSTITUTION_ID.ToString()).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.BehalfSingleInstitutionOther).Length(0, 200).When(x => (x.SingleOrMultiInstitutionPIA == "single" || x.SingleOrMultiInstitutionPIA == "single_related") && x.BehalfSingleInstitution == Institution.OTHER_INSTITUTION_ID.ToString()).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // BehalfMultipleInstitutionLead (Page: page_step_2_1_q_2_1_2b)
            RuleFor(x => x.BehalfMultipleInstitutionLead).NotEmpty().When(x => x.SingleOrMultiInstitutionPIA == "multi").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // BehalfMultipleInstitutionLeadOther (Page: page_step_2_1_q_2_1_2b)
            RuleFor(x => x.BehalfMultipleInstitutionLeadOther).NotEmpty().When(x => x.SingleOrMultiInstitutionPIA == "multi" && x.BehalfMultipleInstitutionLead == Institution.OTHER_INSTITUTION_ID.ToString()).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.BehalfMultipleInstitutionLeadOther).Length(0, 200).When(x => x.SingleOrMultiInstitutionPIA == "multi" && x.BehalfMultipleInstitutionLead == Institution.OTHER_INSTITUTION_ID.ToString()).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

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
            RuleFor(x => x.HeadYourInstitutionFullname).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // HeadYourInstitutionTitle (Page: page_step_2_1_q_2_1_5)
            RuleFor(x => x.HeadYourInstitutionTitle).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.HeadYourInstitutionTitle).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // HeadYourInstitutionSection (Page: page_step_2_1_q_2_1_5)
            RuleFor(x => x.HeadYourInstitutionSection).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.HeadYourInstitutionSection).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // HeadYourInstitutionEmail (Page: page_step_2_1_q_2_1_5)
            RuleFor(x => x.HeadYourInstitutionEmail).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.HeadYourInstitutionEmail).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
            RuleFor(x => x.HeadYourInstitutionEmail).EmailAddress();

            // BehalfMultipleInstitutionOthers (Page: page_step_2_1_q_2_1_6)

            // SeniorOfficialFullname (Page: page_step_2_1_q_2_1_7)
            RuleFor(x => x.SeniorOfficialFullname).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.SeniorOfficialFullname).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // SeniorOfficialTitle (Page: page_step_2_1_q_2_1_7)
            RuleFor(x => x.SeniorOfficialTitle).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.SeniorOfficialTitle).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // SeniorOfficialSection (Page: page_step_2_1_q_2_1_7)
            RuleFor(x => x.SeniorOfficialSection).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.SeniorOfficialSection).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // SeniorOfficialEmail (Page: page_step_2_1_q_2_1_7)
            RuleFor(x => x.SeniorOfficialEmail).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.SeniorOfficialEmail).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
            RuleFor(x => x.SeniorOfficialEmail).EmailAddress();

            // BehalfMultipleInstitutionOthers (Page: page_step_2_1_q_2_1_8)

            // PersonContact (Page: page_step_2_1_q_2_1_9)
            RuleFor(x => x.PersonContact).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // AnotherContactFullname (Page: page_step_2_1_q_2_1_9)
            RuleFor(x => x.AnotherContactFullname).NotEmpty().When(x => x.PersonContact == "another").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.AnotherContactFullname).Length(0, 200).When(x => x.PersonContact == "another").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // AnotherContactTitle (Page: page_step_2_1_q_2_1_9)
            RuleFor(x => x.AnotherContactTitle).NotEmpty().When(x => x.PersonContact == "another").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.AnotherContactTitle).Length(0, 200).When(x => x.PersonContact == "another").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // AnotherContactSection (Page: page_step_2_1_q_2_1_9)
            RuleFor(x => x.AnotherContactSection).NotEmpty().When(x => x.PersonContact == "another").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.AnotherContactSection).Length(0, 200).When(x => x.PersonContact == "another").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // AnotherContactEmail (Page: page_step_2_1_q_2_1_9)
            RuleFor(x => x.AnotherContactEmail).NotEmpty().When(x => x.PersonContact == "another").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.AnotherContactEmail).Length(0, 200).When(x => x.PersonContact == "another").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
            RuleFor(x => x.AnotherContactEmail).EmailAddress();

            // NewOrUpdatedPIA (Page: page_step_2_1_q_2_1_10)
            RuleFor(x => x.NewOrUpdatedPIA).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.NewOrUpdatedPIA).Must(x => new List<string> { "new_pia", "update_pia", "new_pia_covers_already_submitted" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // UpdatePIANumberAssigned (Page: page_step_2_1_q_2_1_11_a)
            RuleFor(x => x.UpdatePIANumberAssigned).NotEmpty().When(x => x.NewOrUpdatedPIA == "update_pia").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.UpdatePIANumberAssigned).Must(x => new List<string> { "update_pia_existing_reference_number", "update_pia_not_existing_reference_number" }.Contains(x)).When(x => x.NewOrUpdatedPIA == "update_pia").WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // UpdatePIAAllReferenceNumbersAssigned (Page: page_step_2_1_q_2_1_11_a)
            RuleFor(x => x.UpdatePIAAllReferenceNumbersAssigned).NotEmpty().When(x => x.NewOrUpdatedPIA == "update_pia" && x.UpdatePIANumberAssigned == "update_pia_existing_reference_number").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.UpdatePIAAllReferenceNumbersAssigned).Length(0, 200).When(x => x.NewOrUpdatedPIA == "update_pia" && x.UpdatePIANumberAssigned == "update_pia_existing_reference_number").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // DetailsPreviousSubmission (Page: page_step_2_1_q_2_1_11_b)
            RuleFor(x => x.DetailsPreviousSubmission).NotEmpty().When(x => x.NewOrUpdatedPIA == "update_pia").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.DetailsPreviousSubmission).Length(0, 5000).When(x => x.NewOrUpdatedPIA == "update_pia").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // NewPIANumberAssigned (Page: page_step_2_1_q_2_1_12)
            RuleFor(x => x.NewPIANumberAssigned).NotEmpty().When(x => x.NewOrUpdatedPIA == "new_pia_covers_already_submitted").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.NewPIANumberAssigned).Must(x => new List<string> { "new_pia_existing_reference_number", "new_pia_not_existing_reference_number" }.Contains(x)).When(x => x.NewOrUpdatedPIA == "new_pia_covers_already_submitted").WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // NewPIAAllReferenceNumbersAssigned (Page: page_step_2_1_q_2_1_12)
            RuleFor(x => x.NewPIAAllReferenceNumbersAssigned).NotEmpty().When(x => x.NewOrUpdatedPIA == "new_pia_covers_already_submitted" && x.NewPIANumberAssigned == "new_pia_existing_reference_number").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.NewPIAAllReferenceNumbersAssigned).Length(0, 200).When(x => x.NewOrUpdatedPIA == "new_pia_covers_already_submitted" && x.NewPIANumberAssigned == "new_pia_existing_reference_number").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

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
            RuleFor(x => x.WhyProgramNecessary).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.WhyProgramNecessary).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // DoesMeasureEffectiveness (Page: page_step_3_1_q_3_1_1_q_3_1_2)
            RuleFor(x => x.DoesMeasureEffectiveness).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // HowMeasureEffectiveness (Page: page_step_3_1_q_3_1_3)
            RuleFor(x => x.HowMeasureEffectiveness).NotEmpty().When(x => x.DoesMeasureEffectiveness == true ).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.HowMeasureEffectiveness).Length(0, 5000).When(x => x.DoesMeasureEffectiveness == true ).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // WhyNotMeasureEffectiveness (Page: page_step_3_1_q_3_1_3)
            RuleFor(x => x.WhyNotMeasureEffectiveness).NotEmpty().When(x => x.DoesMeasureEffectiveness == false ).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.WhyNotMeasureEffectiveness).Length(0, 5000).When(x => x.DoesMeasureEffectiveness == false ).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // AssessProportionalityExplanation (Page: page_step_3_1_q_3_1_4)
            RuleFor(x => x.AssessProportionalityExplanation).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.AssessProportionalityExplanation).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // DidConsiderLessIntrusiveSolution (Page: page_step_3_1_q_3_1_5)
            RuleFor(x => x.DidConsiderLessIntrusiveSolution).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // DescriptionConsiderLessIntrusiveSolution (Page: page_step_3_1_q_3_1_6)
            RuleFor(x => x.DescriptionConsiderLessIntrusiveSolution).NotEmpty().When(x => x.DidConsiderLessIntrusiveSolution == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.DescriptionConsiderLessIntrusiveSolution).Length(0, 5000).When(x => x.DidConsiderLessIntrusiveSolution == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // WhyNotConsiderLessIntrusiveSolution (Page: page_step_3_1_q_3_1_6)
            RuleFor(x => x.WhyNotConsiderLessIntrusiveSolution).NotEmpty().When(x => x.DidConsiderLessIntrusiveSolution == false).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.WhyNotConsiderLessIntrusiveSolution).Length(0, 5000).When(x => x.DidConsiderLessIntrusiveSolution == false).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // ContactATIPQ317 (Page: page_step_3_1_q_3_1_7)
            RuleFor(x => x.ContactATIPQ317).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ContactATIPQ317).Must(x => new List<string> { "receive_email", "conduct_pia" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // UserEmailAddress317 (Page: page_step_3_1_q_3_1_7)
            RuleFor(x => x.UserEmailAddress317).NotEmpty().When(x => x.ContactATIPQ317 == "receive_email").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.UserEmailAddress317).Length(0, 200).When(x => x.ContactATIPQ317 == "receive_email").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
            RuleFor(x => x.UserEmailAddress317).EmailAddress();

            // IsHeadYourInstitutionResponsibleWithPA (Page: page_step_3_2_q_3_2_1)
            RuleFor(x => x.IsHeadYourInstitutionResponsibleWithPA).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // ResponsibleComplianceWithPA (Page: page_step_3_2_q_3_2_1)
            RuleFor(x => x.ResponsibleComplianceWithPA).NotEmpty().When(x => x.IsHeadYourInstitutionResponsibleWithPA == false).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ResponsibleComplianceWithPA).Length(0, 5000).When(x => x.IsHeadYourInstitutionResponsibleWithPA == false).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // ListPoliciesAndProcedures (Page: page_step_3_2_q_3_2_3)
            RuleFor(x => x.ListPoliciesAndProcedures).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ListPoliciesAndProcedures).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // DoesStaffReceivedTraining (Page: page_step_3_2_q_3_2_4_q_3_2_5)
            RuleFor(x => x.DoesStaffReceivedTraining).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // StaffReceivedTrainingDescription (Page: page_step_3_2_q_3_2_4_q_3_2_5)
            RuleFor(x => x.StaffReceivedTrainingDescription).NotEmpty().When(x => x.DoesStaffReceivedTraining == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.StaffReceivedTrainingDescription).Length(0, 5000).When(x => x.DoesStaffReceivedTraining == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // ProcessHandlingPrivacyComplaint (Page: page_step_3_2_q_3_2_6)
            RuleFor(x => x.ProcessHandlingPrivacyComplaint).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ProcessHandlingPrivacyComplaint).Must(x => new List<string> { "yes_in_place", "yes_not_established", "no" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // ProcessHandlingPrivacyComplaintDescription (Page: page_step_3_2_q_3_2_6)
            RuleFor(x => x.ProcessHandlingPrivacyComplaintDescription).NotEmpty().When(x => new List<string>() { "yes_in_place", "yes_not_established" }.Contains(x.ProcessHandlingPrivacyComplaint)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ProcessHandlingPrivacyComplaintDescription).Length(0, 5000).When(x => new List<string>() { "yes_in_place", "yes_not_established" }.Contains(x.ProcessHandlingPrivacyComplaint)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // HasDataMinimization (Page: page_step_3_3_1)
            RuleFor(x => x.HasDataMinimization).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.HasDataMinimization).Must(x => new List<string> { "yes", "not_yet_planned", "no" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // DataMinimizationDetails (Page: page_step_3_3_1)
            RuleFor(x => x.DataMinimizationDetails).NotEmpty().When(x => x.HasDataMinimization == "yes").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.DataMinimizationDetails).Length(0, 5000).When(x => x.HasDataMinimization == "yes").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // DataMinimizationPlannedDetails (Page: page_step_3_3_1)
            RuleFor(x => x.DataMinimizationPlannedDetails).NotEmpty().When(x => x.HasDataMinimization == "not_yet_planned").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.DataMinimizationPlannedDetails).Length(0, 5000).When(x => x.HasDataMinimization == "not_yet_planned").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // PersonalInformationCategory (Page: page_step_3_3_2_and_3_3_3)
            RuleFor(x => x.PersonalInformationCategory).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // IsContextualSensitivities (Page: page_step_3_3_4)
            RuleFor(x => x.IsContextualSensitivities).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // ContextualSensitivitiesDetails (Page: page_step_3_3_4)
            RuleFor(x => x.ContextualSensitivitiesDetails).NotEmpty().When(x => x.IsContextualSensitivities == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ContextualSensitivitiesDetails).Length(0, 5000).When(x => x.IsContextualSensitivities == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // PersonalInformationPhysicalAndOrElectronicFormat (Page: page_step_pre_3_3_5)
            RuleFor(x => x.PersonalInformationPhysicalAndOrElectronicFormat).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.PersonalInformationPhysicalAndOrElectronicFormat).Must(x => new List<string> { "physical", "electronic", "both" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // InformationPhysicalFormatDescription (Page: page_step_3_3_5)
            RuleFor(x => x.InformationPhysicalFormatDescription).NotEmpty().When(x => new List<string>() { "physical", "both" }.Contains(x.PersonalInformationPhysicalAndOrElectronicFormat)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.InformationPhysicalFormatDescription).Length(0, 5000).When(x => new List<string>() { "physical", "both" }.Contains(x.PersonalInformationPhysicalAndOrElectronicFormat)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // IsPhysicalConvertedToElectronic (Page: page_step_3_3_5)
            RuleFor(x => x.IsPhysicalConvertedToElectronic).NotEmpty().When(x => new List<string>() { "physical", "both" }.Contains(x.PersonalInformationPhysicalAndOrElectronicFormat)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // InformationPhysicalConvertedCopyDescription (Page: page_step_3_3_5)
            RuleFor(x => x.InformationPhysicalConvertedCopyDescription).NotEmpty().When(x => new List<string>() { "physical", "both" }.Contains(x.PersonalInformationPhysicalAndOrElectronicFormat) && x.IsPhysicalConvertedToElectronic == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.InformationPhysicalConvertedCopyDescription).Length(0, 5000).When(x => new List<string>() { "physical", "both" }.Contains(x.PersonalInformationPhysicalAndOrElectronicFormat) && x.IsPhysicalConvertedToElectronic == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // PersonalInformationElectronicFormatDescription (Page: page_step_3_3_6)
            RuleFor(x => x.PersonalInformationElectronicFormatDescription).NotEmpty().When(x => new List<string>() { "electronic", "both" }.Contains(x.PersonalInformationPhysicalAndOrElectronicFormat)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.PersonalInformationElectronicFormatDescription).Length(0, 5000).When(x => new List<string>() { "electronic", "both" }.Contains(x.PersonalInformationPhysicalAndOrElectronicFormat)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // IsElectronicConvertedToPhysical (Page: page_step_3_3_6)
            RuleFor(x => x.IsElectronicConvertedToPhysical).NotEmpty().When(x => new List<string>() { "electronic", "both" }.Contains(x.PersonalInformationPhysicalAndOrElectronicFormat)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // IsInformationElectronicConvertedCopyDescription (Page: page_step_3_3_6)
            RuleFor(x => x.IsInformationElectronicConvertedCopyDescription).NotEmpty().When(x => new List<string>() { "electronic", "both" }.Contains(x.PersonalInformationPhysicalAndOrElectronicFormat) && x.IsElectronicConvertedToPhysical == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.IsInformationElectronicConvertedCopyDescription).Length(0, 5000).When(x => new List<string>() { "electronic", "both" }.Contains(x.PersonalInformationPhysicalAndOrElectronicFormat) && x.IsElectronicConvertedToPhysical == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // IsThereCollectNotIntended (Page: page_step_3_3_8a)
            RuleFor(x => x.IsThereCollectNotIntended).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // DoesHavePoliciesProcedures (Page: page_step_3_3_8a)
            RuleFor(x => x.DoesHavePoliciesProcedures).NotEmpty().When(x => x.IsThereCollectNotIntended == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // HavePoliciesProceduresDescription (Page: page_step_3_3_8a)
            RuleFor(x => x.HavePoliciesProceduresDescription).NotEmpty().When(x => x.DoesHavePoliciesProcedures == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.HavePoliciesProceduresDescription).Length(0, 5000).When(x => x.DoesHavePoliciesProcedures == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // IsDirectlyFromIndividual (Page: page_step_3_4_1)
            RuleFor(x => x.IsDirectlyFromIndividual).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // WhereWhomInfoCollected (Page: page_step_3_4_2)
            RuleFor(x => x.WhereWhomInfoCollected).NotEmpty().When(x => x.IsDirectlyFromIndividual == false).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleForEach(x => x.WhereWhomInfoCollected).Must(x => new List<string> { "directly", "federal", "provincial", "not_government", "indirectly", "other" }.Contains(x)).When(x => x.IsDirectlyFromIndividual == false).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // WhereWhomInfoCollectedOtherDetails (Page: page_step_3_4_2)
            RuleFor(x => x.WhereWhomInfoCollectedOtherDetails).NotEmpty().When(x => x.IsDirectlyFromIndividual == false && x.WhereWhomInfoCollected.Contains("other")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.WhereWhomInfoCollectedOtherDetails).Length(0, 5000).When(x => x.IsDirectlyFromIndividual == false && x.WhereWhomInfoCollected.Contains("other")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // IsOriginalPurposeConsistent (Page: page_step_3_4_3_and_3_4_4)
            RuleFor(x => x.IsOriginalPurposeConsistent).NotEmpty().When(x => x.IsDirectlyFromIndividual == false && x.WhereWhomInfoCollected.Contains("directly")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // OriginalPurposeIsConsistentSummary (Page: page_step_3_4_3_and_3_4_4)
            RuleFor(x => x.OriginalPurposeIsConsistentSummary).NotEmpty().When(x => x.IsDirectlyFromIndividual == false && x.WhereWhomInfoCollected.Contains("directly") && x.IsOriginalPurposeConsistent == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.OriginalPurposeIsConsistentSummary).Length(0, 5000).When(x => x.IsDirectlyFromIndividual == false && x.WhereWhomInfoCollected.Contains("directly") && x.IsOriginalPurposeConsistent == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // OriginalPurposeIsNotConsistentJustification (Page: page_step_3_4_3_and_3_4_4)
            RuleFor(x => x.OriginalPurposeIsNotConsistentJustification).NotEmpty().When(x => x.IsDirectlyFromIndividual == false && x.WhereWhomInfoCollected.Contains("directly") && x.IsOriginalPurposeConsistent == false).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.OriginalPurposeIsNotConsistentJustification).Length(0, 5000).When(x => x.IsDirectlyFromIndividual == false && x.WhereWhomInfoCollected.Contains("directly") && x.IsOriginalPurposeConsistent == false).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // WhyCollectInfoOtherSource (Page: page_step_3_4_5)
            RuleFor(x => x.WhyCollectInfoOtherSource).NotEmpty().When(x => x.IsDirectlyFromIndividual == false && (x.WhereWhomInfoCollected.Contains("federal") || x.WhereWhomInfoCollected.Contains("provincial") || x.WhereWhomInfoCollected.Contains("not_government") || x.WhereWhomInfoCollected.Contains("indirectly") || x.WhereWhomInfoCollected.Contains("other"))).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.WhyCollectInfoOtherSource).Length(0, 5000).When(x => x.IsDirectlyFromIndividual == false && (x.WhereWhomInfoCollected.Contains("federal") || x.WhereWhomInfoCollected.Contains("provincial") || x.WhereWhomInfoCollected.Contains("not_government") || x.WhereWhomInfoCollected.Contains("indirectly") || x.WhereWhomInfoCollected.Contains("other"))).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // FormalInformationSharingAgreement (Page: page_step_3_4_6_A_and_C)
            RuleFor(x => x.FormalInformationSharingAgreement).NotEmpty().When(x => x.IsDirectlyFromIndividual == false && (x.WhereWhomInfoCollected.Contains("federal") || x.WhereWhomInfoCollected.Contains("provincial") || x.WhereWhomInfoCollected.Contains("not_government") || x.WhereWhomInfoCollected.Contains("indirectly") || x.WhereWhomInfoCollected.Contains("other"))).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.FormalInformationSharingAgreement).Must(x => new List<string> { "yes_already_in_place", "yes_not_established", "no" }.Contains(x)).When(x => x.IsDirectlyFromIndividual == false && (x.WhereWhomInfoCollected.Contains("federal") || x.WhereWhomInfoCollected.Contains("provincial") || x.WhereWhomInfoCollected.Contains("not_government") || x.WhereWhomInfoCollected.Contains("indirectly") || x.WhereWhomInfoCollected.Contains("other"))).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // ThirdPartyCollectionDescription (Page: page_step_3_4_6_A_and_C)
            RuleFor(x => x.ThirdPartyCollectionDescription).NotEmpty().When(x => x.IsDirectlyFromIndividual == false && (x.WhereWhomInfoCollected.Contains("federal") || x.WhereWhomInfoCollected.Contains("provincial") || x.WhereWhomInfoCollected.Contains("not_government") || x.WhereWhomInfoCollected.Contains("indirectly") || x.WhereWhomInfoCollected.Contains("other")) && x.FormalInformationSharingAgreement == "no").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ThirdPartyCollectionDescription).Length(0, 5000).When(x => x.IsDirectlyFromIndividual == false && (x.WhereWhomInfoCollected.Contains("federal") || x.WhereWhomInfoCollected.Contains("provincial") || x.WhereWhomInfoCollected.Contains("not_government") || x.WhereWhomInfoCollected.Contains("indirectly") || x.WhereWhomInfoCollected.Contains("other")) && x.FormalInformationSharingAgreement == "no").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // DocumentationRelevantISA (Page: page_step_3_4_6B)
            RuleFor(x => x.DocumentationRelevantISA).NotEmpty().When(x => x.IsDirectlyFromIndividual == false && x.FormalInformationSharingAgreement == "yes_already_in_place").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.DocumentationRelevantISA).Must(x => new List<string> { "upload", "link", "not_able" }.Contains(x)).When(x => x.IsDirectlyFromIndividual == false && x.FormalInformationSharingAgreement == "yes_already_in_place").WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // DocumentationRelevantISALink (Page: page_step_3_4_6B)
            RuleFor(x => x.DocumentationRelevantISALink).NotEmpty().When(x => x.IsDirectlyFromIndividual == false && x.FormalInformationSharingAgreement == "yes_already_in_place" && x.DocumentationRelevantISA == "link").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.DocumentationRelevantISALink).Length(0, 200).When(x => x.IsDirectlyFromIndividual == false && x.FormalInformationSharingAgreement == "yes_already_in_place" && x.DocumentationRelevantISA == "link").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // DocumentationISAMissingExplanation (Page: page_step_3_4_6B)
            RuleFor(x => x.DocumentationISAMissingExplanation).NotEmpty().When(x => x.IsDirectlyFromIndividual == false && x.FormalInformationSharingAgreement == "yes_already_in_place" && x.DocumentationRelevantISA == "not_able").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.DocumentationISAMissingExplanation).Length(0, 5000).When(x => x.IsDirectlyFromIndividual == false && x.FormalInformationSharingAgreement == "yes_already_in_place" && x.DocumentationRelevantISA == "not_able").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // Documentation_file_upload (Page: page_step_3_4_6B)
            RuleFor(x => x.Documentation_file_upload).NotEmpty().When(x => x.IsDirectlyFromIndividual == false && x.FormalInformationSharingAgreement == "yes_already_in_place" && x.DocumentationRelevantISA == "upload").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // HasExistingPIB (Page: page_step_3_4_7)
            RuleFor(x => x.HasExistingPIB).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // ExistingPIBDetails (Page: page_step_3_4_8)
            RuleFor(x => x.ExistingPIBDetails).NotEmpty().When(x => x.HasExistingPIB == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ExistingPIBDetails).Length(0, 5000).When(x => x.HasExistingPIB == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // NoPIBStatusReason (Page: page_step_3_4_9)
            RuleFor(x => x.NoPIBStatusReason).NotEmpty().When(x => x.HasExistingPIB == false).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.NoPIBStatusReason).Must(x => new List<string> { "new_pib", "update_pib", "no_pib" }.Contains(x)).When(x => x.HasExistingPIB == false).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // HasNewPIBWorkBegun (Page: page_step_3_4_10_a_b_c)
            RuleFor(x => x.HasNewPIBWorkBegun).NotEmpty().When(x => x.HasExistingPIB == false && (x.NoPIBStatusReason == "new_pib" || x.NoPIBStatusReason == "update_pib")).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // PIBStatusExplanationType (Page: page_step_3_4_10_a_b_c)
            RuleFor(x => x.PIBStatusExplanationType).NotEmpty().When(x => x.HasExistingPIB == false && (x.NoPIBStatusReason == "new_pib" || x.NoPIBStatusReason == "update_pib") && x.HasNewPIBWorkBegun == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.PIBStatusExplanationType).Must(x => new List<string> { "text_field", "upload" }.Contains(x)).When(x => x.HasExistingPIB == false && (x.NoPIBStatusReason == "new_pib" || x.NoPIBStatusReason == "update_pib") && x.HasNewPIBWorkBegun == true).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // PIBStatusExplanationText (Page: page_step_3_4_10_a_b_c)
            RuleFor(x => x.PIBStatusExplanationText).NotEmpty().When(x => x.HasExistingPIB == false && (x.NoPIBStatusReason == "new_pib" || x.NoPIBStatusReason == "update_pib") && x.HasNewPIBWorkBegun == true && x.PIBStatusExplanationType == "text_field").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.PIBStatusExplanationText).Length(0, 5000).When(x => x.HasExistingPIB == false && (x.NoPIBStatusReason == "new_pib" || x.NoPIBStatusReason == "update_pib") && x.HasNewPIBWorkBegun == true && x.PIBStatusExplanationType == "text_field").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // PIB_documentation_file_upload (Page: page_step_3_4_10_a_b_c)
            RuleFor(x => x.PIB_documentation_file_upload).NotEmpty().When(x => x.HasExistingPIB == false && (x.NoPIBStatusReason == "new_pib" || x.NoPIBStatusReason == "update_pib") && x.HasNewPIBWorkBegun == true && x.PIBStatusExplanationType == "upload").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // NoBeganPIBStatusExplanationText (Page: page_step_3_4_10_a_b_c)
            RuleFor(x => x.NoBeganPIBStatusExplanationText).NotEmpty().When(x => x.HasExistingPIB == false && (x.NoPIBStatusReason == "new_pib" || x.NoPIBStatusReason == "update_pib") && x.HasNewPIBWorkBegun == false).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.NoBeganPIBStatusExplanationText).Length(0, 5000).When(x => x.HasExistingPIB == false && (x.NoPIBStatusReason == "new_pib" || x.NoPIBStatusReason == "update_pib") && x.HasNewPIBWorkBegun == false).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // WhyNoPlanToReflectNewConsistentPIB (Page: page_step_3_4_10_d_e)
            RuleFor(x => x.WhyNoPlanToReflectNewConsistentPIB).NotEmpty().When(x => x.HasExistingPIB == false && x.NoPIBStatusReason == "no_pib" && x.IsDirectlyFromIndividual == false && x.WhereWhomInfoCollected.Contains("directly") && x.IsOriginalPurposeConsistent == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.WhyNoPlanToReflectNewConsistentPIB).Length(0, 5000).When(x => x.HasExistingPIB == false && x.NoPIBStatusReason == "no_pib" && x.IsDirectlyFromIndividual == false && x.WhereWhomInfoCollected.Contains("directly") && x.IsOriginalPurposeConsistent == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // WhyNoPIBExplanation (Page: page_step_3_4_10_d_e)
            RuleFor(x => x.WhyNoPIBExplanation).NotEmpty().When(x => x.HasExistingPIB == false && x.NoPIBStatusReason == "no_pib" && !(x.IsOriginalPurposeConsistent == true)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.WhyNoPIBExplanation).Length(0, 5000).When(x => x.HasExistingPIB == false && x.NoPIBStatusReason == "no_pib" && !(x.IsOriginalPurposeConsistent == true)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // InstitutionNotifyIndividuals (Page: page_step_3_4_11)
            RuleFor(x => x.InstitutionNotifyIndividuals).NotEmpty().When(x => x.HasExistingPIB == true || x.NoPIBStatusReason == "new_pib" || x.NoPIBStatusReason == "update_pib" || (x.IsDirectlyFromIndividual == false && x.WhereWhomInfoCollected.Contains("directly") && x.IsOriginalPurposeConsistent == true)).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.InstitutionNotifyIndividuals).Must(x => new List<string> { "yes_already_in_place", "yes_not_established", "no" }.Contains(x)).When(x => x.HasExistingPIB == true || (x.NoPIBStatusReason == "new_pib" || x.NoPIBStatusReason == "update_pib") || (x.IsDirectlyFromIndividual == false && x.WhereWhomInfoCollected.Contains("directly") && x.IsOriginalPurposeConsistent == true)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // HasPrivacyNoticeStatement (Page: page_step_3_4_11_a_b_c)
            RuleFor(x => x.HasPrivacyNoticeStatement).NotEmpty().When(x => x.HasExistingPIB == false && x.NoPIBStatusReason == "no_pib" && x.IsDirectlyFromIndividual == false && !x.IsOriginalPurposeConsistent == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // HasDraftFinalVersionPNSAvailable (Page: page_step_3_4_11_a_b_c)
            RuleFor(x => x.HasDraftFinalVersionPNSAvailable).NotEmpty().When(x => x.HasExistingPIB == false && x.NoPIBStatusReason == "no_pib" && x.IsDirectlyFromIndividual == false && !x.IsOriginalPurposeConsistent == true && x.HasPrivacyNoticeStatement == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // PNSStatement (Page: page_step_3_4_11_a_b_c)
            RuleFor(x => x.PNSStatement).NotEmpty().When(x => x.HasExistingPIB == false && x.NoPIBStatusReason == "no_pib" && x.IsDirectlyFromIndividual == false && !x.IsOriginalPurposeConsistent == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // InstitutionNotifyIndividualsDescription (Page: page_step_3_4_12_and_13)
            RuleFor(x => x.InstitutionNotifyIndividualsDescription).NotEmpty().When(x => (x.HasExistingPIB == true && (x.InstitutionNotifyIndividuals == "yes_already_in_place" || x.NoPIBStatusReason == "yes_not_established")) || (x.HasExistingPIB == false && x.NoPIBStatusReason == "no_pib" && x.IsDirectlyFromIndividual == false && (x.IsOriginalPurposeConsistent == null))).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            RuleFor(x => x.InstitutionNotifyIndividualsDescription).Length(0, 5000).When(x => (x.HasExistingPIB == true && (x.InstitutionNotifyIndividuals == "yes_already_in_place" || x.NoPIBStatusReason == "yes_not_established")) || (x.HasExistingPIB == false && x.NoPIBStatusReason == "no_pib" && x.IsDirectlyFromIndividual == false && !(x.IsOriginalPurposeConsistent == true))).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // NotificationTime (Page: page_step_3_4_12_and_13)
            RuleFor(x => x.NotificationTime).NotEmpty().When(x => (x.HasExistingPIB == true && (x.InstitutionNotifyIndividuals == "yes_already_in_place" || x.NoPIBStatusReason == "yes_not_established")) || (x.HasExistingPIB == false && x.NoPIBStatusReason == "no_pib" && x.IsDirectlyFromIndividual == false && !(x.IsOriginalPurposeConsistent == true))).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.NotificationTime).Must(x => new List<string> { "before", "at", "after" }.Contains(x)).When(x => (x.HasExistingPIB == true && (x.InstitutionNotifyIndividuals == "yes_already_in_place" || x.NoPIBStatusReason == "yes_not_established")) || (x.HasExistingPIB == false && x.NoPIBStatusReason == "no_pib" && x.IsDirectlyFromIndividual == false && !(x.IsOriginalPurposeConsistent == true))).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // HasLibraryArchiveCanadaApproved (Page: page_step_3_5_1)
            RuleFor(x => x.HasLibraryArchiveCanadaApproved).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // RecordDispositionAuthorityIdentification (Page: page_step_3_5_1)
            RuleFor(x => x.RecordDispositionAuthorityIdentification).NotEmpty().When(x => x.HasLibraryArchiveCanadaApproved == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.RecordDispositionAuthorityIdentification).Length(0, 5000).When(x => x.HasLibraryArchiveCanadaApproved == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // HowLongRetainPersonalInformationInPhysicalFormat (Page: page_step_3_5_2)
            RuleFor(x => x.HowLongRetainPersonalInformationInPhysicalFormat).NotEmpty().When(x => x.PersonalInformationPhysicalAndOrElectronicFormat == "physical" || x.PersonalInformationPhysicalAndOrElectronicFormat == "both" || x.IsElectronicConvertedToPhysical == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.HowLongRetainPersonalInformationInPhysicalFormat).Length(0, 5000).When(x => x.PersonalInformationPhysicalAndOrElectronicFormat == "physical" || x.PersonalInformationPhysicalAndOrElectronicFormat == "both" || x.IsElectronicConvertedToPhysical == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // HowLongRetainPersonalInformationInElectronicFormat (Page: page_step_3_5_3)
            RuleFor(x => x.HowLongRetainPersonalInformationInElectronicFormat).NotEmpty().When(x => (x.PersonalInformationPhysicalAndOrElectronicFormat == "electronic" && !x.IsElectronicConvertedToPhysical == true) || x.PersonalInformationPhysicalAndOrElectronicFormat == "both" || x.IsPhysicalConvertedToElectronic == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.HowLongRetainPersonalInformationInElectronicFormat).Length(0, 5000).When(x => (x.PersonalInformationPhysicalAndOrElectronicFormat == "electronic" && !x.IsElectronicConvertedToPhysical == true) || x.PersonalInformationPhysicalAndOrElectronicFormat == "both" || x.IsPhysicalConvertedToElectronic == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // ControlsProceduresImplementation (Page: page_step_3_5_4)
            RuleFor(x => x.ControlsProceduresImplementation).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ControlsProceduresImplementation).Must(x => new List<string> { "yes_in_place", "yes_not_established", "no" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // ControlsProceduresImplementationDescription (Page: page_step_3_5_4)
            RuleFor(x => x.ControlsProceduresImplementationDescription).NotEmpty().When(x => x.ControlsProceduresImplementation == "yes_in_place" || x.ControlsProceduresImplementation == "yes_not_established").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ControlsProceduresImplementationDescription).Length(0, 5000).When(x => x.ControlsProceduresImplementation == "yes_in_place" || x.ControlsProceduresImplementation == "yes_not_established").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // WillRetainInformationUnintentionally (Page: page_step_3_5_5)
            RuleFor(x => x.WillRetainInformationUnintentionally).NotEmpty().When(x => x.IsThereCollectNotIntended == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            // WillRetainInformationUnintentionallyDescription (Page: page_step_3_5_5)
            RuleFor(x => x.WillRetainInformationUnintentionallyDescription).NotEmpty().When(x => x.IsThereCollectNotIntended == true && x.WillRetainInformationUnintentionally == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.WillRetainInformationUnintentionallyDescription).Length(0, 5000).When(x => x.IsThereCollectNotIntended == true && x.WillRetainInformationUnintentionally == true).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // MechanismToCorrectPersonalInformation (Page: page_step_3_6_1_a)
            RuleFor(x => x.MechanismToCorrectPersonalInformation).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.MechanismToCorrectPersonalInformation).Must(x => new List<string> { "yes_in_place", "yes_not_established", "no" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // NoMechanismToCorrectPersonalInformationExplanation (Page: page_step_3_6_1_a)
            RuleFor(x => x.NoMechanismToCorrectPersonalInformationExplanation).NotEmpty().When(x => x.MechanismToCorrectPersonalInformation == "no").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.NoMechanismToCorrectPersonalInformationExplanation).Length(0, 5000).When(x => x.MechanismToCorrectPersonalInformation == "no").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // MechanismToCorrectPersonalInformationDescription (Page: page_step_3_6_1_b_c)
            RuleFor(x => x.MechanismToCorrectPersonalInformationDescription).NotEmpty().When(x => x.MechanismToCorrectPersonalInformation == "yes_in_place" || x.MechanismToCorrectPersonalInformation == "yes_not_established").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.MechanismToCorrectPersonalInformationDescription).Length(0, 5000).When(x => x.MechanismToCorrectPersonalInformation == "yes_in_place" || x.MechanismToCorrectPersonalInformation == "yes_not_established").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // WillProvideOpportunityToAddStatement (Page: page_step_3_6_1_b_c)
            RuleFor(x => x.WillProvideOpportunityToAddStatement).NotEmpty().When(x => x.MechanismToCorrectPersonalInformation == "yes_in_place" || x.MechanismToCorrectPersonalInformation == "yes_not_established").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.WillProvideOpportunityToAddStatement).Must(x => new List<string> { "yes_in_place", "yes_not_established", "no" }.Contains(x)).When(x => x.MechanismToCorrectPersonalInformation == "yes_in_place" || x.MechanismToCorrectPersonalInformation == "yes_not_established").WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // NotProvideOpportunityToAddStatementExplanation (Page: page_step_3_6_1_b_c)
            RuleFor(x => x.NotProvideOpportunityToAddStatementExplanation).NotEmpty().When(x => x.MechanismToCorrectPersonalInformation == "yes_in_place" || x.MechanismToCorrectPersonalInformation == "yes_not_established" && x.WillProvideOpportunityToAddStatement == "no").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.NotProvideOpportunityToAddStatementExplanation).Length(0, 5000).When(x => x.MechanismToCorrectPersonalInformation == "yes_in_place" || x.MechanismToCorrectPersonalInformation == "yes_not_established" && x.WillProvideOpportunityToAddStatement == "no").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // IsCollectInformationAuthoritativeSources (Page: page_step_3_6_2)
            RuleFor(x => x.IsCollectInformationAuthoritativeSources).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.IsCollectInformationAuthoritativeSources).Must(x => new List<string> { "yes_in_all_collection", "yes_but_not_in_all_collection", "no" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // CollectInformationAuthoritativeSourcesExplanation (Page: page_step_3_6_2)
            RuleFor(x => x.CollectInformationAuthoritativeSourcesExplanation).NotEmpty().When(x => x.IsCollectInformationAuthoritativeSources == "yes_in_all_collection" || x.IsCollectInformationAuthoritativeSources == "yes_but_not_in_all_collection").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.CollectInformationAuthoritativeSourcesExplanation).Length(0, 5000).When(x => x.IsCollectInformationAuthoritativeSources == "yes_in_all_collection" || x.IsCollectInformationAuthoritativeSources == "yes_but_not_in_all_collection").WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // ProgramAccuracyDescription (Page: page_step_3_6_3)
            RuleFor(x => x.ProgramAccuracyDescription).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.ProgramAccuracyDescription).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            RuleForEach(x => x.BehalfMultipleInstitutionOthers).SetValidator(new BehalfMultipleInstitutionOthersValidator(_localizer)).When(x => x.SingleOrMultiInstitutionPIA == "multi");

            RuleForEach(x => x.PersonalInformationCategory).SetValidator(new PersonalInformationCategoryValidator(_localizer));

            RuleForEach(x => x.PNSStatement).SetValidator(new PNSStatementValidator(_localizer)).When(x => x.HasExistingPIB == false && x.NoPIBStatusReason == "no_pib" && x.IsDirectlyFromIndividual == false && !(x.IsOriginalPurposeConsistent == true) && x.HasPrivacyNoticeStatement == true && x.HasDraftFinalVersionPNSAvailable == true );
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
            RuleFor(x => x.OtherInstitutionHeadFullname).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // OtherInstitutionHeadTitle
            RuleFor(x => x.OtherInstitutionHeadTitle).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.OtherInstitutionHeadTitle).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // OtherInstitutionSection
            RuleFor(x => x.OtherInstitutionSection).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.OtherInstitutionSection).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // OtherInstitutionEmail
            RuleFor(x => x.OtherInstitutionEmail).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.OtherInstitutionEmail).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
            RuleFor(x => x.OtherInstitutionEmail).EmailAddress();

            // SeniorOfficialOtherFullname
            RuleFor(x => x.SeniorOfficialOtherFullname).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.SeniorOfficialOtherFullname).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // SeniorOfficialOtherTitle
            RuleFor(x => x.SeniorOfficialOtherTitle).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.SeniorOfficialOtherTitle).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // SeniorOfficialOtherSection
            RuleFor(x => x.SeniorOfficialOtherSection).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.SeniorOfficialOtherSection).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // SeniorOfficialOtherEmail
            RuleFor(x => x.SeniorOfficialOtherEmail).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.SeniorOfficialOtherEmail).Length(0, 200).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));
            RuleFor(x => x.SeniorOfficialOtherEmail).EmailAddress();

        }
    }
    public partial class PersonalInformationCategoryValidator : AbstractValidator<PersonalInformationCategory>
    {
        public PersonalInformationCategoryValidator(SharedLocalizer _localizer)
        {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Continue;

            // Category
            RuleFor(x => x.Category).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Category).Must(x => new List<string> { "name", "address", "contact_information", "from_social_media", "personal_website", "d_o_b", "current_age", "sex_gender", "physical_attribute", "marital_status", "family", "racial_identity", "ethnic_origin", "city_state_country", "citizenship", "opinions", "affiliations_or_association", "religious", "criminal", "government_issued_id", "employer", "business", "travel", "recorded_files", "transactions", "financial", "medical", "parental_guardian", "substitute_decision_maker", "allegations", "bodily_samples", "physical_biometrics", "behavioural_biometrics", "other" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("SelectedValueNotValid"));

            // SupplementaryInformation
            RuleFor(x => x.SupplementaryInformation).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

            // PersonalInformationElement
            RuleFor(x => x.PersonalInformationElement).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.PersonalInformationElement).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

        }
    }
    public partial class PNSStatementValidator : AbstractValidator<PNSStatement>
    {
        public PNSStatementValidator(SharedLocalizer _localizer)
        {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Continue;

            // Description
            RuleFor(x => x.Description).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
            RuleFor(x => x.Description).Length(0, 5000).WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsOverCharacterLimit"));

        }
    }
}

