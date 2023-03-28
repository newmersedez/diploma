using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma.Persistence.Models
{
    [Table("chat_users")]
    public class ChatUser
    {
        [Key, Column("id")]
        public Guid Id { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("chat_id")]
        public Guid ChatId { get; set; }

        [Column("role")]
        public ChatRole Role { get; set; }

        #region Relationships

        public User User { get; set; }

        public Chat Chat { get; set; }

        #endregion
    }
}