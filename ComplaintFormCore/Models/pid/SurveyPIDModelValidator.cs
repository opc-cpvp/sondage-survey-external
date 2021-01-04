using ComplaintFormCore.Resources;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComplaintFormCore.Models.pid
{
	public class SurveyPIDModelValidator : AbstractValidator<SurveyPIDModel>
	{
		public SurveyPIDModelValidator(SharedLocalizer _localizer)
		{
			ValidatorOptions.Global.CascadeMode = CascadeMode.Continue;
		}
	}
}
