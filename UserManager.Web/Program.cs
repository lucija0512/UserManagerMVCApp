using Serilog;
using UserManager.Application.Configuration;
using UserManager.Application.Interfaces;
using UserManager.Application.Services;
using UserManager.Domain.Interfaces;
using UserManager.Domain.Repositories;
using UserManager.Infrastructure.ExternalServices.Api;
using UserManager.Infrastructure.ExternalServices.Email;
using UserManager.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection(EmailSettings.EmailConfig));
builder.Services
    .AddHttpClient<IJsonPlaceholderService, JsonPlaceholderService>(x =>
    {
        x.BaseAddress = new Uri(builder.Configuration.GetValue<string>("JsonPlaceholderApi"));
    });
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserManagerService, UserManagerService>();
builder.Services.AddScoped<IEmailService, EmailService>();
// Add serilog
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();

app.UseSerilogRequestLogging();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
