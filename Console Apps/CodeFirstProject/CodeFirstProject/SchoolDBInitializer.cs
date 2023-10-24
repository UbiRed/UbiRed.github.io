using CodeFirstProject;
using System.Collections.Generic;
using System.Data.Entity;

namespace CodeFirstProject
{
    public class SchoolDBInitializer : DropCreateDatabaseAlways<SchoolContext>
    {
        protected override void Seed(SchoolContext context)
        {
            IList<Grade> grades = new List<Grade>();
            grades.Add(new Grade() { GradeName = "Grade 0", Section = "A" });
            grades.Add(new Grade() { GradeName = "Grade 1", Section = "B" });
            grades.Add(new Grade() { GradeName = "Grade 2", Section = "C" });
            grades.Add(new Grade() { GradeName = "Grade 3", Section = "D" });
            grades.Add(new Grade() { GradeName = "Grade 4", Section = "E" });
            grades.Add(new Grade() { GradeName = "Grade 5", Section = "F" });
            grades.Add(new Grade() { GradeName = "Grade 6", Section = "G" });
            grades.Add(new Grade() { GradeName = "Grade 7", Section = "H" });
            grades.Add(new Grade() { GradeName = "Grade 8", Section = "I" });
            grades.Add(new Grade() { GradeName = "Grade 9", Section = "J" });
            grades.Add(new Grade() { GradeName = "Grade 10", Section = "K" });
            grades.Add(new Grade() { GradeName = "Grade 11", Section = "L" });
            grades.Add(new Grade() { GradeName = "Grade 12", Section = "M" });

            context.Grades.AddRange(grades);

            base.Seed(context);
        }
    }
}