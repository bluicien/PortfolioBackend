using System.ComponentModel.DataAnnotations;

namespace PortoflioBackend.Models;

public class Feedback
{
    [Key]
    public int UserId { get; set; }
    public required string Username { get; set; }
    public string? Company { get; set; }
    public string? Position { get; set; }
    public int Rating { get; set; }
    public string? Message { get; set; }
    public int? UserIpAddress { get; set; }
    public bool IsApproved { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}