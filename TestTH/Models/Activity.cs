using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestTH.Models;

namespace TestTH
{
    public class Activity
    {

        [Key]
        public int activityid { get; set; }

        [Required]
        public int propertyid { get; set; }

        [Required]
        public DateTime schedule { get; set; }

        [Required]
        [MaxLength(255, ErrorMessage = "Max lenght for title are 255 characters")]
        public string title { get; set; }

        [Required]
        public DateTime created_at { get; set; }

        [Required]
        public DateTime updated_at { get; set; }

        [MaxLength(35, ErrorMessage = "Max lenght for status are 35 characters")]
        public string status { get; set; }

        public Property Property { get; set; }
        
        public ICollection<Survey> Survey { get; set; }
    }
}
