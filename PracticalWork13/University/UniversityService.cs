using System.Collections.Generic;
using System.Linq;
using PracticalWork13.Common;

namespace PracticalWork13.University
{
    /// <summary>
    /// Ведение базы студентов, групп и успеваемости
    /// (деканат / учебная часть).
    /// </summary>
    public class UniversityService
    {
        private readonly List<StudyGroup> _groups = new List<StudyGroup>();
        private int _nextGroupId = 1;
        private int _nextStudentId = 1;

        /// <summary>Создает учебную группу.</summary>
        public StudyGroup AddGroup(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Название группы не должно быть пустым.");
            var group = new StudyGroup { Id = _nextGroupId++, Name = name };
            _groups.Add(group);
            return group;
        }

        /// <summary>Добавляет студента в группу (TC-UN-01).</summary>
        public Student AddStudent(string fullName, int groupId)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ValidationException("ФИО студента не должно быть пустым.");

            var group = _groups.FirstOrDefault(g => g.Id == groupId);
            if (group == null)
                throw new ValidationException(string.Format("Группа с ID {0} не найдена.", groupId));

            var student = new Student { Id = _nextStudentId++, FullName = fullName, GroupId = groupId };
            group.Students.Add(student);
            return student;
        }

        /// <summary>
        /// Проставляет оценку по дисциплине (TC-UN-02):
        /// оценка должна быть в диапазоне от 2 до 5.
        /// </summary>
        public void AddGrade(int studentId, string discipline, int grade)
        {
            if (grade < 2 || grade > 5)
                throw new ValidationException("Оценка должна быть в диапазоне от 2 до 5.");

            if (string.IsNullOrWhiteSpace(discipline))
                throw new ValidationException("Дисциплина не должна быть пустой.");

            GetStudent(studentId).Grades[discipline] = grade;
        }

        /// <summary>Средний балл студента (TC-UN-03).</summary>
        public double GetAverage(int studentId)
        {
            var grades = GetStudent(studentId).Grades.Values;
            if (grades.Count == 0)
                return 0;
            return grades.Average();
        }

        public Student GetStudent(int studentId)
        {
            var student = _groups.SelectMany(g => g.Students).FirstOrDefault(s => s.Id == studentId);
            if (student == null)
                throw new ValidationException(string.Format("Студент с ID {0} не найден.", studentId));
            return student;
        }

        public IReadOnlyList<StudyGroup> GetGroups()
        {
            return _groups;
        }
    }
}
