using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class mcqTBLadedd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mcqs",
                columns: table => new
                {
                    McqId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubTopicId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TopicId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    QuestionText = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    OptionA = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    OptionB = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    OptionC = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    OptionD = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mcqs", x => x.McqId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mcqs");
        }
    }
}
