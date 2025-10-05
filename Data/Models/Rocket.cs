using MongoDB.Bson.Serialization.Attributes;

namespace Data.Models
{
    public class Rocket
    {
        [BsonId]
        public Guid? Channel { get; set; }
        public string? Type { get; set; }
        public int Speed { get; set; }
        public string? Mission { get; set; }
        public string? Status { get; set; } //Launched / Exploded / Idle
        public string? ExplosionReason { get; set; }
        public int LastMessageNumber { get; set; }
        public DateTimeOffset LastUpdate { get; set; }

    }
}
