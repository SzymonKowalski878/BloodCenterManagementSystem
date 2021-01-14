using Microsoft.EntityFrameworkCore.Migrations;

namespace BloodCenterManagementSystem.DataAccess.Migrations
{
    public partial class initialMigration : Migration
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
                name: "Users");

            migrationBuilder.DropTable(
                name: "BloodDonators");

            migrationBuilder.DropTable(
                name: "BloodTypes");
        }
    }
}
