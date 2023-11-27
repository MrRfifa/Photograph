using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class updatedImageModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_ImagesFiles_ImageFileId",
                table: "Images");

            migrationBuilder.AlterColumn<int>(
                name: "ImageFileId",
                table: "Images",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_ImagesFiles_ImageFileId",
                table: "Images",
                column: "ImageFileId",
                principalTable: "ImagesFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_ImagesFiles_ImageFileId",
                table: "Images");

            migrationBuilder.AlterColumn<int>(
                name: "ImageFileId",
                table: "Images",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_ImagesFiles_ImageFileId",
                table: "Images",
                column: "ImageFileId",
                principalTable: "ImagesFiles",
                principalColumn: "Id");
        }
    }
}
