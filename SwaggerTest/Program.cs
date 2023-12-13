using Microsoft.EntityFrameworkCore;
using SwaggerTest.Repository;
using System.Text.Json.Serialization;

namespace SwaggerTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen();
            builder.Services.AddMvc()
                            .AddJsonOptions(o =>
                            {
                                o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                            });
            string connectionString = builder.Configuration.GetConnectionString("Default");

            
            builder.Services.AddDbContext<EJEMPLOCDASQLContext>(config =>
            {
                config.UseSqlServer(connectionString);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();  
            app.UseHttpsRedirection();
            app.MapControllers();

            app.Run();
        }
    }
}
