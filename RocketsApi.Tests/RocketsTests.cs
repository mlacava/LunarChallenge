using Common.Commands;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using System.Text.Json.Nodes;

namespace RocketsApi.Tests
{
    [TestCaseOrderer("RocketsApi.Tests.PriorityOrderer", "RocketsApi.Tests")]
    public class RocketsTests : IDisposable
    {
        private readonly HttpClient _httpClient = new() { BaseAddress = new Uri("http://localhost:8088") };
        private static string newGuid = Guid.NewGuid().ToString();

        public void Dispose()
        {
            _httpClient.DeleteAsync("/state").GetAwaiter().GetResult();
        }


        [Fact, Priority(1)]
        public async Task Message_WhenCalling_With_RocketLaunched_Type_ThenReturnsExpectedResponse()
        {
            // Arrange
            var expectedStatusCode = System.Net.HttpStatusCode.OK;
            var expectedContent =
                new MessageCommand
                {
                    Metadata = new Common.MessageMetadata
                    {
                        Channel = newGuid,
                        MessageNumber = 1,
                        MessageTime = DateTime.UtcNow,
                        MessageType = "RocketLaunched"
                    },
                    Message = JsonObject.Parse(JsonConvert.SerializeObject(new
                    {
                        Type = "Falcon 9",
                        LaunchSpeed = 27000,
                        Mission = "Starlink"
                    }))!.AsObject()
                };

            var stopwatch = Stopwatch.StartNew();

            // Act
            var response = await _httpClient.PostAsync("/messages", TestHelpers.GetJsonStringContent(expectedContent));

            // Assert
            await TestHelpers.AssertResponseWithoutContentAsync<MessageCommand>(stopwatch, response, expectedStatusCode);
        }


        [Fact, Priority(3)]
        public async Task GetAllRockets_WhenCalling_ThenReturnsExpectedResponse()
        {
            // Arrange
            var expectedStatusCode = System.Net.HttpStatusCode.OK;

            // Act
            var response = await _httpClient.GetAsync("/rockets_list");
            var stopwatch = Stopwatch.StartNew();
            var expectedContent = new List<Data.Models.Rocket>
            {
                new Data.Models.Rocket
                {
                    Channel = Guid.Parse(newGuid),
                    Type = "Falcon 9",
                    Speed = 27000,
                    Mission = "Starlink",
                    Status = "Launched"
                }
            };

            // Assert
            await TestHelpers.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }


        [Fact, Priority(2)]
        public async Task GetRocket_WhenCalling_ThenReturnsExpectedResponse()
        {
            // Arrange
            var expectedStatusCode = System.Net.HttpStatusCode.OK;

            // Act
            var response = await _httpClient.GetAsync(String.Format("/rocket?channel={0}", Guid.Parse(newGuid).ToString()));
            var stopwatch = Stopwatch.StartNew();
            var expectedContent =
                new Data.Models.Rocket
                {
                    Channel = Guid.Parse(newGuid),
                    Type = "Falcon 9",
                    Speed = 27000,
                    Mission = "Starlink",
                    Status = "Launched"
                };

            // Assert
            await TestHelpers.AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
        }

        [Fact, Priority(4)]
        public async Task Message_WhenCalling_With_RocketSpeedIncreased_Type_ThenReturnsExpectedResponse()
        {
            // Arrange
            var expectedStatusCode = System.Net.HttpStatusCode.OK;
            var expectedContent =
                new MessageCommand
                {
                    Metadata = new Common.MessageMetadata
                    {
                        Channel = newGuid,
                        MessageNumber = 1,
                        MessageTime = DateTime.UtcNow,
                        MessageType = "RocketSpeedIncreased"
                    },
                    Message = JsonObject.Parse(JsonConvert.SerializeObject(new
                    {
                        By = 2000
                    }))!.AsObject()
                };

            var stopwatch = Stopwatch.StartNew();

            // Act
            var response = await _httpClient.PostAsync("/messages", TestHelpers.GetJsonStringContent(expectedContent));

            // Assert
            await TestHelpers.AssertResponseWithoutContentAsync<MessageCommand>(stopwatch, response, expectedStatusCode);
        }

        [Fact, Priority(5)]
        public async Task Message_WhenCalling_With_RocketSpeedDecreased_Type_ThenReturnsExpectedResponse()
        {
            // Arrange
            var expectedStatusCode = System.Net.HttpStatusCode.OK;
            var expectedContent =
                new MessageCommand
                {
                    Metadata = new Common.MessageMetadata
                    {
                        Channel = newGuid,
                        MessageNumber = 1,
                        MessageTime = DateTime.UtcNow,
                        MessageType = "RocketSpeedDecreased"
                    },
                    Message = JsonObject.Parse(JsonConvert.SerializeObject(new
                    {
                        By = 2000
                    }))!.AsObject()
                };

            var stopwatch = Stopwatch.StartNew();

            // Act
            var response = await _httpClient.PostAsync("/messages", TestHelpers.GetJsonStringContent(expectedContent));

            // Assert
            await TestHelpers.AssertResponseWithoutContentAsync<MessageCommand>(stopwatch, response, expectedStatusCode);
        }

        [Fact, Priority(6)]
        public async Task Message_WhenCalling_With_RocketExploded_Type_ThenReturnsExpectedResponse()
        {
            // Arrange
            var expectedStatusCode = System.Net.HttpStatusCode.OK;
            var expectedContent =
                new MessageCommand
                {
                    Metadata = new Common.MessageMetadata
                    {
                        Channel = newGuid,
                        MessageNumber = 1,
                        MessageTime = DateTime.UtcNow,
                        MessageType = "RocketExploded"
                    },
                    Message = JsonObject.Parse(JsonConvert.SerializeObject(new
                    {
                        Reason = "PRESSURE_VESSEL_FAILURE"
                    }))!.AsObject()
                };

            var stopwatch = Stopwatch.StartNew();

            // Act
            var response = await _httpClient.PostAsync("/messages", TestHelpers.GetJsonStringContent(expectedContent));

            // Assert
            await TestHelpers.AssertResponseWithoutContentAsync<MessageCommand>(stopwatch, response, expectedStatusCode);
        }

        [Fact, Priority(6)]
        public async Task Message_WhenCalling_With_RocketMissionChanged_Type_ThenReturnsExpectedResponse()
        {
            // Arrange
            var expectedStatusCode = System.Net.HttpStatusCode.OK;
            var expectedContent =
                new MessageCommand
                {
                    Metadata = new Common.MessageMetadata
                    {
                        Channel = newGuid,
                        MessageNumber = 1,
                        MessageTime = DateTime.UtcNow,
                        MessageType = "RocketMissionChanged"
                    },
                    Message = JsonObject.Parse(JsonConvert.SerializeObject(new
                    {
                        NewMission = "SHUTTLE_MIR"
                    }))!.AsObject()
                };

            var stopwatch = Stopwatch.StartNew();

            // Act
            var response = await _httpClient.PostAsync("/messages", TestHelpers.GetJsonStringContent(expectedContent));

            // Assert
            await TestHelpers.AssertResponseWithoutContentAsync<MessageCommand>(stopwatch, response, expectedStatusCode);
        }
    }
}