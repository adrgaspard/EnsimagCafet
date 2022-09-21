using EnsimagCafet.EntityFrameworkCore.DbMigrator;
using System.Configuration;

Configuration config = ConfigurationManager.OpenMappedExeConfiguration(new() { ExeConfigFilename = "App.config" }, ConfigurationUserLevel.None);

using var context = new ApplicationDbContextFactory(config).CreateDbContext(Array.Empty<string>());

new ApplicationDbDataSeeder(new() { SuperUserDefaultPassword = config.AppSettings.Settings["SuperUserDefaultPassword"].Value }).Seed(context);