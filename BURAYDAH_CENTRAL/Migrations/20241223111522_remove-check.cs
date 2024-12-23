using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BURAYDAH_CENTRAL.Migrations
{
    /// <inheritdoc />
    public partial class removecheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "checkedd",
                table: "PathologyAnalysis");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "checkedd",
                table: "PathologyAnalysis",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
