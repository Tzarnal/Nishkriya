using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nishkriya.Extensions
{
    public static class DateTimeExtensions
    {
        public static string TimeSince(this DateTime dateTime)
        {
            //The forum account used to scrape should be set to the UTC timezone.
            var timeDifference = (DateTime.UtcNow - dateTime);

            string result;
            if (timeDifference.TotalHours < 1)
            {
                result = Math.Round(timeDifference.TotalMinutes) == 1 ? "1 Minute Ago" : string.Format("{0} Minutes ago", Math.Round(timeDifference.TotalMinutes));
            }
            else if (timeDifference.TotalDays < 1)
            {
                result = Math.Round(timeDifference.TotalHours) == 1 ? "1 Hour Ago" : string.Format("{0} Hours ago", Math.Round(timeDifference.TotalHours));
            }
            else if (timeDifference.TotalDays < 7)
            {
                result = Math.Round(timeDifference.TotalDays) == 1 ? "1 Day Ago" : string.Format("{0} Days ago", Math.Round(timeDifference.TotalDays));
            }
            else
            {
                result = dateTime.ToShortDateString();
            }

            return result;
        }

    }
}