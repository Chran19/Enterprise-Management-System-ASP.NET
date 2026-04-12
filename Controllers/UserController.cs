using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly UserRepository _repository;

        public UserController()
        {
            _repository = new UserRepository();
        }

        public IActionResult Index()
        {
            var users = _repository.GetAllUsers();
            return View(users);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserRegistration model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Validation
            if (string.IsNullOrWhiteSpace(model.Username))
            {
                ModelState.AddModelError("Username", "Username is required");
                return View(model);
            }

            if (model.Username.Length < 3)
            {
                ModelState.AddModelError("Username", "Username must be at least 3 characters");
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.Email) || !model.Email.Contains("@"))
            {
                ModelState.AddModelError("Email", "Valid email is required");
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.Password) || model.Password.Length < 6)
            {
                ModelState.AddModelError("Password", "Password must be at least 6 characters");
                return View(model);
            }

            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Passwords do not match");
                return View(model);
            }

            // Check if username already exists
            if (_repository.UsernameExists(model.Username))
            {
                ModelState.AddModelError("Username", "Username already exists");
                return View(model);
            }

            // Check if email already exists
            if (_repository.EmailExists(model.Email))
            {
                ModelState.AddModelError("Email", "Email already registered");
                return View(model);
            }

            // Create new user
            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = UserRepository.HashPassword(model.Password)
            };

            _repository.AddUser(user);
            TempData["Success"] = "Registration successful! You can now login.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var user = _repository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _repository.DeleteUser(id);
            TempData["Success"] = "User deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
