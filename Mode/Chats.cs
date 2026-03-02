using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dolphin_AI.Mode
{
    public class Chats
    {
        [Key]
        public int ChatsId { get; set; }
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
        public string question { get; set; } = string.Empty;
        public string answer {  get; set; } = string.Empty;
        public DateTime created_at { get; set; } = DateTime.Now;

    }

    public class ChatsDto
    {
        public int UserId { get; set; }
        public string? question { get; set; }
        public string? answer { get; set; } 
    }
}
