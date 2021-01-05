using System.Collections.Generic;
using System;

namespace ComplaintFormCore.Models
{
	public class SurveyPIDModel
	{
		/// <summary>
		/// Page: page_q_1_0<br/>
		/// Institution name<br/>
		/// Possible choices: [/api/Institution/GetAll?lang={locale}]<br/>
		/// Survey question type: dropdown
		/// </summary>
		public string Institution { get; set; }

		/// <summary>
		/// Page: page_q_1_1<br/>
		/// Has this notification been formally approved in accordance with your instit...<br/>
		/// Survey question type: boolean
		/// </summary>
		public bool? HasNotificationApproved { get; set; }

		/// <summary>
		/// Page: page_q_1_1<br/>
		/// Name:<br/>
		/// Required condition: {HasNotificationApproved} = true<br/>
		/// Survey question type: text
		/// </summary>
		public string ApproverName { get; set; }

		/// <summary>
		/// Page: page_q_1_1<br/>
		/// Position title:<br/>
		/// Required condition: {HasNotificationApproved} = true<br/>
		/// Survey question type: text
		/// </summary>
		public string ApproverTitle { get; set; }

		/// <summary>
		/// Page: page_q_1_2<br/>
		/// Name:<br/>
		/// Survey question type: text
		/// </summary>
		public string ContactName { get; set; }

		/// <summary>
		/// Page: page_q_1_2<br/>
		/// Email address:<br/>
		/// Survey question type: text (email)
		/// </summary>
		public string ContactEmail { get; set; }

		/// <summary>
		/// Page: page_q_1_3<br/>
		/// If applicable, please provide your institutional reference or file number<br/>
		/// Survey question type: text
		/// </summary>
		public string ReferenceFileNumber { get; set; }

		/// <summary>
		/// Page: page_q_2_0<br/>
		/// Please provide information about the timing of the disclosure<br/>
		/// Possible choices: [already, future]<br/>
		/// Survey question type: radiogroup
		/// </summary>
		public string TimingOfDisclosure { get; set; }

		/// <summary>
		/// Page: page_q_2_0_a<br/>
		/// Date of Disclosure<br/>
		/// Survey question type: datepicker (date)
		/// </summary>
		public DateTime? DateOfDisclosure { get; set; }

		/// <summary>
		/// Page: page_q_2_0_a<br/>
		/// Please explain why the disclosure occurred prior to notification<br/>
		/// Survey question type: comment
		/// </summary>
		public string DisclosurePriorToNotificationExplanation { get; set; }

		/// <summary>
		/// Page: page_q_2_0_a<br/>
		/// Notification to the Privacy Commissioner<br/>
		/// Possible choices: [not_practical]<br/>
		/// Survey question type: checkbox
		/// </summary>
		public List<string> WasNotPracticalToNotifyPCPrior { get; set; }

		/// <summary>
		/// Page: page_q_2_0_a<br/>
		/// Survey question type: comment
		/// </summary>
		public string NotPracticalToNotifyOPCPriorAddiotnalInfo { get; set; }

		/// <summary>
		/// Page: page_q_2_0_b<br/>
		/// Anticipated Date of Disclosure<br/>
		/// Possible choices: [yes, no]<br/>
		/// Survey question type: radiogroup
		/// </summary>
		public string AnticipatedDateOfDisclosureKnown { get; set; }

		/// <summary>
		/// Page: page_q_2_0_b<br/>
		/// Anticipated Date of Disclosure<br/>
		/// Required condition: {AnticipatedDateOfDisclosureKnown} contains 'yes'<br/>
		/// Survey question type: datepicker (date)
		/// </summary>
		public DateTime? AnticipatedDateOfDisclosure { get; set; }

		/// <summary>
		/// Page: page_q_2_0_b<br/>
		/// If you do not know the exact date, please use the open text box to enter an...<br/>
		/// Required condition: {AnticipatedDateOfDisclosureKnown} contains 'no'<br/>
		/// Survey question type: comment
		/// </summary>
		public string AnticipatedDateOfDisclosureText { get; set; }

		/// <summary>
		/// Page: page_q_3_0<br/>
		/// Please indicate under what legislative authority the disclosure was/will be...<br/>
		/// Possible choices: [public, individual, desda]<br/>
		/// Survey question type: checkbox
		/// </summary>
		public List<string> LegislativeAuthority { get; set; }

		/// <summary>
		/// Page: page_q_3_1<br/>
		/// Has the institution considered other options for this disclosure, such as s...<br/>
		/// Possible choices: [yes, no, not_apply]<br/>
		/// Survey question type: checkbox
		/// </summary>
		public List<string> OtherOptionsForDisclosure { get; set; }

		/// <summary>
		/// Page: page_q_3_1<br/>
		/// Additional information <br/>
		/// Survey question type: comment
		/// </summary>
		public string OtherOptionsForDisclosureAdditonalInfo { get; set; }

	}
}

