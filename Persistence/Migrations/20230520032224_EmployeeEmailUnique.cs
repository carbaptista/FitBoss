using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeEmailUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Trainers",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Trainers",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseSalary",
                table: "Trainers",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Branch",
                table: "Trainers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "HiredDate",
                table: "Trainers",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Receptionists",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Receptionists",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseSalary",
                table: "Receptionists",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Branch",
                table: "Receptionists",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "HiredDate",
                table: "Receptionists",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Members",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Members",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Managers",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Managers",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseSalary",
                table: "Managers",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Branch",
                table: "Managers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "HiredDate",
                table: "Managers",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_Email",
                table: "Trainers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Receptionists_Email",
                table: "Receptionists",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Managers_Email",
                table: "Managers",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trainers_Email",
                table: "Trainers");

            migrationBuilder.DropIndex(
                name: "IX_Receptionists_Email",
                table: "Receptionists");

            migrationBuilder.DropIndex(
                name: "IX_Managers_Email",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "BaseSalary",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "Branch",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "HiredDate",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "BaseSalary",
                table: "Receptionists");

            migrationBuilder.DropColumn(
                name: "Branch",
                table: "Receptionists");

            migrationBuilder.DropColumn(
                name: "HiredDate",
                table: "Receptionists");

            migrationBuilder.DropColumn(
                name: "BaseSalary",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "Branch",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "HiredDate",
                table: "Managers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Trainers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Trainers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Receptionists",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Receptionists",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Members",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Members",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Managers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Managers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");
        }
    }
}
