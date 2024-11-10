
using IdettaTestServer.DAL;
using Microsoft.EntityFrameworkCore;

namespace IdettaTestServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // add SQL Server Localdb connectivity
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(c => c.UseSqlServer(connectionString));

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(policyBuilder =>
                policyBuilder.AddDefaultPolicy(policy =>
                policy.WithOrigins("*").AllowAnyHeader().AllowAnyHeader())
);
            
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors();
            //app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}