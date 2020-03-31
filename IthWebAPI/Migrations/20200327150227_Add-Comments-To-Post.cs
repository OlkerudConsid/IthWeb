using Microsoft.EntityFrameworkCore.Migrations;

namespace IthWebAPI.Migrations
{
    public partial class AddCommentsToPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Comment_BlogPostId",
                table: "Comment",
                column: "BlogPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_BlogPost_BlogPostId",
                table: "Comment",
                column: "BlogPostId",
                principalTable: "BlogPost",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_BlogPost_BlogPostId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_BlogPostId",
                table: "Comment");
        }
    }
}
