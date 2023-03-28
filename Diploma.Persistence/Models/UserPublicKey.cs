using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma.Persistence.Models
{
    [Table("user_public_keys")]
    public class UserPublicKey
    {
        [Key, Column("id")]
        public Guid Id { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }

        [Required]
        [Column("x")]
        public string X { get; set; }

        [Required]
        [Column("y")]
        public string Y { get; set; }

        #region Relationships

        public User User { get; set; }

        #endregion
    }
}