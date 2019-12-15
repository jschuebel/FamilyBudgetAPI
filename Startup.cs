using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Microsoft.EntityFrameworkCore;

using FamilyBudget.Infrastructure;
using FamilyBudget.Application;
namespace FamilyBudget.WebAPI
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
            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.AddInfrastructure(Configuration);
        //    services.AddDbContext<familyinf.DataContext.BudgetDbContext>(options =>
        //      options.UseSqlite(@"Data Source=budget.db")
             //usesq.UseSqlServer(cfgbuilder.GetConnectionString("DefaultConnection"))
        //    );
            services.AddApplication();

 //          services.AddScoped<familyapp.Interface.IProductRepo, familyinf.Repositories.ProductRepo>();
 //          services.AddScoped<familyapp.Interface.IProductService, familyapp.Services.ProductService>();
            // services.AddCors(); // Make sure you call this previous to AddMvc
            services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:52293").AllowAnyMethod().AllowAnyHeader();
                    builder.WithOrigins("http://localhost:5000").AllowAnyMethod().AllowAnyHeader();
                    builder.WithOrigins("https://localhost:5001").AllowAnyMethod().AllowAnyHeader();
                    builder.WithOrigins("http://localhost:5020").AllowAnyMethod().AllowAnyHeader();
                    builder.WithOrigins("https://localhost:5021").AllowAnyMethod().AllowAnyHeader();
                }));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("ApiCorsPolicy");
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
