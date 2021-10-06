using BloodCenterManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.DataAccess.Mappings
{
    public class BloodTypeMapping : IEntityTypeConfiguration<BloodTypeModel>
    {
        public void Configure(EntityTypeBuilder<BloodTypeModel> builder)
        {
            builder
                .HasKey(m => m.Id);
        }
    }
}
