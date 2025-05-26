using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class CommunityPostTableCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Topics",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "SubTopics",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AddColumn<Guid>(
                name: "CourseId",
                table: "SubTopics",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DifficultyLevelId",
                table: "SubTopics",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "SubTopics",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QuestionAnswer",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "Question",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<Guid>(
                name: "AddedBy",
                table: "Questions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApproveStatus",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CourseId",
                table: "Questions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DifficultyLevelId",
                table: "Questions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Questions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TechStack",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AddedBy",
                table: "Mcqs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AnswerDescription",
                table: "Mcqs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ApproveStatus",
                table: "Mcqs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Mcqs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CourseId",
                table: "Mcqs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DifficultyLevelId",
                table: "Mcqs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Mcqs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TechStack",
                table: "Mcqs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApproveStatus",
                table: "AdminUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CollegeName",
                table: "AdminUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "AdminUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "AdminUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TokenExpiryTime",
                table: "AdminUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    FeedbackId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeedbackType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FeedbackMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.FeedbackId);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.QuestionId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Topics_CourseId",
                table: "Topics",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_DifficultyLevelId",
                table: "Topics",
                column: "DifficultyLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_SubTopics_CourseId",
                table: "SubTopics",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_SubTopics_DifficultyLevelId",
                table: "SubTopics",
                column: "DifficultyLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_SubTopics_TopicId",
                table: "SubTopics",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_AddedBy",
                table: "Questions",
                column: "AddedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_CourseId",
                table: "Questions",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_DifficultyLevelId",
                table: "Questions",
                column: "DifficultyLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_SubTopicId",
                table: "Questions",
                column: "SubTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TopicId",
                table: "Questions",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Mcqs_AddedBy",
                table: "Mcqs",
                column: "AddedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Mcqs_CourseId",
                table: "Mcqs",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Mcqs_DifficultyLevelId",
                table: "Mcqs",
                column: "DifficultyLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Mcqs_SubTopicId",
                table: "Mcqs",
                column: "SubTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Mcqs_TopicId",
                table: "Mcqs",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_BranchId",
                table: "Courses",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseTypeId",
                table: "Courses",
                column: "CourseTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Branches_BranchId",
                table: "Courses",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_CourseTypes_CourseTypeId",
                table: "Courses",
                column: "CourseTypeId",
                principalTable: "CourseTypes",
                principalColumn: "CourseTypeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mcqs_AdminUsers_AddedBy",
                table: "Mcqs",
                column: "AddedBy",
                principalTable: "AdminUsers",
                principalColumn: "AdminUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mcqs_Courses_CourseId",
                table: "Mcqs",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mcqs_DifficultyLevels_DifficultyLevelId",
                table: "Mcqs",
                column: "DifficultyLevelId",
                principalTable: "DifficultyLevels",
                principalColumn: "DifficultyLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mcqs_SubTopics_SubTopicId",
                table: "Mcqs",
                column: "SubTopicId",
                principalTable: "SubTopics",
                principalColumn: "SubTopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mcqs_Topics_TopicId",
                table: "Mcqs",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_AdminUsers_AddedBy",
                table: "Questions",
                column: "AddedBy",
                principalTable: "AdminUsers",
                principalColumn: "AdminUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Courses_CourseId",
                table: "Questions",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_DifficultyLevels_DifficultyLevelId",
                table: "Questions",
                column: "DifficultyLevelId",
                principalTable: "DifficultyLevels",
                principalColumn: "DifficultyLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_SubTopics_SubTopicId",
                table: "Questions",
                column: "SubTopicId",
                principalTable: "SubTopics",
                principalColumn: "SubTopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Topics_TopicId",
                table: "Questions",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubTopics_Courses_CourseId",
                table: "SubTopics",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubTopics_DifficultyLevels_DifficultyLevelId",
                table: "SubTopics",
                column: "DifficultyLevelId",
                principalTable: "DifficultyLevels",
                principalColumn: "DifficultyLevelId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubTopics_Topics_TopicId",
                table: "SubTopics",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "TopicId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_Courses_CourseId",
                table: "Topics",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_DifficultyLevels_DifficultyLevelId",
                table: "Topics",
                column: "DifficultyLevelId",
                principalTable: "DifficultyLevels",
                principalColumn: "DifficultyLevelId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Branches_BranchId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_CourseTypes_CourseTypeId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Mcqs_AdminUsers_AddedBy",
                table: "Mcqs");

            migrationBuilder.DropForeignKey(
                name: "FK_Mcqs_Courses_CourseId",
                table: "Mcqs");

            migrationBuilder.DropForeignKey(
                name: "FK_Mcqs_DifficultyLevels_DifficultyLevelId",
                table: "Mcqs");

            migrationBuilder.DropForeignKey(
                name: "FK_Mcqs_SubTopics_SubTopicId",
                table: "Mcqs");

            migrationBuilder.DropForeignKey(
                name: "FK_Mcqs_Topics_TopicId",
                table: "Mcqs");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_AdminUsers_AddedBy",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Courses_CourseId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_DifficultyLevels_DifficultyLevelId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_SubTopics_SubTopicId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Topics_TopicId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_SubTopics_Courses_CourseId",
                table: "SubTopics");

            migrationBuilder.DropForeignKey(
                name: "FK_SubTopics_DifficultyLevels_DifficultyLevelId",
                table: "SubTopics");

            migrationBuilder.DropForeignKey(
                name: "FK_SubTopics_Topics_TopicId",
                table: "SubTopics");

            migrationBuilder.DropForeignKey(
                name: "FK_Topics_Courses_CourseId",
                table: "Topics");

            migrationBuilder.DropForeignKey(
                name: "FK_Topics_DifficultyLevels_DifficultyLevelId",
                table: "Topics");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Topics_CourseId",
                table: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_Topics_DifficultyLevelId",
                table: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_SubTopics_CourseId",
                table: "SubTopics");

            migrationBuilder.DropIndex(
                name: "IX_SubTopics_DifficultyLevelId",
                table: "SubTopics");

            migrationBuilder.DropIndex(
                name: "IX_SubTopics_TopicId",
                table: "SubTopics");

            migrationBuilder.DropIndex(
                name: "IX_Questions_AddedBy",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_CourseId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_DifficultyLevelId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_SubTopicId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_TopicId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Mcqs_AddedBy",
                table: "Mcqs");

            migrationBuilder.DropIndex(
                name: "IX_Mcqs_CourseId",
                table: "Mcqs");

            migrationBuilder.DropIndex(
                name: "IX_Mcqs_DifficultyLevelId",
                table: "Mcqs");

            migrationBuilder.DropIndex(
                name: "IX_Mcqs_SubTopicId",
                table: "Mcqs");

            migrationBuilder.DropIndex(
                name: "IX_Mcqs_TopicId",
                table: "Mcqs");

            migrationBuilder.DropIndex(
                name: "IX_Courses_BranchId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_CourseTypeId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "SubTopics");

            migrationBuilder.DropColumn(
                name: "DifficultyLevelId",
                table: "SubTopics");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "SubTopics");

            migrationBuilder.DropColumn(
                name: "AddedBy",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "ApproveStatus",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "DifficultyLevelId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "TechStack",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "AddedBy",
                table: "Mcqs");

            migrationBuilder.DropColumn(
                name: "AnswerDescription",
                table: "Mcqs");

            migrationBuilder.DropColumn(
                name: "ApproveStatus",
                table: "Mcqs");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Mcqs");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Mcqs");

            migrationBuilder.DropColumn(
                name: "DifficultyLevelId",
                table: "Mcqs");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Mcqs");

            migrationBuilder.DropColumn(
                name: "TechStack",
                table: "Mcqs");

            migrationBuilder.DropColumn(
                name: "ApproveStatus",
                table: "AdminUsers");

            migrationBuilder.DropColumn(
                name: "CollegeName",
                table: "AdminUsers");

            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "AdminUsers");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "AdminUsers");

            migrationBuilder.DropColumn(
                name: "TokenExpiryTime",
                table: "AdminUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "SubTopics",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "QuestionAnswer",
                table: "Questions",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Question",
                table: "Questions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
