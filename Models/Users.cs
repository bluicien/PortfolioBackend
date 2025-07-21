using System.ComponentModel.DataAnnotations;

namespace PortfolioBackend.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}