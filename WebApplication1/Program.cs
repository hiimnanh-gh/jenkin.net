var builder = WebApplication.CreateBuilder(args);

// ✅ Lọc các tham số dạng URL từ args để dùng cho UseUrls
var urls = args.Where(arg => arg.StartsWith("http://") || arg.StartsWith("https://")).ToArray();
builder.WebHost.UseUrls(urls);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.WebHost.UseUrls("http://0.0.0.0:81");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
