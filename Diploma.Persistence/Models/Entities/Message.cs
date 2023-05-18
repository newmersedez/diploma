using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma.Persistence.Models.Entities
{
    [Table("messages")]
    public class Message
    {
        [Key, Column("id")]
        public Guid Id { get; set; }

        [Column("chat_id")]
        public Guid ChatId { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("file_id")]
        public Guid? FileId { get; set; }

        [Required]
        [Column("text")]
        public string Text { get; set; }

        [Column("date_create")]
        public DateTime DateCreate { get; set; }

        #region Relationships

        public Chat Chat { get; set; }

        public User User { get; set; }

        public File File { get; set; }

        #endregion
    }
}