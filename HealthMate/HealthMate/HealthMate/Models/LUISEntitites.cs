using System;
using System.Collections.Generic;
using System.Text;

namespace HealthMate.Models
{
    public class Entity
    {
        public string entity { get; set; }
        public string type { get; set; }
        public int startIndex { get; set; }
        public int endIndex { get; set; }
        public Resolution resolution { get; set; }

    }

    public class Resolution
    {
        public List<object> values { get; set; }
        public string subtype { get; set; }
        public string value { get; set; }
    }

    public class ValueDetail
    {
        public string timex { get; set; }
        public string type { get; set; }
        public string value { get; set; }

    }
}
