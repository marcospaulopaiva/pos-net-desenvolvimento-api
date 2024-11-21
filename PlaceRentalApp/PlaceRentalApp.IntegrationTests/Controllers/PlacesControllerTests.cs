using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PlaceRentalApp.Application.Models;
using PlaceRentalApp.Infrastructure.Persistence;
using System.Text;

namespace PlaceRentalApp.IntegrationTests.Controllers
{
    public class PlacesControllerTests : IClassFixture<TestWebFactory<Program>>
    {
        private readonly TestWebFactory<Program> _factory;
        private readonly HttpClient _httpClient;

        public PlacesControllerTests(TestWebFactory<Program> factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateClient();

            _httpClient = _factory.WithWebHostBuilder(builder
                =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication(defaultScheme: "TestScheme")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    "TestScheme", options => { });
                });

            }).CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            _httpClient.DefaultRequestHeaders.Authorization
                = new System.Net.Http.Headers.AuthenticationHeaderValue("TestScheme");
        }

        [Fact]
        public async Task AddPlace_IsCalledWithValidData_Success()
        {
            // Arrange
            var inputModel = new CreatePlaceInputModel()
            {
                Title = "Charming Beach House",
                Description = "A beautiful and relaxing beach house located",
                DailyPrice = 150.00m,
                Address = new AddressInputModel
                {
                    Street = "123 Ocean View",
                    Number = "100",
                    District = "Seafront",
                    ZipCode = "60721-340",
                    City = "Seaside",
                    State = "CA",
                    Country = "USA"
                },
                AllowedNumberPerson = 4,
                AllowPets = true,
                CreatedBy = _factory.UserId
            }; 

            var json = JsonConvert.SerializeObject(inputModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _httpClient.PostAsync("api/places", content);
            // Assert
            response.EnsureSuccessStatusCode();
            var location = response.Headers.GetValues("location").First();
            var id = int.Parse(location.Split("/").Last());

            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<PlaceRentalDbContext>();

                var exists = context.Places.Any(p => p.Id == id);

                Assert.True(exists);
            }
        }
    }
}
