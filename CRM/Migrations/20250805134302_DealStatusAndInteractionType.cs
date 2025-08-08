using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CRM.Migrations
{
    /// <inheritdoc />
    public partial class DealStatusAndInteractionType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Interactions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Deals");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Interactions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Deals",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DealStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InteractionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InteractionTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "DealStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "New" },
                    { 2, "InProgress" },
                    { 3, "Completed" }
                });

            migrationBuilder.InsertData(
                table: "InteractionTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Call" },
                    { 2, "Email" },
                    { 3, "Meeting" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Interactions_TypeId",
                table: "Interactions",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_StatusId",
                table: "Deals",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deals_DealStatuses_StatusId",
                table: "Deals",
                column: "StatusId",
                principalTable: "DealStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Interactions_InteractionTypes_TypeId",
                table: "Interactions",
                column: "TypeId",
                principalTable: "InteractionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deals_DealStatuses_StatusId",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_Interactions_InteractionTypes_TypeId",
                table: "Interactions");

            migrationBuilder.DropTable(
                name: "DealStatuses");

            migrationBuilder.DropTable(
                name: "InteractionTypes");

            migrationBuilder.DropIndex(
                name: "IX_Interactions_TypeId",
                table: "Interactions");

            migrationBuilder.DropIndex(
                name: "IX_Deals_StatusId",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Interactions");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Deals");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Interactions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Deals",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
