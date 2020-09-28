using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ComplaintFormCore.Helpers
{
    public class ValidatorHelpers
    {
        private static readonly Regex _emailRegex = new Regex(@"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$", RegexOptions.Compiled);
        private static readonly Regex _USZipRegEx = new Regex(@"^\d{5}(?:[-\s]\d{4})?$", RegexOptions.Compiled);
        private static readonly Regex _CAZipRegEx = new Regex(@"^([ABCEGHJKLMNPRSTVXY]\d[ABCEGHJKLMNPRSTVWXYZ])\ {0,1}(\d[ABCEGHJKLMNPRSTVWXYZ]\d)$", RegexOptions.Compiled);
        private static readonly Regex _phoneNumberRegex = new Regex(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", RegexOptions.Compiled);

        public static bool IsEmailValid(string email) => !string.IsNullOrWhiteSpace(email) && _emailRegex.Match(email).Success;

        public static bool IsPhoneNumberValid(string number, string country)
        {
            // Validate the phone number if the country is Canada or US
            if (country == "CA" || country == "US")
            {
                var util = PhoneNumberUtil.GetInstance();
                var phoneNumber = new PhoneNumber();
                try
                {
                    phoneNumber = util.Parse(number, country);
                }
                catch
                {
                    // This is not a number, return thats its not good
                    return false;
                }
                var phoneNumberIsValid = util.IsValidNumber(phoneNumber);
                return phoneNumberIsValid;
            }

            return true;
        }

        public static bool IsUsorCanadianZipCode(string zipCode, string country)
        {
            if (string.IsNullOrWhiteSpace(zipCode))
            {
                return false;
            }

            // Canada or US? Check if valid
            // Other, check if its not empty
            switch (country)
            {
                case "CA":
                    return _CAZipRegEx.Match(zipCode.ToUpper()).Success;
                case "US":
                    return _USZipRegEx.Match(zipCode.ToUpper()).Success;
                default:
                    return !string.IsNullOrEmpty(zipCode);
            }
        }
    }
}
