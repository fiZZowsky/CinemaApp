using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedIdFiledInTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeatTicket_Tickets_TicketsId",
                table: "SeatTicket");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tickets",
                table: "Tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SeatTicket",
                table: "SeatTicket");

            migrationBuilder.DropIndex(
                name: "IX_SeatTicket_TicketsId",
                table: "SeatTicket");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "TicketsId",
                table: "SeatTicket");

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TicketsGuid",
                table: "SeatTicket",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tickets",
                table: "Tickets",
                column: "Guid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeatTicket",
                table: "SeatTicket",
                columns: new[] { "SeatsId", "TicketsGuid" });

            migrationBuilder.CreateIndex(
                name: "IX_SeatTicket_TicketsGuid",
                table: "SeatTicket",
                column: "TicketsGuid");

            migrationBuilder.AddForeignKey(
                name: "FK_SeatTicket_Tickets_TicketsGuid",
                table: "SeatTicket",
                column: "TicketsGuid",
                principalTable: "Tickets",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeatTicket_Tickets_TicketsGuid",
                table: "SeatTicket");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tickets",
                table: "Tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SeatTicket",
                table: "SeatTicket");

            migrationBuilder.DropIndex(
                name: "IX_SeatTicket_TicketsGuid",
                table: "SeatTicket");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "TicketsGuid",
                table: "SeatTicket");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "TicketsId",
                table: "SeatTicket",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tickets",
                table: "Tickets",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeatTicket",
                table: "SeatTicket",
                columns: new[] { "SeatsId", "TicketsId" });

            migrationBuilder.CreateIndex(
                name: "IX_SeatTicket_TicketsId",
                table: "SeatTicket",
                column: "TicketsId");

            migrationBuilder.AddForeignKey(
                name: "FK_SeatTicket_Tickets_TicketsId",
                table: "SeatTicket",
                column: "TicketsId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
