using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rBR.Scraper.Data.Internal.Migrations
{
    /// <inheritdoc />
    public partial class V_1_0_0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CommonScrapers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Created = table.Column<string>(type: "varchar(33)", nullable: false, defaultValueSql: "sysdate()")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Modified = table.Column<string>(type: "varchar(33)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Removed = table.Column<string>(type: "varchar(33)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DataId = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<string>(type: "varchar(33)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifiedAt = table.Column<string>(type: "varchar(33)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonScrapers", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ScrapersRuns",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Created = table.Column<string>(type: "varchar(33)", nullable: false, defaultValueSql: "sysdate()")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Modified = table.Column<string>(type: "varchar(33)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Removed = table.Column<string>(type: "varchar(33)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DataId = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataStatus = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DatasetId = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartedAt = table.Column<string>(type: "varchar(33)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FinishedAt = table.Column<string>(type: "varchar(33)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Imported = table.Column<ulong>(type: "bit", nullable: false),
                    ImportingError = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ScraperId = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScrapersRuns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScrapersRuns_CommonScrapers_ScraperId",
                        column: x => x.ScraperId,
                        principalTable: "CommonScrapers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InstagramProfileScraperDatasets",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Created = table.Column<string>(type: "varchar(33)", nullable: false, defaultValueSql: "sysdate()")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Modified = table.Column<string>(type: "varchar(33)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Removed = table.Column<string>(type: "varchar(33)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DataId = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Url = table.Column<string>(type: "varchar(300)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InputUrl = table.Column<string>(type: "varchar(300)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserName = table.Column<string>(type: "varchar(150)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FullName = table.Column<string>(type: "varchar(150)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FollowersCount = table.Column<int>(type: "int", nullable: true),
                    Verified = table.Column<ulong>(type: "bit", nullable: true),
                    Timestamp = table.Column<string>(type: "varchar(33)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RunId = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FullObject = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstagramProfileScraperDatasets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstagramProfileScraperDatasets_ScrapersRuns_RunId",
                        column: x => x.RunId,
                        principalTable: "ScrapersRuns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InstagramScraperDatasets",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Created = table.Column<string>(type: "varchar(33)", nullable: false, defaultValueSql: "sysdate()")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Modified = table.Column<string>(type: "varchar(33)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Removed = table.Column<string>(type: "varchar(33)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DataId = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Url = table.Column<string>(type: "varchar(300)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InputUrl = table.Column<string>(type: "varchar(300)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Timestamp = table.Column<string>(type: "varchar(33)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RunId = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InstagramProfileId = table.Column<string>(type: "varchar(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FullObject = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstagramScraperDatasets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstagramScraperDatasets_InstagramProfileScraperDatasets_Ins~",
                        column: x => x.InstagramProfileId,
                        principalTable: "InstagramProfileScraperDatasets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstagramScraperDatasets_ScrapersRuns_RunId",
                        column: x => x.RunId,
                        principalTable: "ScrapersRuns",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CommonScrapers_DataId",
                table: "CommonScrapers",
                column: "DataId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommonScrapers_Id",
                table: "CommonScrapers",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InstagramProfileScraperDatasets_Id",
                table: "InstagramProfileScraperDatasets",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InstagramProfileScraperDatasets_RunId",
                table: "InstagramProfileScraperDatasets",
                column: "RunId");

            migrationBuilder.CreateIndex(
                name: "IX_InstagramProfileScraperDatasets_UserName_InputUrl_DataId_Run~",
                table: "InstagramProfileScraperDatasets",
                columns: new[] { "UserName", "InputUrl", "DataId", "RunId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InstagramScraperDatasets_Id",
                table: "InstagramScraperDatasets",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InstagramScraperDatasets_InstagramProfileId",
                table: "InstagramScraperDatasets",
                column: "InstagramProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_InstagramScraperDatasets_RunId",
                table: "InstagramScraperDatasets",
                column: "RunId");

            migrationBuilder.CreateIndex(
                name: "IX_ScrapersRuns_DataId_DatasetId",
                table: "ScrapersRuns",
                columns: new[] { "DataId", "DatasetId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScrapersRuns_Id",
                table: "ScrapersRuns",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScrapersRuns_ScraperId",
                table: "ScrapersRuns",
                column: "ScraperId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstagramScraperDatasets");

            migrationBuilder.DropTable(
                name: "InstagramProfileScraperDatasets");

            migrationBuilder.DropTable(
                name: "ScrapersRuns");

            migrationBuilder.DropTable(
                name: "CommonScrapers");
        }
    }
}
