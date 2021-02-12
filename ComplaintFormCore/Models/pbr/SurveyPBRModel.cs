using System.Collections.Generic;
using System;

namespace ComplaintFormCore.Models
{
	public class SurveyPBRModel
	{
		/// <summary>
		/// Page: page_organization<br/>
		/// Legal name of the organization<br/>
		/// Survey question type: text
		/// </summary>
		public string OrganizationName { get; set; }

		/// <summary>
		/// Page: page_organization<br/>
		/// Address<br/>
		/// Survey question type: text
		/// </summary>
		public string OrganizationAddress { get; set; }

		/// <summary>
		/// Page: page_organization<br/>
		/// City<br/>
		/// Survey question type: text
		/// </summary>
		public string OrganizationCity { get; set; }

		/// <summary>
		/// Page: page_organization<br/>
		/// Province / State<br/>
		/// Possible choices: [/api/Province?lang={locale}&addOther=true]<br/>
		/// Survey question type: dropdown
		/// </summary>
		public int? OrganizationProvince { get; set; }

		/// <summary>
		/// Page: page_organization<br/>
		/// Province / State<br/>
		/// Survey question type: text
		/// </summary>
		public string OrganizationProvinceOther { get; set; }

		/// <summary>
		/// Page: page_organization<br/>
		/// Country<br/>
		/// Possible choices: [/api/Country?lang={locale}]<br/>
		/// Survey question type: dropdown
		/// </summary>
		public int? OrganizationCountry { get; set; }

		/// <summary>
		/// Page: page_organization<br/>
		/// ZIP / Postal code<br/>
		/// Survey question type: text
		/// </summary>
		public string OrganizationPostalCode { get; set; }

		/// <summary>
		/// Page: page_organization<br/>
		/// Contact information of a person who can answer, on behalf of the organizati...<br/>
		/// Possible choices: [internal_representative, external_representative]<br/>
		/// Survey question type: radiogroup
		/// </summary>
		public string RepresentativeType { get; set; }

		/// <summary>
		/// Page: page_organization<br/>
		/// Name<br/>
		/// Survey question type: text
		/// </summary>
		public string ContactName { get; set; }

		/// <summary>
		/// Page: page_organization<br/>
		/// Title / Position<br/>
		/// Survey question type: text
		/// </summary>
		public string ContactTitle { get; set; }

		/// <summary>
		/// Page: page_organization<br/>
		/// Telephone<br/>
		/// Survey question type: text (tel)
		/// </summary>
		public string ContactPhoneWithCountryCode { get; set; }

		/// <summary>
		/// Page: page_organization<br/>
		/// Extension<br/>
		/// Survey question type: text (number)
		/// </summary>
		public int? ContactPhoneExtension { get; set; }

		/// <summary>
		/// Page: page_organization<br/>
		/// Email<br/>
		/// Survey question type: text (email)
		/// </summary>
		public string ContactEmail { get; set; }

		/// <summary>
		/// Page: page_organization<br/>
		/// Address<br/>
		/// Survey question type: text
		/// </summary>
		public string ContactAddress { get; set; }

		/// <summary>
		/// Page: page_organization<br/>
		/// City<br/>
		/// Survey question type: text
		/// </summary>
		public string ContactCity { get; set; }

		/// <summary>
		/// Page: page_organization<br/>
		/// Province / State<br/>
		/// Possible choices: [/api/Province?lang={locale}&addOther=true]<br/>
		/// Survey question type: dropdown
		/// </summary>
		public int? ContactProvince { get; set; }

		/// <summary>
		/// Page: page_organization<br/>
		/// Province / State<br/>
		/// Survey question type: text
		/// </summary>
		public string ContactProvinceOther { get; set; }

		/// <summary>
		/// Page: page_organization<br/>
		/// Country<br/>
		/// Possible choices: [/api/Country?lang={locale}]<br/>
		/// Survey question type: dropdown
		/// </summary>
		public int? ContactCountry { get; set; }

		/// <summary>
		/// Page: page_organization<br/>
		/// ZIP / Postal code<br/>
		/// Survey question type: text
		/// </summary>
		public string ContactPostalCode { get; set; }

		/// <summary>
		/// Page: page_breach_description_affected<br/>
		/// Do you know the total number of individuals affected?<br/>
		/// Survey question type: boolean
		/// </summary>
		public bool? NumberOfInvidualKnown { get; set; }

		/// <summary>
		/// Page: page_breach_description_affected<br/>
		/// Total number of individuals affected<br/>
		/// Survey question type: text (number)
		/// </summary>
		public int? NumberOfInvidualAffected { get; set; }

		/// <summary>
		/// Page: page_breach_description_affected<br/>
		/// Total number of canadians affected<br/>
		/// Survey question type: text (number)
		/// </summary>
		public int? NumberOfCanadiansAffected { get; set; }

		/// <summary>
		/// Page: page_breach_description_affected<br/>
		/// Additional comments<br/>
		/// Survey question type: comment
		/// </summary>
		public string NumberAffectedComment { get; set; }

		/// <summary>
		/// Page: page_breach_description_dates<br/>
		/// Start date of breach occurrence<br/>
		/// Survey question type: datepicker (date)
		/// </summary>
		public DateTime? DateBreachOccurrenceStart { get; set; }

		/// <summary>
		/// Page: page_breach_description_dates<br/>
		/// End date of breach occurrence<br/>
		/// Survey question type: datepicker (date)
		/// </summary>
		public DateTime? DateBreachOccurrenceEnd { get; set; }

		/// <summary>
		/// Page: page_breach_description_dates<br/>
		/// Additional comments<br/>
		/// Survey question type: comment
		/// </summary>
		public string BreachOccurrenceComment { get; set; }

		/// <summary>
		/// Page: page_breach_description_type<br/>
		/// Type of breach<br/>
		/// Possible choices: [accidental_disclosure, loss_physical_devices, theft_physical_devices, unauthorized_access, other]<br/>
		/// Survey question type: dropdown
		/// </summary>
		public string TypeOfBreach { get; set; }

		/// <summary>
		/// Page: page_breach_description_type<br/>
		/// Type of breach<br/>
		/// Survey question type: comment
		/// </summary>
		public string TypeOfBreachOther { get; set; }

		/// <summary>
		/// Page: page_breach_description_circumstances<br/>
		/// Description of the circumstances of the breach, and, if known, the cause<br/>
		/// Survey question type: comment
		/// </summary>
		public string BreachCircumstanceDescription { get; set; }

		/// <summary>
		/// Page: page_breach_descriptionsecurity<br/>
		/// Description of relevant security safeguards in place at the time of the bre...<br/>
		/// Survey question type: comment
		/// </summary>
		public string SecuritySafeguardsDescription { get; set; }

		/// <summary>
		/// Page: page_breach_description_subject<br/>
		/// Type of personal information that was breached<br/>
		/// Possible choices: [name, phone_number, email_address, account_number, social_insurance_number]<br/>
		/// Survey question type: checkbox
		/// </summary>
		public List<string> PersonalInformation { get; set; }

		/// <summary>
		/// Page: page_breach_description_subject<br/>
		/// Description of the personal information that is the subject of the breach t...<br/>
		/// Survey question type: comment
		/// </summary>
		public string SubjectOfBreach { get; set; }

		/// <summary>
		/// Page: page_notification_steps<br/>
		/// Have affected individuals been notified?<br/>
		/// Survey question type: boolean
		/// </summary>
		public bool? IsAffectedNotified { get; set; }

		/// <summary>
		/// Page: page_notification_steps<br/>
		/// Date notification began (or is planned)<br/>
		/// Survey question type: datepicker
		/// </summary>
		public DateTime? DateNotificationBegan { get; set; }

		/// <summary>
		/// Page: page_notification_steps<br/>
		/// Date notification was completed<br/>
		/// Survey question type: datepicker
		/// </summary>
		public DateTime? DateNotificationCompleted { get; set; }

		/// <summary>
		/// Page: page_notification_steps<br/>
		/// Method of notification used for affected individuals<br/>
		/// Possible choices: [directly, indirectly, directly_and_indirectly, not_notified]<br/>
		/// Survey question type: dropdown
		/// </summary>
		public string MethodOfNotification { get; set; }

		/// <summary>
		/// Page: page_notification_steps<br/>
		/// If you have chosen to notify indirectly, describe the rationale for doing s...<br/>
		/// Required condition: {MethodOfNotification} = 'indirectly'<br/>
		/// Survey question type: comment
		/// </summary>
		public string IndirectlyNotificationDescription { get; set; }

		/// <summary>
		/// Page: page_notification_description<br/>
		/// Form of notification<br/>
		/// Possible choices: [letter, mail, telephone, newspaper]<br/>
		/// Survey question type: checkbox
		/// </summary>
		public List<string> NotificationType { get; set; }

		/// <summary>
		/// Page: page_notification_description<br/>
		/// Describe the form of notification<br/>
		/// Survey question type: comment
		/// </summary>
		public string NotificationDescription { get; set; }

		/// <summary>
		/// Page: page_risk_mitigation_steps<br/>
		/// Description of any steps (apart from notification to affected individuals) ...<br/>
		/// Survey question type: comment
		/// </summary>
		public string StepsReduceRisksDescription { get; set; }

		/// <summary>
		/// Page: page_risk_mitigation_organization<br/>
		/// Have any other organizations and/or government institutions been notified a...<br/>
		/// Survey question type: boolean
		/// </summary>
		public bool? OrganizationNotified { get; set; }

		/// <summary>
		/// Page: page_risk_mitigation<br/>
		/// Description of the steps taken to reduce the risk of a similar event occurr...<br/>
		/// Survey question type: comment
		/// </summary>
		public string StepsReduceRisksFuture { get; set; }


		public List<OrganizationDescription> OrganizationDescription { get; set; }

	}
	public class OrganizationDescription
	{
		/// <summary>
		/// Organization name<br/>
		/// Survey question type: text
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Date notified<br/>
		/// Survey question type: datepicker
		/// </summary>
		public string DateNotified { get; set; }

	}
}

