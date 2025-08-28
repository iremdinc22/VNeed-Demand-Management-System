using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vneed.Data.Migrations
{
    /// <inheritdoc />
    public partial class SoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "user",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "role",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "role",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "product_suggestion",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "product_suggestion",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "product",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "demand",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "demand",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "category",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DeletedAt", "IsActive" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DeletedAt", "IsActive" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DeletedAt", "IsActive" },
                values: new object[] { null, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "user");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "role");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "role");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "product_suggestion");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "product_suggestion");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "product");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "demand");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "demand");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "category");
        }
    }
}
