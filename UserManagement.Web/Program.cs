using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Data;
using Westwind.AspNetCore.Markdown;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddDomainServices()
    .AddMarkdown()
    .AddControllersWithViews();

//Change DataContext to standard DataContext for in-memory database
//Change DataContext to SqlDataContext for SQL database
builder.Services.AddScoped<IDataContext, DataContext>();

builder.Services.AddDbContext<SqlDataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

var app = builder.Build();

app.UseMarkdown();

app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
