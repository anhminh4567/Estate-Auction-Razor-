global using Repository.Interfaces.AppAccount;
using RazorAucionWebapp.Configure;
using Repository.Database;
using Repository.Implementation;
using Repository.Implementation.AppAccount;
using Repository.Implementation.Auction;
using Repository.Implementation.RealEstate;
using Repository.Interfaces;
using Repository.Interfaces.Auction;
using Repository.Interfaces.RealEstate;
using Service.Services.AuctionService;
using Service.Services.VnpayService.VnpayUtility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddMyRepositories();



builder.Services.AddScoped<VnpayBuildUrl>();
builder.Services.AddScoped<VnpayQuery>();
builder.Services.AddScoped<VnpayRefund>();

builder.Services.AddScoped<BidServices>();
builder.Services.AddSingleton<ServerDefaultValue>();

builder.Services.AddAuthentication("cookie")
    .AddCookie("cookie",options => 
    {
        options.LoginPath = "/Login";
        options.LogoutPath = "/Logout";
        options.AccessDeniedPath = "/Unauthorized";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
