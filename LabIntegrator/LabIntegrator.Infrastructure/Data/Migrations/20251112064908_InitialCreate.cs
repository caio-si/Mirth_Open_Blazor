using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LabIntegrator.Infrastructure.Data.Migrations
{
    /// <summary>
    /// Migração inicial que cria as tabelas base do domínio (canais, mensagens e resultados).
    /// Mantida no repositório para permitir versionamento e fácil provisionamento do banco.
    /// </summary>
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LabChannels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabChannels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LabMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChannelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    PayloadRaw = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PayloadNormalized = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceivedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabMessages_LabChannels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "LabChannels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LabResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PatientName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TestCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TestDescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Units = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ReferenceRange = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ResultedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabResults_LabMessages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "LabMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LabMessages_ChannelId_ExternalId",
                table: "LabMessages",
                columns: new[] { "ChannelId", "ExternalId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LabResults_MessageId",
                table: "LabResults",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_LabResults_PatientId_TestCode_ResultedAt",
                table: "LabResults",
                columns: new[] { "PatientId", "TestCode", "ResultedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LabResults");

            migrationBuilder.DropTable(
                name: "LabMessages");

            migrationBuilder.DropTable(
                name: "LabChannels");
        }
    }
}
