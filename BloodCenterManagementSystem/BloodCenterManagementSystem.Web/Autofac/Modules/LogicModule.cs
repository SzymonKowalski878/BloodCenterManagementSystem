using Autofac;
using BloodCenterManagementSystem.Logics.Interfaces;
using BloodCenterManagementSystem.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.Autofac.Modules
{
    public class LogicModule:Module
    {
        protected override void Load (ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(ILogic).Assembly)
                .Where(t => typeof(ILogic).IsAssignableFrom(t))
                .AsImplementedInterfaces();

            builder.Register(c =>
            {
                var config = c.Resolve<IConfiguration>();
                var emailConfig = config.GetSection("EmailConfiguration")
                .Get<EmailConfigModel>();

                return emailConfig;
            }).AsSelf().InstancePerLifetimeScope();
        }
    }
}
