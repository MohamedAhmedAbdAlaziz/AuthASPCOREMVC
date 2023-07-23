using Core.Models;
using EmployeeManagement.Security;
using Infrastrucrture.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddAuthorization(options => {
 options.AddPolicy("DeleteRolePolicy",
 policy=> policy.RequireClaim("Delete Role","true"));
// options.AddPolicy("EditRolePolicy",
//  policy=> policy.RequireClaim("Edit Role"));


// options.AddPolicy("EditRolePolicy",
//  policy=> policy.AddRequirements(new ManageAdminRolesAndClaimsRequirment()));

options.AddPolicy("EditRolePolicy",
 policy=> policy.RequireClaim("Edit Role","true"));

options.AddPolicy("EditRolePolicy",policy=>policy.RequireAssertion(context=>
(context.User.IsInRole("Admin") &&
 context.User.HasClaim(calim => calim.Type == "Edit Role" && calim.Value == "true" ))||
 context.User.IsInRole("Super Admin")
));
// options.AddPolicy("EditRolePolicy",
//  policy=> policy.RequireClaim("Edit Role")
//                 .RequireClaim("Edit","true")
//                 .RequireClaim("Super Admin")
//  );


//   options.AddPolicy("AllowedCountryPolicy",
//  policy=> policy.RequireClaim("Country","USA","India", "UK"));

//  options.AddPolicy("AdminRolePolicy",
//  policy=> policy.RequireClaim("Admin"));

});
builder.Services.AddDbContext<AppDbContext>(x=> x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<ApplicationUser,IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddScoped<IEmployeeRepository, SqlEmployeeRepository>();
//builder.Services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();
// builder.Services.ConfigureApplicationCookie(option=>{
//  option.AccessDeniedPath= new PathString("/Administration/AccessDenied");
// });
// builder.Services.AddMvc(config=>{
//     var policy= new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
//     config.Filters.Add(new AuthorizeFilter(policy));
    
// });
// builder.Services.AddIdentity<IdentityUser,IdentityRole>(options=>{
//     options.Password.RequiredLength=10;
//     options.Password.RequiredUniqueChars=3;

// }).AddEntityFrameworkStores<AppDbContext>();
// builder.Services.Configure<IdentityOptions>(options=>{
//     options.Password.RequiredLength=10;
//     options.Password.RequiredUniqueChars=3;

// }
// );
var app = builder.Build();

// Configure the HTTP request pipeline.
 
  if (!app.Environment.IsDevelopment())
 {
      //  app.UseExceptionHandler("/Error");
      //  app.UseStatusCodePagesWithRedirects("/Error/{0}");
      app.UseExceptionHandler("/Home/Error");
     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
      app.UseHsts();
  }
// else{
    
//     app.UseExceptionHandler("/Error");
//     app.UseStatusCodePagesWithRedirects("/Error/{0}");
// }
app.UseRouting();
app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
