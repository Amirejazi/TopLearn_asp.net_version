using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopLearn.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class Course_mig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseLevels",
                columns: table => new
                {
                    LevelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LevelTitle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseLevels", x => x.LevelId);
                });

            migrationBuilder.CreateTable(
                name: "CourseStatus",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusTitle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseStatus", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    SubGroupId = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    LevelId = table.Column<int>(type: "int", nullable: false),
                    CourseTitle = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoursePrice = table.Column<int>(type: "int", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: false),
                    CourseImageName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DemoFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseStatusStatusId = table.Column<int>(type: "int", nullable: false),
                    CourseLevelLevelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.CourseId);
                    table.ForeignKey(
                        name: "FK_Courses_CourseGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "CourseGroups",
                        principalColumn: "GroupId");
                    table.ForeignKey(
                        name: "FK_Courses_CourseGroups_SubGroupId",
                        column: x => x.SubGroupId,
                        principalTable: "CourseGroups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Courses_CourseLevels_CourseLevelLevelId",
                        column: x => x.CourseLevelLevelId,
                        principalTable: "CourseLevels",
                        principalColumn: "LevelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Courses_CourseStatus_CourseStatusStatusId",
                        column: x => x.CourseStatusStatusId,
                        principalTable: "CourseStatus",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Courses_Users_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseEpisodes",
                columns: table => new
                {
                    EpisodeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    EpisodeTitle = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    EpisodeTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EpisodeFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsFree = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseEpisodes", x => x.EpisodeId);
                    table.ForeignKey(
                        name: "FK_CourseEpisodes_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseEpisodes_CourseId",
                table: "CourseEpisodes",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseLevelLevelId",
                table: "Courses",
                column: "CourseLevelLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseStatusStatusId",
                table: "Courses",
                column: "CourseStatusStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_GroupId",
                table: "Courses",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_SubGroupId",
                table: "Courses",
                column: "SubGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TeacherId",
                table: "Courses",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseEpisodes");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "CourseLevels");

            migrationBuilder.DropTable(
                name: "CourseStatus");
        }
    }
}
