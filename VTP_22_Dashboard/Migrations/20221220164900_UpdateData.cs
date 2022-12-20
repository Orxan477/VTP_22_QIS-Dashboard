using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VTP_22_Dashboard.Migrations
{
    public partial class UpdateData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UniversitiesId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Universities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Universities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UniversitiesId",
                table: "AspNetUsers",
                column: "UniversitiesId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Universities_UniversitiesId",
                table: "AspNetUsers",
                column: "UniversitiesId",
                principalTable: "Universities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Universities_UniversitiesId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Universities");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UniversitiesId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UniversitiesId",
                table: "AspNetUsers");
        }
    }
}
