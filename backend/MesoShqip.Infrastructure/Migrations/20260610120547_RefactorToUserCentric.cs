using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MesoShqip.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorToUserCentric : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AiStories_ChildProfiles_ChildProfileId",
                table: "AiStories");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonProgresses_ChildProfiles_ChildProfileId",
                table: "LessonProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_PronunciationAttempts_ChildProfiles_ChildProfileId",
                table: "PronunciationAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizSessions_ChildProfiles_ChildProfileId",
                table: "QuizSessions");

            migrationBuilder.DropTable(
                name: "ChildBadges");

            migrationBuilder.DropTable(
                name: "ChildProfiles");

            migrationBuilder.DropIndex(
                name: "IX_LessonProgresses_ChildProfileId",
                table: "LessonProgresses");

            migrationBuilder.RenameColumn(
                name: "ChildProfileId",
                table: "QuizSessions",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_QuizSessions_ChildProfileId",
                table: "QuizSessions",
                newName: "IX_QuizSessions_UserId");

            migrationBuilder.RenameColumn(
                name: "ChildProfileId",
                table: "PronunciationAttempts",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PronunciationAttempts_ChildProfileId",
                table: "PronunciationAttempts",
                newName: "IX_PronunciationAttempts_UserId");

            migrationBuilder.RenameColumn(
                name: "ChildProfileId",
                table: "LessonProgresses",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ChildProfileId",
                table: "AiStories",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AiStories_ChildProfileId",
                table: "AiStories",
                newName: "IX_AiStories_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "User",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "Parent");

            migrationBuilder.AddColumn<int>(
                name: "CurrentStreak",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastActivityDate",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NativeLanguage",
                table: "Users",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "en");

            migrationBuilder.AddColumn<bool>(
                name: "OnboardingCompleted",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TotalPoints",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ScorePercent",
                table: "LessonProgresses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AttemptsCount",
                table: "LessonProgresses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "UserBadges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BadgeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EarnedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBadges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBadges_Badges_BadgeId",
                        column: x => x.BadgeId,
                        principalTable: "Badges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBadges_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessonProgresses_UserId_LessonId",
                table: "LessonProgresses",
                columns: new[] { "UserId", "LessonId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserBadges_BadgeId",
                table: "UserBadges",
                column: "BadgeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBadges_UserId_BadgeId",
                table: "UserBadges",
                columns: new[] { "UserId", "BadgeId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AiStories_Users_UserId",
                table: "AiStories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LessonProgresses_Users_UserId",
                table: "LessonProgresses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PronunciationAttempts_Users_UserId",
                table: "PronunciationAttempts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizSessions_Users_UserId",
                table: "QuizSessions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AiStories_Users_UserId",
                table: "AiStories");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonProgresses_Users_UserId",
                table: "LessonProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_PronunciationAttempts_Users_UserId",
                table: "PronunciationAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizSessions_Users_UserId",
                table: "QuizSessions");

            migrationBuilder.DropTable(
                name: "UserBadges");

            migrationBuilder.DropIndex(
                name: "IX_LessonProgresses_UserId_LessonId",
                table: "LessonProgresses");

            migrationBuilder.DropColumn(
                name: "CurrentStreak",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastActivityDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NativeLanguage",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OnboardingCompleted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TotalPoints",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "QuizSessions",
                newName: "ChildProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_QuizSessions_UserId",
                table: "QuizSessions",
                newName: "IX_QuizSessions_ChildProfileId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "PronunciationAttempts",
                newName: "ChildProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_PronunciationAttempts_UserId",
                table: "PronunciationAttempts",
                newName: "IX_PronunciationAttempts_ChildProfileId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "LessonProgresses",
                newName: "ChildProfileId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "AiStories",
                newName: "ChildProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_AiStories_UserId",
                table: "AiStories",
                newName: "IX_AiStories_ChildProfileId");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Parent",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "User");

            migrationBuilder.AlterColumn<int>(
                name: "ScorePercent",
                table: "LessonProgresses",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "AttemptsCount",
                table: "LessonProgresses",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ChildProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AvatarCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "eagle"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrentLevel = table.Column<int>(type: "int", nullable: false),
                    CurrentStreak = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DisplayName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastActivityDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NativeLanguage = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "en"),
                    TotalPoints = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChildProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChildProfiles_Users_ParentUserId",
                        column: x => x.ParentUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChildBadges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BadgeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChildProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EarnedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChildBadges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChildBadges_Badges_BadgeId",
                        column: x => x.BadgeId,
                        principalTable: "Badges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChildBadges_ChildProfiles_ChildProfileId",
                        column: x => x.ChildProfileId,
                        principalTable: "ChildProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessonProgresses_ChildProfileId",
                table: "LessonProgresses",
                column: "ChildProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ChildBadges_BadgeId",
                table: "ChildBadges",
                column: "BadgeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChildBadges_ChildProfileId",
                table: "ChildBadges",
                column: "ChildProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ChildProfiles_ParentUserId",
                table: "ChildProfiles",
                column: "ParentUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AiStories_ChildProfiles_ChildProfileId",
                table: "AiStories",
                column: "ChildProfileId",
                principalTable: "ChildProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LessonProgresses_ChildProfiles_ChildProfileId",
                table: "LessonProgresses",
                column: "ChildProfileId",
                principalTable: "ChildProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PronunciationAttempts_ChildProfiles_ChildProfileId",
                table: "PronunciationAttempts",
                column: "ChildProfileId",
                principalTable: "ChildProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizSessions_ChildProfiles_ChildProfileId",
                table: "QuizSessions",
                column: "ChildProfileId",
                principalTable: "ChildProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
