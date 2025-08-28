using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vneed.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeleteBehaviors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_demand_product_ProductId",
                table: "demand");

            migrationBuilder.DropForeignKey(
                name: "FK_demand_user_UserId",
                table: "demand");

            migrationBuilder.DropForeignKey(
                name: "FK_demand_status_history_demand_DemandId",
                table: "demand_status_history");

            migrationBuilder.DropForeignKey(
                name: "FK_product_suggestion_category_CategoryId",
                table: "product_suggestion");

            migrationBuilder.DropForeignKey(
                name: "FK_product_suggestion_user_UserId",
                table: "product_suggestion");

            migrationBuilder.DropForeignKey(
                name: "FK_user_mail_token_user_UserId",
                table: "user_mail_token");

            migrationBuilder.DropForeignKey(
                name: "FK_user_token_user_UserId",
                table: "user_token");

            migrationBuilder.AddForeignKey(
                name: "FK_demand_product_ProductId",
                table: "demand",
                column: "ProductId",
                principalTable: "product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_demand_user_UserId",
                table: "demand",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_demand_status_history_demand_DemandId",
                table: "demand_status_history",
                column: "DemandId",
                principalTable: "demand",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_product_suggestion_category_CategoryId",
                table: "product_suggestion",
                column: "CategoryId",
                principalTable: "category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_product_suggestion_user_UserId",
                table: "product_suggestion",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_user_mail_token_user_UserId",
                table: "user_mail_token",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_user_token_user_UserId",
                table: "user_token",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_demand_product_ProductId",
                table: "demand");

            migrationBuilder.DropForeignKey(
                name: "FK_demand_user_UserId",
                table: "demand");

            migrationBuilder.DropForeignKey(
                name: "FK_demand_status_history_demand_DemandId",
                table: "demand_status_history");

            migrationBuilder.DropForeignKey(
                name: "FK_product_suggestion_category_CategoryId",
                table: "product_suggestion");

            migrationBuilder.DropForeignKey(
                name: "FK_product_suggestion_user_UserId",
                table: "product_suggestion");

            migrationBuilder.DropForeignKey(
                name: "FK_user_mail_token_user_UserId",
                table: "user_mail_token");

            migrationBuilder.DropForeignKey(
                name: "FK_user_token_user_UserId",
                table: "user_token");

            migrationBuilder.AddForeignKey(
                name: "FK_demand_product_ProductId",
                table: "demand",
                column: "ProductId",
                principalTable: "product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_demand_user_UserId",
                table: "demand",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_demand_status_history_demand_DemandId",
                table: "demand_status_history",
                column: "DemandId",
                principalTable: "demand",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_product_suggestion_category_CategoryId",
                table: "product_suggestion",
                column: "CategoryId",
                principalTable: "category",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_product_suggestion_user_UserId",
                table: "product_suggestion",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_mail_token_user_UserId",
                table: "user_mail_token",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_token_user_UserId",
                table: "user_token",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
