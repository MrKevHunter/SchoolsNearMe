using System.ComponentModel.DataAnnotations;

namespace SchoolsNearMe.Models
{
    public class ContactUs
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string From { get; set; }
        [Required]
        public string Body { get; set; } 
    }
}