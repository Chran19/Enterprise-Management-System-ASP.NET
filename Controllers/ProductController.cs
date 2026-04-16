using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductRepository _repository;

        public ProductController()
        {
            _repository = new ProductRepository();
        }

        public IActionResult Index(string searchTerm = "")
        {
            List<Product> products;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                products = _repository.GetAllProducts();
            }
            else
            {
                products = _repository.SearchProducts(searchTerm);
                ViewBag.SearchTerm = searchTerm;
            }

            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            if (string.IsNullOrWhiteSpace(product.Name))
            {
                ModelState.AddModelError("Name", "Product name is required");
                return View(product);
            }

            if (product.Price <= 0)
            {
                ModelState.AddModelError("Price", "Price must be greater than 0");
                return View(product);
            }

            if (string.IsNullOrWhiteSpace(product.Category))
            {
                ModelState.AddModelError("Category", "Category is required");
                return View(product);
            }

            if (product.Stock < 0)
            {
                ModelState.AddModelError("Stock", "Stock cannot be negative");
                return View(product);
            }

            _repository.AddProduct(product);
            TempData["Success"] = "Product added successfully!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var product = _repository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            if (string.IsNullOrWhiteSpace(product.Name))
            {
                ModelState.AddModelError("Name", "Product name is required");
                return View(product);
            }

            if (product.Price <= 0)
            {
                ModelState.AddModelError("Price", "Price must be greater than 0");
                return View(product);
            }

            if (string.IsNullOrWhiteSpace(product.Category))
            {
                ModelState.AddModelError("Category", "Category is required");
                return View(product);
            }

            if (product.Stock < 0)
            {
                ModelState.AddModelError("Stock", "Stock cannot be negative");
                return View(product);
            }

            _repository.UpdateProduct(product);
            TempData["Success"] = "Product updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var product = _repository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _repository.DeleteProduct(id);
            TempData["Success"] = "Product deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
