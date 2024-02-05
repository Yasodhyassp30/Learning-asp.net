using AllInOne.Middleware;
using AllInOne.Models;
using AllInOne.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<MongoDb>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddControllersWithViews();
builder.Services.AddLogging(builder => builder.AddConsole());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseWhen(context => context.Request.Path.StartsWithSegments("/api") && !context.Request.Path.Equals("/api/user/register") && !context.Request.Path.Equals("/api/user/login"), app =>
{
    app.UseMiddleware<JwtMiddleware>();
});



app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
