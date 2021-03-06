﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BettingApplication.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BettingApplication.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net;
using BettingApplication.Services;
using BettingApplication.Services.Hosted;
using BettingApplication.Services.Implementations;
using BettingApplication.Services.Interfaces;
//using BettingApplication.Services;
using Microsoft.AspNetCore.Identity;

namespace BettingApplication
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
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddIdentity<AppUser, IdentityRole>(options =>
             {
                 options.User.RequireUniqueEmail = false;
                 options.SignIn.RequireConfirmedEmail = true;
             }).AddEntityFrameworkStores<BettingApplicationContext>();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireNonAlphanumeric=false;
            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(100);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(options =>
               {
                   options.LoginPath = "/user/login";
               });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<BettingApplicationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("BettingApplicationContext")));
            services.AddHostedService<ResultHostedService>();
            services.AddHostedService<OfferHostedService>();

            #region Services

            services.AddScoped<IMatchService, MatchService>();
            services.AddScoped<IOfferService, OfferService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IBetSlipService, BetSlipService>();
            services.AddScoped<IAdminService, AdminService>();
            //services.AddScoped<IHomeService, HomeService>();
            services.AddScoped<IBetSlipService, BetSlipService>();
            services.AddScoped<IResultService, ResultService>();
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/User/Login");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            SeedData.methodSeedData(userManager, roleManager);
            //app.UseStatusCodePages(async context => {
            //    var response = context.HttpContext.Response;

            //    if (response.StatusCode == (int)HttpStatusCode.Unauthorized ||
            //        response.StatusCode == (int)HttpStatusCode.Forbidden)
            //        response.Redirect("/Error/Unauthorized");
            //});
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}