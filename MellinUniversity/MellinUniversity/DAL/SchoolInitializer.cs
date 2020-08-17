using MellinUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MellinUniversity.DAL
{
    public class SchoolInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<SchoolContext>
    {
        protected override void Seed(SchoolContext context)
        {
            var students = new List<Student>
            {
                new Student{FirstMidName="John",LastName="Smith",EnrollmentDate=DateTime.Parse("1999-08-11")},
                new Student{FirstMidName="Sally",LastName="Lee",EnrollmentDate=DateTime.Parse("2000-03-03")},
                new Student{FirstMidName="Lilly",LastName="Thomas",EnrollmentDate=DateTime.Parse("2001-05-19")},
                new Student{FirstMidName="Devan",LastName="Davis",EnrollmentDate=DateTime.Parse("2000-06-21")},
                new Student{FirstMidName="Jill",LastName="James",EnrollmentDate=DateTime.Parse("1998-10-29")},
                new Student{FirstMidName="Mark",LastName="Zynson",EnrollmentDate=DateTime.Parse("2000-01-01")},
                new Student{FirstMidName="Sue",LastName="Snowman",EnrollmentDate=DateTime.Parse("2000-09-17")},
                new Student{FirstMidName="Matt",LastName="Clorox",EnrollmentDate=DateTime.Parse("1999-2-04")}
            };

            students.ForEach(s => context.Students.Add(s));
            context.SaveChanges();
            var courses = new List<Course>
            {
                new Course{CourseID=1050,Title="Chemistry",Credits=3,},
                new Course{CourseID=4022,Title="Microeconomics",Credits=3,},
                new Course{CourseID=4041,Title="Macroeconomics",Credits=3,},
                new Course{CourseID=1045,Title="Calculus",Credits=4,},
                new Course{CourseID=3141,Title="Trigonometry",Credits=4,},
                new Course{CourseID=2021,Title="Composition",Credits=3,},
                new Course{CourseID=2042,Title="Literature",Credits=4,}
            };
            courses.ForEach(s => context.Courses.Add(s));
            context.SaveChanges();
            var enrollments = new List<Enrollment>
            {
                new Enrollment{StudentID=1,CourseID=1050,Grade=Grade.C},
                new Enrollment{StudentID=1,CourseID=4022,Grade=Grade.A},
                new Enrollment{StudentID=1,CourseID=4041,Grade=Grade.F},
                new Enrollment{StudentID=2,CourseID=1045,Grade=Grade.C},
                new Enrollment{StudentID=2,CourseID=3141,Grade=Grade.B},
                new Enrollment{StudentID=2,CourseID=2021},
                new Enrollment{StudentID=3,CourseID=1050,Grade=Grade.A},
                new Enrollment{StudentID=4,CourseID=1050,Grade=Grade.F},
                new Enrollment{StudentID=4,CourseID=4022,Grade=Grade.A},
                new Enrollment{StudentID=5,CourseID=4041,Grade=Grade.B},
                new Enrollment{StudentID=6,CourseID=1045,Grade=Grade.C},
                new Enrollment{StudentID=7,CourseID=3141,Grade=Grade.B},
            };
            enrollments.ForEach(s => context.Enrollments.Add(s));
            context.SaveChanges();
        }
    }
}