using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitSync.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsActiveFieldInWorkoutPlanEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "WorkoutPlans",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "WorkoutPlans");
        }
    }
}
