using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    public partial class renamed_iotdevices_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IotDeviceEntity_Users_UserEntityId",
                table: "IotDeviceEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IotDeviceEntity",
                table: "IotDeviceEntity");

            migrationBuilder.RenameTable(
                name: "IotDeviceEntity",
                newName: "IotDevices");

            migrationBuilder.RenameIndex(
                name: "IX_IotDeviceEntity_UserEntityId",
                table: "IotDevices",
                newName: "IX_IotDevices_UserEntityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IotDevices",
                table: "IotDevices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IotDevices_Users_UserEntityId",
                table: "IotDevices",
                column: "UserEntityId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IotDevices_Users_UserEntityId",
                table: "IotDevices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IotDevices",
                table: "IotDevices");

            migrationBuilder.RenameTable(
                name: "IotDevices",
                newName: "IotDeviceEntity");

            migrationBuilder.RenameIndex(
                name: "IX_IotDevices_UserEntityId",
                table: "IotDeviceEntity",
                newName: "IX_IotDeviceEntity_UserEntityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IotDeviceEntity",
                table: "IotDeviceEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IotDeviceEntity_Users_UserEntityId",
                table: "IotDeviceEntity",
                column: "UserEntityId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
