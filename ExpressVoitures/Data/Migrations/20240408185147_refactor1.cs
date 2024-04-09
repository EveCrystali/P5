using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressVoitures.Data.Migrations
{
    /// <inheritdoc />
    public partial class refactor1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModelName",
                table: "CarModel",
                newName: "CarModelName");

            migrationBuilder.RenameColumn(
                name: "Brand",
                table: "CarBrand",
                newName: "CarBrandName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CarModelName",
                table: "CarModel",
                newName: "ModelName");

            migrationBuilder.RenameColumn(
                name: "CarBrandName",
                table: "CarBrand",
                newName: "Brand");
        }
    }
}
