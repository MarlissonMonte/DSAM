using System.ComponentModel.DataAnnotations;

namespace app.api.DTOs
{
    public class CreateAppointmentRequest
    {
        [Required]
        public int DoctorId { get; set; }
        
        [Required]
        public DateTime AppointmentDate { get; set; }
        
        public string? Notes { get; set; }
    }

    public class UpdateAppointmentRequest
    {
        public DateTime? AppointmentDate { get; set; }
        public string? Notes { get; set; }
        public string? Status { get; set; }
    }

    public class AppointmentDto
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string AppointmentDate { get; set; } = string.Empty; // data (yyyy-MM-dd)
        public string AppointmentTime { get; set; } = string.Empty; // hora (HH:mm)
        public string? Notes { get; set; }
        public string Status { get; set; } = string.Empty;
        public string CreatedAt { get; set; } = string.Empty;
    }

    public class AppointmentSearchRequest
    {
        public int? DoctorId { get; set; }
        public int? PatientId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Status { get; set; }
    }
} 