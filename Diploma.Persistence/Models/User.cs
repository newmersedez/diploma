using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma.Persistence.Models
{
    [Table("users")]
    public class User
    {
        [Key, Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("email")]
        public string Email { get; set; }
        
        [Required]
        [Column("password_hash")]
        public string PasswordHash { get; set; } 

        #region Relationships

        public UserPublicKey PublicKey { get; set; }
        
        public UserPrivateKey PrivateKey { get; set; }

        public ICollection<Attachment> Attachments { get; set; }

        #endregion
    }
}