using System;
using System.ComponentModel.DataAnnotations;

public class Appointment
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public DateTime AppointmentDate { get; set; }

    
}
