using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RebootIT.TimesheetApp.Data
{
    public class Staff
    {
        public int Id { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Forename { get; set; }
        [Required]
        public string Email { get; set; }

        public string Fullname { get { return Forename + " " + Surname; } }
    }
}
