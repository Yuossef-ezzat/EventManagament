using DomainLayer.Contract;
using DomainLayer.Models;
using DomainLayer.Models.EventModule;
using DomainLayer.Models.Registeration;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PresistenceLayer.Data;
using PresistenceLayer.Data.Configurations;
using PresistenceLayer.Repos;
using ServiceAbstraction;
using ServiceLayer.Services;
using System;
using System.Text;

namespace OnlineClinic
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            Env.Load();
            // Add services to the container.

            //DbContext
            builder.Services.AddDbContextPool<EventDbContext>(
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
            

            builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
            })

            .AddEntityFrameworkStores<EventDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddScoped<IAuthService,AuthService>();
            builder.Services.AddScoped<IEventService,EventService>();
            builder.Services.AddScoped<IEmailService,EmailService>();
            builder.Services.AddScoped<IPayMobService, PayMobService>();
            builder.Services.AddScoped<IRegisrationService, RegistrationService>();
            builder.Services.AddScoped<INotifService, NotifService>();
            builder.Services.AddScoped(typeof(IGenaricRepository<,>), typeof(GenaricRepository<,>));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.RegisterMapsterConfiguration();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.MapOpenApi();
            //}
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<EventDbContext>();

                if (!context.Events.Any())
                {
                    context.Events.Add(new Event
                    {
                        Title = "Test Event",
                        Description = "Test Description",
                        Date = DateTime.UtcNow.AddDays(30),
                        Location = "Cairo",
                        MaxAttendance = 100,
                        PaymentRequired = false,
                        //RegisterationStatus = RegistrationStatus.active
                    });

                    await context.SaveChangesAsync();
                }
            }
            //app.UseHttpsRedirection();

            //app.UseAuthorization();

            app.UseStaticFiles();
            app.UseAuthorization();
            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
