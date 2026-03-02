using System.ComponentModel.DataAnnotations;

namespace Dolphin_AI.Mode
{
    public class Contactus
    {
        [Key]

        public int Id { get; set; }

        [Required(ErrorMessage ="name is required")]
        public string? fullname { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "phone number is required")]
        public long? Phoneno {  get; set; }

        [Required(ErrorMessage = "message is required")]
        public string? Message { get; set; }
        public DateTime create_date { get; set; } = DateTime.UtcNow;

    }

    public class ContactusDto
    {
        public string? fullname { get; set; }
        public string? Email { get; set; }
        public long? Phoneno { get; set; }
        public string? Message { get; set; }
    }
}
