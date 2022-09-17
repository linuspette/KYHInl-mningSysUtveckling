using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApi.Helpers;
using WebApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddAzureKeyVault(new Uri(builder.Configuration["KeyVaultUri"]), new DefaultAzureCredential());
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(builder.Configuration["SysDevDbCon"]));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserManager, UserRepository>();
builder.Services.AddScoped<ITokenHandler, JwtTokenRepository>();
builder.Services.AddScoped<ITokenReturnStatements, TokenReturnStatements>();
builder.Services.AddScoped<IAzureFunctionsClient, AzureFunctionsClient>();

builder.Services.AddScoped<OnStartUp>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(x =>
{
    x.IdleTimeout = TimeSpan.FromHours(20);
    x.Cookie.IsEssential = true;
    x.Cookie.HttpOnly = true;
});
builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    context.Fail("Unauthorized");

                return Task.CompletedTask;
            },

            OnTokenValidated = context =>
            {
                if (string.IsNullOrEmpty(context.Principal?.FindFirst("id")?.Value))
                    context.Fail("Unauthorized");

                return Task.CompletedTask;
            }
        };

        x.RequireHttpsMetadata = true;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateLifetime = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SysDevSecret"]))
        };
    });


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
#pragma warning disable CS8602
    await scope.ServiceProvider.GetService<OnStartUp>().InitializeAsync();
#pragma warning restore CS8602
}

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
