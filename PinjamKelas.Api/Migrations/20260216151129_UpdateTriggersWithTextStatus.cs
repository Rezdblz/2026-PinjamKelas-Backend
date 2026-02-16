using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PinjamKelas.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTriggersWithTextStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop old triggers and functions
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_classroom_status_change ON classroom;");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS log_classroom_status_change();");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_post_status_to_classroom ON posts;");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS update_classroom_status_on_time();");

            // Create new function with text conversion for classroom status changes
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION log_classroom_status_change()
                RETURNS TRIGGER AS $$
                DECLARE
                    old_status_text VARCHAR;
                    new_status_text VARCHAR;
                BEGIN
                    IF NEW.status <> OLD.status THEN
                        old_status_text := CASE OLD.status
                            WHEN 0 THEN 'Available'
                            WHEN 1 THEN 'Unavailable'
                            WHEN 2 THEN 'Maintenance'
                            ELSE 'Unknown'
                        END;

                        new_status_text := CASE NEW.status
                            WHEN 0 THEN 'Available'
                            WHEN 1 THEN 'Unavailable'
                            WHEN 2 THEN 'Maintenance'
                            ELSE 'Unknown'
                        END;

                        INSERT INTO status_log (id_classroom, description, log_time, created_at)
                        VALUES (NEW.id, 'Status changed from ' || old_status_text || ' to ' || new_status_text, NOW(), NOW());
                    END IF;
                    RETURN NEW;
                END;
                $$ LANGUAGE plpgsql;
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER trg_classroom_status_change
                AFTER UPDATE ON classroom
                FOR EACH ROW
                EXECUTE FUNCTION log_classroom_status_change();
            ");

            // Create new function with text conversion for post status to classroom
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION update_classroom_status_on_time()
                RETURNS TRIGGER AS $$
                DECLARE
                    new_status INT;
                    old_status INT;
                    new_status_text VARCHAR;
                    old_status_text VARCHAR;
                BEGIN
                    SELECT CASE 
                        WHEN EXISTS (
                            SELECT 1 FROM posts 
                            WHERE id_classroom = NEW.id_classroom
                            AND status = 1
                            AND start_time <= NOW()
                            AND end_time > NOW()
                            AND deleted_at IS NULL
                        ) THEN 1
                        ELSE 0
                    END INTO new_status;

                    SELECT status INTO old_status FROM classroom WHERE id = NEW.id_classroom;

                    new_status_text := CASE new_status
                        WHEN 0 THEN 'Available'
                        WHEN 1 THEN 'Unavailable'
                        WHEN 2 THEN 'Maintenance'
                        ELSE 'Unknown'
                    END;

                    old_status_text := CASE old_status
                        WHEN 0 THEN 'Available'
                        WHEN 1 THEN 'Unavailable'
                        WHEN 2 THEN 'Maintenance'
                        ELSE 'Unknown'
                    END;

                    UPDATE classroom
                    SET status = new_status
                    WHERE id = NEW.id_classroom;

                    INSERT INTO status_log (id_classroom, description, log_time, created_at)
                    VALUES (
                        NEW.id_classroom,
                        'Status changed from ' || old_status_text || ' to ' || new_status_text,
                        NOW(),
                        NOW()
                    );
                    
                    RETURN NEW;
                END;
                $$ LANGUAGE plpgsql;
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER trg_post_status_to_classroom
                AFTER INSERT OR UPDATE OF status, start_time, end_time ON posts
                FOR EACH ROW
                EXECUTE FUNCTION update_classroom_status_on_time();
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_classroom_status_change ON classroom;");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS log_classroom_status_change();");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_post_status_to_classroom ON posts;");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS update_classroom_status_on_time();");
        }
    }
}
