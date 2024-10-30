using API.Data;
using API.Extentions;
using API.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/*builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(opt => {
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
builder.Services.AddCors();
builder.Services.AddScoped<ITokenService, TokenService>();*/

builder.Services.AddApplicationServices(builder.Configuration);  // extention static method has the above-mentioned services


/*builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            var tokenKey = builder.Configuration["TokenKey"] ?? throw new Exception("TokenKey not found");
            options.TokenValidationParameters = new TokenValidationParameters{
                ValidateIssuerSigningKey =true,
                IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
                ValidateIssuer= false,
                ValidateAudience = false
            };
        });*/
        
builder.Services.AddIdentityServices(builder.Configuration);  // it has the above-mentioned Authentication code

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Seed the database at startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<DataContext>();
        await context.Database.MigrateAsync(); // beim restart our App applay migrtaions, if we drop the database, our database will be recreated 
        // Ensure the database is created (for SQLite or other database types without migrations)
        //await context.Database.EnsureCreatedAsync();
        // Seed the users
        await Seed.SeedUsers(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during migration or seeding.");
    }
}

// Configure the HTTP request pipeline.if (!app.Environment.IsDevelopment()){app.UseExceptionHandler("/Error");app.UseHsts();}

app.UseMiddleware<ExceptionMiddleware>();

app.UseRouting();

// Configure the HTTP request pipeline.   zwischen UseRouting & UseAuthentication
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod()                // x = policy
    .WithOrigins("http://localhost:4200", "https://localhost:4200"));

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

//app.MapControllers();
app.UseEndpoints(endPoints => {  _ = endPoints.MapControllers(); });

app.Run();
