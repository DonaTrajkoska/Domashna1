using Microsoft.EntityFrameworkCore.Migrations;

namespace EShop.Repository.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BiletInShoppingCards",
                table: "BiletInShoppingCards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BiletInOrders",
                table: "BiletInOrders");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BiletInShoppingCards",
                table: "BiletInShoppingCards",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BiletInOrders",
                table: "BiletInOrders",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BiletInShoppingCards_BiletId",
                table: "BiletInShoppingCards",
                column: "BiletId");

            migrationBuilder.CreateIndex(
                name: "IX_BiletInOrders_BiletId",
                table: "BiletInOrders",
                column: "BiletId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BiletInShoppingCards",
                table: "BiletInShoppingCards");

            migrationBuilder.DropIndex(
                name: "IX_BiletInShoppingCards_BiletId",
                table: "BiletInShoppingCards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BiletInOrders",
                table: "BiletInOrders");

            migrationBuilder.DropIndex(
                name: "IX_BiletInOrders_BiletId",
                table: "BiletInOrders");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BiletInShoppingCards",
                table: "BiletInShoppingCards",
                columns: new[] { "BiletId", "ShoppingCartId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_BiletInOrders",
                table: "BiletInOrders",
                columns: new[] { "BiletId", "OrderId" });
        }
    }
}
