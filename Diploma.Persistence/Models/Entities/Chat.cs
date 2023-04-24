using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma.Persistence.Models.Entities
{
    [Table("chats")]
    public class Chat
    {
        [Key, Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        #region Relationships

        public ICollection<ChatUser> ChatUsers { get; set; }

        public ICollection<Message> Messages { get; set; }

        #endregion
    }
}