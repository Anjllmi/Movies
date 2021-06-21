using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Movies.Migrations
{
    public partial class newtablecategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "category_foreign_key",
                table: "movies",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_movies_category_foreign_key",
                table: "movies",
                column: "category_foreign_key");

            migrationBuilder.AddForeignKey(
                name: "fk_movies_categories_category_foreign_key",
                table: "movies",
                column: "category_foreign_key",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_movies_categories_category_foreign_key",
                table: "movies");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropIndex(
                name: "ix_movies_category_foreign_key",
                table: "movies");

            migrationBuilder.DropColumn(
                name: "category_foreign_key",
                table: "movies");
        }
    }
}
