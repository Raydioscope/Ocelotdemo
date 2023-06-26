using Ocelot.DependencyInjection;
using Ocelot.Middleware;



var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("ocelot.json", optional:false, reloadOnChange: true);
                            
builder.Services.AddOcelot(builder.Configuration);
// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();
await app.UseOcelot();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
