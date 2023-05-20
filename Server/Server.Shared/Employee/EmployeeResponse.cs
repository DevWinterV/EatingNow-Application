using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.Employee
{
    public class EmployeeResponse
    {
        public int Id { get; set; }
        public string UserLogin { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }
        public string LocationName { get; set; }
		public string LocationCode { get; set; }
	 	public string ImageRecordId { get; set; }
        public List<string> Avarta { get; set; }
        public bool check { get; set; }
    }
}
