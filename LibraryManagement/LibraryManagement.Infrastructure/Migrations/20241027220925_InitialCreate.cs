using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagement.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ISBN = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Author = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TotalCopies = table.Column<int>(type: "int", nullable: false),
                    AvailableCopies = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BorrowingRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BorrowDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowingRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BorrowingRecords_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BorrowingRecords_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "AvailableCopies", "CreatedAt", "CreatedBy", "ISBN", "Status", "Title", "TotalCopies", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "Harper Lee", 2, new DateTime(2024, 10, 27, 22, 9, 25, 214, DateTimeKind.Utc).AddTicks(7537), "System", "9780061120084", 1, "To Kill a Mockingbird", 3, null, null },
                    { 2, "Jane Austen", 2, new DateTime(2024, 10, 27, 22, 9, 25, 214, DateTimeKind.Utc).AddTicks(7537), "System", "9780141439518", 2, "Pride and Prejudice", 2, null, null },
                    { 3, "F. Scott Fitzgerald", 4, new DateTime(2024, 10, 27, 22, 9, 25, 214, DateTimeKind.Utc).AddTicks(7537), "System", "9780743273565", 1, "The Great Gatsby", 4, null, null },
                    { 4, "George Orwell", 3, new DateTime(2024, 10, 27, 22, 9, 25, 214, DateTimeKind.Utc).AddTicks(7537), "System", "9780451524935", 1, "1984", 3, null, null },
                    { 5, "J.R.R. Tolkien", 2, new DateTime(2024, 10, 27, 22, 9, 25, 214, DateTimeKind.Utc).AddTicks(7537), "System", "9780618640157", 1, "The Lord of the Rings", 2, null, null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Email", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 10, 27, 22, 9, 25, 214, DateTimeKind.Utc).AddTicks(7537), "System", "john.doe@email.com", "John Doe", null, null },
                    { 2, new DateTime(2024, 10, 27, 22, 9, 25, 214, DateTimeKind.Utc).AddTicks(7537), "System", "jane.smith@email.com", "Jane Smith", null, null },
                    { 3, new DateTime(2024, 10, 27, 22, 9, 25, 214, DateTimeKind.Utc).AddTicks(7537), "System", "robert.johnson@email.com", "Robert Johnson", null, null },
                    { 4, new DateTime(2024, 10, 27, 22, 9, 25, 214, DateTimeKind.Utc).AddTicks(7537), "System", "maria.garcia@email.com", "Maria Garcia", null, null },
                    { 5, new DateTime(2024, 10, 27, 22, 9, 25, 214, DateTimeKind.Utc).AddTicks(7537), "System", "david.wilson@email.com", "David Wilson", null, null }
                });

            migrationBuilder.InsertData(
                table: "BorrowingRecords",
                columns: new[] { "Id", "BookId", "BorrowDate", "CreatedAt", "CreatedBy", "DueDate", "ReturnDate", "UpdatedAt", "UpdatedBy", "UserId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 10, 17, 22, 9, 25, 214, DateTimeKind.Utc).AddTicks(7537), new DateTime(2024, 10, 27, 22, 9, 25, 214, DateTimeKind.Utc).AddTicks(7537), "System", new DateTime(2024, 10, 31, 22, 9, 25, 214, DateTimeKind.Utc).AddTicks(7537), null, null, null, 1 },
                    { 2, 2, new DateTime(2024, 10, 22, 22, 9, 25, 214, DateTimeKind.Utc).AddTicks(7537), new DateTime(2024, 10, 27, 22, 9, 25, 214, DateTimeKind.Utc).AddTicks(7537), "System", new DateTime(2024, 11, 5, 22, 9, 25, 214, DateTimeKind.Utc).AddTicks(7537), null, null, null, 2 },
                    { 3, 3, new DateTime(2024, 10, 7, 22, 9, 25, 214, DateTimeKind.Utc).AddTicks(7537), new DateTime(2024, 10, 27, 22, 9, 25, 214, DateTimeKind.Utc).AddTicks(7537), "System", new DateTime(2024, 10, 21, 22, 9, 25, 214, DateTimeKind.Utc).AddTicks(7537), new DateTime(2024, 10, 12, 22, 9, 25, 214, DateTimeKind.Utc).AddTicks(7537), null, null, 3 },
                    { 4, 4, new DateTime(2024, 10, 12, 22, 9, 25, 214, DateTimeKind.Utc).AddTicks(7537), new DateTime(2024, 10, 27, 22, 9, 25, 214, DateTimeKind.Utc).AddTicks(7537), "System", new DateTime(2024, 10, 26, 22, 9, 25, 214, DateTimeKind.Utc).AddTicks(7537), new DateTime(2024, 10, 17, 22, 9, 25, 214, DateTimeKind.Utc).AddTicks(7537), null, null, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_ISBN",
                table: "Books",
                column: "ISBN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BorrowingRecords_BookId",
                table: "BorrowingRecords",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowingRecords_UserId",
                table: "BorrowingRecords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BorrowingRecords");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
