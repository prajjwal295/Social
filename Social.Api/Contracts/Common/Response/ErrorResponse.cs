namespace Social.Api.Contracts.Common.Response
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string StatusPhase { get; set; }
        public DateTime TimeStamp { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
