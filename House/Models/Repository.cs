using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace House.Models
{
    public class Repository
    {
        public static List<Hour> fetchHours()
        {
            List <Hour> hours = new List<Hour>();
            hours.Add(new Hour() { HourID = 1, Period = "11h-13h" });
            hours.Add(new Hour() { HourID = 2, Period = "15h-17h" });
            return hours;
        }
    }
}
