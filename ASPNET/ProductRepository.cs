using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNET.Models
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbConnection _conn;

        //Gets connection from startup file
        public ProductRepository(IDbConnection conn)
        {
            _conn = conn;
        }
        public IEnumerable<Product> GetAllProducts()
        {
            return _conn.Query<Product>("SELECT * FROM products;");
        }
        public Product GetProduct(int id)
        {
            return (Product)_conn.QuerySingle<Product>("SELECT * FROM products WHERE ProductID = @id", new { id = id });
        }
        public void UpdateProduct(Product product)
        {
            _conn.Execute("UPDATE products SET Name = @name, Price = @price WHERE ProductID = @id;",
                new { name = product.Name, price = product.Price, id = product.ProductID });
        }
        public void InsertProduct(Product productToInsert)
        {
            _conn.Execute("INSERT INTO products (Name, Price, CategoryID) VALUES (@name, @price, @categoryID);",
                new { name = productToInsert.Name, price = productToInsert.Price, categoryID = productToInsert.CategoryID });
        }
        public IEnumerable<Category> GetCategories()
        {
            return _conn.Query<Category>("SELECT * FROM categories;");
        }
        public Product AssignCategory()
        {
            var categoryList = GetCategories();
            var product = new Product();
            product.Categories = categoryList;

            return product;
        }
        public void DeleteProduct(Product product)
        {
            _conn.Execute("DELETE FROM reviews WHERE ProductID = @id;", new { id = product.ProductID });
            _conn.Execute("DELETE FROM sales WHERE ProductID = @id;", new { id = product.ProductID });
            _conn.Execute("DELETE FROM products WHERE ProductID = @id;", new { id = product.ProductID });
        }
    }
}
