namespace BoschCodeChallenge
{
    public class PartProductionMessage
    {
        public Part PartInfo { get; set; }
        public long Timestamp { get; set; }
        public bool Successful { get; set; }
        public string ErrorMessage { get; set; }
    }
}
