namespace Common
{
    public class MessageMetadata
    {
        public string? Channel { get; set; }
        public int MessageNumber { get; set; }
        public DateTimeOffset MessageTime { get; set; }
        public required string MessageType { get; set; }

        public MessageMetadata()
        {
            MessageTime = DateTime.UtcNow;
        }

    }
}