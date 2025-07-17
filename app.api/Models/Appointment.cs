using System.ComponentModel.DataAnnotations;

namespace app.api.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        
        [Required]
        public int DoctorId { get; set; }
        
        [Required]
        public int PatientId { get; set; }
        
        [Required]
        public DateTime AppointmentDate { get; set; }
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        [Required]
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual User Doctor { get; set; } = null!;
        public virtual User Patient { get; set; } = null!;
    }
    
    public enum AppointmentStatus
    {
        Scheduled,
        Confirmed,
        Completed,
        Cancelled,
        NoShow
    }
} 