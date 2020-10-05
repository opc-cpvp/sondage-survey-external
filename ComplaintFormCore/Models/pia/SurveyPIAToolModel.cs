using System.Collections.Generic;
using System;

namespace ComplaintFormCore.Models
{
    public class SurveyPIAToolModel
    {
        /// <summary>
        /// Page: page_before_begin_q_0_1<br/>
        /// Question: 0.1 <br/>
        /// Do you have, or will you soon have, legal authority for this program or act<br/>
        /// </summary>
        public bool HasLegalAuthority { get; set; }

        /// <summary>
        /// Page: page_before_begin_q_0_2<br/>
        /// Question: 0.2 <br/>
        /// What is or will be your legal authority for this program or activity? List <br/>
        /// Required condition: {HasLegalAuthority} = true
        /// </summary>
        public string RelevantLegislationPolicies { get; set; }

        /// <summary>
        /// Page: page_step_1_a<br/>
        /// Question: 1.1<br/>
        /// What is the name of your program or activity?<br/>
        /// </summary>
        public string ProgramName { get; set; }

        /// <summary>
        /// Page: page_step_1_a<br/>
        /// Question:  1.2<br/>
        /// Is this a new program or activity?<br/>
        /// </summary>
        public bool IsNewprogram { get; set; }

        /// <summary>
        /// Page: page_step_1_b<br/>
        /// Question: 1.3<br/>
        /// Is this program or activity undergoing major changes?<br/>
        /// Required condition: {IsNewprogram} = false
        /// </summary>
        public bool? ProgamHasMajorChanges { get; set; }

        /// <summary>
        /// Page: page_step_1_b<br/>
        /// Question: 1.4<br/>
        /// Is the program or activity being contracted out, or transferred to another <br/>
        /// Required condition: {IsNewprogram} = false
        /// </summary>
        public bool? IsProgamContractedOut { get; set; }

        /// <summary>
        /// Page: page_step_1_q_1_5<br/>
        /// Question: 1.5<br/>
        /// Will the program or activity involve personal information?<br/>
        /// </summary>
        public bool IsProgramInvolvePersonalInformation { get; set; }

        /// <summary>
        /// Page: page_step_1_q_1_6<br/>
        /// Question: 1.6<br/>
        /// Select<br/>
        /// Possible choices: [receive_email, no_email, conduct_pia]<br/>
        /// </summary>
        public string ContactATIPQ16 { get; set; }

        /// <summary>
        /// Page: page_step_1_q_1_6<br/>
        /// Question: <br/>
        /// Please provide your email address. (1.6)<br/>
        /// </summary>
        public string UserEmailAddress { get; set; }

        /// <summary>
        /// Page: page_step_1_q_1_7<br/>
        /// Question: 1.7<br/>
        /// Will the program or activity use personal information as part of a decision<br/>
        /// Possible choices: [admin_purpose, non_admin_purpose]<br/>
        /// </summary>
        public string PersonalInfoUsedFor { get; set; }

        /// <summary>
        /// Page: page_step_1_q_1_8<br/>
        /// Question: 1.8<br/>
        /// Select<br/>
        /// Possible choices: [receive_email, no_email, conduct_pia]<br/>
        /// </summary>
        public string ContactATIPQ18 { get; set; }

        /// <summary>
        /// Page: page_step_1_q_1_9<br/>
        /// Question: 1.9<br/>
        /// Which of the following best describes the subject of your PIA?<br/>
        /// Possible choices: [program_activity, other]<br/>
        /// </summary>
        public string SubjectOfPIA { get; set; }

        /// <summary>
        /// Page: page_step_1_q_1_10<br/>
        /// Question: 1.10<br/>
        /// Select<br/>
        /// Possible choices: [receive_email, no_email, conduct_pia]<br/>
        /// </summary>
        public string ContactATIPQ110 { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_1<br/>
        /// Question: 2.1.1<br/>
        /// Is this a single or multi-institutional PIA submission?<br/>
        /// Possible choices: [single, multi, single_related]<br/>
        /// </summary>
        public string SingleOrMultiInstitutionPIA { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_1<br/>
        /// Question: 2.1.1<br/>
        /// Name of institution: <br/>
        /// </summary>
        public string RelatedPIANameInstitution { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_1<br/>
        /// Question: 2.1.1<br/>
        /// Name of program: <br/>
        /// </summary>
        public string RelatedPIANameProgram { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_1<br/>
        /// Question: 2.1.1<br/>
        /// Brief description of the relationship between the PIAs:<br/>
        /// </summary>
        public string RelatedPIADescription { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_2a<br/>
        /// Question: 2.1.2A<br/>
        /// On behalf of which institution are you submitting a PIA? Select from the dr<br/>
        /// </summary>
        public string BehalfSingleInstitution { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_2a<br/>
        /// Question: 2.1.2A<br/>
        /// Name of institution: <br/>
        /// Required condition: {BehalfSingleInstitution} = 9999
        /// </summary>
        public string BehalfSingleInstitutionOther { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_2b<br/>
        /// Question: 2.1.2B<br/>
        /// On behalf of which institution are you submitting a PIA?<br/>
        /// </summary>
        public string BehalfMultipleInstitutionLead { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_2b<br/>
        /// Question: 2.1.2A<br/>
        /// Name of institution: <br/>
        /// Required condition: {BehalfMultipleInstitutionLead} = 9999
        /// </summary>
        public string BehalfMultipleInstitutionLeadOther { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_2b<br/>
        /// Question: 2.1.2B<br/>
        /// Other institutions involved in this PIA.<br/>
        /// </summary>
        public List<BehalfMultipleInstitutionOthers> BehalfMultipleInstitutionOthers { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_3<br/>
        /// Question: 2.1.3<br/>
        /// We would just like to confirm that the lead institution has, in fact, consu...<br/>
        /// Possible choices: [yes, no]<br/>
        /// </summary>
        public string HasLeadInstitutionConsultedOther { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_3<br/>
        /// Question: 2.1.3<br/>
        /// As per the TBS Directive on Privacy Impact Assessment, the lead institution<br/>
        /// Required condition: {HasLeadInstitutionConsultedOther} contains 'no'
        /// </summary>
        public string LeadInstitutionHasNotConsultedOtherReason { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_4<br/>
        /// Question: 2.1.4<br/>
        /// Is your institution seeking Treasury Board approval as a part of the develo<br/>
        /// </summary>
        public bool IsTreasuryBoardApproval { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_5<br/>
        /// Question: 2.1.5<br/>
        /// Full name:<br/>
        /// </summary>
        public string HeadYourInstitutionFullname { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_5<br/>
        /// Question: 2.1.5<br/>
        /// Title<br/>
        /// </summary>
        public string HeadYourInstitutionTitle { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_5<br/>
        /// Question: 2.1.5<br/>
        /// Institution and section:<br/>
        /// </summary>
        public string HeadYourInstitutionSection { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_5<br/>
        /// Question: 2.1.5<br/>
        /// Email address:<br/>
        /// </summary>
        public string HeadYourInstitutionEmail { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_7<br/>
        /// Question: 2.1.7<br/>
        /// Full name:<br/>
        /// </summary>
        public string SeniorOfficialFullname { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_7<br/>
        /// Question: 2.1.7<br/>
        /// Title<br/>
        /// </summary>
        public string SeniorOfficialTitle { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_7<br/>
        /// Question: 2.1.7<br/>
        /// Institution and section:<br/>
        /// </summary>
        public string SeniorOfficialSection { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_7<br/>
        /// Question: 2.1.7<br/>
        /// Email address:<br/>
        /// </summary>
        public string SeniorOfficialEmail { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_9<br/>
        /// Question: 2.1.9<br/>
        /// Who is the best person to contact if we have questions related to this PIA?<br/>
        /// </summary>
        public string PersonContact { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_9<br/>
        /// Question: 2.1.9<br/>
        /// Full name:<br/>
        /// </summary>
        public string AnotherContactFullname { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_9<br/>
        /// Question: 2.1.9<br/>
        /// Title<br/>
        /// </summary>
        public string AnotherContactTitle { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_9<br/>
        /// Question: 2.1.9<br/>
        /// Institution and section:<br/>
        /// </summary>
        public string AnotherContactSection { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_9<br/>
        /// Question: 2.1.9<br/>
        /// Email address:<br/>
        /// </summary>
        public string AnotherContactEmail { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_10<br/>
        /// Question: 2.1.10<br/>
        /// Is this a new PIA, an update or addendum to a previously submitted PIA, or ...<br/>
        /// Possible choices: [new_pia, update_pia, new_pia_covers_already_submitted]<br/>
        /// </summary>
        public string NewOrUpdatedPIA { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_11_a<br/>
        /// Question: 2.1.11A<br/>
        /// You have chosen to update a previous PIA submission. Please indicate the PI<br/>
        /// Possible choices: [update_pia_existing_reference_number, update_pia_not_existing_reference_number]<br/>
        /// </summary>
        public string UpdatePIANumberAssigned { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_11_a<br/>
        /// Question: 2.1.11A<br/>
        /// Please provide us with all reference numbers that you have for the past PIA<br/>
        /// Required condition: {UpdatePIANumberAssigned} contains 'update_pia_existing_reference_number'
        /// </summary>
        public string UpdatePIAAllReferenceNumbersAssigned { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_11_b<br/>
        /// Question: 2.1.11B<br/>
        /// Now let us add some detail. Use the headings from your previous submission <br/>
        /// </summary>
        public string DetailsPreviousSubmission { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_12<br/>
        /// Question: 2.1.12<br/>
        /// You have indicated that this PIA covers a phase of a program or activity, a<br/>
        /// Possible choices: [new_pia_existing_reference_number, new_pia_not_existing_reference_number]<br/>
        /// </summary>
        public string NewPIANumberAssigned { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_12<br/>
        /// Question: 2.1.12<br/>
        /// Please provide us with all reference numbers that you have for the past PIA<br/>
        /// Required condition: {NewPIANumberAssigned} contains 'new_pia_existing_reference_number'
        /// </summary>
        public string NewPIAAllReferenceNumbersAssigned { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_1<br/>
        /// Question: 2.2.1<br/>
        /// Please give us an overview of this program or activity.<br/>
        /// </summary>
        public string ProgramOverview { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_2<br/>
        /// Question: 2.2.2<br/>
        /// Please select the option that best describes participation in your program <br/>
        /// Possible choices: [participation_mandatory_and_automatic, participation_not_mandatory_but_automatic,
        ///                     participation_not_mandatory_but_can_be, participation_voluntary]<br/>
        /// </summary>
        public string ParticipationOptions { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_3<br/>
        /// Question: 2.2.3<br/>
        /// Are there other initiatives that this program or activity is related to?<br/>
        /// </summary>
        public bool OtherInitiatives { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_3<br/>
        /// Question: 2.2.3<br/>
        /// Please provide the name of the related initiative(s) and describe how this <br/>
        /// Required condition: {OtherInitiatives} = true
        /// </summary>
        public string OtherInitiativesDescription { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_4<br/>
        /// Question: 2.2.4<br/>
        /// Please describe the duration of the program or activity by selecting from t<br/>
        /// Possible choices: [pilot, one_time, short_term, long_term]<br/>
        /// </summary>
        public string DurationOptions { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_4<br/>
        /// Question: 2.2.4<br/>
        /// Please type in your answer(s)<br/>
        /// Required condition: {DurationOptions} contains 'long_term'
        /// </summary>
        public string DurationOptionsDescriptions { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_5<br/>
        /// Question: 2.2.5<br/>
        /// Is the program or activity being rolled out in multiple phases?<br/>
        /// </summary>
        public bool IsProgramRolledOutPhases { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_6<br/>
        /// Question: 2.2.6<br/>
        /// Is there an anticipated start date to the program or activity?<br/>
        /// </summary>
        public bool IsAnticipatedStartDate { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_6<br/>
        /// Question: 2.2.6<br/>
        /// Please provide the anticipated start date<br/>
        /// Required condition: {IsAnticipatedStartDate} = true
        /// </summary>
        public string AnticipatedStartDate { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_7<br/>
        /// Question: 2.2.7<br/>
        /// Is there an anticipated end date to the program or activity?<br/>
        /// </summary>
        public bool IsAnticipatedEndDate { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_7<br/>
        /// Question: 2.2.7<br/>
        /// Please provide the anticipated end date<br/>
        /// Required condition: {IsAnticipatedEndDate} = true
        /// </summary>
        public string AnticipatedEndDate { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_8<br/>
        /// Question: 2.2.8<br/>
        /// Does your program or activity involve implementation of a new electronic sy<br/>
        /// </summary>
        public bool IsInvolveNewSoftware { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_8<br/>
        /// Question: 2.2.9<br/>
        /// Please describe the new electronic system or the new application or softwar<br/>
        /// Required condition: {IsInvolveNewSoftware} = true
        /// </summary>
        public string InvolveNewSoftwareDescription { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_10<br/>
        /// Question: 2.2.10<br/>
        /// Does your program or activity require any modifications to information tech...<br/>
        /// </summary>
        public bool DoesRequireModificationToIT { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_10<br/>
        /// Question: 2.2.11<br/>
        /// Please describe the required modifications to information technology (IT) l<br/>
        /// Required condition: {DoesRequireModificationToIT} = true TODO
        /// </summary>
        public string RequireModificationToITDescription { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_12<br/>
        /// Question: 2.2.12<br/>
        /// Are there any changes to your business requirements that will have an impac<br/>
        /// </summary>
        public bool HaschangesToBusinessRequirements { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_12<br/>
        /// Question: 2.2.12<br/>
        /// Please explain the changes to your business requirements that will have an <br/>
        /// Required condition: {HaschangesToBusinessRequirements} = true
        /// </summary>
        public string ChangesToBusinessRequirements { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_13<br/>
        /// Question: 2.2.13<br/>
        /// Are current IT legacy systems and services that will be retained, or those <br/>
        /// </summary>
        public bool WillITLegacySystemRetained { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_14<br/>
        /// Question: 2.2.14<br/>
        /// Please identify any awareness activities related to protection of privacy r<br/>
        /// </summary>
        public string AwarenessActivities { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_15<br/>
        /// Question: 2.2.15<br/>
        /// Describe what is in scope for this phase:<br/>
        /// Required condition: {IsProgramRolledOutPhases} = true
        /// </summary>
        public string ScopeThisPhase { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_15<br/>
        /// Question: 2.2.15<br/>
        /// Describe the scope of other phases (past or future):<br/>
        /// Required condition: {IsProgramRolledOutPhases} = true
        /// </summary>
        public string ScopeOtherPhases { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_15<br/>
        /// Question: 2.2.15<br/>
        /// Describe what is not in scope of the PIAs and justify the decision to not i<br/>
        /// Required condition: {IsProgramRolledOutPhases} = true
        /// </summary>
        public string NotInScopePia { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_15<br/>
        /// Question: 2.2.15B<br/>
        /// Describe what is in scope:<br/>
        /// Required condition: {IsProgramRolledOutPhases} = false
        /// </summary>
        public string ScopePia { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_15<br/>
        /// Question: 2.2.15B<br/>
        /// Describe what is not in scope and justify the decision to not include this <br/>
        /// Required condition: {IsProgramRolledOutPhases} = false TODO
        /// </summary>
        public string NotInScope { get; set; }

        /// <summary>
        /// Page: page_step_3_1_q_3_1_1_q_3_1_2<br/>
        /// Question: 3.1.1<br/>
        /// Please describe why this program or activity is necessary.<br/>
        /// </summary>
        public string WhyProgramNecessary { get; set; }

        /// <summary>
        /// Page: page_step_3_1_q_3_1_1_q_3_1_2<br/>
        /// Question: 3.1.2<br/>
        /// Does your institution plan to measure the effectiveness of this program or ...<br/>
        /// </summary>
        public bool? DoesMeasureEffectiveness { get; set; }

        /// <summary>
        /// Page: page_step_3_1_q_3_1_3<br/>
        /// Question: 3.1.3A<br/>
        /// Please describe how the institution will measure the program or activity's ...<br/>
        /// </summary>
        public string HowMeasureEffectiveness { get; set; }

        /// <summary>
        /// Page: page_step_3_1_q_3_1_3<br/>
        /// Question: 3.1.3B<br/>
        /// Please explain why your institution will not measure this program or activi...<br/>
        /// </summary>
        public string WhyNotMeasureEffectiveness { get; set; }

        /// <summary>
        /// Page: page_step_3_1_q_3_1_4<br/>
        /// Question: 3.1.4<br/>
        /// Please explain how the intrusion on privacy caused by the program or activi...<br/>
        /// </summary>
        public string AssessProportionalityExplanation { get; set; }

        /// <summary>
        /// Page: page_step_3_1_q_3_1_5<br/>
        /// Question: 3.1.5<br/>
        /// Did your institution consider any less privacy intrusive solutions for this...<br/>
        /// </summary>
        public bool? DidConsiderLessIntrusiveSolution { get; set; }

        /// <summary>
        /// Page: page_step_3_1_q_3_1_6<br/>
        /// Question: 3.1.6A<br/>
        /// Please describe any less privacy intrusive solutions you considered.<br/>
        /// </summary>
        public string DescriptionConsiderLessIntrusiveSolution { get; set; }

        /// <summary>
        /// Page: page_step_3_1_q_3_1_6<br/>
        /// Question: 3.1.6B<br/>
        /// Please explain why your institution did not consider any less privacy intru...<br/>
        /// </summary>
        public string WhyNotConsiderLessIntrusiveSolution { get; set; }

        /// <summary>
        /// Page: page_step_3_1_q_3_1_7<br/>
        /// Question: 3.1.7<br/>
        /// Possible choices: [receive_email, conduct_pia]<br/>
        /// </summary>
        public string ContactATIPQ317 { get; set; }

        /// <summary>
        /// Page: page_step_3_2_q_3_2_1<br/>
        /// Question: 3.2.1<br/>
        /// Is the following individual responsible for your institution's compliance w...<br/>
        /// </summary>
        public bool? IsHeadYourInstitutionResponsibleWithPA { get; set; }

        /// <summary>
        /// Page: page_step_3_2_q_3_2_1<br/>
        /// Question: 3.2.2<br/>
        /// Who is responsible for your institution's compliance with the <em>Privacy A...<br/>
        /// </summary>
        public string ResponsibleComplianceWithPA { get; set; }

        /// <summary>
        /// Page: page_step_3_2_q_3_2_3<br/>
        /// Question: 3.2.3<br/>
        /// Please list and briefly describe the policies and procedures that your inst...<br/>
        /// </summary>
        public string ListPoliciesAndProcedures { get; set; }

        /// <summary>
        /// Page: page_step_3_2_q_3_2_4_q_3_2_5<br/>
        /// Question: 3.2.4<br/>
        /// Does your institution ensure that staff receive privacy-related training?<br/>
        /// </summary>
        public bool? DoesStaffReceivedTraining { get; set; }

        /// <summary>
        /// Page: page_step_3_2_q_3_2_4_q_3_2_5<br/>
        /// Question: 3.2.5<br/>
        /// Please describe the privacy training that your staff receives.<br/>
        /// </summary>
        public string StaffReceivedTrainingDescription { get; set; }

        /// <summary>
        /// Page: page_step_3_2_q_3_2_6<br/>
        /// Question: 3.2.6<br/>
        /// Does the institution have or intend to have a process for handling a privac...<br/>
        /// Possible choices: [yes_in_place, yes_not_established, no]<br/>
        /// </summary>
        public string ProcessHandlingPrivacyComplaint { get; set; }

        /// <summary>
        /// Page: page_step_3_2_q_3_2_6<br/>
        /// Question: 3.2.7<br/>
        /// Please describe the process for handling a privacy complaint or inquiry.<br/>
        /// </summary>
        public string ProcessHandlingPrivacyComplaintDescription { get; set; }

    }
    public class BehalfMultipleInstitutionOthers
    {
        /// <summary>
        /// Question: 2.1.2B<br/>
        /// </summary>
        public string BehalfMultipleInstitutionOther { get; set; }

        /// <summary>
        /// Question: 2.1.6<br/>
        /// Head of the government institution or delegate - Full name:<br/>
        /// </summary>
        public string OtherInstitutionHeadFullname { get; set; }

        /// <summary>
        /// Question: 2.1.6<br/>
        /// Head of the government institution or delegate - Title<br/>
        /// </summary>
        public string OtherInstitutionHeadTitle { get; set; }

        /// <summary>
        /// Question: 2.1.6<br/>
        /// Head of the government institution or delegate - Institution and section:<br/>
        /// </summary>
        public string OtherInstitutionSection { get; set; }

        /// <summary>
        /// Question: 2.1.6<br/>
        /// Head of the government institution or delegate - Email address:<br/>
        /// </summary>
        public string OtherInstitutionEmail { get; set; }

        /// <summary>
        /// Question: 2.1.8<br/>
        /// Senior official or executive responsible - Full name:<br/>
        /// </summary>
        public string SeniorOfficialOtherFullname { get; set; }

        /// <summary>
        /// Question: 2.1.8<br/>
        /// Senior official or executive responsible - Title<br/>
        /// </summary>
        public string SeniorOfficialOtherTitle { get; set; }

        /// <summary>
        /// Question: 2.1.8<br/>
        /// Senior official or executive responsible - Institution and section:<br/>
        /// </summary>
        public string SeniorOfficialOtherSection { get; set; }

        /// <summary>
        /// Question: 2.1.8<br/>
        /// Senior official or executive responsible - Email address:<br/>
        /// </summary>
        public string SeniorOfficialOtherEmail { get; set; }

    }
}

