using Microsoft.EntityFrameworkCore;

namespace PortfolioBackend.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;

        [Precision(18, 2)] // 18 total digits, 2 after the decimal
        public decimal Price { get; set; }
    }
}