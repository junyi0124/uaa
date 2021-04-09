using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UAA.AuthApi.Data.MysqlMigrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_auth_users",
                columns: table => new
                {
                    id = table.Column<byte[]>(type: "binary(16)", nullable: false, defaultValueSql: "(uuid_to_bin(uuid()))"),
                    username = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    display = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    email = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    pwd_hash = table.Column<byte[]>(type: "longblob", nullable: true),
                    pwd_salt = table.Column<byte[]>(type: "longblob", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    created = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP()"),
                    lastupdate = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP() ON UPDATE CURRENT_TIMESTAMP()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_auth_users", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_auth_users_username",
                table: "t_auth_users",
                column: "username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_auth_users");
        }
    }
}
