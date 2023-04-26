using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tutor1.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "appSettings",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VARNAME = table.Column<string>(nullable: true),
                    INTVALUE = table.Column<int>(nullable: false),
                    TXTVALUE = table.Column<string>(nullable: true),
                    VARGROUP = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appSettings", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ClinicOrders",
                columns: table => new
                {
                    ClinicOrderId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    clinicOrder_seqid = table.Column<string>(nullable: true),
                    DateOfClinicOrder = table.Column<DateTime>(nullable: false),
                    seeDoctor = table.Column<bool>(nullable: false),
                    customer = table.Column<string>(maxLength: 30, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    LastUpdatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicOrders", x => x.ClinicOrderId);
                });

            //migrationBuilder.CreateTable(
            //    name: "FileOnFileSystemModels",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(nullable: true),
            //        FileType = table.Column<string>(nullable: true),
            //        Extension = table.Column<string>(nullable: true),
            //        Description = table.Column<string>(nullable: true),
            //        UploadedBy = table.Column<string>(nullable: true),
            //        CreatedOn = table.Column<DateTime>(nullable: true),
            //        FilePath = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_FileOnFileSystemModels", x => x.Id);
            //    });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    ProductID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.ProductID);
                });

            migrationBuilder.CreateTable(
                name: "ClinicOrderDetails",
                columns: table => new
                {
                    ClinicOrderDetailID = table.Column<int>(nullable: false),
                    ClinicOrderID = table.Column<int>(nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    ProductID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicOrderDetails", x => new { x.ClinicOrderID, x.ClinicOrderDetailID });
                    table.ForeignKey(
                        name: "FK_ClinicOrderDetails_ClinicOrders_ClinicOrderID",
                        column: x => x.ClinicOrderID,
                        principalTable: "ClinicOrders",
                        principalColumn: "ClinicOrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClinicOrderDetails_products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClinicOrderDetails_ProductID",
                table: "ClinicOrderDetails",
                column: "ProductID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "appSettings");

            migrationBuilder.DropTable(
                name: "ClinicOrderDetails");

            //migrationBuilder.DropTable(
            //    name: "FileOnFileSystemModels");

            migrationBuilder.DropTable(
                name: "ClinicOrders");

            migrationBuilder.DropTable(
                name: "products");
        }
    }
}
