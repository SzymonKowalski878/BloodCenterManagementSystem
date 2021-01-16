using BloodCenterManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.DataAccess.Mappings
{
    public class BloodStorageMapping : IEntityTypeConfiguration<BloodStorageModel>
    {
        public void Configure(EntityTypeBuilder<BloodStorageModel> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .HasOne(m => m.BloodType)
                .WithMany()
                .HasForeignKey(m => m.BloodTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
