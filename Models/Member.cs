using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PortCityPacers.Models
{
    public class Member
    {
        public int Id { get; set; }

        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }
        public string? Type { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZIP { get; set; }
        [Display(Name = "Join Date")]
        [DataType(DataType.Date)]
        public DateTime Joindate { get; set; }
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }
    }
}
