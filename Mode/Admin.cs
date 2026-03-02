using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Dolphin_AI.Mode
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }
        public string? name { get; set; }
        public string? fullname { get; set; }

        [Required(ErrorMessage ="email is required")]
        [EmailAddress]
        public string? email { get; set; }
        public string? phoneno { get; set; }
        public string? Adress { get; set; }

        [Required(ErrorMessage = "password is required")]
        public string? password { get; set; }
        public string? Logo { get; set; }
        public string? Baner { get; set; }
        public string? Mapurl { get; set; }
        public DateTime? created_at { get; set; } = DateTime.UtcNow;
    }

    public class Authenticationa
    {
        public string? email { get; set; }
        public string? password { get; set; }
    }

   
}
