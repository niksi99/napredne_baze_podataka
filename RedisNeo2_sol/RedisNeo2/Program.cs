using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Neo4jClient;
using RedisNeo2.Models.Entities;
using RedisNeo2.Services.Implementation;
using RedisNeo2.Services.Usage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "RedisNeoPr1_";
});

var clientNeo4j = new BoltGraphClient(new Uri("bolt://localhost:7687"), "neo4j", "redisneo2");
clientNeo4j.ConnectAsync();
//var clientNeo4j2 = new GraphClient(new Uri("bolt://localhost:7687"), "neo4j", "redisneo2");
//clientNeo4j2.ConnectAsync();

builder.Services.AddSingleton<IGraphClient>(clientNeo4j);
//builder.Services.AddSingleton<GraphClient>(clientNeo4j2);

builder.Services.AddScoped<IDogadjajService, DogadjajService>();
builder.Services.AddScoped<INGOService, NGOService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IKorisnikService, KorisnikService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => {
        options.LoginPath = "/Login/";
        options.Cookie.Name = "MojKolac/";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
});

builder.Services.AddMvc();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();



builder.Services.AddCors(options => {
    options.AddPolicy("CORS", o => {
        o.WithOrigins(new string[] {
                 "http://localhost:8080",
                 "https://localhost:8080",
                 "http://127.0.0.1:8080",
                 "https://127.0.0.1:8080",
                 "http://127.0.0.1:7241",
                 "https://127.0.0.1:7241",
                 "https://localhost:7241",
                 "http://localhost:7241"
             }).AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("CORS");

app.UseAuthentication();

app.UseAuthorization();

app.UseCookiePolicy();

app.UseEndpoints(e => {
    e.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
});


app.Run();
