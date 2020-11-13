using System.Collections.Generic;
using System;

namespace ComplaintFormCore.Models
{
    public class SurveyPIAToolModel
    {
        /// <summary>
        /// Page: page_before_begin_q_0_1<br/>
        /// Section: 1<br/>
        /// Do you have, or will you soon have, legal authority for this program or act...<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? HasLegalAuthority { get; set; }

        /// <summary>
        /// Page: page_before_begin_q_0_2<br/>
        /// Section: 1<br/>
        /// What is or will be your legal authority for this program or activity? List ...<br/>
        /// Required condition: {HasLegalAuthority} = true<br/>
        /// Survey question type: comment
        /// </summary>
        public string RelevantLegislationPolicies { get; set; }

        /// <summary>
        /// Page: page_step_1_a<br/>
        /// Section: 1<br/>
        /// What is the name of your program or activity? (1.1)<br/>
        /// Survey question type: text
        /// </summary>
        public string ProgramName { get; set; }

        /// <summary>
        /// Page: page_step_1_a<br/>
        /// Section: 1<br/>
        /// Is this a new program or activity? (1.2)<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? IsNewprogram { get; set; }

        /// <summary>
        /// Page: page_step_1_b<br/>
        /// Section: 1<br/>
        /// Is this program or activity undergoing major changes? (1.3)<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? ProgamHasMajorChanges { get; set; }

        /// <summary>
        /// Page: page_step_1_b<br/>
        /// Section: 1<br/>
        /// Is the program or activity being contracted out, or transferred to another ...<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? IsProgamContractedOut { get; set; }

        /// <summary>
        /// Page: page_step_1_q_1_5<br/>
        /// Section: 1<br/>
        /// Will the program or activity involve personal information? (1.5)<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? IsProgramInvolvePersonalInformation { get; set; }

        /// <summary>
        /// Page: page_step_1_q_1_6<br/>
        /// Section: 1<br/>
        /// Select<br/>
        /// Possible choices: [receive_email, no_email, conduct_pia]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string ContactATIPQ16 { get; set; }

        /// <summary>
        /// Page: page_step_1_q_1_6<br/>
        /// Section: 1<br/>
        /// Please provide your email address. (1.6)<br/>
        /// Survey question type: text (email)
        /// </summary>
        public string UserEmailAddress16 { get; set; }

        /// <summary>
        /// Page: page_step_1_q_1_7<br/>
        /// Section: 1<br/>
        /// Will the program or activity use personal information as part of a decision...<br/>
        /// Possible choices: [admin_purpose, non_admin_purpose]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string PersonalInfoUsedFor { get; set; }

        /// <summary>
        /// Page: page_step_1_q_1_8<br/>
        /// Section: 1<br/>
        /// Select<br/>
        /// Possible choices: [receive_email, no_email, conduct_pia]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string ContactATIPQ18 { get; set; }

        /// <summary>
        /// Page: page_step_1_q_1_8<br/>
        /// Section: 1<br/>
        /// Please provide your email address. (1.8)<br/>
        /// Survey question type: text (email)
        /// </summary>
        public string UserEmailAddress18 { get; set; }

        /// <summary>
        /// Page: page_step_1_q_1_9<br/>
        /// Section: 1<br/>
        /// Which of the following best describes the subject of your PIA?<br/>
        /// Possible choices: [program_activity, other]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string SubjectOfPIA { get; set; }

        /// <summary>
        /// Page: page_step_1_q_1_10<br/>
        /// Section: 1<br/>
        /// Select<br/>
        /// Possible choices: [receive_email, no_email, conduct_pia]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string ContactATIPQ110 { get; set; }

        /// <summary>
        /// Page: page_step_1_q_1_10<br/>
        /// Section: 1<br/>
        /// Please provide your email address. (1.10)<br/>
        /// Survey question type: text (email)
        /// </summary>
        public string UserEmailAddress110 { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_1<br/>
        /// Section: 2<br/>
        /// Is this a single or multi-institutional PIA submission? (2.1.1)<br/>
        /// Possible choices: [single, multi, single_related]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string SingleOrMultiInstitutionPIA { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_1<br/>
        /// Section: 2<br/>
        /// Name of institution: <br/>
        /// Survey question type: text
        /// </summary>
        public string RelatedPIANameInstitution { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_1<br/>
        /// Section: 2<br/>
        /// Name of program: <br/>
        /// Survey question type: text
        /// </summary>
        public string RelatedPIANameProgram { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_1<br/>
        /// Section: 2<br/>
        /// Brief description of the relationship between the PIAs:<br/>
        /// Survey question type: text
        /// </summary>
        public string RelatedPIADescription { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_2a<br/>
        /// Section: 2<br/>
        /// On behalf of which institution are you submitting a PIA? Select from the dr...<br/>
        /// Possible choices: [/api/Institution/GetAll?lang={locale}&addOther=true]<br/>
        /// Survey question type: dropdown
        /// </summary>
        public string BehalfSingleInstitution { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_2a<br/>
        /// Section: 2<br/>
        /// Name of institution: <br/>
        /// Required condition: {BehalfSingleInstitution} = 9999<br/>
        /// Survey question type: text
        /// </summary>
        public string BehalfSingleInstitutionOther { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_2b<br/>
        /// Section: 2<br/>
        /// On behalf of which institution are you submitting a PIA?<br/>
        /// Possible choices: [/api/Institution/GetAll?lang={locale}&addOther=true]<br/>
        /// Survey question type: dropdown
        /// </summary>
        public string BehalfMultipleInstitutionLead { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_2b<br/>
        /// Section: 2<br/>
        /// Name of institution: <br/>
        /// Required condition: {BehalfMultipleInstitutionLead} = 9999<br/>
        /// Survey question type: text
        /// </summary>
        public string BehalfMultipleInstitutionLeadOther { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_3<br/>
        /// Section: 2<br/>
        /// We would just like to confirm that the lead institution has, in fact, consu...<br/>
        /// Possible choices: [yes, no]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string HasLeadInstitutionConsultedOther { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_3<br/>
        /// Section: 2<br/>
        /// As per the TBS Directive on Privacy Impact Assessment, the lead institution...<br/>
        /// Required condition: {HasLeadInstitutionConsultedOther} contains 'no'<br/>
        /// Survey question type: comment
        /// </summary>
        public string LeadInstitutionHasNotConsultedOtherReason { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_4<br/>
        /// Section: 2<br/>
        /// Is your institution seeking Treasury Board approval as a part of the develo...<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? IsTreasuryBoardApproval { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_5<br/>
        /// Section: 2<br/>
        /// Full name:<br/>
        /// Survey question type: text
        /// </summary>
        public string HeadYourInstitutionFullname { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_5<br/>
        /// Section: 2<br/>
        /// Title<br/>
        /// Survey question type: text
        /// </summary>
        public string HeadYourInstitutionTitle { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_5<br/>
        /// Section: 2<br/>
        /// Institution and section:<br/>
        /// Survey question type: text
        /// </summary>
        public string HeadYourInstitutionSection { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_5<br/>
        /// Section: 2<br/>
        /// Email address:<br/>
        /// Survey question type: text (email)
        /// </summary>
        public string HeadYourInstitutionEmail { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_7<br/>
        /// Section: 2<br/>
        /// Full name:<br/>
        /// Survey question type: text
        /// </summary>
        public string SeniorOfficialFullname { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_7<br/>
        /// Section: 2<br/>
        /// Title<br/>
        /// Survey question type: text
        /// </summary>
        public string SeniorOfficialTitle { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_7<br/>
        /// Section: 2<br/>
        /// Institution and section:<br/>
        /// Survey question type: text
        /// </summary>
        public string SeniorOfficialSection { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_7<br/>
        /// Section: 2<br/>
        /// Email address:<br/>
        /// Survey question type: text (email)
        /// </summary>
        public string SeniorOfficialEmail { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_9<br/>
        /// Section: 2<br/>
        /// Who is the best person to contact if we have questions related to this PIA?...<br/>
        /// Possible choices: []<br/>
        /// Survey question type: dropdown
        /// </summary>
        public string PersonContact { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_9<br/>
        /// Section: 2<br/>
        /// Full name:<br/>
        /// Survey question type: text
        /// </summary>
        public string AnotherContactFullname { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_9<br/>
        /// Section: 2<br/>
        /// Title<br/>
        /// Survey question type: text
        /// </summary>
        public string AnotherContactTitle { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_9<br/>
        /// Section: 2<br/>
        /// Institution and section:<br/>
        /// Survey question type: text
        /// </summary>
        public string AnotherContactSection { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_9<br/>
        /// Section: 2<br/>
        /// Email address:<br/>
        /// Survey question type: text (email)
        /// </summary>
        public string AnotherContactEmail { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_10<br/>
        /// Section: 2<br/>
        /// Is this a new PIA, an update or addendum to a previously submitted PIA, or ...<br/>
        /// Possible choices: [new_pia, update_pia, new_pia_covers_already_submitted]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string NewOrUpdatedPIA { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_11_a<br/>
        /// Section: 2<br/>
        /// You have chosen to update a previous PIA submission. Please indicate the PI...<br/>
        /// Possible choices: [update_pia_existing_reference_number, update_pia_not_existing_reference_number]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string UpdatePIANumberAssigned { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_11_a<br/>
        /// Section: 2<br/>
        /// Please provide us with all reference numbers that you have for the past PIA...<br/>
        /// Required condition: {UpdatePIANumberAssigned} contains 'update_pia_existing_reference_number'<br/>
        /// Survey question type: text
        /// </summary>
        public string UpdatePIAAllReferenceNumbersAssigned { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_11_b<br/>
        /// Section: 2<br/>
        /// Now let us add some detail. Use the headings from your previous submission ...<br/>
        /// Survey question type: comment
        /// </summary>
        public string DetailsPreviousSubmission { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_12<br/>
        /// Section: 2<br/>
        /// You have indicated that this PIA covers a phase of a program or activity, a...<br/>
        /// Possible choices: [new_pia_existing_reference_number, new_pia_not_existing_reference_number]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string NewPIANumberAssigned { get; set; }

        /// <summary>
        /// Page: page_step_2_1_q_2_1_12<br/>
        /// Section: 2<br/>
        /// Please provide us with all reference numbers that you have for the past PIA...<br/>
        /// Required condition: {NewPIANumberAssigned} contains 'new_pia_existing_reference_number'<br/>
        /// Survey question type: text
        /// </summary>
        public string NewPIAAllReferenceNumbersAssigned { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_1<br/>
        /// Section: 2<br/>
        /// Please give us an overview of this program or activity.<br/>
        /// Survey question type: comment
        /// </summary>
        public string ProgramOverview { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_2<br/>
        /// Section: 2<br/>
        /// Please select the option that best describes participation in your program ...<br/>
        /// Possible choices: [participation_mandatory_and_automatic, participation_not_mandatory_but_automatic, participation_not_mandatory_but_can_be, participation_voluntary]<br/>
        /// Survey question type: dropdown
        /// </summary>
        public string ParticipationOptions { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_3<br/>
        /// Section: 2<br/>
        /// Are there other initiatives that this program or activity is related to?<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? OtherInitiatives { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_3<br/>
        /// Section: 2<br/>
        /// Please provide the name of the related initiative(s) and describe how this ...<br/>
        /// Required condition: {OtherInitiatives} = true<br/>
        /// Survey question type: comment
        /// </summary>
        public string OtherInitiativesDescription { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_4<br/>
        /// Section: 2<br/>
        /// Please describe the duration of the program or activity by selecting from t...<br/>
        /// Possible choices: [pilot, one_time, short_term, long_term]<br/>
        /// Survey question type: dropdown
        /// </summary>
        public string DurationOptions { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_4<br/>
        /// Section: 2<br/>
        /// Please type in your answer(s)<br/>
        /// Required condition: {DurationOptions} contains 'long_term'<br/>
        /// Survey question type: comment
        /// </summary>
        public string DurationOptionsDescriptions { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_5<br/>
        /// Section: 2<br/>
        /// Is the program or activity being rolled out in multiple phases?<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? IsProgramRolledOutPhases { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_6<br/>
        /// Section: 2<br/>
        /// Is there an anticipated start date to the program or activity?<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? IsAnticipatedStartDate { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_6<br/>
        /// Section: 2<br/>
        /// Please provide the anticipated start date<br/>
        /// Required condition: {IsAnticipatedStartDate} = true<br/>
        /// Survey question type: comment
        /// </summary>
        public string AnticipatedStartDate { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_7<br/>
        /// Section: 2<br/>
        /// Is there an anticipated end date to the program or activity?<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? IsAnticipatedEndDate { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_7<br/>
        /// Section: 2<br/>
        /// Please provide the anticipated end date<br/>
        /// Required condition: {IsAnticipatedEndDate} = true<br/>
        /// Survey question type: comment
        /// </summary>
        public string AnticipatedEndDate { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_8<br/>
        /// Section: 2<br/>
        /// Does your program or activity involve implementation of a new electronic sy...<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? IsInvolveNewSoftware { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_8<br/>
        /// Section: 2<br/>
        /// Please describe the new electronic system or the new application or softwar...<br/>
        /// Required condition: {IsInvolveNewSoftware} = true<br/>
        /// Survey question type: comment
        /// </summary>
        public string InvolveNewSoftwareDescription { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_10<br/>
        /// Section: 2<br/>
        /// Does your program or activity require any modifications to information tech...<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? DoesRequireModificationToIT { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_10<br/>
        /// Section: 2<br/>
        /// Please describe the required modifications to information technology (IT) l...<br/>
        /// Required condition: {DoesRequireModificationToIT} = true<br/>
        /// Survey question type: comment
        /// </summary>
        public string RequireModificationToITDescription { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_12<br/>
        /// Section: 2<br/>
        /// Are there any changes to your business requirements that will have an impac...<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? HaschangesToBusinessRequirements { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_12<br/>
        /// Section: 2<br/>
        /// Please explain the changes to your business requirements that will have an ...<br/>
        /// Required condition: {HaschangesToBusinessRequirements} = true<br/>
        /// Survey question type: comment
        /// </summary>
        public string ChangesToBusinessRequirements { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_13<br/>
        /// Section: 2<br/>
        /// Are current IT legacy systems and services that will be retained, or those ...<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? WillITLegacySystemRetained { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_14<br/>
        /// Section: 2<br/>
        /// Please identify any awareness activities related to protection of privacy r...<br/>
        /// Survey question type: comment
        /// </summary>
        public string AwarenessActivities { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_15<br/>
        /// Section: 2<br/>
        /// Describe what is in scope for this phase:<br/>
        /// Required condition: {IsProgramRolledOutPhases} = true<br/>
        /// Survey question type: comment
        /// </summary>
        public string ScopeThisPhase { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_15<br/>
        /// Section: 2<br/>
        /// Describe the scope of other phases (past or future):<br/>
        /// Required condition: {IsProgramRolledOutPhases} = true<br/>
        /// Survey question type: comment
        /// </summary>
        public string ScopeOtherPhases { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_15<br/>
        /// Section: 2<br/>
        /// Describe what is not in scope of the PIAs and justify the decision to not i...<br/>
        /// Required condition: {IsProgramRolledOutPhases} = true<br/>
        /// Survey question type: comment
        /// </summary>
        public string NotInScopePia { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_15<br/>
        /// Section: 2<br/>
        /// Describe what is in scope:<br/>
        /// Required condition: {IsProgramRolledOutPhases} = false<br/>
        /// Survey question type: comment
        /// </summary>
        public string ScopePia { get; set; }

        /// <summary>
        /// Page: page_step_2_2_q_2_2_15<br/>
        /// Section: 2<br/>
        /// Describe what is not in scope and justify the decision to not include this ...<br/>
        /// Required condition: {IsProgramRolledOutPhases} = false<br/>
        /// Survey question type: comment
        /// </summary>
        public string NotInScope { get; set; }

        /// <summary>
        /// Page: page_step_3_1_q_3_1_1_q_3_1_2<br/>
        /// Section: 3<br/>
        /// Please describe why this program or activity is necessary.<br/>
        /// Survey question type: comment
        /// </summary>
        public string WhyProgramNecessary { get; set; }

        /// <summary>
        /// Page: page_step_3_1_q_3_1_1_q_3_1_2<br/>
        /// Section: 3<br/>
        /// Does your institution plan to measure the effectiveness of this program or ...<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? DoesMeasureEffectiveness { get; set; }

        /// <summary>
        /// Page: page_step_3_1_q_3_1_3<br/>
        /// Section: 3<br/>
        /// Please describe how the institution will measure the program or activity's ...<br/>
        /// Required condition: {DoesMeasureEffectiveness} = true <br/>
        /// Survey question type: comment
        /// </summary>
        public string HowMeasureEffectiveness { get; set; }

        /// <summary>
        /// Page: page_step_3_1_q_3_1_3<br/>
        /// Section: 3<br/>
        /// Please explain why your institution will not measure this program or activi...<br/>
        /// Required condition: {DoesMeasureEffectiveness} = false <br/>
        /// Survey question type: comment
        /// </summary>
        public string WhyNotMeasureEffectiveness { get; set; }

        /// <summary>
        /// Page: page_step_3_1_q_3_1_4<br/>
        /// Section: 3<br/>
        /// Please explain how the intrusion on privacy caused by the program or activi...<br/>
        /// Survey question type: comment
        /// </summary>
        public string AssessProportionalityExplanation { get; set; }

        /// <summary>
        /// Page: page_step_3_1_q_3_1_5<br/>
        /// Section: 3<br/>
        /// Did your institution consider any less privacy intrusive solutions for this...<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? DidConsiderLessIntrusiveSolution { get; set; }

        /// <summary>
        /// Page: page_step_3_1_q_3_1_6<br/>
        /// Section: 3<br/>
        /// Please describe any less privacy intrusive solutions you considered.<br/>
        /// Required condition: {DidConsiderLessIntrusiveSolution} = true<br/>
        /// Survey question type: comment
        /// </summary>
        public string DescriptionConsiderLessIntrusiveSolution { get; set; }

        /// <summary>
        /// Page: page_step_3_1_q_3_1_6<br/>
        /// Section: 3<br/>
        /// Please explain why your institution did not consider any less privacy intru...<br/>
        /// Required condition: {DidConsiderLessIntrusiveSolution} = false<br/>
        /// Survey question type: comment
        /// </summary>
        public string WhyNotConsiderLessIntrusiveSolution { get; set; }

        /// <summary>
        /// Page: page_step_3_1_q_3_1_7<br/>
        /// Section: 3<br/>
        /// Select<br/>
        /// Possible choices: [receive_email, conduct_pia]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string ContactATIPQ317 { get; set; }

        /// <summary>
        /// Page: page_step_3_1_q_3_1_7<br/>
        /// Section: 3<br/>
        /// Please provide your email address.<br/>
        /// Survey question type: text (email)
        /// </summary>
        public string UserEmailAddress317 { get; set; }

        /// <summary>
        /// Page: page_step_3_2_q_3_2_1<br/>
        /// Section: 3<br/>
        /// Is the following individual responsible for your institution's compliance w...<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? IsHeadYourInstitutionResponsibleWithPA { get; set; }

        /// <summary>
        /// Page: page_step_3_2_q_3_2_1<br/>
        /// Section: 3<br/>
        /// Who is responsible for your institution's compliance with the <em>Privacy A...<br/>
        /// Required condition: {IsHeadYourInstitutionResponsibleWithPA} = false<br/>
        /// Survey question type: comment
        /// </summary>
        public string ResponsibleComplianceWithPA { get; set; }

        /// <summary>
        /// Page: page_step_3_2_q_3_2_3<br/>
        /// Section: 3<br/>
        /// Please list and briefly describe the policies and procedures that your inst...<br/>
        /// Survey question type: comment
        /// </summary>
        public string ListPoliciesAndProcedures { get; set; }

        /// <summary>
        /// Page: page_step_3_2_q_3_2_4_q_3_2_5<br/>
        /// Section: 3<br/>
        /// Does your institution ensure that staff receive privacy-related training?<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? DoesStaffReceivedTraining { get; set; }

        /// <summary>
        /// Page: page_step_3_2_q_3_2_4_q_3_2_5<br/>
        /// Section: 3<br/>
        /// Please describe the privacy training that your staff receives.<br/>
        /// Required condition: {DoesStaffReceivedTraining} = true<br/>
        /// Survey question type: comment
        /// </summary>
        public string StaffReceivedTrainingDescription { get; set; }

        /// <summary>
        /// Page: page_step_3_2_q_3_2_6<br/>
        /// Section: 3<br/>
        /// Does the institution have or intend to have a process for handling a privac...<br/>
        /// Possible choices: [yes_in_place, yes_not_established, no]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string ProcessHandlingPrivacyComplaint { get; set; }

        /// <summary>
        /// Page: page_step_3_2_q_3_2_6<br/>
        /// Section: 3<br/>
        /// Please describe the process for handling a privacy complaint or inquiry.<br/>
        /// Required condition: {ProcessHandlingPrivacyComplaint} anyof ['yes_in_place','yes_not_established'] <br/>
        /// Survey question type: comment
        /// </summary>
        public string ProcessHandlingPrivacyComplaintDescription { get; set; }

        /// <summary>
        /// Page: page_step_3_3_1<br/>
        /// Section: 3.3<br/>
        /// Have you undertaken a data minimization exercise?<br/>
        /// Possible choices: [yes, not_yet_planned, no]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string HasDataMinimization { get; set; }

        /// <summary>
        /// Page: page_step_3_3_1<br/>
        /// Section: 3.3<br/>
        /// Please provide additional details about this exercise.<br/>
        /// Required condition: {HasDataMinimization} contains 'yes'<br/>
        /// Survey question type: comment
        /// </summary>
        public string DataMinimizationDetails { get; set; }

        /// <summary>
        /// Page: page_step_3_3_1<br/>
        /// Section: 3.3<br/>
        /// Please provide additional details about this exercise.<br/>
        /// Required condition: {HasDataMinimization} contains 'not_yet_planned'<br/>
        /// Survey question type: comment
        /// </summary>
        public string DataMinimizationPlannedDetails { get; set; }

        /// <summary>
        /// Page: page_step_3_3_4<br/>
        /// Section: 3.3<br/>
        /// Is the context in which the personal information is collected and used sens...<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? IsContextualSensitivities { get; set; }

        /// <summary>
        /// Page: page_step_3_3_4<br/>
        /// Section: 3.3<br/>
        /// Please provide more details about the context in which the personal informa...<br/>
        /// Required condition: {IsContextualSensitivities} = true<br/>
        /// Survey question type: comment
        /// </summary>
        public string ContextualSensitivitiesDetails { get; set; }

        /// <summary>
        /// Page: page_step_pre_3_3_5<br/>
        /// Section: 3.3<br/>
        /// Will your institution be collecting any personal information in a physical ...<br/>
        /// Possible choices: [physical, electronic, both]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string PersonalInformationPhysicalAndOrElectronicFormat { get; set; }

        /// <summary>
        /// Page: page_step_3_3_5<br/>
        /// Section: 3.3<br/>
        /// Please describe your collection of personal information in physical format....<br/>
        /// Survey question type: comment
        /// </summary>
        public string InformationPhysicalFormatDescription { get; set; }

        /// <summary>
        /// Page: page_step_3_3_5<br/>
        /// Section: 3.3<br/>
        /// Will your institution convert or copy any personal information in a physica...<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? IsPhysicalConvertedToElectronic { get; set; }

        /// <summary>
        /// Page: page_step_3_3_5<br/>
        /// Section: 3.3<br/>
        /// Please describe your conversion of personal information from a physical for...<br/>
        /// Required condition: {IsPhysicalConvertedToElectronic} = true<br/>
        /// Survey question type: comment
        /// </summary>
        public string InformationPhysicalConvertedCopyDescription { get; set; }

        /// <summary>
        /// Page: page_step_3_3_6<br/>
        /// Section: 3.3<br/>
        /// Please describe your collection of personal information in electronic forma...<br/>
        /// Survey question type: comment
        /// </summary>
        public string PersonalInformationElectronicFormatDescription { get; set; }

        /// <summary>
        /// Page: page_step_3_3_6<br/>
        /// Section: 3.3<br/>
        /// Will your institution convert or copy any personal information in an electr...<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? IsElectronicConvertedToPhysical { get; set; }

        /// <summary>
        /// Page: page_step_3_3_6<br/>
        /// Section: 3.3<br/>
        /// Please describe your conversion of personal information from an electronic ...<br/>
        /// Required condition: {IsElectronicConvertedToPhysical} = true<br/>
        /// Survey question type: comment
        /// </summary>
        public string IsInformationElectronicConvertedCopyDescription { get; set; }

        /// <summary>
        /// Page: page_step_3_3_8a<br/>
        /// Section: 3.3<br/>
        /// Think now to how you collect personal information, in any format. Is there ...<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? IsThereCollectNotIntended { get; set; }

        /// <summary>
        /// Page: page_step_3_3_8a<br/>
        /// Section: 3.3<br/>
        /// Does your institution have policies or procedures in place to manage the in...<br/>
        /// Required condition: {IsThereCollectNotIntended} = true<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? DoesHavePoliciesProcedures { get; set; }

        /// <summary>
        /// Page: page_step_3_3_8a<br/>
        /// Section: 3.3<br/>
        /// Please describe the policies or procedures in place to manage the inadverte...<br/>
        /// Required condition: {DoesHavePoliciesProcedures} = true<br/>
        /// Survey question type: comment
        /// </summary>
        public string HavePoliciesProceduresDescription { get; set; }

        /// <summary>
        /// Page: page_step_3_4_1<br/>
        /// Section: 3.4<br/>
        /// Do you collect the personal information directly from the individual for th...<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? IsDirectlyFromIndividual { get; set; }

        /// <summary>
        /// Page: page_step_3_4_2<br/>
        /// Section: 3.4<br/>
        /// From where or whom is the personal information collected? Please select all...<br/>
        /// Survey question type: tagbox
        /// </summary>
        public List<string> WhereWhomInfoCollected { get; set; }

        /// <summary>
        /// Page: page_step_3_4_2<br/>
        /// Section: 3.4<br/>
        /// Please type in your answer(s)<br/>
        /// Required condition: {WhereWhomInfoCollected} contains 'other'<br/>
        /// Survey question type: comment
        /// </summary>
        public string WhereWhomInfoCollectedOtherDetails { get; set; }

        /// <summary>
        /// Page: page_step_3_4_3_and_3_4_4<br/>
        /// Section: 3.4<br/>
        /// You have indicated that you collect the personal information directly from ...<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? IsOriginalPurposeConsistent { get; set; }

        /// <summary>
        /// Page: page_step_3_4_3_and_3_4_4<br/>
        /// Section: 3.4<br/>
        /// You have indicated that the personal information collected by your institut...<br/>
        /// Required condition: {IsOriginalPurposeConsistent} = true<br/>
        /// Survey question type: comment
        /// </summary>
        public string OriginalPurposeIsConsistentSummary { get; set; }

        /// <summary>
        /// Page: page_step_3_4_3_and_3_4_4<br/>
        /// Section: 3.4<br/>
        /// You have indicated that you collect the personal information directly from ...<br/>
        /// Required condition: {IsOriginalPurposeConsistent} = false<br/>
        /// Survey question type: comment
        /// </summary>
        public string OriginalPurposeIsNotConsistentJustification { get; set; }

        /// <summary>
        /// Page: page_step_3_4_5<br/>
        /// Section: 3.4<br/>
        /// You have indicated that you do not collect the personal information directl...<br/>
        /// Survey question type: comment
        /// </summary>
        public string WhyCollectInfoOtherSource { get; set; }

        /// <summary>
        /// Page: page_step_3_4_6_A_and_C<br/>
        /// Section: 3.4<br/>
        /// You have indicated that you collect personal information from at least one ...<br/>
        /// Possible choices: [yes_already_in_place, yes_not_established, no]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string FormalInformationSharingAgreement { get; set; }

        /// <summary>
        /// Page: page_step_3_4_6_A_and_C<br/>
        /// Section: 3.4<br/>
        /// In the absence of an information sharing agreement, please provide a descri...<br/>
        /// Required condition: {FormalInformationSharingAgreement} = 'no'<br/>
        /// Survey question type: comment
        /// </summary>
        public string ThirdPartyCollectionDescription { get; set; }

        /// <summary>
        /// Page: page_step_3_4_6B<br/>
        /// Section: 3.4<br/>
        /// Please provide a copy (or copies) of relevant ISAs. (3.4.6B)<br/>
        /// Possible choices: [upload, link, not_able]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string DocumentationRelevantISA { get; set; }

        /// <summary>
        /// Page: page_step_3_4_6B<br/>
        /// Section: 3.4<br/>
        /// Please explain why you are unable to provide a copy or copies of relevant I...<br/>
        /// Required condition: {DocumentationRelevantISA} = 'not_able'<br/>
        /// Survey question type: comment
        /// </summary>
        public string DocumentationISAMissingExplanation { get; set; }

        /// <summary>
        /// Page: page_step_3_4_6B<br/>
        /// Section: 3.4<br/>
        /// Survey question type: file
        /// </summary>
        public List<SurveyFile> ISAFiles { get; set; }

        /// <summary>
        /// Page: page_step_3_4_7<br/>
        /// Section: 3.4<br/>
        /// Is there an existing PIB that, in its current state, accurately describes t...<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? HasExistingPIB { get; set; }

        /// <summary>
        /// Page: page_step_3_4_8<br/>
        /// Section: 3.4<br/>
        /// You have indicated that an existing PIB covers this program or activity. Pl...<br/>
        /// Survey question type: comment
        /// </summary>
        public string ExistingPIBDetails { get; set; }

        /// <summary>
        /// Page: page_step_3_4_9<br/>
        /// Section: 3.4<br/>
        /// You have indicated that there is not currently a PIB that accurately descri...<br/>
        /// Possible choices: [new_pib, update_pib, no_pib]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string NoPIBStatusReason { get; set; }

        /// <summary>
        /// Page: page_step_3_4_10_a_b_c<br/>
        /// Section: 3.4<br/>
        /// You have indicated that your institution plans to either create a new PIB o...<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? HasNewPIBWorkBegun { get; set; }

        /// <summary>
        /// Page: page_step_3_4_10_a_b_c<br/>
        /// Section: 3.4<br/>
        /// You have indicated that your institution has already begun working on eithe...<br/>
        /// Possible choices: [text_field, upload]<br/>
        /// Required condition: {HasNewPIBWorkBegun} = true<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string PIBStatusExplanationType { get; set; }

        /// <summary>
        /// Page: page_step_3_4_10_a_b_c<br/>
        /// Section: 3.4<br/>
        /// Required condition: {PIBStatusExplanationType} contains 'text_field'<br/>
        /// Survey question type: comment
        /// </summary>
        public string PIBStatusExplanationText { get; set; }

        /// <summary>
        /// Page: page_step_3_4_10_a_b_c<br/>
        /// Section: 3.4<br/>
        /// Required condition: {PIBStatusExplanationType} contains 'upload'<br/>
        /// Survey question type: file
        /// </summary>
        public List<SurveyFile> PIBFiles { get; set; }

        /// <summary>
        /// Page: page_step_3_4_10_a_b_c<br/>
        /// Section: 3.4<br/>
        /// You have indicated that your institution plans to either create a new PIB o...<br/>
        /// Required condition: {HasNewPIBWorkBegun} = false<br/>
        /// Survey question type: comment
        /// </summary>
        public string NoBeganPIBStatusExplanationText { get; set; }

        /// <summary>
        /// Page: page_step_3_4_10_d_e<br/>
        /// Section: 3.4<br/>
        /// You have indicated that the institution does not plan create or update any ...<br/>
        /// Required condition: {IsDirectlyFromIndividual} = false and {WhereWhomInfoCollected} = ['directly'] and {IsOriginalPurposeConsistent} = true<br/>
        /// Survey question type: comment
        /// </summary>
        public string WhyNoPlanToReflectNewConsistentPIB { get; set; }

        /// <summary>
        /// Page: page_step_3_4_10_d_e<br/>
        /// Section: 3.4<br/>
        /// You have indicated that the institution does not plan to create or update a...<br/>
        /// Required condition: !({IsOriginalPurposeConsistent} = true)<br/>
        /// Survey question type: comment
        /// </summary>
        public string WhyNoPIBExplanation { get; set; }

        /// <summary>
        /// Page: page_step_3_4_11<br/>
        /// Section: 3.4<br/>
        /// Does your institution notify or intend to notify individuals of the purpose...<br/>
        /// Possible choices: [yes_already_in_place, yes_not_established, no]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string InstitutionNotifyIndividuals { get; set; }

        /// <summary>
        /// Page: page_step_3_4_11_a_b_c<br/>
        /// Section: 3.4<br/>
        /// Will there be a privacy notice statement for this program or activity? (3.4...<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? HasPrivacyNoticeStatement { get; set; }

        /// <summary>
        /// Page: page_step_3_4_11_a_b_c<br/>
        /// Section: 3.4<br/>
        /// Do you have draft or final versions of your PNS available to include in you...<br/>
        /// Required condition: {HasPrivacyNoticeStatement} = true<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? HasDraftFinalVersionPNSAvailable { get; set; }

        /// <summary>
        /// Page: page_step_3_4_12_and_13<br/>
        /// Section: 3.4<br/>
        /// Please describe how you notify affected individuals and what information yo...<br/>
        /// Survey question type: comment
        /// </summary>
        public string InstitutionNotifyIndividualsDescription { get; set; }

        /// <summary>
        /// Page: page_step_3_4_12_and_13<br/>
        /// Section: 3.4<br/>
        /// At what point in time relative to the collection taking place will you noti...<br/>
        /// Possible choices: [before, at, after]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string NotificationTime { get; set; }

        /// <summary>
        /// Page: page_step_3_5_1<br/>
        /// Section: 3.5<br/>
        /// Has Library and Archive Canada approved a records retention and disposal sc...<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? HasLibraryArchiveCanadaApproved { get; set; }

        /// <summary>
        /// Page: page_step_3_5_1<br/>
        /// Section: 3.5<br/>
        /// Please identify the Record Disposition Authority (RDA).  (3.5.1)<br/>
        /// Required condition: {HasLibraryArchiveCanadaApproved} = true<br/>
        /// Survey question type: comment
        /// </summary>
        public string RecordDispositionAuthorityIdentification { get; set; }

        /// <summary>
        /// Page: page_step_3_5_2<br/>
        /// Section: 3.5<br/>
        /// How long do you retain personal information <strong>in physical format</str...<br/>
        /// Survey question type: comment
        /// </summary>
        public string HowLongRetainPersonalInformationInPhysicalFormat { get; set; }

        /// <summary>
        /// Page: page_step_3_5_3<br/>
        /// Section: 3.5<br/>
        /// How long do you retain personal information <strong>in electronic format</s...<br/>
        /// Survey question type: comment
        /// </summary>
        public string HowLongRetainPersonalInformationInElectronicFormat { get; set; }

        /// <summary>
        /// Page: page_step_3_5_4<br/>
        /// Section: 3.5<br/>
        /// Have you implemented, or do you plan to implement, controls and procedures ...<br/>
        /// Possible choices: [yes_in_place, yes_not_established, no]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string ControlsProceduresImplementation { get; set; }

        /// <summary>
        /// Page: page_step_3_5_4<br/>
        /// Section: 3.5<br/>
        /// Please briefly describe the controls and procedures that you have implement...<br/>
        /// Required condition: {ControlsProceduresImplementation} anyof ['yes_in_place','yes_not_established']<br/>
        /// Survey question type: comment
        /// </summary>
        public string ControlsProceduresImplementationDescription { get; set; }

        /// <summary>
        /// Page: page_step_3_5_5<br/>
        /// Section: 3.5<br/>
        /// You previously indicated that it is possible that your institution could in...<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? WillRetainInformationUnintentionally { get; set; }

        /// <summary>
        /// Page: page_step_3_5_5<br/>
        /// Section: 3.5<br/>
        /// Please describe the circumstances in which you would retain the personal in...<br/>
        /// Required condition: {WillRetainInformationUnintentionally} = 'true'<br/>
        /// Survey question type: comment
        /// </summary>
        public string WillRetainInformationUnintentionallyDescription { get; set; }

        /// <summary>
        /// Page: page_step_3_6_1_a<br/>
        /// Section: 3.6<br/>
        /// Does your institution provide, or plan to provide, individuals with a mecha...<br/>
        /// Possible choices: [yes_in_place, yes_not_established, no]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string MechanismToCorrectPersonalInformation { get; set; }

        /// <summary>
        /// Page: page_step_3_6_1_a<br/>
        /// Section: 3.6<br/>
        /// Please explain why your institution does not intend to provide individuals ...<br/>
        /// Required condition: {MechanismToCorrectPersonalInformation} = 'no'<br/>
        /// Survey question type: comment
        /// </summary>
        public string NoMechanismToCorrectPersonalInformationExplanation { get; set; }

        /// <summary>
        /// Page: page_step_3_6_1_b_c<br/>
        /// Section: 3.6<br/>
        /// Please describe the mechanism(s) by which individuals can or will be able t...<br/>
        /// Survey question type: comment
        /// </summary>
        public string MechanismToCorrectPersonalInformationDescription { get; set; }

        /// <summary>
        /// Page: page_step_3_6_1_b_c<br/>
        /// Section: 3.6<br/>
        /// Does your institution provide, or plan to provide, individuals the opportun...<br/>
        /// Possible choices: [yes_in_place, yes_not_established, no]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string WillProvideOpportunityToAddStatement { get; set; }

        /// <summary>
        /// Page: page_step_3_6_1_b_c<br/>
        /// Section: 3.6<br/>
        /// Please explain why your institution does not intend to provide individuals ...<br/>
        /// Required condition: {WillProvideOpportunityToAddStatement} contains 'no'<br/>
        /// Survey question type: comment
        /// </summary>
        public string NotProvideOpportunityToAddStatementExplanation { get; set; }

        /// <summary>
        /// Page: page_step_3_6_2<br/>
        /// Section: 3.6<br/>
        /// Does your program collect personal information from authoritative sources?<br/>
        /// Possible choices: [yes_in_all_collection, yes_but_not_in_all_collection, no]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string IsCollectInformationAuthoritativeSources { get; set; }

        /// <summary>
        /// Page: page_step_3_6_2<br/>
        /// Section: 3.6<br/>
        /// Please explain how you determined that your sources for personal informatio...<br/>
        /// Required condition: {IsCollectInformationAuthoritativeSources} anyof ['yes_in_all_collection','yes_but_not_in_all_collection']<br/>
        /// Survey question type: comment
        /// </summary>
        public string CollectInformationAuthoritativeSourcesExplanation { get; set; }

        /// <summary>
        /// Page: page_step_3_6_3<br/>
        /// Section: 3.6<br/>
        /// In addition to any information provided in the previous questions in this s...<br/>
        /// Survey question type: comment
        /// </summary>
        public string ProgramAccuracyDescription { get; set; }

        /// <summary>
        /// Page: page_step_3_7_1<br/>
        /// Section: 3.7<br/>
        /// Does the institution have, or intend to have, an established process by whi...<br/>
        /// Possible choices: [yes_in_place, yes_not_established, no]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string ProcessDisposalPhysicalFormat { get; set; }

        /// <summary>
        /// Page: page_step_3_7_1<br/>
        /// Section: 3.7<br/>
        /// Have you documented and made available to employees, or are planning to do ...<br/>
        /// Possible choices: [yes_in_place, yes_not_established, no]<br/>
        /// Required condition: {ProcessDisposalPhysicalFormat} anyof ['yes_in_place','yes_not_established']<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string ProcessDisposalDocumentationPhysicalFormat { get; set; }

        /// <summary>
        /// Page: page_step_3_7_1<br/>
        /// Section: 3.7<br/>
        /// How will the institution dispose of the personal information in physical fo...<br/>
        /// Survey question type: comment
        /// </summary>
        public string DisposalPhysicalFormatDescription { get; set; }

        /// <summary>
        /// Page: page_step_3_7_2<br/>
        /// Section: 3.7<br/>
        /// Does your institution have or plan to have, an established process by which...<br/>
        /// Possible choices: [yes_in_place, yes_not_established, no]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string ProcessDisposalElectronicFormat { get; set; }

        /// <summary>
        /// Page: page_step_3_7_2<br/>
        /// Section: 3.7<br/>
        /// Have you documented and made available to employees, or are planning to do ...<br/>
        /// Possible choices: [yes_in_place, yes_not_established, no]<br/>
        /// Required condition: {ProcessDisposalElectronicFormat} anyof ['yes_in_place','yes_not_established']<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string ProcessDisposalDocumentationElectronicFormat { get; set; }

        /// <summary>
        /// Page: page_step_3_7_2<br/>
        /// Section: 3.7<br/>
        /// How will the institution dispose of the personal information in electronic ...<br/>
        /// Survey question type: comment
        /// </summary>
        public string DisposalElectronicFormatDescription { get; set; }

        /// <summary>
        /// Page: page_step_3_7_3<br/>
        /// Section: 3.7<br/>
        /// What triggers the process to dispose of the personal information, in all fo...<br/>
        /// Survey question type: comment
        /// </summary>
        public string TriggerProcessToDispose { get; set; }

        /// <summary>
        /// Page: page_step_3_7_4<br/>
        /// Section: 3.7<br/>
        /// Do you keep, or plan to keep, a record of your disposal of personal informa...<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? WillKeepRecordOfDisposal { get; set; }

        /// <summary>
        /// Page: page_step_3_7_5<br/>
        /// Section: 3.7<br/>
        /// When disposing of equipment or devices used for storing personal informatio...<br/>
        /// Possible choices: [yes_in_place, yes_not_established, no]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string EquipementDeviceDeleteStoredInformation { get; set; }

        /// <summary>
        /// Page: page_step_3_8_1<br/>
        /// Section: 3.8<br/>
        /// Please describe the population that will be impacted by the program or acti...<br/>
        /// Possible choices: [certain_employees, all_employees, certain_individuals, all_individuals]<br/>
        /// Survey question type: dropdown
        /// </summary>
        public string PopulationImpactedByUseOfPI { get; set; }

        /// <summary>
        /// Page: page_step_3_8_1<br/>
        /// Section: 3.8<br/>
        /// Please provide additional details to further refine your answer. Examples i...<br/>
        /// Survey question type: comment
        /// </summary>
        public string PopulationImpactedByUseOfPIAdditionalInfo { get; set; }

        /// <summary>
        /// Page: page_step_3_8_2<br/>
        /// Section: 3.8<br/>
        /// Does the program or activity focus on vulnerable groups? (3.8.2)<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? DoesFocusVulnerableGroups { get; set; }

        /// <summary>
        /// Page: page_step_3_8_2<br/>
        /// Section: 3.8<br/>
        /// Please provide a description of the vulnerable group(s) on which your progr...<br/>
        /// Required condition: {DoesFocusVulnerableGroups} = true<br/>
        /// Survey question type: comment
        /// </summary>
        public string FocusVulnerableGroupsDescription { get; set; }

        /// <summary>
        /// Page: page_step_3_8_3<br/>
        /// Section: 3.8<br/>
        /// Which of the following best describes the size of the population impacted b...<br/>
        /// Possible choices: [small, significant, entire]<br/>
        /// Survey question type: dropdown
        /// </summary>
        public string SizePopulationImpacted { get; set; }

        /// <summary>
        /// Page: page_step_3_8_4_and_5<br/>
        /// Section: 3.8<br/>
        /// How does the institution use personal information under the program or acti...<br/>
        /// Possible choices: [, administration, compliance, criminal_investigation]<br/>
        /// Survey question type: dropdown
        /// </summary>
        public string HowInstitutionUsePersonalInformation { get; set; }

        /// <summary>
        /// Page: page_step_3_8_4_and_5<br/>
        /// Section: 3.8<br/>
        /// Please provide more detail. More specifically, how and for what purpose doe...<br/>
        /// Survey question type: comment
        /// </summary>
        public string HowInstitutionUsePersonalInformationMoreDetails { get; set; }

        /// <summary>
        /// Page: page_step_3_8_6<br/>
        /// Section: 3.8<br/>
        /// Does your institution use the personal information: (3.8.6)<br/>
        /// Survey question type: tagbox
        /// </summary>
        public List<string> InstitutionUsePersonalInformation { get; set; }

        /// <summary>
        /// Page: page_step_3_8_7<br/>
        /// Section: 3.8<br/>
        /// Is any of the personal information used for any of the following? Select al...<br/>
        /// Survey question type: tagbox
        /// </summary>
        public List<string> PersonalInformationUsedFor { get; set; }

        /// <summary>
        /// Page: page_step_3_8_8_and_9<br/>
        /// Section: 3.8<br/>
        /// Do you use de-identified information, where possible?<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? DoesUseDeIndentification { get; set; }

        /// <summary>
        /// Page: page_step_3_8_8_and_9<br/>
        /// Section: 3.8<br/>
        /// Please describe the process and parameters used for de-identification.<br/>
        /// Survey question type: comment
        /// </summary>
        public string UseDeIndentificationDescription { get; set; }

        /// <summary>
        /// Page: page_step_3_9_1<br/>
        /// Section: 3.9<br/>
        /// Will the program area that is responsible for collection of the personal in...<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? IsCollectionReasonOtherThanStorage { get; set; }

        /// <summary>
        /// Page: page_step_3_9_2<br/>
        /// Section: 3.9<br/>
        /// Does your institution disclose the personal information: (3.9.2)<br/>
        /// Survey question type: tagbox
        /// </summary>
        public List<string> DisclosePersonalInformationMethod { get; set; }

        /// <summary>
        /// Page: page_step_3_9_4<br/>
        /// Section: 3.9<br/>
        /// Will all of the personal information elements that you are collecting (as d...<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? WillAllPICollectedBeDisclosed { get; set; }

        /// <summary>
        /// Page: page_step_3_9_6A<br/>
        /// Section: 3.9<br/>
        /// Are information sharing agreements (ISAs) in place with any of the third pa...<br/>
        /// Possible choices: [yes_in_place, yes_not_established, no]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string InformationSharingAgreementInPlace { get; set; }

        /// <summary>
        /// Page: page_step_3_9_6B<br/>
        /// Section: 3.9<br/>
        /// Does/do the ISA(s) require any updates to protect adequately the personal i...<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? DoesISARequireUpdates { get; set; }

        /// <summary>
        /// Page: page_step_3_9_6B<br/>
        /// Section: 3.9<br/>
        /// Please briefly describe the required updates. You will have the opportunity...<br/>
        /// Required condition: {DoesISARequireUpdates} = true<br/>
        /// Survey question type: comment
        /// </summary>
        public string ISARequireUpdatesDescription { get; set; }

        /// <summary>
        /// Page: page_step_3_9_6D<br/>
        /// Section: 3.9<br/>
        /// In the absence of an information sharing agreement, please provide a descri...<br/>
        /// Survey question type: comment
        /// </summary>
        public string NoISADisclosureDescription { get; set; }

        /// <summary>
        /// Page: page_step_3_9_7_and_3_9_8<br/>
        /// Section: 3.9<br/>
        /// Does your institution inform individuals of the purpose for which it disclo...<br/>
        /// Possible choices: [yes_in_place, yes_not_established, no]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string InformIndividuals { get; set; }

        /// <summary>
        /// Page: page_step_3_9_7_and_3_9_8<br/>
        /// Section: 3.9<br/>
        /// How will the institution communicate the purpose of its disclosure of perso...<br/>
        /// Required condition: {InformIndividuals} anyof ['yes_in_place','yes_not_established']<br/>
        /// Survey question type: comment
        /// </summary>
        public string InformIndividualsCommunication { get; set; }

        /// <summary>
        /// Page: page_step_3_9_9<br/>
        /// Section: 3.9<br/>
        /// Flow chart or diagram<br/>
        /// Survey question type: file
        /// </summary>
        public List<SurveyFile> FlowChartFiles { get; set; }

        /// <summary>
        /// Page: page_step_3_10_1<br/>
        /// Section: 3.10<br/>
        /// Have you assigned a security designation to the personal information?<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? HasSecurityDesignation { get; set; }

        /// <summary>
        /// Page: page_step_3_10_2<br/>
        /// Section: 3.10<br/>
        /// Is the security designation that has been assigned to the personal informat...<br/>
        /// Survey question type: boolean
        /// </summary>
        public bool? IsSecurityDesignationSameToAllPICollected { get; set; }

        /// <summary>
        /// Page: page_step_3_10_3<br/>
        /// Section: 3.10<br/>
        /// What is the *highest* security designation that you have assigned to the pe...<br/>
        /// Possible choices: [protected_A, protected_B, protected_C, classified, confidential, secret, top_secret, other]<br/>
        /// Survey question type: dropdown
        /// </summary>
        public string HighestSecurityDesignation { get; set; }

        /// <summary>
        /// Page: page_step_3_10_3<br/>
        /// Section: 3.10<br/>
        /// Security Designation: <br/>
        /// Required condition: {HighestSecurityDesignation} = 'other'<br/>
        /// Survey question type: comment
        /// </summary>
        public string SecurityDesignationDescription { get; set; }

        /// <summary>
        /// Page: page_step_3_10_4_A<br/>
        /// Section: 3.10<br/>
        /// Has your institution completed or plans to complete any security assessment...<br/>
        /// Survey question type: tagbox
        /// </summary>
        public List<string> CompleteSecuritySssessments { get; set; }


        public List<BehalfMultipleInstitutionOthers> BehalfMultipleInstitutionOthers { get; set; }

        public List<DocumentationRelevantISALinks> DocumentationRelevantISALinks { get; set; }

        public List<OtherPartiesSharePersonalInformation> OtherPartiesSharePersonalInformation { get; set; }

        public List<PartiesSharePersonalInformation> PartiesSharePersonalInformation { get; set; }

        public List<PersonalInformationCategory> PersonalInformationCategory { get; set; }

        public List<PNSStatement> PNSStatement { get; set; }

    }
    public class BehalfMultipleInstitutionOthers
    {
        /// <summary>
        ///  <br/>
        /// Possible choices: [/api/Institution/GetAll?lang={locale}&addOther=false]<br/>
        /// Survey question type: dropdown
        /// </summary>
        public string BehalfMultipleInstitutionOther { get; set; }

        /// <summary>
        /// Full name:<br/>
        /// Survey question type: text
        /// </summary>
        public string OtherInstitutionHeadFullname { get; set; }

        /// <summary>
        /// Title<br/>
        /// Survey question type: text
        /// </summary>
        public string OtherInstitutionHeadTitle { get; set; }

        /// <summary>
        /// Institution and section:<br/>
        /// Survey question type: text
        /// </summary>
        public string OtherInstitutionSection { get; set; }

        /// <summary>
        /// Email address:<br/>
        /// Survey question type: text (email)
        /// </summary>
        public string OtherInstitutionEmail { get; set; }

        /// <summary>
        /// Full name:<br/>
        /// Survey question type: text
        /// </summary>
        public string SeniorOfficialOtherFullname { get; set; }

        /// <summary>
        /// Title<br/>
        /// Survey question type: text
        /// </summary>
        public string SeniorOfficialOtherTitle { get; set; }

        /// <summary>
        /// Institution and section:<br/>
        /// Survey question type: text
        /// </summary>
        public string SeniorOfficialOtherSection { get; set; }

        /// <summary>
        /// Email address:<br/>
        /// Survey question type: text (email)
        /// </summary>
        public string SeniorOfficialOtherEmail { get; set; }

    }
    public class DocumentationRelevantISALinks
    {
        /// <summary>
        /// Link (s)<br/>
        /// Survey question type: text (url)
        /// </summary>
        public string Link { get; set; }

    }
    public class OtherPartiesSharePersonalInformation
    {
        /// <summary>
        /// Party or parties: <br/>
        /// Survey question type: text
        /// </summary>
        public string Party { get; set; }

        /// <summary>
        /// Purpose of disclosure<br/>
        /// Survey question type: comment
        /// </summary>
        public string PurposeOfDisclosure { get; set; }

    }
    public class PartiesSharePersonalInformation
    {
        /// <summary>
        /// Possible choices: [same_institution, federal_government_institution, provincial_in_canada, regional_in_canada, government_outside_canada, non_governmental_organization_in_canada, non_governmental_organization_outside_canada, private_sector_in_canada, private_sector_outside_canada, other]<br/>
        /// Survey question type: dropdown
        /// </summary>
        public string Party { get; set; }

        /// <summary>
        /// Description<br/>
        /// Survey question type: comment
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Source<br/>
        /// Possible choices: [upload, link, not_able]<br/>
        /// Survey question type: radiogroup
        /// </summary>
        public string RelevantISASourceType { get; set; }

        /// <summary>
        /// Required condition: {panel.RelevantISASourceType} = 'upload'<br/>
        /// Survey question type: file
        /// </summary>
        public List<SurveyFile> ISAFilesParties { get; set; }

        /// <summary>
        /// Please enter a web URL that links to a copy or copies of relevant ISAs<br/>
        /// Required condition: {panel.RelevantISASourceType} = 'link'<br/>
        /// Survey question type: matrixdynamic
        /// </summary>
        public class RelevantISALinksObject
        {
            /// <summary>
            /// Link (s)<br/>
            /// Survey question type: text (url)
            /// </summary>
            public string Link { get; set; }

        }
        public List<RelevantISALinksObject> RelevantISALinks { get; set; }

        /// <summary>
        /// Please explain why you are unable to provide a copy or copies of relevant I...<br/>
        /// Required condition: {panel.RelevantISASourceType} = 'not_able'<br/>
        /// Survey question type: comment
        /// </summary>
        public string DocumentationISAMissingExplanation { get; set; }

    }
    public class PersonalInformationCategory
    {
        /// <summary>
        /// Possible choices: [name, address, contact_information, from_social_media, personal_website, d_o_b, current_age, sex_gender, physical_attribute, marital_status, family, racial_identity, ethnic_origin, city_state_country, citizenship, opinions, affiliations_or_association, religious, criminal, government_issued_id, employer, business, travel, recorded_files, transactions, financial, medical, parental_guardian, substitute_decision_maker, allegations, bodily_samples, physical_biometrics, behavioural_biometrics, other]<br/>
        /// Survey question type: dropdown
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Supplementary information<br/>
        /// Survey question type: comment
        /// </summary>
        public string SupplementaryInformation { get; set; }

        /// <summary>
        /// Personal information element<br/>
        /// Survey question type: comment
        /// </summary>
        public string PersonalInformationElement { get; set; }

        /// <summary>
        /// Purpose of disclosure<br/>
        /// Survey question type: comment
        /// </summary>
        public string PurposeOfDisclosure { get; set; }

        /// <summary>
        /// Receiving parties:<br/>
        /// Possible choices: []<br/>
        /// Survey question type: dropdown
        /// </summary>
        public string ReceivingParties { get; set; }

    }
    public class PNSStatement
    {
        /// <summary>
        /// Description:<br/>
        /// Survey question type: comment
        /// </summary>
        public string Description { get; set; }

    }
}

