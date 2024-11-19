using backend.Controllers;
using backend.data;
using backend.data.Interface;
using backend.data.Repository;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Connect SQL
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DBCS")));

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


// Register services
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AdminUserRepo>();
builder.Services.AddScoped<BranchRepo>();
builder.Services.AddScoped<CourseTypeRepo>();
builder.Services.AddScoped<DifficultyLevelRepo>();
builder.Services.AddScoped<CourseRepo>();
builder.Services.AddScoped<TopicRepo>();
builder.Services.AddScoped<SubTopicRepo>();
builder.Services.AddScoped<QuestionRepo>();
builder.Services.AddScoped<McqController>();

// Interface
builder.Services.AddScoped<AdminUserInterface, AdminUserRepo>();
builder.Services.AddScoped<BranchInterface, BranchRepo>();
builder.Services.AddScoped<CourseTypeInterface, CourseTypeRepo>();
builder.Services.AddScoped<DifficultyLevelInterface, DifficultyLevelRepo>();
builder.Services.AddScoped<CourseInterface, CourseRepo>();
builder.Services.AddScoped<TopicInterface, TopicRepo>();
builder.Services.AddScoped<SubTopicInterface, SubTopicRepo>();
builder.Services.AddScoped<QuestionInterface, QuestionRepo>();
builder.Services.AddScoped<McqInterface, McqRepo>();


// Cloudinary Configuration
var cloudinaryAccount = new Account(
    builder.Configuration["Cloudinary:CloudName"],
    builder.Configuration["Cloudinary:ApiKey"],
    builder.Configuration["Cloudinary:ApiSecret"]
);

var cloudinary = new Cloudinary(cloudinaryAccount);

// Register Cloudinary as a service
builder.Services.AddSingleton(cloudinary);


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
