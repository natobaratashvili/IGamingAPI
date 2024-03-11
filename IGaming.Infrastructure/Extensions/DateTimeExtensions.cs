using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGaming.Infrastructure.Security.Extensions
{
    public static class DateTimeExtensions
    {
        public static long ToUnix(this DateTime date)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date.ToUniversalTime() - start).TotalSeconds);
        }
    }
}
