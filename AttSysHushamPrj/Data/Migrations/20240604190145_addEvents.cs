using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AttSysHushamPrj.Data.Migrations
{
    public partial class addEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    EventID = table.Column<Guid>(nullable: false),
                    EventDate = table.Column<DateTime>(nullable: false),
                    EventTime = table.Column<TimeSpan>(nullable: false),
                    EventType = table.Column<int>(nullable: false),
                    BadgeNum = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.EventID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Event");
        }
    }
}
