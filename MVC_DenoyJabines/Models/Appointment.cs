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

            // Patient / Client Info
            [Required(ErrorMessage = "First name is required")]
            [StringLength(100)]
            public string FirstName { get; set; } = string.Empty;

            [Required(ErrorMessage = "Last name is required")]
            [StringLength(100)]
            public string LastName { get; set; } = string.Empty;

            [StringLength(100)]
            public string? MiddleName { get; set; }

            [Required(ErrorMessage = "Email is required")]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Phone]
            [StringLength(20)]
            public string ContactNumber { get; set; } = string.Empty;

            // Appointment Details
            [Required(ErrorMessage = "Appointment date is required")]
            [DataType(DataType.DateTime)]
            public DateTime AppointmentDate { get; set; } = DateTime.Now;

            [Required(ErrorMessage = "Appointment type is required")]
            [StringLength(100)]
            public string AppointmentType { get; set; } = string.Empty;

            [StringLength(500)]
            public string? Notes { get; set; }

            // Status (Pending, Confirmed, Cancelled, Completed)
            [Required]
            [StringLength(50)]
            public string Status { get; set; }

            // Audit Fields
            public DateTime CreatedAt { get; set; } 

            public DateTime? UpdatedAt { get; set; } 

        // User ID
        [Required]
            public int UserId { get; set; }  // FK to Users table

            [ForeignKey("UserId")]
            public Users? User { get; set; }
    }
}