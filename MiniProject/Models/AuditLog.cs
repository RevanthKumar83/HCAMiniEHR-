using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniProject.Models
{
    [Table("AuditLog", Schema = "Healthcare")]
    public class AuditLog
    {
        [Key]
        public int LogId { get; set; }

        [Required]
        [StringLength(50)]
        public string TableName { get; set; } = string.Empty;

        [Required]
        public int RecordId { get; set; }

        [Required]
        [StringLength(50)]
        public string Action { get; set; } = string.Empty;

        [Required]
        public DateTime LogDate { get; set; }

        public string? Details { get; set; }
    }
}
