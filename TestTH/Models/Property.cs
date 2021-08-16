using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestTH.Models
{
    public class Property
    {

        [Key]
        public int propertyid { get; set; }

        [Required]
        [MaxLength(255, ErrorMessage = "Max lenght for title are 255 characters")]
        public string title { get; set; }

        [Required]
        public string address { get; set; }

        [Required]
        public string description { get; set; }

        [Required]
        public DateTime created_at { get; set; }

        [Required]
        public DateTime updated_at { get; set; }

        public DateTime? disabled_at { get; set; }

        [Required]
        [MaxLength(35, ErrorMessage = "Max lenght for status are 35 characters")]
        public string status { get; set; }

        public ICollection<Activity> Activity { get; set; }
    }
}
