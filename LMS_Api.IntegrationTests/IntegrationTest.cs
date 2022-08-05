using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DataAccess.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

namespace LMS_Api.IntegrationTests
{
    public class IntegrationTest
    {
        protected readonly HttpClient TestClient;

        public IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(AppDbContext));
                        services.AddDbContext<AppDbContext>(options =>
                        { 
                            options.UseInMemoryDatabase("TestDb");
                        });
                    });
                });

            TestClient = appFactory.CreateClient();
        }

        protected async Task AuthenticationAsync()
        {
            TestClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", await GetJwtAsync());
        }

        private async Task<string> GetJwtAsync()
        {
            var response = await TestClient.PostAsJsonAsync("https://localhost:5001/api/user/login", new RegisterDTO
            {
                Name = "TestName",
                Surname = "TestSurname",
                Username = "testusername",
                Email = "testuser@mail.ru",
                Password = "Password@12345"
            });

            var registrationResponse = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();

            return registrationResponse.Token;
        }
    }
}
