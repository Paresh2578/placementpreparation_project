using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class difficultyLevelsadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "difficultyLevels",
                columns: table => new
                {
                    DifficultyLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DifficultyLevelName = table.Column<string>(type: "string", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_difficultyLevels", x => x.DifficultyLevelId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "difficultyLevels");
        }
    }
}
