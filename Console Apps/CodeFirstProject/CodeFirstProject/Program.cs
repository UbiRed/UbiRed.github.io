using System;

namespace CodeFirstProject
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var ctx = new SchoolContext())
            {
                var student = new Student();

                Console.Write("Enter Student Name: ");
                student.StudentName = Console.ReadLine();

                Console.Write("Enter Date of Birth (YYYY-MM-DD): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime dateOfBirth))
                {
                    student.DateOfBirth = dateOfBirth;
                }
                else
                {
                    Console.WriteLine("Invalid date format.");
                    return;
                }

                Console.Write("Enter Height (in feet): ");
                if (decimal.TryParse(Console.ReadLine(), out decimal height))
                {
                    student.Height = height;
                }
                else
                {
                    Console.WriteLine("Invalid height format.");
                    return;
                }

                Console.Write("Enter Weight (in pounds): ");
                if (float.TryParse(Console.ReadLine(), out float weight))
                {
                    student.Weight = weight;
                }
                else
                {
                    Console.WriteLine("Invalid weight format.");
                    return;
                }

                Console.Write("Enter Grade ID: ");
                if (int.TryParse(Console.ReadLine(), out int gradeId))
                {
                    student.GradeId = gradeId;
                }
                else
                {
                    Console.WriteLine("Invalid grade ID format.");
                    return;
                }

                ctx.Students.Add(student);
                ctx.SaveChanges();

                Console.WriteLine("Student Information:");
                Console.WriteLine($"Name: {student.StudentName}");
                Console.WriteLine($"Date of Birth: {student.DateOfBirth}");
                Console.WriteLine($"Height: {student.Height} Feet");
                Console.WriteLine($"Weight: {student.Weight} Lbs");
                Console.WriteLine($"Grade ID: {student.GradeId}");
            }

            Console.WriteLine("Code-First demonstration Completed!");
            Console.ReadLine();
        }
    }
}
