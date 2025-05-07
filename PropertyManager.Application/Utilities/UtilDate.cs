using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyManager.Application.Utilities
{
    public static class UtilDate
    {
        public static bool BeValidDate(string? dateString)
        {
            int currentYear = 1900;
            var valid = DateTime.TryParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None,
                out _);
            int dobYear = valid ? DateTime.Parse(dateString!).Year : 0;

            var valid1 = dobYear >= currentYear;

            return (valid && valid1);
        }
    }
}
