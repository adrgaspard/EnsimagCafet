using EnsimagCafet.Domain.Identity;
using EnsimagCafet.Domain.Shared.Identity;
using EnsimagCafet.EntityFrameworkCore;
using EnsimagCafet.MailKit;
using EnsimagCafet.Web.Authorization;
using EnsimagCafet.Web.Emailing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
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
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}
builder.Services.AddIdentity<User, Role>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

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

if (Environment.GetEnvironmentVariable("SMTP_SENDER_EMAIL") is string senderEmail
    && Environment.GetEnvironmentVariable("SMTP_SENDER_NAME") is string senderName
    && Environment.GetEnvironmentVariable("SMTP_SENDER_PASSWORD") is string senderPassword
    && Environment.GetEnvironmentVariable("SMTP_HOST") is string smtpHost
    && int.TryParse(Environment.GetEnvironmentVariable("SMTP_PORT"), out int smtpPort)
    && bool.TryParse(Environment.GetEnvironmentVariable("SMTP_USE_SSL"), out bool useSsl)
    && bool.TryParse(Environment.GetEnvironmentVariable("SMTP_CHECK_CERTIFICATE_REVOCATION"), out bool checkCertificateRevocation)
    && Enum.TryParse(Environment.GetEnvironmentVariable("SMTP_CONTENT_TYPE"), out MailContentType contentType))
{
    builder.Services.AddTransient<IEmailSender, MailKitEmailSender>();
    builder.Services.Configure<MailKitEmailSenderOptions>(options =>
    {
        options.SenderEmail = senderEmail;
        options.SenderName = senderName;
        options.SenderPassword = senderPassword;
        options.SmtpHost = smtpHost;
        options.SmtpPort = smtpPort;
        options.UseSsl = useSsl;
        options.CheckCertificateRevocation = checkCertificateRevocation;
        options.ContentType = contentType;
    });
}
else
{
    builder.Services.AddSingleton<IEmailSender, MockEmailSender>();
}

// Add data protection to the container.

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new (@"/var/af-keys/"))
    .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration{ EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC, ValidationAlgorithm = ValidationAlgorithm.HMACSHA256 });

// Configure header forwarding.

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.RequireHeaderSymmetry = false;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

// Configure application cookie.

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Error/403";
    options.LoginPath = "/Identity/Login";
    options.LogoutPath = "/Identity/LogOff";
});

// Build the app.

var app = builder.Build();

// Enable header forwarding.

app.UseForwardedHeaders();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error/500");
}
app.UseStatusCodePagesWithRedirects("/Error/{0}");
app.UseStaticFiles();
app.UseRouting();

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