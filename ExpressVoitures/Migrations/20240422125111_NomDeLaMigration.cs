using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressVoitures.Migrations
{
    /// <inheritdoc />
    public partial class addCodeVIN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodeVIN",
                table: "Car",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeVIN",
                table: "Car");
        }
    }
}
