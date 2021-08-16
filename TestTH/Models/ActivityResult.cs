using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTH.Models
{
    public class ActivityResult
    {
        public int ID { get; set; }
        public DateTime schedule { get; set; }
        public string title { get; set; }
        public DateTime created_at { get; set; }
        public string status { get; set; }
        public string condition { get; set; }
        public string survey { get; set; }

        public PropertyResult PropertyResult { get; set; }
    }

    public class PropertyResult
    {
        public int ID { get; set; }
        public string title { get; set; }
        public string address { get; set; }
    }
}
