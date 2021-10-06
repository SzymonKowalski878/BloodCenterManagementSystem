using Autofac;
using BloodCenterManagementSystem.Logics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.Autofac.Modules
{
    public class ValidatorModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMyValidator).Assembly)
                .Where(t => typeof(IMyValidator).IsAssignableFrom(t))
                .AsImplementedInterfaces();
        }
    }
}
