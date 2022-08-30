namespace ds.pms.bl.clients.Models
{
    public class ClientCommonResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Client Client { get; set; }
        public UpdateClient UpdateClient { get; set; }
    }
}
