using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Entities.DTOs;
using FluentAssertions;
using Xunit;

namespace LMS_Api.IntegrationTests
{
    public class OptionControllerTests:IntegrationTest
    {
        [Theory]
        [InlineData("option/getoptions")]
        public async Task GetAll_ReturnEmptyResponse(string route)
        {
            await AuthenticationAsync();

            var response = await TestClient.GetAsync($"https://localhost:5001/api/{route}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            (await response.Content.ReadFromJsonAsync<List<OptionDTO>>()).Should().NotBeEmpty();
        }
    }
}
