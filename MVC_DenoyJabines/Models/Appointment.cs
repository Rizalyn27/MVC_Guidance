using Microsoft.AspNetCore.Mvc;
using MVC_DenoyJabines.Models;
using System;
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

            // Foreign Key: Student
            [Required(ErrorMessage = "Student is required")]
            public int StudentID { get; set; }

            [ForeignKey("StudentID")]
            public Students Student { get; set; }

            // Foreign Key: Counselor / User (optional)
            public int? CounselorID { get; set; }

            [ForeignKey("CounselorID")]
            public Users Counselor { get; set; }

            // Appointment Info
            [Required(ErrorMessage = "Appointment date is required")]
            [DataType(DataType.DateTime)]
            public DateTime AppointmentDate { get; set; }

            [Required(ErrorMessage = "Appointment type is required")]
            [StringLength(100, ErrorMessage = "Type must not exceed 100 characters")]
            public string AppointmentType { get; set; }  // e.g., Consultation, Follow-up

            [StringLength(500, ErrorMessage = "Notes must not exceed 500 characters")]
            public string Notes { get; set; }

            // Status
            [Required]
            [StringLength(50)]
            public string Status { get; set; } = "Pending"; // Pending, Completed, Cancelled
        }
}
