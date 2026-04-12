using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class StudentGradeController : Controller
    {
        private readonly StudentGradeRepository _repository;

        public StudentGradeController()
        {
            _repository = new StudentGradeRepository();
        }

        public IActionResult Index()
        {
            var grades = _repository.GetAllGrades();
            return View(grades);
        }

        public IActionResult Summary()
        {
            var summary = _repository.GetStudentSummary();
            return View(summary);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(StudentGrade grade)
        {
            if (!ModelState.IsValid)
            {
                return View(grade);
            }

            if (string.IsNullOrWhiteSpace(grade.StudentName))
            {
                ModelState.AddModelError("StudentName", "Student name is required");
                return View(grade);
            }

            if (string.IsNullOrWhiteSpace(grade.Subject))
            {
                ModelState.AddModelError("Subject", "Subject is required");
                return View(grade);
            }

            if (grade.Marks < 0 || grade.Marks > 100)
            {
                ModelState.AddModelError("Marks", "Marks must be between 0 and 100");
                return View(grade);
            }

            _repository.AddGrade(grade);
            TempData["Success"] = "Grade added successfully!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var grade = _repository.GetGradeById(id);
            if (grade == null)
            {
                return NotFound();
            }

            return View(grade);
        }

        [HttpPost]
        public IActionResult Edit(int id, StudentGrade grade)
        {
            if (id != grade.GradeId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(grade);
            }

            if (string.IsNullOrWhiteSpace(grade.StudentName))
            {
                ModelState.AddModelError("StudentName", "Student name is required");
                return View(grade);
            }

            if (string.IsNullOrWhiteSpace(grade.Subject))
            {
                ModelState.AddModelError("Subject", "Subject is required");
                return View(grade);
            }

            if (grade.Marks < 0 || grade.Marks > 100)
            {
                ModelState.AddModelError("Marks", "Marks must be between 0 and 100");
                return View(grade);
            }

            _repository.UpdateGrade(grade);
            TempData["Success"] = "Grade updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var grade = _repository.GetGradeById(id);
            if (grade == null)
            {
                return NotFound();
            }

            return View(grade);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _repository.DeleteGrade(id);
            TempData["Success"] = "Grade deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
