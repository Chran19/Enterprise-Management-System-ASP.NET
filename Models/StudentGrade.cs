namespace WebApp.Models
{
    public class StudentGrade
    {
        public int GradeId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public int Marks { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class StudentSummary
    {
        public string StudentName { get; set; } = string.Empty;
        public double AverageMarks { get; set; }
        public int TotalSubjects { get; set; }
    }
}
