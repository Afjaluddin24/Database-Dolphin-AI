using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dolphin_AI.Mode
{
    public class BlockedUsers
    {
        [Key]

        public int BlockedUsersId { get; set; }
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
        public DateTime? ExpireAt { get; set; }

    }

    public class BlockedUsersDto
    {
        public int? UserId { get; set; }
        public DateTime? ExpireAt { get; set; }
    }

}
