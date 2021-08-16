using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace TestTH.Models
{
    public class Survey
    {

        [Key]
        public int surveyid { get; set; }

        [Required]
        public int activityid { get; set; }

        [Required]
        public JsonDocument answers { get; set; }        

        [Required]
        public DateTime created_at { get; set; }

        public Activity Activity { get; set; }
    }
}
