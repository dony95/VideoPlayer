using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VideoPlayer.DAL.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cartoons",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Categories = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ImdbURL = table.Column<string>(nullable: true),
                    ImgURL = table.Column<string>(nullable: true),
                    Language = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Name_ENG = table.Column<string>(nullable: true),
                    SubtitleURL = table.Column<string>(nullable: true),
                    TrailerURL = table.Column<string>(nullable: true),
                    VideoURL = table.Column<string>(nullable: false),
                    Year = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cartoons", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Films",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Categories = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ImdbURL = table.Column<string>(nullable: true),
                    ImgURL = table.Column<string>(nullable: true),
                    Length = table.Column<int>(nullable: false),
                    ListActors = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Name_ENG = table.Column<string>(nullable: true),
                    SubtitleURL = table.Column<string>(nullable: true),
                    TrailerURL = table.Column<string>(nullable: true),
                    VideoURL = table.Column<string>(nullable: false),
                    Year = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Films", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Series",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Categories = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ImdbURL = table.Column<string>(nullable: true),
                    ImgURL = table.Column<string>(nullable: true),
                    Language = table.Column<int>(nullable: false),
                    ListActors = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Name_ENG = table.Column<string>(nullable: true),
                    TrailerURL = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Season",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    SeasonNumber = table.Column<int>(nullable: false),
                    SeriesId = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Season", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Season_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Episode",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    EpisodeNumber = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    SeasonId = table.Column<int>(nullable: false),
                    SubtitleURL = table.Column<string>(nullable: true),
                    VideoURL = table.Column<string>(nullable: false),
                    Year = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Episode", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Episode_Season_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Season",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Episode_SeasonId",
                table: "Episode",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Season_SeriesId",
                table: "Season",
                column: "SeriesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cartoons");

            migrationBuilder.DropTable(
                name: "Episode");

            migrationBuilder.DropTable(
                name: "Films");

            migrationBuilder.DropTable(
                name: "Season");

            migrationBuilder.DropTable(
                name: "Series");
        }
    }
}
