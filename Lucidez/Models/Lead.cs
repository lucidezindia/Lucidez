using System.ComponentModel.DataAnnotations;

namespace Lucidez.Models
{
    public class Lead
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        public string? Company { get; set; }

        public string Source { get; set; } = "Website";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
