﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingCart_MVC.Migrations
{
    /// <inheritdoc />
    public partial class AddImagePathToItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Items",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Items");
        }
    }
}
