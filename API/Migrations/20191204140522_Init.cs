using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Credentials",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    UserType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credentials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Families",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Families", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    RecipeDetails = table.Column<string>(nullable: false),
                    FamilyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recipes_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Families",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CredentialId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    ProfilePicturePath = table.Column<string>(nullable: true),
                    ProfileColor = table.Column<string>(nullable: false, defaultValue: "#808080"),
                    FamilyId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Credentials_CredentialId",
                        column: x => x.CredentialId,
                        principalTable: "Credentials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Families",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 2400, nullable: true, defaultValue: ""),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    AllDay = table.Column<bool>(nullable: false),
                    RepeatDetailsId = table.Column<Guid>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    Alert = table.Column<DateTime>(nullable: true),
                    OwnerId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventRepeatDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Frequency = table.Column<int>(nullable: false),
                    WeekDays = table.Column<string>(nullable: true),
                    EndRepeat = table.Column<DateTime>(nullable: true),
                    Exceptions = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventRepeatDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventRepeatDetails_Events_Id",
                        column: x => x.Id,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserEvent",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    EventId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEvent", x => new { x.UserId, x.EventId });
                    table.ForeignKey(
                        name: "FK_UserEvent_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserEvent_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Credentials_Email",
                table: "Credentials",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_OwnerId",
                table: "Events",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_FamilyId",
                table: "Recipes",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEvent_EventId",
                table: "UserEvent",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CredentialId",
                table: "Users",
                column: "CredentialId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FamilyId",
                table: "Users",
                column: "FamilyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventRepeatDetails");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "UserEvent");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Credentials");

            migrationBuilder.DropTable(
                name: "Families");
        }
    }
}
