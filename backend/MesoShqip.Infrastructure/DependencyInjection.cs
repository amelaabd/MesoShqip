using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MesoShqip.Application.Interfaces;
using MesoShqip.Domain.Interfaces;
using MesoShqip.Infrastructure.Data;
using MesoShqip.Infrastructure.Repositories;
using MesoShqip.Infrastructure.Services;
using MesoShqip.Infrastructure.Services.AI;

namespace MesoShqip.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };
            });
        services.AddScoped<IStoryGeneratorService, StoryGeneratorService>();

        services.AddHttpClient<IQuizGeneratorService, QuizGeneratorService>(client =>
        {
            client.DefaultRequestHeaders.Add("x-api-key", configuration["Anthropic:ApiKey"]);
            client.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
            client.Timeout = TimeSpan.FromSeconds(60);
        });

        return services;
    }
}