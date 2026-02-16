using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PinjamKelas.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTriggerToHandlePostDeletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the old trigger that doesn't handle post deletion
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_post_status_to_classroom ON posts;");

            // Create updated trigger that listens for deleted_at changes
            migrationBuilder.Sql(@"
                CREATE TRIGGER trg_post_status_to_classroom
                AFTER INSERT OR UPDATE OF status, start_time, end_time, deleted_at ON posts
                FOR EACH ROW
                EXECUTE FUNCTION update_classroom_status_on_time();
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_post_status_to_classroom ON posts;");
            migrationBuilder.Sql(@"
                CREATE TRIGGER trg_post_status_to_classroom
                AFTER INSERT OR UPDATE OF status, start_time, end_time ON posts
                FOR EACH ROW
                EXECUTE FUNCTION update_classroom_status_on_time();
            ");
        }
    }
}
