using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma.Persistence.Models
{
    [Table("chats")]
    public class Chat
    {
        [Key, Column("id")]
        public Guid Id { get; set; }

        [Column("type")]
        public ChatType Type { get; set; }

        #region Relationships

        public ICollection<ChatUser> ChatUsers { get; set; }

        #endregion
    }
}