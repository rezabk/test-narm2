using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PracticeQuestionAnswerentityupdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileExtension",
                table: "PracticeQuestionAnswer");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "PracticeQuestionAnswer");

            migrationBuilder.AddColumn<double>(
                name: "Score",
                table: "PracticeQuestionAnswer",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "204f1f18-2468-4d9d-8aee-a0fc77bc3c6c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "a80b5847-5afa-437d-a4b7-025d172c1070");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "b65094ce-9a55-4880-b914-8b9120173666");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "PracticeQuestionAnswer");

            migrationBuilder.AddColumn<string>(
                name: "FileExtension",
                table: "PracticeQuestionAnswer",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "PracticeQuestionAnswer",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "2e5d53af-0823-42bc-ba52-d7a0737e60e0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "49eee04f-948f-46e9-a766-feef09edebb4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "093b78e6-fa13-4474-931b-3aa3e7f8c54d");
        }
    }
}
