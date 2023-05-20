using DaiPhucVinh.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.Feedback
{
    public class FeedbackRequest : BaseRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public bool Approved { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedDescription { get; set; }
    }
}
