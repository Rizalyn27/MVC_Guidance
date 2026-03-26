using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_DenoyJabines.Models
{
    public class Students
    {
        // ID
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [HiddenInput(DisplayValue = false)]
        public int StuID { get; set; }

        // Student ID
        [Required(ErrorMessage = "Student ID is required")]
        [StringLength(20, MinimumLength = 4)]
        [Display(Name = "Student ID (LRN)")]
        public string StuLRN { get; set; }

        // Name
        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z\s]+$")]
        [Display(Name = "First Name")]
        public string StuFName { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z\s]+$")]
        [Display(Name = "Last Name")]
        public string StuLName { get; set; }

        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z\s]*$")]
        [Display(Name = "Middle Name")]
        public string StuMName { get; set; }

        [Display(Name = "Active Status")]
        public bool StuStatus { get; set; }

        [Display(Name = "Full Name")]
        public string StuFullName => $"{StuFName} {StuMName} {StuLName}".Replace("  ", " ").Trim();

        // Gender
        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        // Birthdate
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Birthdate")]
        public DateTime Birthdate { get; set; }

        // Address
        [Required]
        [StringLength(200)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        // Contact
        [Required]
        [RegularExpression(@"^\d{11}$")]
        [StringLength(11)]
        [Display(Name = "Contact Number")]
        public string Contact { get; set; }

        // Email
        [Required]
        [EmailAddress]
        [StringLength(100)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        // Academic Info
        [Required]
        [StringLength(100)]
        [Display(Name = "Department")]
        public string Department { get; set; }

        [Required]
        [Range(7, 12)]
        [Display(Name = "Year Level")]
        public int YearLevel { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Section")]
        public string Section { get; set; }

        [StringLength(100)]
        [Display(Name = "Adviser")]
        public string Adviser { get; set; }

        [Display(Name = "Grade & Section")]
        public string GradeSec => $"Grade {YearLevel} - {Section}";

        // Guardian Info
        [Required]
        [StringLength(100)]
        [Display(Name = "Guardian Name")]
        public string GuardianName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Relationship")]
        public string Relationship { get; set; }

        [Required]
        [RegularExpression(@"^\d{11}$")]
        [StringLength(11)]
        [Display(Name = "Guardian Contact")]
        public string GuardianContact { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Guardian Address")]
        public string GuardianAddress { get; set; }

        // Guidance Info
        [StringLength(200)]
        [Display(Name = "Reason for Counseling")]
        public string Reason { get; set; }

        [StringLength(50)]
        [Display(Name = "Case Type")]
        public string CaseType { get; set; }

        [StringLength(50)]
        [Display(Name = "Counseling Status")]
        public string CounselingStatus { get; set; }

        [StringLength(500)]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }
    }
}