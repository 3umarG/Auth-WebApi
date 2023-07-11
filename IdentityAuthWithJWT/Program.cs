using IdentityAuthWithJWT;
using IdentityAuthWithJWT.Data;
using IdentityAuthWithJWT.Extensions;
using IdentityAuthWithJWT.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
//builder.Services.ConfigureIdentity();

builder.Services.AddIdentity<ApiUser, IdentityRole>(options =>
		{
			options.User.RequireUniqueEmail = false;
		})
	   .AddEntityFrameworkStores<ApplicationDbContext>()
	   .AddDefaultTokenProviders();


// Mapping JWT values from appsettings.json to object
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

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
