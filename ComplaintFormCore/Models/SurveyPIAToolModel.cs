using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ComplaintFormCore.Models
{
    public class SurveyPIAToolModel : IValidatableObject
    {
        /// <summary>
        /// page_before_begin_q_0_1
        /// </summary>
        [Required]
        public bool HasLegalAuthority { get; set; }

        /// <summary>
        /// page_before_begin_q_0_2
        /// </summary>
        [Required]
        public string RelevantLegislationPolicies { get; set; }

        /// <summary>
        /// page_step_1_a
        /// </summary>
        [Required]
        public string ProgramName { get; set; }

        /// <summary>
        /// page_step_1_a
        /// </summary>
        [Required]
        public bool IsNewprogram { get; set; }

        /// <summary>
        /// page_step_1_b
        /// </summary>
        [Required]
        public bool ProgamHasMajorChanges { get; set; }

        /// <summary>
        /// page_step_1_b
        /// </summary>
        public bool IsProgamContractedOut { get; set; }

        /// <summary>
        /// page_step_1_q_1_5
        /// </summary>
        public bool IsProgramInvolvePersonalInformation { get; set; }

        /// <summary>
        /// page_step_1_q_1_6
        /// Possible values: [receive_email, no_email, conduct_pia]
        /// </summary>
        public string ContactATIPQ16 { get; set; }

        /// <summary>
        /// page_step_1_q_1_7
        /// Possible values: [admin_purpose, non_admin_purpose]
        /// </summary>
        public string PersonalInfoUsedFor { get; set; }

        /// <summary>
        /// page_step_1_q_1_8
        /// Possible values: [receive_email, no_email, conduct_pia]
        /// </summary>
        public string ContactATIPQ18 { get; set; }

        /// <summary>
        /// page_step_1_q_1_9
        /// Possible values: [program_activity, other]
        /// </summary>
        public string SubjectOfPIA { get; set; }

        /// <summary>
        /// page_step_1_q_1_10
        /// Possible values: [receive_email, no_email, conduct_pia]
        /// </summary>
        public string ContactATIPQ110 { get; set; }

        /// <summary>
        /// page_step_2_1_q_2_1_1
        /// Possible values: [single, multi, single_related]
        /// </summary>
        public string SingleOrMultiInstitutionPIA { get; set; }

        /// <summary>
        /// page_step_2_1_q_2_1_1
        /// </summary>
        public string RelatedPIANameInstitution { get; set; }

        /// <summary>
        /// page_step_2_1_q_2_1_1
        /// </summary>
        public string RelatedPIANameProgram { get; set; }

        /// <summary>
        /// page_step_2_1_q_2_1_1
        /// </summary>
        public string RelatedPIADescription { get; set; }

        /// <summary>
        /// page_step_2_1_q_2_1_2a
        /// </summary>
        public string BehalfSingleInstitution { get; set; }

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
        /// page_step_2_1_q_2_1_2a
        /// </summary>
        public string BehalfSingleInstitutionOther { get; set; }

        /// <summary>
        /// page_step_2_1_q_2_1_2b
        /// </summary>
        public string BehalfMultipleInstitutionLead { get; set; }

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
        /// page_step_2_1_q_2_1_2b
        /// </summary>
        public string BehalfMultipleInstitutionLeadOther { get; set; }

       // public List<BehalfMultipleInstitutionOthers> BehalfMultipleInstitutionOthers { get; set; }

        /// <summary>
        /// page_step_2_1_q_2_1_3
        /// Possible values: [yes, no]
        /// </summary>
        public string LeadInstitutionConsultedOther { get; set; }

        /// <summary>
        /// page_step_2_1_q_2_1_3
        /// </summary>
        public string LeadInstitutionHasNotConsultedOtherReason { get; set; }

        /// <summary>
        /// page_step_2_1_q_2_1_4
        /// </summary>
        public bool IsTreasuryBoardApproval { get; set; }

        /// <summary>
        /// page_step_2_1_q_2_1_5
        /// </summary>
        public string HeadYourInstitutionEmail { get; set; }

        /// <summary>
        /// page_step_2_1_q_2_1_5
        /// </summary>
        public string HeadYourInstitutionFullname { get; set; }

        /// <summary>
        /// page_step_2_1_q_2_1_5
        /// </summary>
        public string HeadYourInstitutionSection { get; set; }

        /// <summary>
        /// page_step_2_1_q_2_1_5
        /// </summary>
        public string HeadYourInstitutionTitle { get; set; }

        /// <summary>
        /// page_step_2_1_q_2_1_7
        /// </summary>
        public string SeniorOfficialEmail { get; set; }

        /// <summary>
        /// page_step_2_1_q_2_1_7
        /// </summary>
        public string SeniorOfficialFullname { get; set; }

        /// <summary>
        /// page_step_2_1_q_2_1_7
        /// </summary>
        public string SeniorOfficialSection { get; set; }

        /// <summary>
        /// page_step_2_1_q_2_1_7
        /// </summary>
        public string SeniorOfficialTitle { get; set; }

        /// <summary>
        /// page_step_2_1_q_2_1_9
        /// </summary>
        public string PersonContact { get; set; }

        /// <summary>
        /// page_step_2_1_q_2_1_9
        /// </summary>
        public string AnotherContactFullname { get; set; }

        /// <summary>
        /// page_step_2_1_q_2_1_9
        /// </summary>
        public string AnotherContactTitle { get; set; }

        /// <summary>
        /// page_step_2_1_q_2_1_9
        /// </summary>
        public string AnotherContactSection { get; set; }

        /// <summary>
        /// page_step_2_1_q_2_1_9
        /// </summary>
        public string AnotherContactEmail { get; set; }

        /// <summary>
        /// page_step_2_1_q_2_1_10
        /// Possible values: [new_pia, update_pia, new_pia_covers_already_submitted]
        /// </summary>
        public string NewOrUpdatedPIA { get; set; }

        /// <summary>
        /// page_step_2_1_q_2_1_11_a
        /// Possible values [update_pia_existing_reference_number, update_pia_not_existing_reference_number]
        /// </summary>
        public string UpdatePIANumberAssigned { get; set; }

        /// <summary>
        /// page_step_2_1_q_2_1_11_a
        /// </summary>
        public string UpdatePIAAllReferenceNumbersAssigned { get; set; }

        /// <summary>
        /// page_step_2_1_q_2_1_11_b
        /// </summary>
        public string DetailsPreviousSubmission { get; set; }

        /// <summary>
        /// page_step_2_1_q_2_1_12
        /// Possible values: [new_pia_existing_reference_number, new_pia_not_existing_reference_number]
        /// </summary>
        public string NewPIANumberAssigned { get; set; }

        /// <summary>
        /// page_step_2_1_q_2_1_12
        /// </summary>
        public string NewPIAAllReferenceNumbersAssigned { get; set; }

        public string UserEmailAddress { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();

        }
    }

    public class BehalfMultipleInstitutionOthers
    {
        public string OtherInstitutionHeadTitle { get; set; }
        public string OtherInstitutionSection { get; set; }
        public string BehalfMultipleInstitutionOther { get; set; }
        public string OtherInstitutionHeadFullname { get; set; }
        public string OtherInstitutionEmail { get; set; }
        public string SeniorOfficialOtherFullname { get; set; }
        public string SeniorOfficialOtherTitle { get; set; }
        public string SeniorOfficialOtherSection { get; set; }
        public string SeniorOfficialOtherEmail { get; set; }
    }
}
