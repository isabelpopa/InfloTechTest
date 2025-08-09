using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Forename = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Forename = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DateOfBirth", "Email", "Forename", "IsActive", "Surname" },
                values: new object[,]
                {
                    { 1L, new DateOnly(1980, 8, 12), "ploew@example.com", "Peter", true, "Loew" },
                    { 2L, new DateOnly(2001, 2, 16), "bfgates@example.com", "Benjamin Franklin", true, "Gates" },
                    { 3L, new DateOnly(1988, 4, 4), "ctroy@example.com", "Castor", false, "Troy" },
                    { 4L, new DateOnly(2005, 12, 24), "mraines@example.com", "Memphis", true, "Raines" },
                    { 5L, new DateOnly(1975, 7, 21), "sgodspeed@example.com", "Stanley", true, "Goodspeed" },
                    { 6L, new DateOnly(1999, 6, 14), "himcdunnough@example.com", "H.I.", true, "McDunnough" },
                    { 7L, new DateOnly(2002, 11, 23), "cpoe@example.com", "Cameron", false, "Poe" },
                    { 8L, new DateOnly(1997, 9, 11), "emalus@example.com", "Edward", false, "Malus" },
                    { 9L, new DateOnly(1998, 5, 4), "dmacready@example.com", "Damon", false, "Macready" },
                    { 10L, new DateOnly(2000, 2, 18), "jblaze@example.com", "Johnny", true, "Blaze" },
                    { 11L, new DateOnly(2004, 10, 12), "rfeld@example.com", "Robin", true, "Feld" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLogs");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
