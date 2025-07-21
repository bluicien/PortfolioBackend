using PortfolioBackend.Models;

namespace PortfolioBackend.Services
{
    public interface IProductService
    {
        IEnumerable<Products>? GetProducts();
        Products? GetProductById(int id);
        void CreateProduct(Products product);
        void UpdateProduct(int id, Products product);
        void DeleteProduct(int id);
    }
}