using Microsoft.Data.Sqlite;
using System;

class Program
{
    static void Main()
    {
        string connectionString = "Data Source=University.db";  // Путь к базе данных (будет создан автоматически)

        // Создаём подключение
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            // Создание таблицы студентов
            var createStudentsTableCmd = connection.CreateCommand();
            createStudentsTableCmd.CommandText =
            @"
                CREATE TABLE IF NOT EXISTS Students (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    DateOfBirth TEXT
                );
            ";
            createStudentsTableCmd.ExecuteNonQuery();

            // Создание таблицы курсов
            var createCoursesTableCmd = connection.CreateCommand();
            createCoursesTableCmd.CommandText =
            @"
                CREATE TABLE IF NOT EXISTS Courses (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    CourseName TEXT NOT NULL,
                    Credits INTEGER NOT NULL
                );
            ";
            createCoursesTableCmd.ExecuteNonQuery();

            // Создание таблицы записей о курсах
            var createEnrollmentsTableCmd = connection.CreateCommand();
            createEnrollmentsTableCmd.CommandText =
            @"
                CREATE TABLE IF NOT EXISTS Enrollments (
                    StudentId INTEGER,
                    CourseId INTEGER,
                    EnrollmentDate TEXT,
                    FOREIGN KEY(StudentId) REFERENCES Students(Id),
                    FOREIGN KEY(CourseId) REFERENCES Courses(Id),
                    PRIMARY KEY (StudentId, CourseId)
                );
            ";
            createEnrollmentsTableCmd.ExecuteNonQuery();

            Console.WriteLine("База данных и таблицы созданы.");

            // Добавление студентов
            var insertStudentCmd = connection.CreateCommand();
            insertStudentCmd.CommandText =
            @"
                INSERT INTO Students (Name, DateOfBirth) 
                VALUES ('Иван Иванов', '2000-01-01'),
                       ('Мария Петрова', '1999-05-15');
            ";
            insertStudentCmd.ExecuteNonQuery();

            // Добавление курсов
            var insertCourseCmd = connection.CreateCommand();
            insertCourseCmd.CommandText =
            @"
                INSERT INTO Courses (CourseName, Credits) 
                VALUES ('Математика', 3),
                       ('Программирование', 5);
            ";
            insertCourseCmd.ExecuteNonQuery();

            // Запись студентов на курсы
            var insertEnrollmentCmd = connection.CreateCommand();
            insertEnrollmentCmd.CommandText =
            @"
                INSERT INTO Enrollments (StudentId, CourseId, EnrollmentDate) 
                VALUES (1, 1, '2025-02-09'),
                       (1, 2, '2025-02-09'),
                       (2, 1, '2025-02-09');
            ";
            insertEnrollmentCmd.ExecuteNonQuery();

            Console.WriteLine("Данные добавлены.");

            // Запрос для вывода всех студентов
            var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = "SELECT * FROM Students";

            Console.WriteLine("\nСписок студентов:");
            using (var reader = selectCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"Id: {reader.GetInt32(0)}, Name: {reader.GetString(1)}, DateOfBirth: {reader.GetString(2)}");
                }
            }

            // Запрос для вывода всех курсов
            selectCmd.CommandText = "SELECT * FROM Courses";
            Console.WriteLine("\nСписок курсов:");
            using (var reader = selectCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"Id: {reader.GetInt32(0)}, CourseName: {reader.GetString(1)}, Credits: {reader.GetInt32(2)}");
                }
            }

            // Запрос для вывода всех записей о курсах
            selectCmd.CommandText = "SELECT s.Name AS StudentName, c.CourseName AS CourseName, e.EnrollmentDate FROM Enrollments e JOIN Students s ON e.StudentId = s.Id JOIN Courses c ON e.CourseId = c.Id";
            Console.WriteLine("\nЗаписи студентов на курсы:");
            using (var reader = selectCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"Student: {reader.GetString(0)}, Course: {reader.GetString(1)}, EnrollmentDate: {reader.GetString(2)}");
                }
            }
        }
    }
}
