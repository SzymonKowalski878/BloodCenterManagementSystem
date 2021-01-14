using BloodCenterManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.DataAccess.Mappings
{
    public class UserMapping:IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .HasIndex(m => m.Email)
                .IsUnique();

            builder
                .Property(m => m.Email)
                .HasMaxLength(45);
        }
    }
}
