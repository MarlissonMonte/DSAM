using System.ComponentModel.DataAnnotations;

namespace app.api.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        
        [Required]
        public int DoctorId { get; set; }
        
        [Required]
        public TimeSpan StartTime { get; set; }
        
        [Required]
        public TimeSpan EndTime { get; set; }
        
        [Required]
        public int DurationMinutes { get; set; } = 30; // Duração padrão de cada consulta
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual User Doctor { get; set; } = null!;
    }
} 