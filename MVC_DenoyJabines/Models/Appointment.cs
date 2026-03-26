using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_DenoyJabines.Models
{
    public class Appointment
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [HiddenInput(DisplayValue = false)] 
        public int AppointmentID { get; set; }

        // Patient / Client Information
        // 
        [Required(ErrorMessage = "First name is required")]
        [StringLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [StringLength(100)]
        [Display(Name = "Middle Name")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [StringLength(20)]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; } = string.Empty;

        // Appointment Details
        [Required(ErrorMessage = "Appointment date is required")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Appointment Date & Time")]
        [FutureDate(ErrorMessage = "Appointment date and time must be in the future.")]
        public DateTime AppointmentDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Appointment type is required")]
        [StringLength(100)]
        [Display(Name = "Appointment Type")]
        public string AppointmentType { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Additional Notes")]
        public string? Notes { get; set; }

        // Status & Audit Fields
        [Required]
        [StringLength(50)]
        [Display(Name = "Status")]
        public string Status { get; set; } 

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } 

        [Display(Name = "Last Updated")]
        public DateTime? UpdatedAt { get; set; } 

        // Link to User
        [Required]
        [Display(Name = "User ID")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        [Display(Name = "User")]
        public Users? User { get; set; } // Navigation property
    }

    // Ensures appointment date is in the future
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value is DateTime date)
            {
                if (date <= DateTime.Now)
                    return new ValidationResult("Appointment date and time must be in the future.");
            }
            return ValidationResult.Success;
        }
    }
}