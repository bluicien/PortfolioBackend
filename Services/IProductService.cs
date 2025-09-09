using PortfolioBackend.Models;

namespace PortfolioBackend.Services
{
    public interface IProductService
    {
        IEnumerable<Products>? GetProducts();
        Products? GetProductById(string id);
        void CreateProduct(Products product);
        void UpdateProduct(string id, Products product);
        void DeleteProduct(string id);
    }
}