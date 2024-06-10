using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Vista.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    category_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.category_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    bio = table.Column<string>(type: "text", nullable: true),
                    followers = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    following = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    profile_pic_url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "followers",
                columns: table => new
                {
                    followed_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    follower_user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_followers", x => new { x.followed_user_id, x.follower_user_id });
                    table.ForeignKey(
                        name: "FK_followers_users_followed_user_id",
                        column: x => x.followed_user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_followers_users_follower_user_id",
                        column: x => x.follower_user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "videos",
                columns: table => new
                {
                    video_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    video_name = table.Column<string>(type: "text", nullable: false),
                    video_length = table.Column<string>(type: "text", nullable: false),
                    video_url = table.Column<string>(type: "text", nullable: false),
                    video_thumbnail_url = table.Column<string>(type: "text", nullable: false),
                    video_description = table.Column<string>(type: "text", nullable: false),
                    likes = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    dislikes = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    views = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    upload_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "Now()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_videos", x => x.video_id);
                    table.ForeignKey(
                        name: "FK_videos_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "category_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_videos_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    comment_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    content = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "Now()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    video_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comments", x => x.comment_id);
                    table.ForeignKey(
                        name: "FK_comments_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_comments_videos_video_id",
                        column: x => x.video_id,
                        principalTable: "videos",
                        principalColumn: "video_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "disliked_video",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    video_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_disliked_video", x => new { x.user_id, x.video_id });
                    table.ForeignKey(
                        name: "FK_disliked_video_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_disliked_video_videos_video_id",
                        column: x => x.video_id,
                        principalTable: "videos",
                        principalColumn: "video_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "liked_video",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    video_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_liked_video", x => new { x.user_id, x.video_id });
                    table.ForeignKey(
                        name: "FK_liked_video_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_liked_video_videos_video_id",
                        column: x => x.video_id,
                        principalTable: "videos",
                        principalColumn: "video_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "watched_videos",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    video_id = table.Column<Guid>(type: "uuid", nullable: false),
                    viewed_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "Now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_watched_videos", x => new { x.user_id, x.video_id });
                    table.ForeignKey(
                        name: "FK_watched_videos_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_watched_videos_videos_video_id",
                        column: x => x.video_id,
                        principalTable: "videos",
                        principalColumn: "video_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "category_id", "category_name" },
                values: new object[,]
                {
                    { 1, "Sports" },
                    { 2, "Photography" },
                    { 3, "Travel" },
                    { 4, "Cooking" },
                    { 5, "Movies" },
                    { 6, "Music" },
                    { 7, "Art" },
                    { 8, "Reading" },
                    { 9, "Writing" },
                    { 10, "Dancing" },
                    { 11, "Gaming" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_comments_user_id",
                table: "comments",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_comments_video_id",
                table: "comments",
                column: "video_id");

            migrationBuilder.CreateIndex(
                name: "IX_disliked_video_video_id",
                table: "disliked_video",
                column: "video_id");

            migrationBuilder.CreateIndex(
                name: "IX_followers_follower_user_id",
                table: "followers",
                column: "follower_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_liked_video_video_id",
                table: "liked_video",
                column: "video_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_username",
                table: "users",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_videos_category_id",
                table: "videos",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_videos_user_id",
                table: "videos",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_watched_videos_video_id",
                table: "watched_videos",
                column: "video_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.DropTable(
                name: "disliked_video");

            migrationBuilder.DropTable(
                name: "followers");

            migrationBuilder.DropTable(
                name: "liked_video");

            migrationBuilder.DropTable(
                name: "watched_videos");

            migrationBuilder.DropTable(
                name: "videos");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
