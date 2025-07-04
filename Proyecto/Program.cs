using Application.UseCase;
using Proyecto.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:8081",                           // Expo Web en PC
                "http://localhost:19006",                          // Expo Web por defecto
                "https://frontend-production.vercel.app",          // (si publicas tu frontend luego)
                "https://backendnuevo-production.up.railway.app"   // Por si usas WebView o testing interno
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddApplicationServices(builder.Configuration);




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseSwagger();

    // Swagger UI en la raíz "/"
    app.UseSwaggerUI(options =>
    {
        options.RoutePrefix = string.Empty; // Esto hace que Swagger UI esté en la raíz
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
// ✅ Aplicar la política de CORS
app.UseCors("AllowFrontend");

app.UseAuthorization();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();