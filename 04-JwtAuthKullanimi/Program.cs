using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

namespace _04_JwtAuthKullanimi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //string secretkey = "{6325763C94124E88839FEB30D58DF6CA}";
            string secretkey = TokenService.SECRETKEY;

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opts =>
                {
                    opts.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true, //Tokený oluþturaný da validate et.
                        ValidateAudience = true, //Tokený üreten kiþiyi de validate et.
                        ValidateLifetime = true, //Sürekli hayat boyu validate et.
                        ValidateIssuerSigningKey = true, //Verify Signature alanýnda kullan demiþ oluruz.
                        ValidIssuer = TokenService.ISSUER,
                        ValidAudience = TokenService.AUDIENCE,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey))
                    };
                });


            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opts=>
            {
                opts.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI Örnek", Version = "v1" });

                opts.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Description="Burada bearer authentication bilgilerini giriniz",
                    Name="Authorization",
                    In=ParameterLocation.Header,
                    Scheme=JwtBearerDefaults.AuthenticationScheme,
                    Type=SecuritySchemeType.Http
                });
            }
            );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            ///////
            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}