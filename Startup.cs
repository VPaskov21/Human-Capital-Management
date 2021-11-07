using HCMApp.Data;
using HCMApp.Data.Repositories;
using HCMApp.Data.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace HCMApp
{
    public class Startup
    {
        private readonly IWebHostEnvironment environment;

        public string ConnectionString { get; set; }
        public Startup(IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            Configuration = configuration;
            this.environment = environment;
            ConnectionString = Configuration.GetConnectionString("DefaultConnectionString");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000")
                        .WithMethods("GET")
                        .AllowAnyHeader();
                    });
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "MySessionCookie";
                    options.LoginPath = "/Login/Index";
                    options.SlidingExpiration = true;
                });


            services.AddControllersWithViews()
                    .AddJsonOptions(options =>
               options.JsonSerializerOptions.PropertyNamingPolicy = null);

            //Configure DBContext with SQL
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(ConnectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));

            /*services.AddDataProtection()
                    .SetApplicationName($"my-app-{environment.EnvironmentName}")
                    .PersistKeysToFileSystem(new DirectoryInfo($@"{environment.ContentRootPath}\Keys"));
            */
            //Configure services
            services.AddTransient<LoginService>();
            services.AddTransient<UserService>();
            services.AddTransient<SalaryService>();
            services.AddTransient<AddressService>();
            services.AddTransient<ImageService>();
            services.AddTransient<RoleService>();
            services.AddTransient<CountryService>();
            services.AddTransient<RegionService>();
            services.AddTransient<CityService>();
            services.AddTransient<PostalCodeService>();
            services.AddTransient<AbsenceService>();
            services.AddTransient<HolidaysService>();
            services.AddTransient<DepartmentService>();
            services.AddTransient<WorkMonthService>();
            services.AddTransient<ValidationService>();

            //Configure repository
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IRegionRepository, RegionRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IPostalCodeRepository, PostalCodeRepository>();
            services.AddScoped<IAbsenceReasonsRepository, AbsenceReasonsRepository>();
            services.AddScoped<IAbsenceRequestsRepository, AbsenceRequestsRepository>();
            services.AddScoped<IWorkMonthsRepository, WorkMonthsRepository>();
            services.AddScoped<ISalaryHistoriesRepository, SalaryHistoriesRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            var cookiePolicyOptions = new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.None
            };

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });

            AppDbInitializer.Seed(app);
        }
    }
}
