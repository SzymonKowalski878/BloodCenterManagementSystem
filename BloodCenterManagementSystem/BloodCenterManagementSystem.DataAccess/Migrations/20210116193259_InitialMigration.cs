using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BloodCenterManagementSystem.DataAccess.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BloodTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BloodTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmmountOfBloodInBank = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BloodDonators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pesel = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    HomeAdress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmmountOfBloodDonated = table.Column<int>(type: "int", nullable: false),
                    BloodTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodDonators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BloodDonators_BloodTypes_BloodTypeId",
                        column: x => x.BloodTypeId,
                        principalTable: "BloodTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Donations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Stage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DonationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BloodDonatorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Donations_BloodDonators_BloodDonatorId",
                        column: x => x.BloodDonatorId,
                        principalTable: "BloodDonators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BloodDonatorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_BloodDonators_BloodDonatorId",
                        column: x => x.BloodDonatorId,
                        principalTable: "BloodDonators",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BloodStorage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ForeignBloodUnitId = table.Column<int>(type: "int", nullable: false),
                    BloodUnitLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    IsAfterCovid = table.Column<bool>(type: "bit", nullable: false),
                    BloodTypeId = table.Column<int>(type: "int", nullable: false),
                    DonationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodStorage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BloodStorage_BloodTypes_BloodTypeId",
                        column: x => x.BloodTypeId,
                        principalTable: "BloodTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BloodStorage_Donations_DonationId",
                        column: x => x.DonationId,
                        principalTable: "Donations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResultsOfExamination",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HB = table.Column<double>(type: "float", nullable: false),
                    HT = table.Column<double>(type: "float", nullable: false),
                    RBC = table.Column<double>(type: "float", nullable: false),
                    WBC = table.Column<double>(type: "float", nullable: false),
                    PLT = table.Column<double>(type: "float", nullable: false),
                    MCH = table.Column<double>(type: "float", nullable: false),
                    MCHC = table.Column<double>(type: "float", nullable: false),
                    MCV = table.Column<double>(type: "float", nullable: false),
                    NE = table.Column<double>(type: "float", nullable: false),
                    EO = table.Column<double>(type: "float", nullable: false),
                    BA = table.Column<double>(type: "float", nullable: false),
                    LY = table.Column<double>(type: "float", nullable: false),
                    MO = table.Column<double>(type: "float", nullable: false),
                    BloodPressureUpper = table.Column<int>(type: "int", nullable: false),
                    BloodPressureLower = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    DonationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultsOfExamination", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResultsOfExamination_Donations_DonationId",
                        column: x => x.DonationId,
                        principalTable: "Donations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BloodDonators_BloodTypeId",
                table: "BloodDonators",
                column: "BloodTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BloodDonators_Pesel",
                table: "BloodDonators",
                column: "Pesel",
                unique: true,
                filter: "[Pesel] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BloodStorage_BloodTypeId",
                table: "BloodStorage",
                column: "BloodTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BloodStorage_DonationId",
                table: "BloodStorage",
                column: "DonationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Donations_BloodDonatorId",
                table: "Donations",
                column: "BloodDonatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultsOfExamination_DonationId",
                table: "ResultsOfExamination",
                column: "DonationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_BloodDonatorId",
                table: "Users",
                column: "BloodDonatorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BloodStorage");

            migrationBuilder.DropTable(
                name: "ResultsOfExamination");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Donations");

            migrationBuilder.DropTable(
                name: "BloodDonators");

            migrationBuilder.DropTable(
                name: "BloodTypes");
        }
    }
}
