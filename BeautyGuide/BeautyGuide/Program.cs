using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = builder.Configuration.GetValue<long>("Kestrel:Limits:MaxRequestBodySize");
    serverOptions.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(builder.Configuration.GetValue<int>("RequestTimeout"));
});
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = builder.Configuration.GetValue<long>("Kestrel:Limits:MaxRequestBodySize");
}); 

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    //options.Cookie.Name = ".AdventureWorks.Session";
    //options.IdleTimeout = TimeSpan.FromSeconds(10);
    ////options.Cookie.HttpOnly = true;
    //options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromSeconds(100);
    options.IOTimeout = TimeSpan.FromSeconds(100);
    options.Cookie.Name = ".MySampleMVCWeb.Session";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Path = "/";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var provider = builder.Services.BuildServiceProvider();
var configuration = provider.GetRequiredService<IConfiguration>();

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
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
