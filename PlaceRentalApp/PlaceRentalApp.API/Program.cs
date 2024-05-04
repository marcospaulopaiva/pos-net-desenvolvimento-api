
using Microsoft.EntityFrameworkCore;
using PlaceRentalApp.API.Middlewares;
using PlaceRentalApp.API.Persistence;

namespace PlaceRentalApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //builder.Services.AddSingleton<PlaceRentalDbContext>();
            var connectionString = builder.Configuration
                .GetConnectionString("PlaceRentalCs");

            //InMemoryDatabase
            //builder.Services.AddDbContext<PlaceRentalDbContext>(
            //    o => o.UseInMemoryDatabase("PlaceRentalDb"));

            builder.Services.AddDbContext<PlaceRentalDbContext>(
                o => o.UseSqlServer(connectionString));

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
