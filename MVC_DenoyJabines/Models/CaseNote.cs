using System.ComponentModel.DataAnnotations;

namespace MVC_DenoyJabines.Models
{
    public class CaseNote
    {
        public int Id { get; set; }

        // Link to student
        public int StudentId { get; set; }
        public Students? Student { get; set; }

        [Required(ErrorMessage = "Background is required.")]
        [Display(Name = "Background of the Case")]
        public string Background { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a counseling approach.")]
        [Display(Name = "Counseling Approach")]
        public string CounselingApproach { get; set; } = string.Empty;

        [Required(ErrorMessage = "Counseling goals are required.")]
        [Display(Name = "Counseling Goals")]
        public string CounselingGoals { get; set; } = string.Empty;

        [Display(Name = "Comments")]
        public string? Comments { get; set; }

        [Display(Name = "Recommendations")]
        public string? Recommendations { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
