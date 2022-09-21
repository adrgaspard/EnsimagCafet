using EnsimagCafet.Domain.Identity;
using EnsimagCafet.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add database services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add domain layer services to the container.

// Add application layer services to the container.

// Add MVC services to the container.

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

// Add authentication & authorization services to the container.

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("IsSuperUser", policy => policy.AddRequirements(new IdentifierRequirement("oui@test.com")));
//});
//builder.Services.AddSingleton<IAuthorizationHandler, IdentifierAuthorizationHandler>();
//builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

// Add logging service to the container.

builder.Services.AddLogging();

// Add localization service to the container.

builder.Services.AddLocalization();

// Add mail services to the container

//builder.Services.AddTransient<IEmailSender, MailKitEmailSender>();
//builder.Services.Configure<MailKitEmailSenderOptions>(options => // Remplacer par builder.Configuration
//{
//    options.SenderName = "Cafet Ensimag";
//    options.SenderEmail = "ensi.cafet@gmail.com";
//    options.SenderPassword = "bbreclkjpcvcafgq";
//});

// Build the app.

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Configure the authentication & autorization services.

app.UseAuthentication();
app.UseAuthorization();

// Configure the MVC services.

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Run the app.

app.Run();
