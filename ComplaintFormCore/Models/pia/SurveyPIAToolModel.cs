using ComplaintFormCore.Helpers;
using ComplaintFormCore.Web_Apis.Models;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ComplaintFormCore.Models
{
    //[Validator(typeof(SurveyPIAToolModelValidator))]
    public class SurveyPIAToolModel
    {

        /// <summary>
        /// Question 0.1
        /// </summary>
        //// [Required]
        public bool? HasLegalAuthority { get; set; }

        /// <summary>
        /// Question 0.2
        /// </summary>
        //// [Required]
        //[StringLength(5000)]
        public string RelevantLegislationPolicies { get; set; }

        /// <summary>
        /// Question 1.1
        /// </summary>
        //// [Required]
        //[StringLength(200)]
        public string ProgramName { get; set; }

        /// <summary>
        /// Question 1.2
        /// </summary>
       // // [Required]
        public bool? IsNewprogram { get; set; }

        /// <summary>
        /// Question 1.3
        /// </summary>
       // //[RequiredIf(nameof(IsNewprogram), false, "This field is required")]
        public bool? ProgamHasMajorChanges { get; set; }

        /// <summary>
        /// Question 1.4
        /// </summary>
       // //[RequiredIf(nameof(IsNewprogram), false, "This field is required")]
        public bool? IsProgamContractedOut { get; set; }

        /// <summary>
        /// Question 1.5
        /// </summary>
      //  // [Required]
        public bool? IsProgramInvolvePersonalInformation { get; set; }

        /// <summary>
        /// Question 1.6 <br/>
        /// Possible values: [receive_email, no_email, conduct_pia]
        /// </summary>
        //// [Required]
        //// [IsInList(new string[] { "receive_email", "no_email", "conduct_pia" }, "Invalid value. Possible choices: receive_email, no_email, conduct_pia")]
        public string ContactATIPQ16 { get; set; }

        /// <summary>
        /// Question 1.7 <br/>
        /// Possible values: [admin_purpose, non_admin_purpose]
        /// </summary>
        //// [Required]
        //// [IsInList(new string[] { "admin_purpose", "non_admin_purpose" }, "Invalid value. Possible choices: admin_purpose, non_admin_purpose")]
        public string PersonalInfoUsedFor { get; set; }

        /// <summary>
        /// Question 1.8
        /// Possible values: [receive_email, no_email, conduct_pia]
        /// </summary>
        ////[RequiredIf(nameof(PersonalInfoUsedFor), "non_admin_purpose", "This field is required")]
        //// [IsInList(new string[] { "receive_email", "no_email", "conduct_pia" }, "Invalid value. Possible choices: receive_email, no_email, conduct_pia")]
        public string ContactATIPQ18 { get; set; }

        /// <summary>
        /// Question 1.9 <br/>
        /// Possible values: [program_activity, other]
        /// </summary>
        // [Required]
        // [IsInList(new string[] { "program_activity", "other" }, "Invalid value. Possible choices: program_activity, other")]
        public string SubjectOfPIA { get; set; }

        /// <summary>
        /// Question 1.10 <br/>
        /// Possible values: [receive_email, no_email, conduct_pia]
        /// </summary>
        //[RequiredIf(nameof(SubjectOfPIA), "other", "This field is required")]
        // [IsInList(new string[] { "receive_email", "no_email", "conduct_pia" }, "Invalid value. Possible choices: receive_email, no_email, conduct_pia")]
        public string ContactATIPQ110 { get; set; }

        /// <summary>
        /// Question 2.1.1 <br/>
        /// Possible values: [single, multi, single_related]
        /// </summary>
        // [Required]
        // [IsInList(new string[] { "single", "multi", "single_related" }, "Invalid value. Possible choices: single, multi, single_related")]
        public string SingleOrMultiInstitutionPIA { get; set; }

        /// <summary>
        /// Question 2.1.1 (if single_related)
        /// </summary>
        //[RequiredIf(nameof(SingleOrMultiInstitutionPIA), "single_related", "This field is required")]
        public string RelatedPIANameInstitution { get; set; }

        /// <summary>
        /// Question 2.1.1 (if single_related)
        /// </summary>
        //[RequiredIf(nameof(SingleOrMultiInstitutionPIA), "single_related", "This field is required")]
        public string RelatedPIANameProgram { get; set; }

        /// <summary>
        /// Question 2.1.1 (if single_related)
        /// </summary>
        //[RequiredIf(nameof(SingleOrMultiInstitutionPIA), "single_related", "This field is required")]
        public string RelatedPIADescription { get; set; }

        /// <summary>
        /// Question 2.1.2A <br/>
        /// On behalf of which institution are you submitting a PIA? Select from the drop-down list.<br/>
        /// If the institution is not listed, please select ”other”.
        /// </summary>
        //[RequiredIfNot(nameof(SingleOrMultiInstitutionPIA), "multi", "This field is required")]
        public string BehalfSingleInstitution { get; set; }

        /// <summary>
        /// Question 2.1.2A
        /// </summary>
        //[RequiredIfNot(nameof(SingleOrMultiInstitutionPIA), "multi", "This field is required")]
        public int? BehalfSingleInstitutionId
        {
            get
            {
                if (int.TryParse(BehalfSingleInstitution, out int institutionId))
                {
                    return institutionId;
                }

                return null;
            }
        }

        /// <summary>
        /// Question 2.1.2A
        /// </summary>
        //[RequiredIf(nameof(BehalfSingleInstitutionId), Institution.OTHER_INSTITUTION_ID, "This field is required")]
        public string BehalfSingleInstitutionOther { get; set; }

        /// <summary>
        /// Question 2.1.2B <br/>
        /// Select the lead institution from the drop-down list. If the institution is not listed, please select ‘other’.
        /// </summary>
        //[RequiredIf(nameof(SingleOrMultiInstitutionPIA), "multi", "This field is required")]
        public string BehalfMultipleInstitutionLead { get; set; }

        /// <summary>
        /// Question 2.1.2B
        /// </summary>
        //[RequiredIf(nameof(SingleOrMultiInstitutionPIA), "multi", "This field is required")]
        public int? BehalfMultipleInstitutionLeadId
        {
            get
            {
                if (int.TryParse(BehalfMultipleInstitutionLead, out int institutionId))
                {
                    return institutionId;
                }

                return null;
            }
        }

        /// <summary>
        /// Question 2.1.2B
        /// </summary>
        //[RequiredIf(nameof(BehalfMultipleInstitutionLeadId), Institution.OTHER_INSTITUTION_ID, "This field is required")]
        public string BehalfMultipleInstitutionLeadOther { get; set; }

        /// <summary>
        /// Question 2.1.2B <br/>
        /// Select all other institutions involved in this PIA. If any of the institutions are not listed, please select ‘other’.
        /// </summary>
        //[RequiredIf(nameof(SingleOrMultiInstitutionPIA), "multi", "This field is required")]
        public List<BehalfMultipleInstitutionOthers> BehalfMultipleInstitutionOthers { get; set; }

        /// <summary>
        /// Question 2.1.3 <br/>
        /// Possible values: [yes, no]
        /// </summary>
        // [Required]
        // [IsInList(new string[] { "yes", "no" }, "Invalid value. Possible choices: yes, no")]
        public string HasLeadInstitutionConsultedOther { get; set; }

        /// <summary>
        /// Question 2.1.3
        /// </summary>
        //[RequiredIf(nameof(HasLeadInstitutionConsultedOther), "yes", "This field is required")]
        public string LeadInstitutionHasNotConsultedOtherReason { get; set; }

        /// <summary>
        /// Question 2.1.4
        /// </summary>
        // [Required]
        public bool? IsTreasuryBoardApproval { get; set; }

        /// <summary>
        /// Question 2.1.5
        /// </summary>
        // [Required]
        [EmailAddress]
        public string HeadYourInstitutionEmail { get; set; }

        /// <summary>
        /// Question 2.1.5
        /// </summary>
        // [Required]
        public string HeadYourInstitutionFullname { get; set; }

        /// <summary>
        /// Question 2.1.5
        /// </summary>
        // [Required]
        public string HeadYourInstitutionSection { get; set; }

        /// <summary>
        /// Question 2.1.5
        /// </summary>
        // [Required]
        public string HeadYourInstitutionTitle { get; set; }

        /// <summary>
        /// Question 2.1.7
        /// </summary>
        // [Required]
        [EmailAddress]
        public string SeniorOfficialEmail { get; set; }

        /// <summary>
        /// Question 2.1.7
        /// </summary>
        // [Required]
        public string SeniorOfficialFullname { get; set; }

        /// <summary>
        /// Question 2.1.7
        /// </summary>
        // [Required]
        public string SeniorOfficialSection { get; set; }

        /// <summary>
        /// Question 2.1.7
        /// </summary>
        // [Required]
        public string SeniorOfficialTitle { get; set; }

        /// <summary>
        /// Question 2.1.9
        /// </summary>
        // [Required]
        public string PersonContact { get; set; }

        /// <summary>
        /// Question 2.1.9
        /// </summary>
        //[RequiredIf(nameof(PersonContact), "another", "This field is required")]
        public string AnotherContactFullname { get; set; }

        /// <summary>
        /// Question 2.1.9
        /// </summary>
        //[RequiredIf(nameof(PersonContact), "another", "This field is required")]
        public string AnotherContactTitle { get; set; }

        /// <summary>
        /// Question 2.1.9
        /// </summary>
        //[RequiredIf(nameof(PersonContact), "another", "This field is required")]
        public string AnotherContactSection { get; set; }

        /// <summary>
        /// Question 2.1.9
        /// </summary>
        //[RequiredIf(nameof(PersonContact), "another", "This field is required")]
        [EmailAddress]
        public string AnotherContactEmail { get; set; }

        /// <summary>
        /// Question 2.1.10 <br/>
        /// Possible values: [new_pia, update_pia, new_pia_covers_already_submitted]
        /// </summary>
        // [Required]
        // [IsInList(new string[] { "new_pia", "update_pia", "new_pia_covers_already_submitted" }, "Invalid value. Possible choices: new_pia, update_pia, new_pia_covers_already_submitted")]
        public string NewOrUpdatedPIA { get; set; }

        /// <summary>
        /// Question 2.1.11A <br/>
        /// Possible values [update_pia_existing_reference_number, update_pia_not_existing_reference_number]
        /// </summary>
        //[RequiredIf(nameof(NewOrUpdatedPIA), "update_pia", "This field is required")]
        // [IsInList(new string[] { "update_pia_existing_reference_number", "update_pia_not_existing_reference_number" }, "Invalid value. Possible choices: update_pia_existing_reference_number, update_pia_not_existing_reference_number")]
        public string UpdatePIANumberAssigned { get; set; }

        /// <summary>
        /// Question 2.1.11A
        /// </summary>
        //[RequiredIf(nameof(UpdatePIANumberAssigned), "update_pia_existing_reference_number", "This field is required")]
        public string UpdatePIAAllReferenceNumbersAssigned { get; set; }

        /// <summary>
        /// Question 2.1.11B
        /// </summary>
        //[RequiredIf(nameof(NewOrUpdatedPIA), "update_pia", "This field is required")]
        public string DetailsPreviousSubmission { get; set; }

        /// <summary>
        /// Question 2.1.12 <br/>
        /// Possible values: [new_pia_existing_reference_number, new_pia_not_existing_reference_number]
        /// </summary>
        //[RequiredIf(nameof(NewOrUpdatedPIA), "new_pia_covers_already_submitted", "This field is required")]
        // [IsInList(new string[] { "new_pia_existing_reference_number", "new_pia_not_existing_reference_number" }, "Invalid value. Possible choices: new_pia_existing_reference_number, new_pia_not_existing_reference_number")]
        public string NewPIANumberAssigned { get; set; }

        /// <summary>
        /// Question 2.1.12
        /// </summary>
        //[RequiredIf(nameof(NewPIANumberAssigned), "new_pia_existing_reference_number", "This field is required")]
        public string NewPIAAllReferenceNumbersAssigned { get; set; }

        [EmailAddress]
        public string UserEmailAddress { get; set; }

        /// <summary>
        /// Question 2.2.1 <br/>
        /// Please give us an overview of this program or activity.
        /// </summary>
      //  // [Required]
      //  [StringLength(5000)]
      //  public string ProgramOverview { get; set; }

      //  /// <summary>
      //  /// Question <br/>
      //  /// Please select the option that best describes participation in your program ...<br/>
      //  /// Possible choices: [participation_mandatory_and_automatic, participation_not_mandatory_but_automatic,
      //  ///                 participation_not_mandatory_but_can_be, participation_voluntary]
      //  /// </summary>
      //  // [Required]
      //  // [IsInList(new string[] { "participation_mandatory_and_automatic", "participation_not_mandatory_but_automatic", "participation_not_mandatory_but_can_be", "participation_voluntary" }, "Invalid value. Possible choices: participation_mandatory_and_automatic, participation_not_mandatory_but_automatic, participation_not_mandatory_but_can_be, participation_voluntary")]
      //  public string ParticipationOptions { get; set; }

      //  /// <summary>
      //  /// Page: 32<br/>
      //  /// Question <br/>
      //  /// Are there other initiatives that this program or activity is related to?<br/>
      //  /// </summary>
      //  // [Required]
      //  public bool? OtherInitiatives { get; set; }

      //  /// <summary>
      //  /// Page: 32<br/>
      //  /// Question <br/>
      //  /// Please provide the name of the related initiative(s) and describe how this ...<br/>
      //  /// </summary>
      // // //[RequiredIf("nameof(property)", "value to valid against", "error message")] // TODO: VERIFY THIS
      //  [StringLength(5000)]
      //  public string OtherInitiativesDescription { get; set; }

      //  /// <summary>
      //  /// Page: 33<br/>
      //  /// Question <br/>
      //  /// Please describe the duration of the program or activity by selecting from t...<br/>
      //  /// Possible choices: [pilot, one_time, short_term, long_term]
      //  /// </summary>
      //  // [Required]
      //  // [IsInList(new string[] { "pilot", "one_time", "short_term", "long_term" }, "Invalid value. Possible choices: pilot, one_time, short_term, long_term")]
      //  public string DurationOptions { get; set; }

      //  /// <summary>
      //  /// Page: 33<br/>
      //  /// Question <br/>
      //  /// Please type in your answer(s)<br/>
      //  /// </summary>
      ////  //[RequiredIf("nameof(property)", "value to valid against", "error message")] // TODO: VERIFY THIS
      //  [StringLength(5000)]
      //  public string DurationOptionsDescriptions { get; set; }

      //  /// <summary>
      //  /// Page: 34<br/>
      //  /// Question <br/>
      //  /// Is the program or activity being rolled out in multiple phases?<br/>
      //  /// </summary>
      //  // [Required]
      //  public bool? IsProgramRolledOutPhases { get; set; }

      //  /// <summary>
      //  /// Page: 35<br/>
      //  /// Question <br/>
      //  /// Is there an anticipated start date to the program or activity?<br/>
      //  /// </summary>
      //  // [Required]
      //  public bool? IsAnticipatedStartDate { get; set; }

      //  /// <summary>
      //  /// Page: 35<br/>
      //  /// Question <br/>
      //  /// Please provide the anticipated start date<br/>
      //  /// </summary>
      //  ////[RequiredIf("nameof(property)", "value to valid against", "error message")] // TODO: VERIFY THIS
      //  [StringLength(5000)]
      //  public string AnticipatedStartDate { get; set; }

      //  /// <summary>
      //  /// Page: 36<br/>
      //  /// Question <br/>
      //  /// Is there an anticipated end date to the program or activity?<br/>
      //  /// </summary>
      //  // [Required]
      //  public bool? IsAnticipatedEndDate { get; set; }

      //  /// <summary>
      //  /// Page: 36<br/>
      //  /// Question <br/>
      //  /// Please provide the anticipated end date<br/>
      //  /// </summary>
      // // //[RequiredIf("nameof(property)", "value to valid against", "error message")] // TODO: VERIFY THIS
      //  [StringLength(5000)]
      //  public string AnticipatedEndDate { get; set; }

      //  /// <summary>
      //  /// Page: 37<br/>
      //  /// Question <br/>
      //  /// Does your program or activity involve implementation of a new electronic sy...<br/>
      //  /// </summary>
      //  // [Required]
      //  public bool? IsInvolveNewSoftware { get; set; }

      //  /// <summary>
      //  /// Page: 37<br/>
      //  /// Question <br/>
      //  /// Please describe the new electronic system or the new application or softwar...<br/>
      //  /// </summary>
      // // //[RequiredIf("nameof(property)", "value to valid against", "error message")] // TODO: VERIFY THIS
      //  [StringLength(5000)]
      //  public string InvolveNewSoftwareDescription { get; set; }

      //  /// <summary>
      //  /// Page: 38<br/>
      //  /// Question <br/>
      //  /// Does your program or activity require any modifications to information tech...<br/>
      //  /// </summary>
      //  // [Required]
      //  public bool? DoesRequireModificationToIT { get; set; }

      //  /// <summary>
      //  /// Page: 38<br/>
      //  /// Question <br/>
      //  /// Please describe the required modifications to information technology (IT) l...<br/>
      //  /// </summary>
      // // //[RequiredIf("nameof(property)", "value to valid against", "error message")] // TODO: VERIFY THIS
      //  [StringLength(5000)]
      //  public string RequireModificationToITDescription { get; set; }

      //  /// <summary>
      //  /// Page: 39<br/>
      //  /// Question <br/>
      //  /// Are there any changes to your business requirements that will have an impac...<br/>
      //  /// </summary>
      //  // [Required]
      //  public bool? HaschangesToBusinessRequirements { get; set; }

      //  /// <summary>
      //  /// Page: 39<br/>
      //  /// Question <br/>
      //  /// Please explain the changes to your business requirements that will have an ...<br/>
      //  /// </summary>
      ////  //[RequiredIf("nameof(property)", "value to valid against", "error message")] // TODO: VERIFY THIS
      //  [StringLength(5000)]
      //  public string ChangesToBusinessRequirements { get; set; }

      //  /// <summary>
      //  /// Page: 40<br/>
      //  /// Question <br/>
      //  /// Are current IT legacy systems and services that will be retained, or those ...<br/>
      //  /// </summary>
      //  // [Required]
      //  public bool? WillITLegacySystemRetained { get; set; }

      //  /// <summary>
      //  /// Page: 41<br/>
      //  /// Question <br/>
      //  /// Please identify any awareness activities related to protection of privacy r...<br/>
      //  /// </summary>
      //  // [Required]
      //  [StringLength(5000)]
      //  public string AwarenessActivities { get; set; }

      //  /// <summary>
      //  /// Page: 42<br/>
      //  /// Question <br/>
      //  /// Describe what is in scope for this phase:<br/>
      //  /// </summary>
      // // //[RequiredIf("nameof(property)", "value to valid against", "error message")] // TODO: VERIFY THIS
      //  [StringLength(5000)]
      //  public string ScopeThisPhase { get; set; }

      //  /// <summary>
      //  /// Page: 42<br/>
      //  /// Question <br/>
      //  /// Describe the scope of other phases (past or future):<br/>
      //  /// </summary>
      // // //[RequiredIf("nameof(property)", "value to valid against", "error message")] // TODO: VERIFY THIS
      //  [StringLength(5000)]
      //  public string ScopeOtherPhases { get; set; }

      //  /// <summary>
      //  /// Page: 42<br/>
      //  /// Question <br/>
      //  /// Describe what is not in scope of the PIAs and justify the decision to not i...<br/>
      //  /// </summary>
      //  ////[RequiredIf("nameof(property)", "value to valid against", "error message")] // TODO: VERIFY THIS
      //  [StringLength(5000)]
      //  public string NotInScopePia { get; set; }

      //  /// <summary>
      //  /// Page: 42<br/>
      //  /// Question <br/>
      //  /// Describe what is in scope:<br/>
      //  /// </summary>
      // // //[RequiredIf("nameof(property)", "value to valid against", "error message")] // TODO: VERIFY THIS
      //  [StringLength(5000)]
      //  public string ScopePia { get; set; }

      //  /// <summary>
      //  /// Page: 42<br/>
      //  /// Question <br/>
      //  /// Describe what is not in scope and justify the decision to not include this ...<br/>
      //  /// </summary>
      // // //[RequiredIf("nameof(property)", "value to valid against", "error message")] // TODO: VERIFY THIS
      //  [StringLength(5000)]
      //  public string NotInScope { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (ContactATIPQ16 != "conduct_pia")
        //    {
        //        yield return new ValidationResult("User should be exited from the e-tool.", new[] { nameof(ContactATIPQ16) });
        //    }

        //    if (ContactATIPQ18 != "conduct_pia")
        //    {
        //        yield return new ValidationResult("User should be exited from the e-tool.", new[] { nameof(ContactATIPQ18) });
        //    }

        //    if (ContactATIPQ110 != "conduct_pia")
        //    {
        //        yield return new ValidationResult("User should be exited from the e-tool.", new[] { nameof(ContactATIPQ110) });
        //    }
        //}
    }

    public class BehalfMultipleInstitutionOthers
    {
        /// <summary>
        /// Question 2.1.2B <br/>
        /// This can be an institution id OR an institution name in case the user has selected 'Other' as institution
        /// </summary>
        // [Required]
        public string BehalfMultipleInstitutionOther { get; set; }

        /// <summary>
        /// Question 2.1.6
        /// </summary>
        // [Required]
        public string OtherInstitutionHeadTitle { get; set; }

        /// <summary>
        /// Question 2.1.6
        /// </summary>
        // [Required]
        public string OtherInstitutionSection { get; set; }

        /// <summary>
        /// Question 2.1.6
        /// </summary>
        // [Required]
        public string OtherInstitutionHeadFullname { get; set; }

        /// <summary>
        /// Question 2.1.6
        /// </summary>
        // [Required]
        [EmailAddress]
        public string OtherInstitutionEmail { get; set; }

        /// <summary>
        /// Question 2.1.8
        /// </summary>
        // [Required]
        public string SeniorOfficialOtherFullname { get; set; }

        /// <summary>
        /// Question 2.1.8
        /// </summary>
        // [Required]
        public string SeniorOfficialOtherTitle { get; set; }

        /// <summary>
        /// Question 2.1.8
        /// </summary>
        // [Required]
        public string SeniorOfficialOtherSection { get; set; }

        /// <summary>
        /// Question 2.1.8
        /// </summary>
        // [Required]
        [EmailAddress]
        public string SeniorOfficialOtherEmail { get; set; }
    }
}
