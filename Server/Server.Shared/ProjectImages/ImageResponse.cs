using DaiPhucVinh.Shared.Common.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Shared.ProjectImages
{
    public class ImageResponse
    {
        public bool IsOk { get; set; }
        public string Message { get; set; }
        public ImageDto Image { get; set; }
    }
}
