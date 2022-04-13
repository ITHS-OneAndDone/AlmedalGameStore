using AlmedalGameStore.DataAccess;
using AlmedalGameStore.DataAccess.GenericRepository;
using AlmedalGameStore.DataAccess.GenericRepository.IGenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using AlemedalGameStore.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//Det s�ger att vi anv�nder SQL server och h�mtar connectionstring i appSettings med hj�lp av DefaultConnection
//inuti ett block som heter ConnectionStrings
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();
//Whenever we request a object of IUnitOFWork, ger de oss implementation som vi definierat inuti UnitOfWork,
//bra f�r dependicy injections
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
//bygger application coockie där vi kan sätta egna paths, exempelvis om en icke registrerad kund
//försöker att köpa en produkt så sätts AccessDeniedPath igång och ber gästen att logga in
//eftersom vi använder cookies så "sparas" kundkorgsvyn för gästen och tas till den sidan efter ha 
//skapat sitt account
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
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

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();
app.MapRazorPages();
////However the Application's Startup code may require additional changes for things to work end to end.
//Add the following code to the Configure method in your Application's Startup class if not already done:
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
