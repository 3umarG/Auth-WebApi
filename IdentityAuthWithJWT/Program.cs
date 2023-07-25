using IdentityAuthWithJWT;
using IdentityAuthWithJWT.Config;
using IdentityAuthWithJWT.Data;
using IdentityAuthWithJWT.Extensions;
using IdentityAuthWithJWT.Interfaces;
using IdentityAuthWithJWT.Models;
using IdentityAuthWithJWT.Models.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger 
builder.Services.AddSwaggerGen(options =>
{
	// Change the main schema for the Swagger
	options.SwaggerDoc("v1", new OpenApiInfo
	{
		Version = "v1",
		Title = "AuthWebApi",
		Description = "Demo Web Api for using Identity Framework with JWT using Access Tokens and Refresh Tokens .",
		TermsOfService = new Uri("https://www.google.com"),
		Contact = new OpenApiContact
		{
			Name = "OmarGomaa",
			Email = "omargomaa.dev@gmail.com",
			Url = new Uri("https://www.google.com")
		},
		License = new OpenApiLicense
		{
			Name = "My license",
			Url = new Uri("https://www.google.com")
		}
	});

	// Add Global Authorization for all controllers with their end point
	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\""
	});

	// Add Specific Authorization option for every end point
	options.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				},
				Name = "Bearer",
				In = ParameterLocation.Header
			},
			new List<string>()
		}
	});
});



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
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
						ClockSkew = TimeSpan.Zero
					};
				})
				.AddGoogle(googleOptions =>
				{
					googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
					googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
				});

// Add Policy for being Admin and Manager
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("RequireAdminAndManagerRoles", policy =>
	{
		policy.Requirements.Add(new AdminAndManagerRequirement());
	});
});

// Register the custom requirement handler
builder.Services.AddAdminAndManagerRequirement();


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
		var unAuthorizedResponse = new UnAuthorizedFailureResponse();
		await context.Response.WriteAsync(unAuthorizedResponse.ToString());
	}
	else if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden) // 403
	{
		context.Response.ContentType = "application/json";
		var forbiddenResponse = new ForbiddenFailureResponse();
		await context.Response.WriteAsync(forbiddenResponse.ToString());
	}
});


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
