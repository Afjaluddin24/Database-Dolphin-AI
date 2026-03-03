using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dolphin_AI.Mode
{
    public class User
    {
        [Key]

        public int Userid { get; set; }

        [Required(ErrorMessage = "name is required")]
        public string username { get; set; } = string.Empty;

        [Required(ErrorMessage = "phone no is required")]
        public string phoneno { get; set; } = string.Empty;

        [Required(ErrorMessage = "email is required")]
        [EmailAddress]
        public string email { get; set; } = string.Empty;
        public string? Gender { get; set; }

        public string? city { get; set; }

        [Required(ErrorMessage = "password is required")]
        public string password { get; set; } = string.Empty;
        public DateTime created_at { get; set; } = DateTime.UtcNow;

        public ICollection<Chats>? chats { get; set; }

        public ICollection<BlockedUsers>? blockedUsers { get; set; }

    }

    public class Authenticationu
    {
        public string email { get; set; } 
        public string password { get; set; }
    }

    public class UserDto
    {
        public string? username { get; set; }
        public string? phoneno { get; set; }
        public string? email { get; set; }
        public string? Gender { get; set; }
        public string? password { get; set; }
        public string? city { get; set; }

    }

    public class UpdateDto
    {
        public int Userid { get; set; }
        public string? username { get; set; }
        public string? phoneno { get; set; }
        public string? email { get; set; }
        public string? Gender { get; set; }
        public string? password { get; set; }
        public string? city { get; set; }
    }
}
