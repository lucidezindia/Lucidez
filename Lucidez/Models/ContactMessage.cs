using System.ComponentModel.DataAnnotations;

namespace Lucidez.Models
{
    public class ContactMessage
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        public string? Company { get; set; }

        [Required]
        public string Message { get; set; } = "";

        public bool Handled { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
