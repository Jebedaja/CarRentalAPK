using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarRentalAPI.Migrations
{
    /// <inheritdoc />
    public partial class Bloby : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://carrentalapkstorage.blob.core.windows.net/obrazki/Corolla.jpg");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://carrentalapkstorage.blob.core.windows.net/obrazki/Octavia.jpg");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://carrentalapkstorage.blob.core.windows.net/obrazki/Golf.jpg");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "https://carrentalapkstorage.blob.core.windows.net/obrazki/bmw3.jpg");

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://carrentalapkstorage.blob.core.windows.net/obrazki/warszawa.jpg");

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://carrentalapkstorage.blob.core.windows.net/obrazki/krak%C3%B3w.jpg");

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://carrentalapkstorage.blob.core.windows.net/obrazki/Gdansk.jpg");

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "ImageUrl", "Name" },
                values: new object[] { 4, "https://carrentalapkstorage.blob.core.windows.net/obrazki/Mediolan.jpg", "Mediolan" });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "Brand", "CityId", "ImageUrl", "IsAvailable", "Model", "PricePerDay", "Year" },
                values: new object[,]
                {
                    { 5, "Fiat", 4, "https://carrentalapkstorage.blob.core.windows.net/obrazki/Fiat.jpg", true, "500", 130.00m, 2022 },
                    { 6, "Setra", 4, "https://carrentalapkstorage.blob.core.windows.net/obrazki/autokar_mediolan.jpg", true, "Inter", 1500.00m, 2011 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://carrentalapkstorage.blob.core.windows.net/images/corolla.jpg");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://carrentalapkstorage.blob.core.windows.net/images/octavia.jpg");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://carrentalapkstorage.blob.core.windows.net/images/golf.jpg");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "https://carrentalapkstorage.blob.core.windows.net/images/bmw3.jpg");

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://carrentalapkstorage.blob.core.windows.net/images/warsaw.jpg");

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://carrentalapkstorage.blob.core.windows.net/images/krakow.jpg");

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://carrentalapkstorage.blob.core.windows.net/images/gdansk.jpg");
        }
    }
}
