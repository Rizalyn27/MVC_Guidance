using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_DenoyJabines.Models
{
    public class CaseNotes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [HiddenInput(DisplayValue = false)]
        public int CaseNoteId { get; set; }

        [Required]
        [Display(Name = "Student ID")]
        public int StuID { get; set; }

        [ForeignKey("StuID")]
        [Display(Name = "Student")]
        public Students? Student { get; set; }

        // I. Background
        [StringLength(2000)]
        [Display(Name = "Background of the Case")]
        public string? BackgroundOfCase { get; set; }

        // II. Counseling Plan
        [StringLength(200)]
        [Display(Name = "Counseling Approach")]
        public string? CounselingApproach { get; set; }

        [StringLength(2000)]
        [Display(Name = "Counseling Goals")]
        public string? CounselingGoals { get; set; }

        // III. Comments
        [StringLength(2000)]
        [Display(Name = "Counselor's Comments")]
        public string? Comments { get; set; }

        // IV. Recommendations
        [StringLength(2000)]
        [Display(Name = "Recommendations")]
        public string? Recommendations { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Last Updated")]
        public DateTime? UpdatedAt { get; set; }
    }
}