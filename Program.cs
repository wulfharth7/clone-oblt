using clone_oblt.Services;
using clone_oblt.Services.Interfaces;
using clone_oblt.Utils;
using static clone_oblt.Services.Interfaces.IObiletApiService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton(SingletonApiKey.GetInstance());
builder.Services.AddHttpClient<IObiletApiService, ObiletApiService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IBusLocationApiService, BusLocationApiService>();
builder.Services.AddScoped<IJourneysApiService, JourneysApiService>();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".Clone.Session";  
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; 
});

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
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
