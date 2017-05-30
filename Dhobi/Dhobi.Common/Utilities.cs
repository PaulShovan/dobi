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
        public static long GetPresentDate()
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
            var singaporetime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
            var nowDate = (long)singaporetime.Date.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
            return nowDate;
        }

        public static long GetPresentDateTime()
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
            var singaporetime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
            var nowDateTime = (long)singaporetime.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
            return nowDateTime;
        }
        public static string GetFormattedDateFromMillisecond(long millisecond)
        {
            var date = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(millisecond.ToString()));
            return date.ToLongDateString();
        }
    }
}
