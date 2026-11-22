using CRUDWebAppInASPCore.Models;
using CRUDWebAppInASPCore.Storage;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews(options =>
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

var sqlServer = builder.Configuration.GetConnectionString("dbcs");
if (string.IsNullOrWhiteSpace(sqlServer))
{
    var dataDirectory = Path.Combine(builder.Environment.ContentRootPath, "App_Data");
    Directory.CreateDirectory(dataDirectory);
    var connectionString = new SqliteConnectionStringBuilder
    {
        DataSource = Path.Combine(dataDirectory, "students.db")
    }.ToString();

    builder.Services.AddSingleton(new SqliteStudentStore(connectionString));
    builder.Services.AddSingleton<IStudentStore>(services => services.GetRequiredService<SqliteStudentStore>());
    builder.Services.AddDataProtection().UseEphemeralDataProtectionProvider();
}
else
{
    builder.Services.AddDbContext<StudentDBContext>(options => options.UseSqlServer(sqlServer));
    builder.Services.AddScoped<IStudentStore, SqlServerStudentStore>();
}

var app = builder.Build();
if (app.Services.GetService<SqliteStudentStore>() is { } sqlite)
{
    await sqlite.InitializeAsync();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
