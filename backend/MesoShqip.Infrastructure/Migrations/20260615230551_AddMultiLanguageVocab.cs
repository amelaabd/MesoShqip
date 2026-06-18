using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MesoShqip.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMultiLanguageVocab : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WordFrench",
                table: "VocabularyItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WordGerman",
                table: "VocabularyItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WordItalian",
                table: "VocabularyItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WordSwedish",
                table: "VocabularyItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WordTurkish",
                table: "VocabularyItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WordFrench",
                table: "VocabularyItems");

            migrationBuilder.DropColumn(
                name: "WordGerman",
                table: "VocabularyItems");

            migrationBuilder.DropColumn(
                name: "WordItalian",
                table: "VocabularyItems");

            migrationBuilder.DropColumn(
                name: "WordSwedish",
                table: "VocabularyItems");

            migrationBuilder.DropColumn(
                name: "WordTurkish",
                table: "VocabularyItems");
        }
    }
}
