using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Frontend.Services;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Placement_Preparation.Areas.Admin.Data.Interface;
using Placement_Preparation.Areas.Admin.Data.Repository;
using Placement_Preparation.Services;
using Placement_Preparation.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(1); // Set session timeout
    options.Cookie.HttpOnly = false; // Make cookie HTTP-only
    options.Cookie.IsEssential = true; // Necessary for GDPR compliance
});


// Register services
builder.Services.AddScoped<ApiClientService>();
builder.Services.AddScoped<ExportService>();
builder.Services.AddScoped<TopicInterface, TopicRepo>();
builder.Services.AddScoped<SubTopicInterface, SubTopicRepo>();
builder.Services.AddScoped<AllDropDown>();




// Add IHttpContextAccessor 
builder.Services.AddHttpContextAccessor();


// Register HttpClient with a common BaseAddress
builder.Services.AddHttpClient<ApiClientService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]!); // Common Base URL for all tables/services
});

// Register the ToastNotification service
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 5; // Duration of the toast notification
    config.IsDismissable = true;  // Whether the toast can be dismissed manually
    config.Position = NotyfPosition.TopRight; // Position of the toast
});

// Allow any origin, method, and header (not recommended for production)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

var app = builder.Build();

// Use CORS
app.UseCors("AllowAll");

// Use the ToastNotification middleware
app.UseNotyf();


// Enable session middleware
app.UseSession();

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

app.UseStatusCodePagesWithReExecute("/Error/{0}"); // Handle status codes, including 404


app.UseAuthorization();
app.MapControllerRoute(
name: "areas",
	pattern: "{area=Student}/{controller=Home}/{action=Home}/{id?}"
	);

app.Run();
