using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressVoitures.Data.Migrations
{
    /// <inheritdoc />
    public partial class migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CarBrandId",
                table: "Car",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CarModelId",
                table: "Car",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CarModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Trim = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarModel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarRepair_IdCar",
                table: "CarRepair",
                column: "IdCar");

            migrationBuilder.CreateIndex(
                name: "IX_Car_CarBrandId",
                table: "Car",
                column: "CarBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Car_CarModelId",
                table: "Car",
                column: "CarModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Car_CarBrand_CarBrandId",
                table: "Car",
                column: "CarBrandId",
                principalTable: "CarBrand",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Car_CarModel_CarModelId",
                table: "Car",
                column: "CarModelId",
                principalTable: "CarModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarRepair_Car_IdCar",
                table: "CarRepair",
                column: "IdCar",
                principalTable: "Car",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Car_CarBrand_CarBrandId",
                table: "Car");

            migrationBuilder.DropForeignKey(
                name: "FK_Car_CarModel_CarModelId",
                table: "Car");

            migrationBuilder.DropForeignKey(
                name: "FK_CarRepair_Car_IdCar",
                table: "CarRepair");

            migrationBuilder.DropTable(
                name: "CarModel");

            migrationBuilder.DropIndex(
                name: "IX_CarRepair_IdCar",
                table: "CarRepair");

            migrationBuilder.DropIndex(
                name: "IX_Car_CarBrandId",
                table: "Car");

            migrationBuilder.DropIndex(
                name: "IX_Car_CarModelId",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "CarBrandId",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "CarModelId",
                table: "Car");
        }
    }
}
