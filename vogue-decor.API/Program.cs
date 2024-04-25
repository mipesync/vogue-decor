using System.Net;
using vogue_decor.Application;
using vogue_decor.Application.DTOs;
using vogue_decor.Domain;
using vogue_decor.Middlewares.ExceptionMiddleware;
using vogue_decor.Persistence;
using vogue_decor.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("PostgreSQL")!;

var jwtOptions = new JwtOptionsDto
{
    RefreshTokenExpires = TimeSpan.FromDays(30),
    EXPIRES = TimeSpan.FromMinutes(30),
    ISSUER = builder.Configuration["JWT:Issuer"]!,
    AUDIENCE = builder.Configuration["JWT:Audience"]!,
    KEY = builder.Configuration["JWT:SecretKey"]!
};    

builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
}).AddEntityFrameworkStores<DBContext>();

builder.Services.AddAuthenticationService(jwtOptions);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddPersistenceService(connectionString, builder.Configuration, builder.Services.BuildServiceProvider().GetService<ILogger<Program>>()!);

builder.Services.AddApplication(jwtOptions);
builder.Services.AddSwaggerService();

// builder.WebHost.ConfigureKestrel((context, options) =>
// {
//     options.Listen(IPAddress.Any, 7216, listenOptions =>
//     {        
//         listenOptions.UseHttps(builder.Configuration["ASPNETCORE_Kestrel_Certificates_Default_Path"]!, 
//             builder.Configuration["ASPNETCORE_Kestrel_Certificates_Default_Password"]!);
//     });
// });

builder.Services.AddCors(options => options.AddPolicy("AllowAllOrigins", builder =>
{
    builder.AllowAnyHeader();
    builder.AllowAnyMethod();
    builder.WithOrigins("http://localhost:3000", "https://localhost:3000",
        "http://79.174.80.7:3000", "https://79.174.80.7:3000", 
        "http://79.174.80.7", "https://79.174.80.7", 
        "http://toplight.pro", "https://toplight.pro");
    builder.AllowCredentials();
}));

var app = builder.Build();

app.UseCors("AllowAllOrigins");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<DBContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

app.UseExceptionMiddleware();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
