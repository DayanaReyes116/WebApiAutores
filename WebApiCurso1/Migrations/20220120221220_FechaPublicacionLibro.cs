﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiAutores.Migrations
{
    public partial class FechaPublicacionLibro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AutoresLibros_AutorId",
                table: "AutoresLibros");

            migrationBuilder.DropIndex(
                name: "IX_AutoresLibros_LibroId",
                table: "AutoresLibros");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaPublicacion",
                table: "Libros",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AutoresLibros_LibroId",
                table: "AutoresLibros",
                column: "LibroId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AutoresLibros_LibroId",
                table: "AutoresLibros");

            migrationBuilder.DropColumn(
                name: "FechaPublicacion",
                table: "Libros");

            migrationBuilder.CreateIndex(
                name: "IX_AutoresLibros_AutorId",
                table: "AutoresLibros",
                column: "AutorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AutoresLibros_LibroId",
                table: "AutoresLibros",
                column: "LibroId",
                unique: true);
        }
    }
}
