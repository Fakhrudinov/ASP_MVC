using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MVC_Messenger.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using MVC_Messenger.Services;
using MVC_Messenger.Models;
using System;
using Quartz.Spi;
using MVC_Messenger.Jobs;
using Quartz;
using Quartz.Impl;
using MVC_Messenger.Data.Repository;
using Microsoft.Extensions.Logging;
using MVC_Messenger.LoggingToFile;
using System.IO;

namespace MVC_Messenger
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
            // database connection option
            services.AddDbContext<MessengerContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => 
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                    options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                });

            services.Configure<DataBaseConnection>(Configuration.GetSection("ConnectionStrings"));

            services.AddSingleton<IDataBaseRepository, DataBaseRepository>();

            // pagination
            services.Configure<PaginationOptions>(options =>
            {
                options.EmailItemsCount = Convert.ToInt32(Configuration["PaginationItemsAtPage:Emails"]);
                options.UserItemsCount = Convert.ToInt32(Configuration["PaginationItemsAtPage:Users"]);
            });

            // email smtp options
            services.Configure<EmailSMTPOptions>(options =>
            {
                options.Host_Address = Configuration["ExternalSMTPProviders:MailKit:SMTP:Address"];
                options.Host_Port = Convert.ToInt32(Configuration["ExternalSMTPProviders:MailKit:SMTP:Port"]);
                options.Host_Username = Configuration["ExternalSMTPProviders:MailKit:SMTP:Account"];
                options.Host_Password = Configuration["ExternalSMTPProviders:MailKit:SMTP:Password"];
                options.Sender_EMail = Configuration["ExternalSMTPProviders:MailKit:SMTP:SenderEmail"];
                options.Sender_Name = Configuration["ExternalSMTPProviders:MailKit:SMTP:SenderName"];
            });

            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddSingleton<ISmsSender, SmsSender>();

            // job to services
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            // add job and schedule
            services.AddSingleton<EmailSenderJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(EmailSenderJob),
                cronExpression: "0 0/10 * * * ?")); // запускать каждые 10 мин
            services.AddHostedService<QuartzHostedService>();


            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            string fileName = "logger_" + DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss") + ".txt";
            loggerFactory.AddFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName));
            var logger = loggerFactory.CreateLogger("FileLogger");

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
            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseAuthentication();    // аутентификация
            app.UseAuthorization();     // авторизация

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
