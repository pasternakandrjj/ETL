using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETLWebApi.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ETLDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tpep_pickup_datetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    tpep_dropoff_datetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    passenger_count = table.Column<int>(type: "int", nullable: false),
                    trip_distance = table.Column<double>(type: "float", nullable: false),
                    store_and_fwd_flag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PULocationID = table.Column<int>(type: "int", nullable: false),
                    DOLocationID = table.Column<int>(type: "int", nullable: false),
                    fare_amount = table.Column<double>(type: "float", nullable: false),
                    tip_amount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ETLDatas", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ETLDatas");
        }
    }
}