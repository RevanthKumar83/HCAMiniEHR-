using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniProject.Models
{
    [Table("Patient", Schema = "Healthcare")]
    public class Patient : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be exactly 10 digits.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must contain only digits (no spaces or special characters) and be exactly 10 digits.")]
        public string Phone { get; set; } = string.Empty;

        // Navigation Property
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateOfBirth > DateTime.Now)
            {
                yield return new ValidationResult(
                    "Date of Birth cannot be in the future.",
                    new[] { nameof(DateOfBirth) });
            }
        }
    }
}
