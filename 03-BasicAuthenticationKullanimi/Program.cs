
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;

namespace _03_BasicAuthenticationKullanimi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            /////
            builder.Services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
            ///// 

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            /////// altý
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.AddSecurityDefinition("basic", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "Burada basic authentication bilgilerini giriniz.",
                    Name = "Authorization",
                    Scheme = "basic",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http
                });

                opt.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "basic" }
                    },
                        new List<string>()
                    }
            });

            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            /////
            app.UseAuthentication(); //Kimlik doðrulamasý.
            /////
            app.UseAuthorization(); //Kullanýcý giriþ saðladýktan sonra varolan olan kullanýcýnýn rollerini kontrol eder.



            app.MapControllers();

            app.Run();
        }
    }
}