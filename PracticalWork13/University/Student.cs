using System.Collections.Generic;

namespace PracticalWork13.University
{
    /// <summary>Студент.</summary>
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int GroupId { get; set; }
        public Dictionary<string, int> Grades { get; } = new Dictionary<string, int>();
    }

    /// <summary>Учебная группа.</summary>
    public class StudyGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Student> Students { get; } = new List<Student>();
    }
}
