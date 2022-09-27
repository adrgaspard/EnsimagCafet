using EnsimagCafet.EntityFrameworkCore.DbMigrator;
using Microsoft.Extensions.Configuration;
using System.Configuration;

var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true).AddEnvironmentVariables().AddCommandLine(Array.Empty<string>()).Build();

using var context = new ApplicationDbContextFactory(config).CreateDbContext(Array.Empty<string>());

new ApplicationDbDataSeeder(new() { SuperUserDefaultPassword = config.GetSection("Seeding").GetSection("SuperUserDefaultPassword").Value }).Seed(context);