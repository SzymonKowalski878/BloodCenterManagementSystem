using BloodCenterManagementSystem.DataAccess.Mappings;
using BloodCenterManagementSystem.Models;
using EntityFramework.Exceptions.SqlServer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.DataAccess
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            :base(options)
        {

        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<BloodDonatorModel> BloodDonators { get; set; }
        public DbSet<BloodTypeModel> BloodTypes { get; set; }
        public DbSet<DonationModel> Donations { get; set; }
        public DbSet<BloodStorageModel> BloodStorage { get; set; }
        public DbSet<ResultOfExaminationModel> ResultsOfExamination { get; set; }

        protected override void OnModelCreating (ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserMapping());
            builder.ApplyConfiguration(new BloodDonatorMapping());
            builder.ApplyConfiguration(new BloodTypeMapping());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseExceptionProcessor();
        }
    }
}
