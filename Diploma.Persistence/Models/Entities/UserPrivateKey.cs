using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma.Persistence.Models.Entities
{
    [Table("user_private_keys")]
    public class UserPrivateKey
    {
        [Key, Column("id")]
        public Guid Id { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }
        
        [Required]
        [Column("key")]
        public string Key { get; set; }

        #region Relationships

        public User User { get; set; }

        #endregion
    }
}