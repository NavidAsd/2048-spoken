using Application.Interface.Pattern;
using Application.Services.Pattern;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMvc();
builder.Services.AddProgressiveWebApp();

#region Serices
builder.Services.AddScoped<IAllServices, AllServices>();
#endregion

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Configure static file middleware to not serve any files from wwwroot
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(app.Environment.ContentRootPath, "wwwroot")),
    RequestPath = "",
    ServeUnknownFileTypes = false,
    OnPrepareResponse = ctx =>
    {
        // The files inside the protected folder cannot be accessed by the user through url
        if (ctx.File.PhysicalPath.Contains(Path.Combine(app.Environment.ContentRootPath, "wwwroot", "protected")))
        {
            // Set the status code to 404 Not Found
            ctx.Context.Response.StatusCode = StatusCodes.Status404NotFound;
            ctx.Context.Response.ContentLength = 0;
            ctx.Context.Response.Body = Stream.Null;
        }
    }
});

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{action=Index}");

app.Run();
