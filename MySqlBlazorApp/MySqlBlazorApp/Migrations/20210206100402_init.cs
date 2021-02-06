using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace MySqlBlazorApp.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Famille",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    NomDeFamille = table.Column<string>(nullable: true),
                    Qf = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Famille", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Enfant",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Nom = table.Column<string>(nullable: true),
                    Prenom = table.Column<string>(nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    FamilleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enfant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enfant_Famille_FamilleId",
                        column: x => x.FamilleId,
                        principalTable: "Famille",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parent",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Nom = table.Column<string>(nullable: true),
                    Prenom = table.Column<string>(nullable: true),
                    Adresse = table.Column<string>(nullable: true),
                    Cp = table.Column<string>(nullable: true),
                    Ville = table.Column<string>(nullable: true),
                    Mail = table.Column<string>(nullable: true),
                    Tel = table.Column<string>(nullable: true),
                    FamilleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parent_Famille_FamilleId",
                        column: x => x.FamilleId,
                        principalTable: "Famille",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Enfant_FamilleId",
                table: "Enfant",
                column: "FamilleId");

            migrationBuilder.CreateIndex(
                name: "IX_Parent_FamilleId",
                table: "Parent",
                column: "FamilleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enfant");

            migrationBuilder.DropTable(
                name: "Parent");

            migrationBuilder.DropTable(
                name: "Famille");
        }
    }
}
