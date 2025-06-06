using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddedStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"
                CREATE OR REPLACE FUNCTION my_proc(
                    id integer
                ) RETURNS SETOF ""Courses""
                AS $$
                BEGIN
                    return query select * from ""Courses"" where ""Id"" = id;
                END;
                    $$
                LANGUAGE plpgsql;
                ";
            migrationBuilder.Sql(sp);
            var v = @"CREATE OR REPLACE VIEW my_view as select * from ""Lessons"";";
            migrationBuilder.Sql(v);
            //migrationBuilder.Sql("CREATE OR REPLACE TRIGGER [name] BEFORE UPDATE ON myTable ...");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sp = @"DROP FUNCTION my_proc;";
            migrationBuilder.Sql(sp);
            var v = @"DROP VIEW my_view;";
            migrationBuilder.Sql(v);
        }
    }
}
