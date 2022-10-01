using EnsimagCafet.Domain.Identity;
using EnsimagCafet.Domain.Shared.Identity;
using EnsimagCafet.EntityFrameworkCore;
using EnsimagCafet.MailKit;
using EnsimagCafet.Web.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add database services to the container.

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add domain layer services to the container.

// Add application layer services to the container.

// Add MVC services to the container.

var mvcBuilder = builder.Services.AddMvc();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

// Add authentication & authorization services to the container.

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthorizationPolicies.IsSuperUser, policy => policy.AddRequirements(new SuperUserRequirement(UserConsts.SuperUserUserName, new List<string> { RoleConsts.AdminRoleName })));
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
});
builder.Services.AddSingleton<IAuthorizationHandler, SuperUserAuthorizationHandler>();

// Add logging service to the container.

builder.Services.AddLogging();

// Add localization service to the container.

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
mvcBuilder.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix, options => options.ResourcesPath = "Resources")
    .AddDataAnnotationsLocalization();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo>()
    {
        new CultureInfo("en-US"),
        new CultureInfo("fr-FR")
    };
    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

// Add mail services to the container.

builder.Services.AddTransient<IEmailSender, MailKitEmailSender>();
builder.Services.Configure<MailKitEmailSenderOptions>(builder.Configuration.GetSection(MailKitEmailSenderOptions.MailKitSectionName));

// Add data protection to the container.

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new (@"/var/af-keys/"))
    .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration{ EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC, ValidationAlgorithm = ValidationAlgorithm.HMACSHA256 });

// Build the app.

builder.Services.AddCertificateForwarding(options => options.CertificateHeader = "X-Forwarded-Tls-Client-Cert");

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error/500");
    app.UseHsts();
}
app.UseStatusCodePagesWithRedirects("/Error/{0}");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.Urls.Add("http://*:5001");
app.Urls.Add("http://*:5000");

// Configure the localization services.

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

// Configure the authentication & autorization services.

app.UseAuthentication();
app.UseAuthorization();

// Configure the MVC services.

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.MapRazorPages();

// Run the app.

app.Run();