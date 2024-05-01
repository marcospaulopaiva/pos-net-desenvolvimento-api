
using PlaceRentalApp.API.Middlewares;
using PlaceRentalApp.API.Models;

namespace PlaceRentalApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Opção 1
            //var min = builder.Configuration.GetValue<int>("PlacesConfig:MinDescription");
            //var max = builder.Configuration.GetValue<int>("PlacesConfig:MaxDescription");

            //var config = new PlacesConfigurarion
            //{
            //    MinDescription = min,
            //    MaxDescription = max
            //};

            // Opção 2
            var config = new PlacesConfigurarion();
            builder.Configuration.GetSection("PlacesConfig").Bind(config);

            builder.Services.Configure<PlacesConfigurarion>(
                builder.Configuration.GetSection("PlacesConfig"));

            builder.Services.AddSingleton(config);

            builder.Services.AddExceptionHandler<ApiExceptionHandler>();
            builder.Services.AddProblemDetails();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseExceptionHandler();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
