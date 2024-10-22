using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Student.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        [Range(1, 5, ErrorMessage = "Оценка должна быть от 1 до 5")]
        public int? Score { get; set; }
        public string? Discipline { get; set; }

    }
}
