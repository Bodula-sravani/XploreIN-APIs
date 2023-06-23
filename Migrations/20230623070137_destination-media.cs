using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XploreIN.Migrations
{
    /// <inheritdoc />
    public partial class destinationmedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "destinationMedias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Destination_id = table.Column<int>(type: "int", nullable: false),
                    Media_URL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created_At = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_destinationMedias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_destinationMedias_ruralDestinations_Destination_id",
                        column: x => x.Destination_id,
                        principalTable: "ruralDestinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_destinationMedias_Destination_id",
                table: "destinationMedias",
                column: "Destination_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "destinationMedias");
        }
    }
}
