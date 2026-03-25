using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_DenoyJabines.Models
{
    public class Students
    {
        // Basic Info

        //ID
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [HiddenInput(DisplayValue = false)]
        public int StuID { get; set; }

        //Student ID
        [Required(ErrorMessage = "Student ID is required")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Student ID must be between 4 and 20 characters")]
        public string StuLRN { get; set; }

        //First Name, Last Name, Middle Name
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name must not exceed 50 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "First name can only contain letters")]
        public string StuFName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name must not exceed 50 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Last name can only contain letters")]
        public string StuLName { get; set; }


        [StringLength(50, ErrorMessage = "Middle name must not exceed 50 characters")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Middle name can only contain letters")]
        public string StuMName { get; set; }

        public bool StuStatus { get; set; }

        public string StuFullName => $"{StuFName} {StuMName} {StuLName}".Trim();


        //Gender
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        //Birthdate
        [Required(ErrorMessage = "Birthdate is required")]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }

        //Address
        [Required(ErrorMessage = "Student address is required")]
        [StringLength(200, ErrorMessage = "Address must not exceed 200 characters")]
        public string Address { get; set; }

        //Student Contact Number
        [Required(ErrorMessage = "Contact number is required")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Contact number must be exactly 11 digits")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Contact number must be exactly 11 digits")]
        public string Contact { get; set; }

        //Student Email
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100, ErrorMessage = "Email must not exceed 100 characters")]
        public string Email { get; set; }



        // Academic Info
        //Department
        [Required(ErrorMessage = "Department is required")]
        [StringLength(100, ErrorMessage = "Department must not exceed 100 characters")]
        public string Department { get; set; }

        //Year level
        [Required(ErrorMessage = "Year level is required")]
        [Range(7, 12, ErrorMessage = "Year level must be between 7 and 12")]
        public int YearLevel { get; set; }

        //Section
        [Required(ErrorMessage = "Section is required")]
        [StringLength(20, ErrorMessage = "Section must not exceed 20 characters")]
        public string Section { get; set; }

        //Adviser
        [StringLength(100, ErrorMessage = "Adviser name must not exceed 100 characters")]
        public string Adviser { get; set; }

        public string GradeSec => $"Grade {YearLevel} - {Section}";


        // Guardian Info
        //Guardian Name
        [Required(ErrorMessage = "Guardian name is required")]
        [StringLength(100, ErrorMessage = "Guardian name must not exceed 100 characters")]
        public string GuardianName { get; set; }

        //Guardian Relationship
        [Required(ErrorMessage = "Relationship is required")]
        [StringLength(50, ErrorMessage = "Relationship must not exceed 50 characters")]
        public string Relationship { get; set; }

        //Guardian Contact Number
        [Required(ErrorMessage = "Guardian contact number is required")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Contact number must be exactly 11 digits")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Contact number must be exactly 11 digits")]
        public string GuardianContact { get; set; }

        //Guardian Address
        [Required(ErrorMessage = "Guardian address is required")]
        [StringLength(200, ErrorMessage = "Guardian address must not exceed 200 characters")]
        public string GuardianAddress { get; set; }


        // Guidance Info

        //Reason for counseling
        [StringLength(200, ErrorMessage = "Reason must not exceed 200 characters")]
        public string Reason { get; set; }

        //Case type
        [StringLength(50, ErrorMessage = "Case type must not exceed 50 characters")]
        public string CaseType { get; set; }

        //Counseling status
        [StringLength(50, ErrorMessage = "Status must not exceed 50 characters")]
        public string CounselingStatus { get; set; }

        //Remarks
        [StringLength(500, ErrorMessage = "Remarks must not exceed 500 characters")]
        public string Remarks { get; set; }
    }
}
