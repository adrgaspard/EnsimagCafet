using EnsimagCafet.EntityFrameworkCore.DbMigrator;

using var context = new ApplicationDbContextFactory().CreateDbContext(Array.Empty<string>());

new ApplicationDbDataSeeder(new()).Seed(context);
