using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETLWebApi.Migrations
{
    public partial class Renaming_And_Indexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "store_and_fwd_flag",
                table: "ETLDatas");

            migrationBuilder.RenameColumn(
                name: "trip_distance",
                table: "ETLDatas",
                newName: "TripDistance");

            migrationBuilder.RenameColumn(
                name: "tpep_pickup_datetime",
                table: "ETLDatas",
                newName: "PickupDatetime");

            migrationBuilder.RenameColumn(
                name: "tpep_dropoff_datetime",
                table: "ETLDatas",
                newName: "DropoffDatetime");

            migrationBuilder.RenameColumn(
                name: "tip_amount",
                table: "ETLDatas",
                newName: "TipAmount");

            migrationBuilder.RenameColumn(
                name: "passenger_count",
                table: "ETLDatas",
                newName: "PassengerCount");

            migrationBuilder.RenameColumn(
                name: "fare_amount",
                table: "ETLDatas",
                newName: "FareAmount");

            migrationBuilder.AddColumn<string>(
                name: "StoreAndFwdFlag",
                table: "ETLDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ETLDatas_DropoffDatetime",
                table: "ETLDatas",
                column: "DropoffDatetime");

            migrationBuilder.CreateIndex(
                name: "IX_ETLDatas_PickupDatetime",
                table: "ETLDatas",
                column: "PickupDatetime");

            migrationBuilder.CreateIndex(
                name: "IX_ETLDatas_PULocationID",
                table: "ETLDatas",
                column: "PULocationID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ETLDatas_DropoffDatetime",
                table: "ETLDatas");

            migrationBuilder.DropIndex(
                name: "IX_ETLDatas_PickupDatetime",
                table: "ETLDatas");

            migrationBuilder.DropIndex(
                name: "IX_ETLDatas_PULocationID",
                table: "ETLDatas");

            migrationBuilder.DropColumn(
                name: "StoreAndFwdFlag",
                table: "ETLDatas");

            migrationBuilder.RenameColumn(
                name: "TripDistance",
                table: "ETLDatas",
                newName: "trip_distance");

            migrationBuilder.RenameColumn(
                name: "TipAmount",
                table: "ETLDatas",
                newName: "tip_amount");

            migrationBuilder.RenameColumn(
                name: "PickupDatetime",
                table: "ETLDatas",
                newName: "tpep_pickup_datetime");

            migrationBuilder.RenameColumn(
                name: "PassengerCount",
                table: "ETLDatas",
                newName: "passenger_count");

            migrationBuilder.RenameColumn(
                name: "FareAmount",
                table: "ETLDatas",
                newName: "fare_amount");

            migrationBuilder.RenameColumn(
                name: "DropoffDatetime",
                table: "ETLDatas",
                newName: "tpep_dropoff_datetime");

            migrationBuilder.AddColumn<string>(
                name: "store_and_fwd_flag",
                table: "ETLDatas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
