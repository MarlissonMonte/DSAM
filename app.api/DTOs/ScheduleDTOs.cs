using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace app.api.DTOs
{
    public class CreateScheduleRequest
    {
        [Required]
        public TimeSpan StartTime { get; set; }
        
        [Required]
        public TimeSpan EndTime { get; set; }
        
        [Required]
        [Range(15, 120)]
        public int DurationMinutes { get; set; } = 30;
    }

    public class UpdateScheduleRequest
    {
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int? DurationMinutes { get; set; }
    }

    public class ScheduleDto
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        [JsonIgnore]
        public TimeSpan StartTime { get; set; }
        [JsonIgnore]
        public TimeSpan EndTime { get; set; }
        public string StartTimeString => StartTime.ToString(@"hh\:mm\:ss");
        public string EndTimeString => EndTime.ToString(@"hh\:mm\:ss");
        public int DurationMinutes { get; set; }
        public List<TimeSlotDto> AvailableTimeSlots { get; set; } = new();
    }

    public class TimeSlotDto
    {
        [JsonIgnore]
        public TimeSpan StartTime { get; set; }
        [JsonIgnore]
        public TimeSpan EndTime { get; set; }
        public string StartTimeString => StartTime.ToString(@"hh\:mm\:ss");
        public string EndTimeString => EndTime.ToString(@"hh\:mm\:ss");
        public bool IsAvailable { get; set; }
    }

    public class ScheduleSearchRequest
    {
        public int? DoctorId { get; set; }
        public bool? AvailableOnly { get; set; } = true;
    }
} 