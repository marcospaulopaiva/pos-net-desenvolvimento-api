using NBomber.CSharp;
using NBomber.Http;
using NBomber.Http.CSharp;
using NBomber.Plugins.Network.Ping;
using PlaceRentalApp.Application.Models;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

new HttpJson().Run();

public class HttpJson
{
    public void Run()
    {
        using var httpClient = new HttpClient();

        var jwt = "";

        var scenario = Scenario.Create("http_scenario", async context =>
        {
            var model = new CreatePlaceInputModel()
            {
                Title = "Title",
                Description = "A beautiful and relaxing beach house located",
                DailyPrice = 150.00m,
                Address = new AddressInputModel
                {
                    Street = "Street",
                    Number = "Number",
                    District = "12345678",
                    City = "City",
                    State = "State",
                    Country = "Country"
                },
                AllowedNumberPerson = 4,
                AllowPets = true,
                CreatedBy = 1
            };

            var json = JsonSerializer.Serialize(model);

            var request1 =
                Http.CreateRequest("POST", "https://localhost:7120/api/places")
                .WithHeader("Authorization", $"Bearer {jwt}")
                .WithJsonBody(json);

            request1.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response1 = await Http.Send(httpClient, request1);

            return response1;

        }).WithInit(async context =>
        {
            using var client = new HttpClient();

            var result = await client.PutAsJsonAsync("https://localhost:7120/api/login",
                new LoginInputModel { Email = "marcos.paulo@email.com", Password = "1234" });

            var viewModel = await result.Content.ReadFromJsonAsync<ResultViewModel<LoginViewModel>>();

            jwt = viewModel.Data.Token;
        })
        .WithoutWarmUp()
        .WithLoadSimulations(Simulation.Inject(rate: 5, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromSeconds(30)));

        NBomberRunner
                .RegisterScenarios(scenario)
                .WithWorkerPlugins(
                new PingPlugin(PingPluginConfig.CreateDefault("nbomber.com")),
                new HttpMetricsPlugin(new[] {HttpVersion.Version1})).Run();
    }
}