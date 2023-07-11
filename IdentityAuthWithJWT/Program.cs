using IdentityAuthWithJWT;
using IdentityAuthWithJWT.Data;
using IdentityAuthWithJWT.Extensions;
using IdentityAuthWithJWT.Interfaces;
using IdentityAuthWithJWT.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// add AutoMapper Service
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<ApplicationDbContext>(
	options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
	//b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)     // when the Context Class in another project
	));

/// For Idetity Setup 
builder.Services.AddAuthentication();
builder.Services.AddIdentity<ApiUser, IdentityRole>(options =>
		{
			options.User.RequireUniqueEmail = false;
		})
	   .AddEntityFrameworkStores<ApplicationDbContext>()
	   .AddDefaultTokenProviders();

// Add our AuthService 
builder.Services.AddScoped<IAuthService, AuthService>();

// Mapping JWT values from appsettings.json to object
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

// Configure our Authentication Shared Schema 
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
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
					};
				});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Unauthorized (401) MiddleWare
app.Use(async (context, next) =>
{
	await next();

	if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized) // 401
	{
		context.Response.ContentType = "application/json";
		var failureResponse = new FailureResponse(401, "You are UnAuthenticated , please provide Token !!");
		await context.Response.WriteAsync(failureResponse.ToString());
	}
	else if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden) // 403
	{
		context.Response.ContentType = "application/json";
		var failureResponse = new FailureResponse(403, "You are Denied from accessing this End point because it needs certain Role !!");
		await context.Response.WriteAsync(failureResponse.ToString());
	}
});


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
