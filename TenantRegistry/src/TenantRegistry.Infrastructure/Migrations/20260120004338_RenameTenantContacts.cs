using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantRegistry.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameTenantContacts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactPersons_Tenants_TenantId",
                table: "ContactPersons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactPersons",
                table: "ContactPersons");

            migrationBuilder.RenameTable(
                name: "ContactPersons",
                newName: "TenantContact");

            migrationBuilder.RenameIndex(
                name: "IX_ContactPersons_TenantId",
                table: "TenantContact",
                newName: "IX_TenantContact_TenantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TenantContact",
                table: "TenantContact",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TenantContact_Tenants_TenantId",
                table: "TenantContact",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TenantContact_Tenants_TenantId",
                table: "TenantContact");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TenantContact",
                table: "TenantContact");

            migrationBuilder.RenameTable(
                name: "TenantContact",
                newName: "ContactPersons");

            migrationBuilder.RenameIndex(
                name: "IX_TenantContact_TenantId",
                table: "ContactPersons",
                newName: "IX_ContactPersons_TenantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactPersons",
                table: "ContactPersons",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactPersons_Tenants_TenantId",
                table: "ContactPersons",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
