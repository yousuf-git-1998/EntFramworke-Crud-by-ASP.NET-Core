using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mev_8.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ProjectType = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    ProjectCost = table.Column<decimal>(type: "money", nullable: false),
                    Picture = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectId);
                });

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    WorkerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkerName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.WorkerId);
                    table.ForeignKey(
                        name: "FK_Workers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });
            //1
            string sql = @"CREATE PROCEDURE InsertWorker
                 @n  NVARCHAR(40),
                 @p NVARCHAR(40),
                 @pid INT
             AS
                 INSERT INTO Workers (WorkerName, PhoneNumber, ProjectId) VALUES (@n, @p, @pid)
             RETURN 0";
            migrationBuilder.Sql(sql);
            //2
            sql = @"CREATE PROCEDURE DeleteWorkersOfProject
                     @pid INT
                 AS
                     DELETE Workers WHERE ProjectId = @pid
                 RETURN 0";
            migrationBuilder.Sql(sql);
            //3
            sql = @"CREATE PROCEDURE DeleteProject
                     @pid INT
                 AS
                     DELETE Projects WHERE ProjectId = @pid
                 RETURN 0";
            migrationBuilder.Sql(sql);
            migrationBuilder.CreateIndex(
                name: "IX_Workers_ProjectId",
                table: "Workers",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Workers");

            migrationBuilder.DropTable(
                name: "Projects");
            migrationBuilder.Sql("DROP PROCEDURE InsertWorker");
            migrationBuilder.Sql("DROP PROCEDURE DeleteWorkersOfProject");
            migrationBuilder.Sql("DROP PROCEDURE DeleteProject");

        }
    }
}
