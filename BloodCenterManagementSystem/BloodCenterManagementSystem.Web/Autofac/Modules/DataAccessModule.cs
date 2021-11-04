using Autofac;
using BloodCenterManagementSystem.DataAccess;
using BloodCenterManagementSystem.Logics.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.Autofac.Modules
{
    public class DataAccessModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var config = c.Resolve<IConfiguration>();
                var opt = new DbContextOptionsBuilder<DataContext>();
                opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));

                return new DataContext(opt.Options);
            }).AsSelf().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(Repository<>).Assembly)
                .AsClosedTypesOf(typeof(IRepository<>))
                .AsImplementedInterfaces();
        }
    }
}
