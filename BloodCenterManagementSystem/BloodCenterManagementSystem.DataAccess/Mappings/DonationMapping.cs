using BloodCenterManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodCenterManagementSystem.DataAccess.Mappings
{
    public class DonationMapping : IEntityTypeConfiguration<DonationModel>
    {
        public void Configure(EntityTypeBuilder<DonationModel> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .HasOne(m => m.BloodDonator)
                .WithMany()
                .HasForeignKey(m => m.BloodDonatorId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(m => m.ResultOfExamination)
                .WithOne(m => m.Donation)
                .HasForeignKey<ResultOfExaminationModel>(m => m.DonationId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(m => m.BloodStorage)
                .WithOne(m => m.Donation)
                .HasForeignKey<BloodStorageModel>(m => m.DonationId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
