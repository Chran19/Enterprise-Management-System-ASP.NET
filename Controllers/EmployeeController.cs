using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeRepository _repository;

        public EmployeeController()
        {
            _repository = new EmployeeRepository();
        }

        public IActionResult Index(string searchTerm = "")
        {
            List<Employee> employees;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                employees = _repository.GetAllEmployees();
            }
            else
            {
                employees = _repository.SearchEmployees(searchTerm);
                ViewBag.SearchTerm = searchTerm;
            }

            return View(employees);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return View(employee);
            }

            if (string.IsNullOrWhiteSpace(employee.Name))
            {
                ModelState.AddModelError("Name", "Employee name is required");
                return View(employee);
            }

            if (string.IsNullOrWhiteSpace(employee.Department))
            {
                ModelState.AddModelError("Department", "Department is required");
                return View(employee);
            }

            if (string.IsNullOrWhiteSpace(employee.Email) || !employee.Email.Contains("@"))
            {
                ModelState.AddModelError("Email", "Valid email is required");
                return View(employee);
            }

            if (string.IsNullOrWhiteSpace(employee.Phone) || employee.Phone.Length < 10)
            {
                ModelState.AddModelError("Phone", "Valid phone number is required");
                return View(employee);
            }

            _repository.AddEmployee(employee);
            TempData["Success"] = "Employee added successfully!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var employee = _repository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost]
        public IActionResult Edit(int id, Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(employee);
            }

            if (string.IsNullOrWhiteSpace(employee.Name))
            {
                ModelState.AddModelError("Name", "Employee name is required");
                return View(employee);
            }

            if (string.IsNullOrWhiteSpace(employee.Department))
            {
                ModelState.AddModelError("Department", "Department is required");
                return View(employee);
            }

            if (string.IsNullOrWhiteSpace(employee.Email) || !employee.Email.Contains("@"))
            {
                ModelState.AddModelError("Email", "Valid email is required");
                return View(employee);
            }

            if (string.IsNullOrWhiteSpace(employee.Phone) || employee.Phone.Length < 10)
            {
                ModelState.AddModelError("Phone", "Valid phone number is required");
                return View(employee);
            }

            _repository.UpdateEmployee(employee);
            TempData["Success"] = "Employee updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var employee = _repository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _repository.DeleteEmployee(id);
            TempData["Success"] = "Employee deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
