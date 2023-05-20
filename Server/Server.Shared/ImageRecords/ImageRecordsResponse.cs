using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.ImageRecords
{
    public class ImageRecordsResponse
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string RelativePath { get; set; }
        public string AbsolutePath { get; set; }
        public bool IsExternal { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsUsed { get; set; }
        public bool Deleted { get; set; }
    }
}
