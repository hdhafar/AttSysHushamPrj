using Microsoft.EntityFrameworkCore.Migrations;

namespace AttSysHushamPrj.Data.Migrations
{
    public partial class updateEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreateBy",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEarly",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsLate",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Justification",
                table: "Event",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "IsEarly",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "IsLate",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "Justification",
                table: "Event");
        }
    }
}
