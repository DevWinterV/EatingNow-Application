using System.Collections.Generic;

namespace DaiPhucVinh.Shared.Common
{
    public class BaseResponse<T>
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; }
        public object CustomData { get; set; }
        public T Item { get; set; }
        public IList<T> Data { get; set; }
        public int DataCount { get; set; }
    }
}
