using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ASM.Data;
using Microsoft.AspNetCore.Identity;
using ASM.Repository;
using System.Security.Policy;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ASMContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ASMContext") ?? throw new InvalidOperationException("Connection string 'ASMContext' not found.")));

builder.Services
    
	.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddEntityFrameworkStores<ASMContext>()
	.AddDefaultUI()
	.AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ICartRepository, CartRepository>();
builder.Services.AddTransient<IUserOrderRepository, UserOrderRepository>();


var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
	await DbSeederRole.SeedDefaultData(scope.ServiceProvider);
}


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

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
ASMDbInitializer.Seed(app);
app.Run();
