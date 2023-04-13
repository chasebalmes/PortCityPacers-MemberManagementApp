using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PortCityPacers.Data;
using Microsoft.Extensions.DependencyInjection;

namespace MembershipManagement
{ 
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<MembersContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MembersContext") ?? throw new InvalidOperationException("Connection string 'MembersContext' not found.")));

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            
            //Set back to true in production
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();
            //Creates roles, if they aren't made already
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Admin", "Manager", "Member" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));

                }
            }

            //creates an admin account if not made
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                string email = "admin@admin.com";
                string password = "0Zpin31!";
                //checks if account exists
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    //create user
                    var user = new IdentityUser();
                    user.UserName = email;
                    user.Email = email;
                    //user.EmailConfirmed = true;
                    //add user to database
                    await userManager.CreateAsync(user, password);

                    //add to role
                    await userManager.AddToRoleAsync(user, "Admin");
                }

            }
            app.Run();
        }
    }
}