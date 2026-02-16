using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PinjamKelas.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddClassroomTimeBasedStatusTrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION update_classroom_status_on_time()
                RETURNS TRIGGER AS $$
                BEGIN
                    -- Update classroom status based on active approved posts
                    UPDATE classroom
                    SET status = CASE 
                        WHEN EXISTS (
                            SELECT 1 FROM posts 
                            WHERE id_classroom = NEW.id_classroom
                            AND status = 1
                            AND start_time <= NOW()
                            AND end_time > NOW()
                            AND deleted_at IS NULL
                        ) THEN 1  -- Unavailable (being used)
                        ELSE 0    -- Available
                    END
                    WHERE id = NEW.id_classroom;
                    
                    RETURN NEW;
                END;
                $$ LANGUAGE plpgsql;

                CREATE TRIGGER trg_post_status_to_classroom
                AFTER INSERT OR UPDATE OF status, start_time, end_time ON posts
                FOR EACH ROW
                EXECUTE FUNCTION update_classroom_status_on_time();
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_post_status_to_classroom ON posts;");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS update_classroom_status_on_time();");
        }
    }
}
