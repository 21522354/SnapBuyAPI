using ChatService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatService.Service
{
    public class ChatDBContext : DbContext
    {
        public ChatDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChatRoom>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<ChatMessage>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<ChatRoom>()
                .HasMany(x => x.ChatMessages)
                .WithOne(x => x.ChatRoom)
                .HasForeignKey(x => x.ChatRoomId);
        }
    }
}
