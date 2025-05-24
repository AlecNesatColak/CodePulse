using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodePulse.API.Migrations
{
    /// <inheritdoc />
    public partial class BlogPostandCategoryRelationshiptypofixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPostCategory_BlogPosts_blogPostsId",
                table: "BlogPostCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogPostCategory_Categories_categoriesId",
                table: "BlogPostCategory");

            migrationBuilder.RenameColumn(
                name: "categoriesId",
                table: "BlogPostCategory",
                newName: "CategoriesId");

            migrationBuilder.RenameColumn(
                name: "blogPostsId",
                table: "BlogPostCategory",
                newName: "BlogPostsId");

            migrationBuilder.RenameIndex(
                name: "IX_BlogPostCategory_categoriesId",
                table: "BlogPostCategory",
                newName: "IX_BlogPostCategory_CategoriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPostCategory_BlogPosts_BlogPostsId",
                table: "BlogPostCategory",
                column: "BlogPostsId",
                principalTable: "BlogPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPostCategory_Categories_CategoriesId",
                table: "BlogPostCategory",
                column: "CategoriesId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPostCategory_BlogPosts_BlogPostsId",
                table: "BlogPostCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogPostCategory_Categories_CategoriesId",
                table: "BlogPostCategory");

            migrationBuilder.RenameColumn(
                name: "CategoriesId",
                table: "BlogPostCategory",
                newName: "categoriesId");

            migrationBuilder.RenameColumn(
                name: "BlogPostsId",
                table: "BlogPostCategory",
                newName: "blogPostsId");

            migrationBuilder.RenameIndex(
                name: "IX_BlogPostCategory_CategoriesId",
                table: "BlogPostCategory",
                newName: "IX_BlogPostCategory_categoriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPostCategory_BlogPosts_blogPostsId",
                table: "BlogPostCategory",
                column: "blogPostsId",
                principalTable: "BlogPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPostCategory_Categories_categoriesId",
                table: "BlogPostCategory",
                column: "categoriesId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
