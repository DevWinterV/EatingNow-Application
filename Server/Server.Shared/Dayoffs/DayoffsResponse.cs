using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.Dayoffs
{
    public class DayoffsResponse 
    {
        public int Id { get; set; }
        public int DayRequests { get; set; }
        public DateTime StartDate { get; set; }
        public string Description { get; set; }
        public int EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public int DayApproved { get; set; }
        public bool Approved { get; set; }
        public string ApprovedDescription { get; set; }
        public string LocationName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Deleted { get; set; }
    }
}
