using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YuLinTu.Practice.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PracticeAuthors",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    last_name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    birth_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    short_bio = table.Column<string>(type: "text", nullable: true),
                    extra_properties = table.Column<string>(type: "text", nullable: true),
                    concurrency_stamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: true),
                    last_modification_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modifier_id = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleter_id = table.Column<Guid>(type: "uuid", nullable: true),
                    deletion_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PracticeAuthors", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PracticeBooks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    author_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    publish_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    price = table.Column<float>(type: "real", nullable: false),
                    extra_properties = table.Column<string>(type: "text", nullable: true),
                    concurrency_stamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: true),
                    last_modification_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modifier_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PracticeBooks", x => x.id);
                    table.ForeignKey(
                        name: "FK_PracticeBooks_PracticeAuthors_author_id",
                        column: x => x.author_id,
                        principalTable: "PracticeAuthors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PracticeAuthors_first_name_last_name",
                table: "PracticeAuthors",
                columns: new[] { "first_name", "last_name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PracticeBooks_author_id",
                table: "PracticeBooks",
                column: "author_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PracticeBooks");

            migrationBuilder.DropTable(
                name: "PracticeAuthors");
        }
    }
}
