using Autofac;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.Autofac.Modules
{
    public class MapperModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(typeof(MapperModule).Assembly).As<Profile>();

            builder
                .Register(context => new MapperConfiguration(cfg =>
                 {
                     foreach (var profile in context.Resolve<IEnumerable<Profile>>())
                     {
                         cfg.AddProfile(profile);
                     }
                 })).AsSelf().SingleInstance();
        }
    }
}
