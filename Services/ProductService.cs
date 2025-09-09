using PortfolioBackend.Models;

namespace PortfolioBackend.Services
{
    public class ProductService : IProductService
    {
        private readonly List<Products> _products;
        public ProductService()
        {
            _products =
            [
                new Products { Id = "1", ProductName = "Product 1", Price = 10.99m },
                new Products { Id = "2", ProductName = "Product 2", Price = 20.99m },
                new Products { Id = "3", ProductName = "Product 3", Price = 30.99m }
            ];
        }

        public void CreateProduct(Products product)
        {
            throw new NotImplementedException();
        }

        public void DeleteProduct(string id)
        {
            Products? product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null) _products.Remove(product);
        }

        public Products? GetProductById(string id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Products> GetProducts()
        {
            return _products;
        }

        public void UpdateProduct(string id, Products product)
        {
            Products? existingProduct = _products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null) return;
            existingProduct.ProductName = product.ProductName;
            existingProduct.Price = product.Price;
        }
    }
}