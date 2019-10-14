using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RebootIT.TimesheetApp.ViewModels.TimeSheets
{
    public class Index
    {
        public Index(List<Data.Timesheet> timesheets, int clientId, int locationId, int staffId)
        {
            Timesheets = timesheets;
            ClientId = clientId;
            LocationId = locationId;
            StaffId = staffId;
        }

        public List<Data.Timesheet> Timesheets { get; set; }

        public int ClientId { get; set; }

        public int LocationId { get; set; }

        public int StaffId { get; set; }
    }
}
