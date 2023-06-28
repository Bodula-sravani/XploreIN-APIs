using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XploreIN.Migrations
{
    /// <inheritdoc />
    public partial class Addpostmediatable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "postMedias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    Media_URL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created_At = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_postMedias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_postMedias_UserPosts_PostId",
                        column: x => x.PostId,
                        principalTable: "UserPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_postMedias_PostId",
                table: "postMedias",
                column: "PostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "postMedias");
        }
    }
}
