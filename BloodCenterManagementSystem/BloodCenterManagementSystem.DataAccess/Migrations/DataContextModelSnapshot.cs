﻿// <auto-generated />
using System;
using BloodCenterManagementSystem.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BloodCenterManagementSystem.DataAccess.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("BloodCenterManagementSystem.Models.BloodDonatorModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("AmmountOfBloodDonated")
                        .HasColumnType("int");

                    b.Property<int>("BloodTypeId")
                        .HasColumnType("int");

                    b.Property<string>("HomeAdress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pesel")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BloodTypeId");

                    b.HasIndex("Pesel")
                        .IsUnique()
                        .HasFilter("[Pesel] IS NOT NULL");

                    b.ToTable("BloodDonators");
                });

            modelBuilder.Entity("BloodCenterManagementSystem.Models.BloodStorageModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("BloodTypeId")
                        .HasColumnType("int");

                    b.Property<string>("BloodUnitLocation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("DonationId")
                        .HasColumnType("int");

                    b.Property<int?>("ForeignBloodUnitId")
                        .HasColumnType("int");

                    b.Property<bool>("IsAfterCovid")
                        .HasColumnType("bit");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("BloodTypeId");

                    b.HasIndex("DonationId")
                        .IsUnique()
                        .HasFilter("[DonationId] IS NOT NULL");

                    b.ToTable("BloodStorage");
                });

            modelBuilder.Entity("BloodCenterManagementSystem.Models.BloodTypeModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("AmmountOfBloodInBank")
                        .HasColumnType("int");

                    b.Property<string>("BloodTypeName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("BloodTypes");
                });

            modelBuilder.Entity("BloodCenterManagementSystem.Models.DonationModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("BloodDonatorId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DonationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("RejectionReason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Stage")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BloodDonatorId");

                    b.ToTable("Donations");
                });

            modelBuilder.Entity("BloodCenterManagementSystem.Models.ResultOfExaminationModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<double>("BA")
                        .HasColumnType("float");

                    b.Property<int>("BloodPressureLower")
                        .HasColumnType("int");

                    b.Property<int>("BloodPressureUpper")
                        .HasColumnType("int");

                    b.Property<int>("DonationId")
                        .HasColumnType("int");

                    b.Property<double>("EO")
                        .HasColumnType("float");

                    b.Property<double>("HB")
                        .HasColumnType("float");

                    b.Property<double>("HT")
                        .HasColumnType("float");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<double>("LY")
                        .HasColumnType("float");

                    b.Property<double>("MCH")
                        .HasColumnType("float");

                    b.Property<double>("MCHC")
                        .HasColumnType("float");

                    b.Property<double>("MCV")
                        .HasColumnType("float");

                    b.Property<double>("MO")
                        .HasColumnType("float");

                    b.Property<double>("NE")
                        .HasColumnType("float");

                    b.Property<double>("PLT")
                        .HasColumnType("float");

                    b.Property<double>("RBC")
                        .HasColumnType("float");

                    b.Property<double>("WBC")
                        .HasColumnType("float");

                    b.Property<int>("Weight")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DonationId")
                        .IsUnique();

                    b.ToTable("ResultsOfExamination");
                });

            modelBuilder.Entity("BloodCenterManagementSystem.Models.UserModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int?>("BloodDonatorId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasMaxLength(45)
                        .HasColumnType("nvarchar(45)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BloodDonatorId")
                        .IsUnique()
                        .HasFilter("[BloodDonatorId] IS NOT NULL");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasFilter("[Email] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BloodCenterManagementSystem.Models.BloodDonatorModel", b =>
                {
                    b.HasOne("BloodCenterManagementSystem.Models.BloodTypeModel", "BloodType")
                        .WithMany()
                        .HasForeignKey("BloodTypeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("BloodType");
                });

            modelBuilder.Entity("BloodCenterManagementSystem.Models.BloodStorageModel", b =>
                {
                    b.HasOne("BloodCenterManagementSystem.Models.BloodTypeModel", "BloodType")
                        .WithMany()
                        .HasForeignKey("BloodTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BloodCenterManagementSystem.Models.DonationModel", "Donation")
                        .WithOne("BloodStorage")
                        .HasForeignKey("BloodCenterManagementSystem.Models.BloodStorageModel", "DonationId");

                    b.Navigation("BloodType");

                    b.Navigation("Donation");
                });

            modelBuilder.Entity("BloodCenterManagementSystem.Models.DonationModel", b =>
                {
                    b.HasOne("BloodCenterManagementSystem.Models.BloodDonatorModel", "BloodDonator")
                        .WithMany()
                        .HasForeignKey("BloodDonatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BloodDonator");
                });

            modelBuilder.Entity("BloodCenterManagementSystem.Models.ResultOfExaminationModel", b =>
                {
                    b.HasOne("BloodCenterManagementSystem.Models.DonationModel", "Donation")
                        .WithOne("ResultOfExamination")
                        .HasForeignKey("BloodCenterManagementSystem.Models.ResultOfExaminationModel", "DonationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Donation");
                });

            modelBuilder.Entity("BloodCenterManagementSystem.Models.UserModel", b =>
                {
                    b.HasOne("BloodCenterManagementSystem.Models.BloodDonatorModel", "BloodDonator")
                        .WithOne("User")
                        .HasForeignKey("BloodCenterManagementSystem.Models.UserModel", "BloodDonatorId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("BloodDonator");
                });

            modelBuilder.Entity("BloodCenterManagementSystem.Models.BloodDonatorModel", b =>
                {
                    b.Navigation("User");
                });

            modelBuilder.Entity("BloodCenterManagementSystem.Models.DonationModel", b =>
                {
                    b.Navigation("BloodStorage");

                    b.Navigation("ResultOfExamination");
                });
#pragma warning restore 612, 618
        }
    }
}
