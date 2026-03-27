using DomainLayer.Contract;
using DomainLayer.Models;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PresistenceLayer.Data;
using PresistenceLayer.Repos;
using ServiceAbstraction;
using ServiceLayer.Services;
using System;
using System.Text;

namespace OnlineClinic
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            Env.Load();
            // Add services to the container.

            //DbContext
            builder.Services.AddDbContext<EventDbContext>(
                options => {
                    options.UseSqlServer(Environment.GetEnvironmentVariable("EventDbContext"));
                }
            );

            builder.Services.AddHttpClient();

            builder.Services.AddAuthentication(Config =>
            {
                Config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                Config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(Config =>
            {
                Config.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JwtOptions:Issuer"],
                    ValidateAudience = true,
                    //ClockSkew = TimeSpan.FromHours(24),
                    ValidAudience = builder.Configuration["JwtOptions:Audience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWTSecretKey")!)),
                };
            });

            //  builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            //  {
            //      options.SignIn.RequireConfirmedAccount = true;
            //      options.Password.RequireDigit = true;
            //      options.Password.RequiredLength = 8;
            //      options.Password.RequireNonAlphanumeric = false;
            //      options.Password.RequireUppercase = true;
            //      options.Password.RequireLowercase = true;

            //  }).AddEntityFrameworkStores<EventDbContext>()
            //.AddDefaultTokenProviders();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>()
                .AddEntityFrameworkStores<EventDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddScoped<IAuthService,AuthService>();
            builder.Services.AddScoped<IEventService,EventService>();
            builder.Services.AddScoped<IPayMobService, PayMobService>();
            builder.Services.AddScoped(typeof(IGenaricRepository<,>), typeof(GenaricRepository<,>));

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
