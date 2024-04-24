using AbilloLLCApplication.API.Background;
using AbilloLLCApplication.API.Middlewares;
using AbilloLLCApplication.Business.DTOs;
using AbilloLLCApplication.Business.DTOs.Common;
using AbilloLLCApplication.Business.Exceptions;
using AbilloLLCApplication.Business.HelperServices.Implementations;
using AbilloLLCApplication.Business.HelperServices.Interfaces;
using AbilloLLCApplication.Business.Hubs;
using AbilloLLCApplication.Business.Mappers;
using AbilloLLCApplication.Business.Services.Implementations;
using AbilloLLCApplication.Business.Services.Interfaces;
using AbilloLLCApplication.Core.Entities.Identity;
using AbilloLLCApplication.Database.Contexts;
using AbilloLLCApplication.Database.Repositories.Implementations;
using AbilloLLCApplication.Database.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(UserMapper).Assembly);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,

        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),
        LifetimeValidator = (_, expires, _, _) => expires != null ? expires > DateTime.UtcNow : false,

    };

});

builder.Services.AddCors(options =>
{
    options.AddPolicy("", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:3001", "").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
});

builder.Services.AddDbContext<AppDbContext>(context =>
{
    context.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<AppDbContextInitializer>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IDriverRepository, DriverRepository>();
builder.Services.AddScoped<IDriverService, DriverService>();
builder.Services.AddScoped<ICargoRepository, CargoRepository>();
builder.Services.AddScoped<IOfferRepository, OfferRepository>();
builder.Services.AddScoped<IOfferService, OfferService>();
builder.Services.AddScoped<IMailService,MailService>();
builder.Services.AddScoped<ISmsService, SmsService>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IMessageChatService, MessageChatService>();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IMessageService, MessageService>();
builder.Services.AddScoped<ICargoService, CargoService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

builder.Services.AddHostedService<OffersEraser>();
builder.Services.AddHostedService<MessageEraser>();
builder.Services.AddHostedService<MessageSaverAPI>();
builder.Services.AddHostedService<ChatMessageEraser>();

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
   


}).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();



var app = builder.Build();
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseMiddleware<WebSocketsMiddleware>();
app.UseAuthorization();
app.UseCors("");
app.MapHub<ChatHub>("/Hub");
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.None,
});

app.UseExceptionHandler(error =>
{
    error.Run(async context =>
    {
      var features = context.Features.Get<IExceptionHandlerFeature>();
        var errorMessage = features.Error.Message;
        var statusCode = HttpStatusCode.InternalServerError;
        if(features.Error is IBaseException)
        {
          var exception =  (IBaseException)features.Error;
            errorMessage = exception.ErrorMessage;
            statusCode = exception.statusCode;
        }
        Log.Error($"Global exception handler: An unhandled error occurred while processing the request. {errorMessage}");

        Log.Error("Unhandled exception caught for request: {Path}", context.Request.Path);

        var response = new ResponseDTO(errorMessage,statusCode);
        context.Response.StatusCode = (int)statusCode;
      await context.Response.WriteAsJsonAsync(response);
       await context.Response.CompleteAsync();

    });

});


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider.GetRequiredService<AppDbContextInitializer>();
    await services.InitializerAsync();
    await services.addRolesAsync();
}



app.MapControllers();


try
{
    Log.Information("...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Error.");
}
finally
{
    Log.CloseAndFlush();
}

