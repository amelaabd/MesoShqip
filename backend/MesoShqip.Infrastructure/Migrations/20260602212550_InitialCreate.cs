using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MesoShqip.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Badges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IconUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CriteriaJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Badges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TitleAlbanian = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TitleEnglish = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    LessonType = table.Column<int>(type: "int", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Parent"),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VocabularyItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LessonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WordAlbanian = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    WordEnglish = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ExampleSentence = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AudioFileUrl = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    PartOfSpeech = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phonetic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DifficultyScore = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VocabularyItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VocabularyItems_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChildProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AvatarCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "eagle"),
                    CurrentLevel = table.Column<int>(type: "int", nullable: false),
                    NativeLanguage = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "en"),
                    TotalPoints = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CurrentStreak = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    LastActivityDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                name: "AiStories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChildProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TitleAlbanian = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BodyAlbanian = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BodyTranslated = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VocabLevel = table.Column<int>(type: "int", nullable: false),
                    NewWordsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiStories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AiStories_ChildProfiles_ChildProfileId",
                        column: x => x.ChildProfileId,
                        principalTable: "ChildProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChildBadges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChildProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BadgeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EarnedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "LessonProgresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChildProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LessonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ScorePercent = table.Column<int>(type: "int", nullable: false),
                    AttemptsCount = table.Column<int>(type: "int", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonProgresses_ChildProfiles_ChildProfileId",
                        column: x => x.ChildProfileId,
                        principalTable: "ChildProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonProgresses_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PronunciationAttempts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChildProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VocabularyItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccuracyScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SttTranscript = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PronunciationAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PronunciationAttempts_ChildProfiles_ChildProfileId",
                        column: x => x.ChildProfileId,
                        principalTable: "ChildProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PronunciationAttempts_VocabularyItems_VocabularyItemId",
                        column: x => x.VocabularyItemId,
                        principalTable: "VocabularyItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChildProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalQuestions = table.Column<int>(type: "int", nullable: false),
                    CorrectAnswers = table.Column<int>(type: "int", nullable: false),
                    PointsEarned = table.Column<int>(type: "int", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizSessions_ChildProfiles_ChildProfileId",
                        column: x => x.ChildProfileId,
                        principalTable: "ChildProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AiStories_ChildProfileId",
                table: "AiStories",
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

            migrationBuilder.CreateIndex(
                name: "IX_LessonProgresses_ChildProfileId",
                table: "LessonProgresses",
                column: "ChildProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonProgresses_LessonId",
                table: "LessonProgresses",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_PronunciationAttempts_ChildProfileId",
                table: "PronunciationAttempts",
                column: "ChildProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_PronunciationAttempts_VocabularyItemId",
                table: "PronunciationAttempts",
                column: "VocabularyItemId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizSessions_ChildProfileId",
                table: "QuizSessions",
                column: "ChildProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VocabularyItems_LessonId",
                table: "VocabularyItems",
                column: "LessonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AiStories");

            migrationBuilder.DropTable(
                name: "ChildBadges");

            migrationBuilder.DropTable(
                name: "LessonProgresses");

            migrationBuilder.DropTable(
                name: "PronunciationAttempts");

            migrationBuilder.DropTable(
                name: "QuizSessions");

            migrationBuilder.DropTable(
                name: "Badges");

            migrationBuilder.DropTable(
                name: "VocabularyItems");

            migrationBuilder.DropTable(
                name: "ChildProfiles");

            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
