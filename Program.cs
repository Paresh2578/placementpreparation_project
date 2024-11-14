using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Placement_Preparation.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// All class 
builder.Services.AddScoped<AllDropDown>();

var app = builder.Build();





// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.MapControllerRoute(
name: "areas",
	pattern: "{area=Student}/{controller=Home}/{action=Home}/{id?}"
	);

app.Run();
