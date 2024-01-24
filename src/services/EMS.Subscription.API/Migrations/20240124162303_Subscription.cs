using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMS.Subscription.API.Migrations;

/// <inheritdoc />
public partial class Subscription : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Plans",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Title = table.Column<string>(type: "varchar(250)", nullable: false),
                SubTitle = table.Column<string>(type: "varchar(400)", nullable: false),
                Price = table.Column<double>(type: "float", nullable: false),
                Benefits = table.Column<string>(type: "Text", nullable: false),
                IsActive = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Plans", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PlanUsers",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UserName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                UserEmail = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                UserCpf = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false),
                IsActive = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PlanUsers", x => x.Id);
                table.ForeignKey(
                    name: "FK_PlanUsers_Plans_PlanId",
                    column: x => x.PlanId,
                    principalTable: "Plans",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_PlanUsers_PlanId",
            table: "PlanUsers",
            column: "PlanId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "PlanUsers");

        migrationBuilder.DropTable(
            name: "Plans");
    }
}
