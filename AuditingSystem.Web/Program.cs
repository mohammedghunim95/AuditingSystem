using AuditingSystem.Database;
using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Lockups;
using AuditingSystem.Entities.RiskAssessments;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using AuditingSystem.Web.Common;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AuditingSystemDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AuditingSystemConnection"));
});
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();

builder.Services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
builder.Services.AddScoped<IBaseRepository<Year>, BaseRepository<Year>>();
builder.Services.AddScoped<IBaseRepository<Role>, BaseRepository<Role>>();

builder.Services.AddScoped<IBaseRepository<Industry>, BaseRepository<Industry>>();
builder.Services.AddScoped<IBaseRepository<Company>, BaseRepository<Company>>();
builder.Services.AddScoped<IBaseRepository<Department>, BaseRepository<Department>>();
builder.Services.AddScoped<IBaseRepository<Function>, BaseRepository<Function>>();
builder.Services.AddScoped<IBaseRepository<Activity>, BaseRepository<Activity>>();
builder.Services.AddScoped<IBaseRepository<Objective>, BaseRepository<Objective>>();
builder.Services.AddScoped<IBaseRepository<Tasks>, BaseRepository<Tasks>>();
builder.Services.AddScoped<IBaseRepository<Practice>, BaseRepository<Practice>>();

builder.Services.AddScoped<IBaseRepository<RiskIdentification>, BaseRepository<RiskIdentification>>();
builder.Services.AddScoped<IBaseRepository<Control>, BaseRepository<Control>>();
builder.Services.AddScoped<IBaseRepository<RiskAssessmentList>, BaseRepository<RiskAssessmentList>>();

builder.Services.AddScoped<IBaseRepository<RiskCategory>, BaseRepository<RiskCategory>>();
builder.Services.AddScoped<IBaseRepository<RiskImpact>, BaseRepository<RiskImpact>>();
builder.Services.AddScoped<IBaseRepository<RiskLikehood>, BaseRepository<RiskLikehood>>();
builder.Services.AddScoped<IBaseRepository<ControlType>, BaseRepository<ControlType>>();
builder.Services.AddScoped<IBaseRepository<ControlProcess>, BaseRepository<ControlProcess>>();
builder.Services.AddScoped<IBaseRepository<ControlFrequency>, BaseRepository<ControlFrequency>>();
builder.Services.AddScoped<IBaseRepository<ControlEffectiveness>, BaseRepository<ControlEffectiveness>>();




builder.Services.AddScoped<AppSession>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Index}/{id?}");

app.Run();
