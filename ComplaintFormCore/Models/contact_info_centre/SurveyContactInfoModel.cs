using System.Collections.Generic;
using System;

namespace ComplaintFormCore.Models
{
	public class SurveyContactInfoModel
	{
		/// <summary>
		/// Page: page__start<br/>
		/// First name<br/>
		/// Survey question type: text
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Page: page__start<br/>
		/// Last name<br/>
		/// Survey question type: text
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Page: page__start<br/>
		/// Phone Number<br/>
		/// Survey question type: text
		/// </summary>
		public string PhoneNumber { get; set; }

		/// <summary>
		/// Page: page__start<br/>
		/// E-mail address (yourname@domain.com)<br/>
		/// Survey question type: text (email)
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Page: page__start<br/>
		/// Province/Territory<br/>
		/// Possible choices: [/api/Province?lang={locale}&addOther=true]<br/>
		/// Survey question type: dropdown
		/// </summary>
		public int? ProvinceOrState { get; set; }

		/// <summary>
		/// Page: page__start<br/>
		/// Are you contacting us...<br/>
		/// Possible choices: [individual, federal_or_agency, private_sector, organization_school_hospital]<br/>
		/// Survey question type: dropdown
		/// </summary>
		public string ContactingUsAs { get; set; }

		/// <summary>
		/// Page: page__start<br/>
		/// Do your questions relate to the personal information handling practices of....<br/>
		/// Possible choices: [individual, federal_or_agency, private_sector, organization_school_hospital]<br/>
		/// Survey question type: dropdown
		/// </summary>
		public string QuestionsRelateTo { get; set; }

		/// <summary>
		/// Page: page_topics<br/>
		/// What is the topic of your enquiry? Making a selection is not mandatory, but...<br/>
		/// Possible choices: [personal_information, advertising_marketing, air_travel, authentication_identification, behavioural_advertising, biometrics, businesses_collection_pi, businesses_safeguarding_pi, casl_spam, cloud, compliance, consent, driver_licence, electronic_disclosure_tribunal_decisions, complaint_pipeda, genetic_privacy, gps, health_information, identity_theft, online_privacy, jurisdiction, landlords_tenants, non_profit_sector, OPC_role_mandate, outsourcing, personal_financial_information, privacy_kids, privacy_breaches, impact_assessments, privacy_policies, safety_law_enforcement, sin, social_networking, surveillance_monitoring, technology_privacy, workplace_privacy, privacy_rights]<br/>
		/// Survey question type: checkbox
		/// </summary>
		public List<string> TopicOfEnquiry { get; set; }

		/// <summary>
		/// Page: page_question_details<br/>
		/// What is your question for the Information Centre? (Please do not provide an...<br/>
		/// Survey question type: comment
		/// </summary>
		public string QuestionToInformationCenterDetails { get; set; }

	}
}
