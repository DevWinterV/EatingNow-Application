using System.Collections.Generic;

namespace DaiPhucVinh.Shared.Core
{
    public class ListDto<T>
    {
        public IList<T> Items { get; set; }
    }
}
