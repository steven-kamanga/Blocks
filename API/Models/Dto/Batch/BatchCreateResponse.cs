namespace API.Models.Dto.Batch
{
    public class BatchCreateResponse
    {
        public string? Message {get; set;}
        public string? BatchName { get; set; }
        public long? Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}