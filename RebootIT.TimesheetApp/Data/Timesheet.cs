using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RebootIT.TimesheetApp.Data
{
    public class Timesheet
    {
        public int Id { get; set; }
        public int MinutesWorked { get; set; }
        public Staff Staff { get; set; }
        [Required]
        public int StaffId { get; set; }
        public Client Client { get; set; }
        [Required]
        public int ClientId { get; set; }
        public Location Location { get; set; }
        [Required]
        public int LocationId { get; set; }
    }
}
