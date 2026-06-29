using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClientTelegram.Migrations
{
    /// <inheritdoc />
    public partial class EncryptMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Messages");

            migrationBuilder.AddColumn<byte[]>(
                name: "Ciphertext",
                table: "Messages",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<int>(
                name: "KeyId",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte[]>(
                name: "Nonce",
                table: "Messages",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "Tag",
                table: "Messages",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ciphertext",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "KeyId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Nonce",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Tag",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
