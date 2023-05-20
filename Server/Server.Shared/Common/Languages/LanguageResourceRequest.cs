namespace DaiPhucVinh.Shared.Common.Languages
{
    public class LanguageResourceRequest : BaseRequest
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public string ResourceKey { get; set; }
        public string ResourceValue { get; set; }
    }
}
