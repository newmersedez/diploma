using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma.Persistence.Models.Entities
{
    [Table("attachments")]
    public class Attachment
    {
        [Key, Column("id")]
        public Guid Id { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }

        [Required]
        [Column("folder")]
        public string Folder { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Column("content_type")]
        public string ContentType { get; set; }

        #region Relationships

        public User User { get; set; }

        public ICollection<Message> Messages { get; set; }

        #endregion
    }
}