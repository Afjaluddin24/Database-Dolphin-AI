using Dolphin_AI.Mode;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Dolphin_AI.ApplicationContext
{
    public class ApplicationDbcontext:DbContext
    {
        public ApplicationDbcontext(DbContextOptions<ApplicationDbcontext> options) :base(options)
        {
            
        }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Chats> Chats { get; set; }
        public DbSet<BlockedUsers> BlockedUsers { get; set; } 
        public DbSet<Contactus> Contactus { get; set; }
    }
}
