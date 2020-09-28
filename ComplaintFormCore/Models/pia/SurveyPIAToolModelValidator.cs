using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ComplaintFormCore.Resources;
using System.Security.Cryptography.X509Certificates;

namespace ComplaintFormCore.Models.pia
{
    public class SurveyPIAToolModelValidator: AbstractValidator<SurveyPIAToolModel>
    {
        public SurveyPIAToolModelValidator(SharedLocalizer _localizer)
        {
            //this.CascadeMode = CascadeMode.Continue;
            ValidatorOptions.Global.CascadeMode = CascadeMode.Continue;

            //  _localizer.GetLocalizedStringResource("FieldIsOverCharacterLimit")
            //  _localizer.GetLocalizedStringResource("AtLeastOneItemMustBeSelected")
            //  _localizer.GetLocalizedStringResource("EmailInvalid")
            //  _localizer.GetLocalizedStringResource("PhoneNumberInvalid")
            //  _localizer.GetLocalizedStringResource("PostalCodeInvalid")
            //

            RuleFor(x => x.HasLegalAuthority).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));

            //RuleFor(x => x.RelevantLegislationPolicies).Length(0, 5000).WithMessage("HasLegalAuthority - " + _localizer.GetLocalizedStringResource("FieldIsOverCharacterLimit"));

            RuleFor(x => x.ProgamHasMajorChanges).NotEmpty().When(y => y.IsNewprogram == false /*gfsdgf*/); //ItemNotValid


            //List<string> conditions = new List<string> { "str1", "str2", "str3" };
            RuleFor(x => x.ContactATIPQ16).Must(x => new List<string> { "receive_email", "no_email", "conduct_pia" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("ItemNotValid"));
            RuleFor(x => x.ContactATIPQ18).Must(x => new List<string> { "receive_email", "no_email", "conduct_pia" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("ItemNotValid"));



        }

        private bool ValidateBool(bool? value)
        {
            return true;
        }
    }
}
