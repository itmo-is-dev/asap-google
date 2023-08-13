using FluentMigrator;
using Itmo.Dev.Platform.Postgres.Migrations;

namespace Itmo.Dev.Asap.Google.DataAccess.Migrations;

#pragma warning disable SA1649

[Migration(1690997008, "initial")]
public class Initial : SqlMigration
{
    protected override string GetUpSql(IServiceProvider serviceProvider)
    {
        return """
        create table subject_courses
        (
            subject_course_id uuid not null primary key,
            subject_course_spreadsheet_id text not null
        );
        """;
    }

    protected override string GetDownSql(IServiceProvider serviceProvider)
    {
        return """
        drop table subject_courses;
        """;
    }
}