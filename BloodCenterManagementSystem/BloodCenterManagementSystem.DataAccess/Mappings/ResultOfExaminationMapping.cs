using BloodCenterManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.DataAccess.Mappings
{
    public class ResultOfExaminationMapping : IEntityTypeConfiguration<ResultOfExaminationModel>
    {
        public void Configure(EntityTypeBuilder<ResultOfExaminationModel> builder)
        {
            builder
                .HasKey(m => m.Id);
        }
    }
}
