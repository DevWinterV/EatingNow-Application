namespace DaiPhucVinh.Shared.Common.Languages
{
    public class LanguageRequest : BaseRequest
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
    }
}
