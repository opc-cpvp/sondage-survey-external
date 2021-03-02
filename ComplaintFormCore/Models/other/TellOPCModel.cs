using System.Collections.Generic;
using System;

namespace ComplaintFormCore.Models
{
	public class SurveyTellOPCModel
	{
		/// <summary>
		/// Page: page_main<br/>
		/// My privacy issue or concern:<br/>
		/// Survey question type: comment
		/// </summary>
		public string PrivacyConcern { get; set; }

		/// <summary>
		/// Page: page_main<br/>
		/// First name<br/>
		/// Survey question type: text
		/// </summary>
		public string Complainant_FirstName { get; set; }

		/// <summary>
		/// Page: page_main<br/>
		/// Last name<br/>
		/// Survey question type: text
		/// </summary>
		public string Complainant_LastName { get; set; }

		/// <summary>
		/// Page: page_main<br/>
		/// E-mail address (yourname@domain.com)<br/>
		/// Survey question type: text (email)
		/// </summary>
		public string Complainant_Email { get; set; }

		/// <summary>
		/// Page: page_main<br/>
		/// Pphone number<br/>
		/// Survey question type: text (tel)
		/// </summary>
		public string Complainant_PhoneNumber { get; set; }

	}
}

