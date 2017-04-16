using EmailValidation;
using System;

namespace Dhobi.Common
{
    public static class Utilities
    {
        public static bool IsValidEmailAddress(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return false;
                }
                return EmailValidator.Validate(email);
            }
            catch (Exception exception)
            {
                throw new Exception("Error in email validation" + exception);
            }
        }
    }
}
