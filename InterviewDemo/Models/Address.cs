using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InterviewDemo.Models
{
    public class Address
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Street Address")]
        public string Street { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string ZIP { get; set; }
    }
}