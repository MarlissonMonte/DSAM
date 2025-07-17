using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace app.api.Migrations
{
    /// <inheritdoc />
    public partial class ScheduleSemData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Schedules_DoctorId_Date_StartTime",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Schedules");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_DoctorId",
                table: "Schedules",
                column: "DoctorId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Schedules_DoctorId",
                table: "Schedules");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Schedules",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_DoctorId_Date_StartTime",
                table: "Schedules",
                columns: new[] { "DoctorId", "Date", "StartTime" });
        }
    }
}
