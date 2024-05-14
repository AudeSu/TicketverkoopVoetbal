using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TicketverkoopVoetbal.Data;
using TicketverkoopVoetbal.Domains.Data;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Repositories;
using TicketverkoopVoetbal.Repositories.Interfaces;
using TicketverkoopVoetbal.Services;
using TicketverkoopVoetbal.Services.Interfaces;
using TicketVerkoopVoetbal.Util.Mail;
using TicketVerkoopVoetbal.Util.Mail.Interfaces;
using TicketVerkoopVoetbal.Util.PDF.Interfaces;
using TicketVerkoopVoetbal.Util.PDF;
using Microsoft.AspNetCore.Mvc.Razor;
using TicketverkoopVoetbal.Areas.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

//asp service provider
builder.Services.AddDbContext<FootballDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Localization
builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.SubFolder) // vertaling op View
    .AddDataAnnotationsLocalization(); // vertaling op ViewModel

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources"); // in welk map de resources staan

var supportedCultures = new[] { "nl-BE", "en-US", "fr-FR" };

builder.Services.Configure<RequestLocalizationOptions>(options => {
    options.SetDefaultCulture(supportedCultures[0])
      .AddSupportedCultures(supportedCultures)  //Culture is used when formatting or parsing culture dependent data like dates, numbers, currencies, etc
      .AddSupportedUICultures(supportedCultures);  //UICulture is used when localizing strings, for example when using resource files.
});

// Email
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddSingleton<IEmailSend, EmailSend>();

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Ticketverkoop Voetbal",
        Version = "version 1",
        Description = "An API to buy football tickets",
        TermsOfService = new Uri("https://ticketverkoop.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "AS",
            Email = "tickets.voetbal.league@gmail.com",
            Url = new Uri("https://vives.be"),
        },
        License = new OpenApiLicense
        {
            Name = "Ticketverkoop Voetbal API LICX",
            Url = new Uri("https://ticketverkoop.com/license"),
        }
    });
});
// Automapper
builder.Services.AddAutoMapper(typeof(Program));

// DI
builder.Services.AddTransient<IMatchService<Match>, MatchService>();
builder.Services.AddTransient<IMatchDAO<Match>, MatchDAO>();

builder.Services.AddTransient<IService<Club>, ClubService>();
builder.Services.AddTransient<IDAO<Club>, ClubDAO>();

builder.Services.AddTransient<IService<Stadion>, StadionService>();
builder.Services.AddTransient<IDAO<Stadion>, StadionDAO>();

builder.Services.AddTransient<IUserService<AspNetUser>, UserService>();
builder.Services.AddTransient<IUserDAO<AspNetUser>, UserDAO>();

builder.Services.AddTransient<IService<Zone>, ZoneService>();
builder.Services.AddTransient<IDAO<Zone>, ZoneDAO>();

builder.Services.AddTransient<IStoelService<Stoeltje>, StoeltjeService>();
builder.Services.AddTransient<IStoelDAO<Stoeltje>, StoeltjeDAO>();

builder.Services.AddTransient<IAbonnementService<Abonnement>, AbonnementService>();
builder.Services.AddTransient<IAbonnementDAO<Abonnement>, AbonnementDAO>();

builder.Services.AddTransient<ITicketService<Ticket>, TicketService>();
builder.Services.AddTransient<ITicketDAO<Ticket>, TicketDAO>();

builder.Services.AddTransient<ICreatePDF, CreatePDF>();

//session
builder.Services.AddSession(options =>
{

    options.Cookie.Name = "be.VIVES.Session";

    options.IdleTimeout = TimeSpan.FromMinutes(10);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures)
    .SetDefaultCulture("nl-BE");

app.UseRequestLocalization(localizationOptions);

var swaggerOptions = new TicketverkoopVoetbal.Options.SwaggerOptions();
builder.Configuration.GetSection(nameof(TicketverkoopVoetbal.Options.SwaggerOptions)).Bind(swaggerOptions);
// Enable middleware to serve generated Swagger as a JSON endpoint.
// RouteTemplate legt het path vast waar de JSON-file wordt aangemaakt
app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });
// By default, your Swagger UI loads up under / swagger /.If you want to change this, it's thankfully very straight-forward. Simply set the RoutePrefix option in your call to app.UseSwaggerUI in Program.cs:
app.UseSwaggerUI(option =>
{
    option.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description);
});
app.UseSwagger();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//add session
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
