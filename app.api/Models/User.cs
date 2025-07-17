using System.ComponentModel.DataAnnotations;

namespace app.api.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Password { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;
        
        [Required]
        public UserType UserType { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual ICollection<Appointment> AppointmentsAsDoctor { get; set; } = new List<Appointment>();
        public virtual ICollection<Appointment> AppointmentsAsPatient { get; set; } = new List<Appointment>();
        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
    
    public enum UserType
    {
        Doctor,
        Patient
    }
} 