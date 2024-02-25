using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pokemon.Infrastructure.Migrations
{
    public partial class firstpokeymigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "myPokemons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Total = table.Column<int>(type: "int", nullable: false),
                    HP = table.Column<int>(type: "int", nullable: false),
                    Attack = table.Column<int>(type: "int", nullable: false),
                    Defense = table.Column<int>(type: "int", nullable: false),
                    SpAtk = table.Column<int>(type: "int", nullable: false),
                    SpDef = table.Column<int>(type: "int", nullable: false),
                    Speed = table.Column<int>(type: "int", nullable: false),
                    Generation = table.Column<int>(type: "int", nullable: false),
                    Legendary = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_myPokemons", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "myPokemons",
                columns: new[] { "Id", "Attack", "Defense", "Generation", "HP", "Legendary", "Name", "SpAtk", "SpDef", "Speed", "Total", "Type1", "Type2" },
                values: new object[] { 1, 49, 49, 1, 45, true, "Bulbasaur", 65, 65, 45, 318, "Grass", "Poison" });

            migrationBuilder.InsertData(
                table: "myPokemons",
                columns: new[] { "Id", "Attack", "Defense", "Generation", "HP", "Legendary", "Name", "SpAtk", "SpDef", "Speed", "Total", "Type1", "Type2" },
                values: new object[] { 2, 62, 63, 1, 60, true, "Ivysaur", 80, 80, 60, 405, "Grass", "Poison" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "myPokemons");
        }
    }
}
