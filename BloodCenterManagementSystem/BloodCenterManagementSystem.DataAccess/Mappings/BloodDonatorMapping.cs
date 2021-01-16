using BloodCenterManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.DataAccess.Mappings
{
    public class BloodDonatorMapping : IEntityTypeConfiguration<BloodDonatorModel>
    {
        public void Configure(EntityTypeBuilder<BloodDonatorModel> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .HasOne(m => m.User)
                .WithOne(m => m.BloodDonator)
                .HasForeignKey<UserModel>(m => m.BloodDonatorId);

            builder
                .HasOne(m => m.BloodType)
                .WithMany()
                .HasForeignKey(m => m.BloodTypeId);

            builder
                .HasIndex(m => m.Pesel)
                .IsUnique();

            builder
                .HasIndex(m => m.BloodTypeId)
                .IsUnique(false);
        }
    }
}
