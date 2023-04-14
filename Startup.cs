using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using tutor1.Extension;

using tutor1.Models.Context;
using tutor1.Services;

namespace tutor1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMvc().AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                
            });

            #region DB
            services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));
            //services.AddDbContext<ClinicContext>(opt =>
            //{
            //    opt.UseInMemoryDatabase("ClinicOrder");
            //    opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            //    opt.EnableSensitiveDataLogging();
            //    opt.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
            //});
            services.AddDbContext<ClinicContext>(opts =>
                opts.UseSqlServer(Configuration.GetConnectionString("sqlConnection")));
            #endregion
            services.AddControllersWithViews();
            services.AddHttpContextAccessor();
            // configure DI for application services
            #region AutoMapper
            var mapperConfig = new MapperConfiguration(mc => {
                mc.AddProfile(new AutoMapperProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            #endregion

            services.AddScoped<DbContext, ClinicContext>();
            services.AddScoped<IClinicOrderService, ClinicOrderDetailService>();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Insert the middleware before all others in the pipeline.
            app.UseRequestResponseLogging();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            //nicole 20230308
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            //MiddleWare for capture http content
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
