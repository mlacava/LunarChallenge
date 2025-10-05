using Common.Commands;
using Data.Contracts;
using Data.Models;
using Logic.Interfaces;
using MapsterMapper;
using MongoDB.Driver;
using System.Text.Json.Nodes;

namespace Logic
{
    public class RocketLogic : IRocketsLogic
    {
        private readonly IRocketRepository _repository;
        private readonly IMapper _mapper;

        public RocketLogic(IRocketRepository rocketsRepository, IMapper mapper)
        {
            _repository = rocketsRepository;
            _mapper = mapper;
        }
        public async Task CreateUpdateRocketAsync(MessageCommand messageCommand)
        {
            var rocket = new Rocket();

            var id = messageCommand.Metadata.Channel;
            var messageType = messageCommand.Metadata.MessageType;
            var messageData = messageCommand.Message;
            var messageNumber = messageCommand.Metadata.MessageNumber;

            id ??= new Guid().ToString();
            var exist = GetRocketAsync(id).Result;
            var isNewRocket = exist is null;

            if (!isNewRocket) rocket = exist;

            LoadAllMessageData(rocket, messageType, messageData);

            VerifyMessageNumber(rocket, messageNumber);

            rocket.LastUpdate = messageCommand.Metadata.MessageTime;

            rocket.Channel = Guid.Parse(id);

            if (!isNewRocket)
            {
                rocket.LastUpdate = DateTimeOffset.UtcNow;
                await _repository.UpdateAsync(id, rocket);
                return;
            }

            await _repository.CreateAsync(rocket);
        }

        private void LoadAllMessageData(Rocket? rocket, string messageType, JsonObject messageData)
        {
            switch (messageType)
            {
                case "RocketLaunched":
                    rocket.Type = messageData["type"]?.ToString();
                    rocket.Speed = messageData["launchSpeed"]?.GetValue<int>() ?? 0;
                    rocket.Mission = messageData["mission"]?.ToString();
                    rocket.Status = "Launched";
                    break;

                case "RocketSpeedIncreased":
                    if (rocket.Status != "Exploded")
                        rocket.Speed += messageData["by"]?.GetValue<int>() ?? 0;
                    break;

                case "RocketSpeedDecreased":
                    if (rocket.Status != "Exploded")
                    {
                        rocket.Speed -= messageData["by"]?.GetValue<int>() ?? 0;
                        if (rocket.Speed < 0) rocket.Speed = 0;
                    }
                    break;

                case "RocketMissionChanged":
                    if (rocket.Status != "Exploded")
                    {
                        rocket.Mission = messageData["newMission"]?.ToString();
                    }
                    break;

                case "RocketExploded":
                    rocket.Status = "Exploded";
                    rocket.ExplosionReason = messageData["reason"]?.ToString();
                    break;
            }
        }

        private void VerifyMessageNumber(Rocket? rocket, int messageNumber)
        {
            if (rocket.LastMessageNumber <= messageNumber)
                rocket.LastMessageNumber = messageNumber;
            else
                rocket.LastMessageNumber++;
        }

        public async Task<List<Rocket>> GetAllRocketsAsync(string sortBy)
        {
            var results = await _repository.GetAsync();
            var mappedResults = _mapper.Map<List<Rocket>>(results);

            var rockets = sortBy switch
            {
                "speed" => mappedResults.OrderByDescending(r => r.Speed).ToList(),
                "mission" => mappedResults.OrderBy(r => r.Mission).ToList(),
                _ => mappedResults.OrderBy(r => r.Type).ToList()
            };

            return rockets;
        }

        public async Task<Rocket?> GetRocketAsync(string id)
        {
            var result = await _repository.GetAsync(id);

            if (result is null) return null;

            var mappedResult = _mapper.Map<Rocket>(result);
            return mappedResult;
        }
    }
}
