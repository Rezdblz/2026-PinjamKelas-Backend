using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PinjamKelas.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusLogTrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

                CREATE TRIGGER trg_classroom_status_change
                AFTER UPDATE ON classroom
                FOR EACH ROW
                EXECUTE FUNCTION log_classroom_status_change();
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_classroom_status_change ON classroom;");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS log_classroom_status_change();");
        }
    }
}
