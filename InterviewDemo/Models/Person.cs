using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InterviewDemo.Models
{
    public class Person
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Birth Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode=true, DataFormatString ="{0:MM/dd/yyyy}")]
        public DateTime BirthDate { get; set; }
        public Address Address { get; set; }
    }
}