
using _01_EFCore.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _01_EFCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //.NET Web Api Validation yapýsýný devre dýþý etmek için kullanýlýr.
            //builder.Services.Configure<ApiBehaviorOptions>(opt =>
            //    opt.SuppressModelStateInvalidFilter = true
            //);
            // Add services to the container.
            builder.Services.AddDbContext<TodoDbContext>(options => 
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"))
            );

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

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