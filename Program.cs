using clone_oblt.Services;
using clone_oblt.Services.Interfaces;
using clone_oblt.Utils;
using static clone_oblt.Services.Interfaces.IObiletApiService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // Use AddControllers() if you have API controllers.
builder.Services.AddDistributedMemoryCache();
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
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// Remove or adjust CORS configuration if not needed.

var app = builder.Build();

// Remove or adjust CORS middleware if not needed.

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

// Map attribute-routed controllers
app.MapControllers();

// Fallback to React app's index.html for other routes
app.MapFallbackToFile("index.html");

app.Run();
