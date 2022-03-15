using FluentMigrator.Runner;
using ICICI.AppCode.DAL;
using ICICI.AppCode.Data;
using ICICI.AppCode.Helper;
using ICICI.AppCode.Interfaces;
using ICICI.AppCode.Migrations;
using ICICI.AppCode.Reops;
using ICICI.AppCode.Reops.Entities;
using ICICI.Models;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using System.Reflection;
using ICICI.AppCode.Interfaces;
using ICICI.AppCode.Reops;
using System.Collections.Generic;

namespace ICICI.AppCode.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void RegisterService(this IServiceCollection services, IConfiguration configuration)
        {
            string dbConnectionString = configuration.GetConnectionString("SqlConnection");
            GlobalDiagnosticsContext.Set("connectionString", dbConnectionString);
            IConnectionString ch = new ConnectionString { connectionString = dbConnectionString };
            services.AddSingleton<IConnectionString>(ch);
            services.AddSingleton<IDapperRepository, DapperRepository>();
            services.AddScoped<ApplicationDbContext>();
            services.AddTransient<IUserStore<ApplicationUser>, UserStore>();
            services.AddTransient<IRoleStore<ApplicationRole>, RoleStore>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<ILog, LogNLog>();
            services.AddSingleton<IRepository<EmailConfig>, EmailConfigRepo>();           
            services.AddSingleton<IUserService, UsersRepo>();           
            services.AddSingleton<IAPIServices, APIServices>();
            services.AddSingleton<IBankService, BankService>();
            services.AddSingleton<Database>();
            services.AddAutoMapper(typeof(Startup));
            services.AddHangfire(x => x.UseSqlServerStorage(dbConnectionString));
            services.AddHangfireServer();
            services.AddLogging(c => c.AddFluentMigratorConsole())
                .AddFluentMigratorCore()
                .ConfigureRunner(c => c.AddSqlServer2016()
                .WithGlobalConnectionString(configuration.GetConnectionString("SqlConnection"))
                .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations());
            List<APIConfig> apiConfig = new List<APIConfig>();
            configuration.GetSection("APIConfig").Bind(apiConfig);
            services.AddSingleton<List<APIConfig>>(apiConfig);
        }
    }
}