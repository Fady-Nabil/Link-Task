using API.Extensions;
using API.Helpers;
using API.Settings;
using Core.Entities.Identity;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

/* 
 * Connect Web API With Sqlserver DataBase
 */
builder.Services.AddDbContext<StoreContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    , b => b.MigrationsAssembly(typeof(StoreContext).Assembly.FullName))
    );

/*Add JWT Configuration */
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

/* Add Asp Core Identity To Project */
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<StoreContext>();

// Add Auththentication with type Bearer Default Type
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.SaveToken = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            ClockSkew = TimeSpan.Zero
        };
    });


/* Add Unit Of Work To Project */
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

/*  Add AutoMapper To Project */
builder.Services.AddAutoMapper(typeof(Program));


/* Add AutoMapper Library To Use */
builder.Services.AddAutoMapper((typeof(MappingProfiles)));

/* Add Token Service To Project */
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddCors();

/* Add Behviour Of Apis */
builder.Services.AddApplicationServices();


/* To Solve json Error 'Fixing the error "A possible object cycle was detected" in different versions of ASP.NET Core' */
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "APIApplication", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllers();

app.Run();
