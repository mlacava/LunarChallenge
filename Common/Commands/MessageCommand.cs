using System.Text.Json.Nodes;

namespace Common.Commands
{   
    public class MessageCommand
    {
        public required MessageMetadata Metadata { get; set; }
        public required JsonObject Message { get; set; }
    }
}
