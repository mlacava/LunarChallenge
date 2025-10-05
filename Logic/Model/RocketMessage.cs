using Data.Models;

namespace Logic.Model
{
    internal class RocketMessage
    {
        public bool IsNew { get; set; }
        public Rocket? Rocket { get; set; }
    }
}
