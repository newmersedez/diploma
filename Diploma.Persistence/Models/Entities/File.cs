using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma.Persistence.Models.Entities
{
    [Table("files")]
    public class File
    {
        [Key, Column("id")]
        public Guid Id { get; set; }

        [Required, Column("folder")]
        public string Folder { get; set; }

        [Required, Column("name")]
        public string Name { get; set; }

        [Required, Column("content_type")]
        public string ContentType { get; set; }
    }
}