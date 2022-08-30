namespace ds.pms.bl.optionheaders.Models
{
    public class OptionHeaderCommonResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public OptionHeader OptionHeader { get; set; }
    }
}
