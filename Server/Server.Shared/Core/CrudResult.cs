namespace DaiPhucVinh.Shared.Core
{

    public class CrudResult
    {
        public bool IsOk { get; set; }
        /// <summary>
        /// Id cua entity sau khi thuc hien, neu co
        /// </summary>
        public int ReturnId { get; set; }
        public object ReturnValue { get; set; }
        public byte[] ReturnByteArray { get; set; }
        /// <summary>
        /// Code cua entity sau khi thuc hien, neu co
        /// </summary>
        public string ReturnCode { get; set; }

        public CommonErrorStatus ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
    }
}
