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
                BEGIN
                    IF NEW.status <> OLD.status THEN
                        INSERT INTO status_log (id_classroom, description, log_time)
                        VALUES (NEW.id, 'Status changed from ' || OLD.status || ' to ' || NEW.status, NOW());
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
