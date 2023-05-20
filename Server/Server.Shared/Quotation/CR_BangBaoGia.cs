using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.Quotation
{
    public class CR_BangBaoGia
    {
        public string stt { get; set; }
        public string name { get; set; }
        public string model { get; set; }
        public string xuatxu { get; set; }
        public string thongsokythuat { get; set; }
        public string qty { get; set; }
        public string price { get; set; }
        public string amt { get; set; }
        public string time { get; set; }
        public string GhiChuDetail { get; set; }
        public List<string> lstFileNameTemp { get; set; }
    }
}
