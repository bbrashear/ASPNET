using ASPNET.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNET.Controllers
{
    public class ProductController : Controller
    {
        //product controller depends on interface to decouple the application
        private readonly IProductRepository repo;

        public ProductController(IProductRepository repo)
        {
            this.repo = repo;
        }

        // GET: /<controller>/
        public IActionResult Index() //returns view of all products
        {
            var products = repo.GetAllProducts();
            return View(products);
        }
        public IActionResult ViewProduct(int id)
        {
            var product = repo.GetProduct(id); //returns view of specific product
            return View(product);
        }
        public IActionResult UpdateProduct(Product product) //goes to Update Product page
        {
            Product prod = repo.GetProduct(product.ProductID); //gets specific product from database
            repo.UpdateProduct(prod); //updates product
            if(prod == null) //error handling
            {
                return View("ProductNotFound");
            }
            return View(prod);
        }
        public IActionResult UpdateProductToDatabase(Product product) //updates product and redirects to new view product page
        {
            repo.UpdateProduct(product);
            return RedirectToAction("View Product", new { id = product.ProductID });
        }
        public IActionResult InsertProduct() //goes to Insert Product page
        {
            var prod = repo.AssignCategory();
            return View(prod);
        }
        public IActionResult InsertProductToDatabase(Product productToInsert) //inserts new product and redirects to "home page" of products
        {
            repo.InsertProduct(productToInsert);
            return RedirectToAction("Index");
        }
        public IActionResult DeleteProduct(Product product) //deletes product and redirects to "home page" of products
        {
            repo.DeleteProduct(product);
            return RedirectToAction("Index");
        }
    }
}
