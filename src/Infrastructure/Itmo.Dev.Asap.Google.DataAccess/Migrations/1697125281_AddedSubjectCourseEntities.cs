using FluentMigrator;
using Itmo.Dev.Platform.Postgres.Migrations;

namespace Itmo.Dev.Asap.Google.DataAccess.Migrations;

#pragma warning disable SA1649

[Migration(1697125281, "Addes subject course students and assignments")]
public class AddedSubjectCourseEntities : SqlMigration
{
    protected override string GetUpSql(IServiceProvider serviceProvider)
    {
        return """
        create table subject_course_students
        (
            student_id uuid ,
            subject_course_id uuid references subject_courses(subject_course_id),
            subject_course_student_ordinal int not null ,
            primary key (student_id, subject_course_id)
        );

        create unique index on subject_course_students(student_id, subject_course_id, subject_course_student_ordinal);

        create table subject_course_assignments
        (
            subject_course_id uuid references subject_courses(subject_course_id),
            assignment_id uuid ,
            subject_course_assignment_ordinal int not null ,
            primary key (subject_course_id, assignment_id)
        );

        create unique index on subject_course_assignments(subject_course_id, assignment_id, subject_course_assignment_ordinal);
        """;
    }

    protected override string GetDownSql(IServiceProvider serviceProvider)
    {
        return """
        drop table subject_course_students;
        drop table subject_course_assignments;
        """;
    }
}