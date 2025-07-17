using app.api.Data;
using app.api.DTOs;
using app.api.Models;
using Microsoft.EntityFrameworkCore;

namespace app.api.Services
{
    public class ScheduleService
    {
        private readonly ApplicationDbContext _context;

        public ScheduleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ScheduleDto?> CreateScheduleAsync(int doctorId, CreateScheduleRequest request)
        {
            // Verificar se o médico existe
            var doctor = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == doctorId && u.UserType == UserType.Doctor);
            
            if (doctor == null)
                return null;

            // Verificar se já existe um horário para este médico
            var existingSchedule = await _context.Schedules
                .FirstOrDefaultAsync(s => s.DoctorId == doctorId);

            if (existingSchedule != null)
                return null;

            var schedule = new Schedule
            {
                DoctorId = doctorId,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                DurationMinutes = request.DurationMinutes
            };

            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();

            return await GetScheduleByIdAsync(schedule.Id);
        }

        public async Task<ScheduleDto?> UpdateScheduleAsync(int scheduleId, int doctorId, UpdateScheduleRequest request)
        {
            var schedule = await _context.Schedules
                .FirstOrDefaultAsync(s => s.Id == scheduleId && s.DoctorId == doctorId);

            if (schedule == null)
                return null;

            if (request.StartTime.HasValue)
                schedule.StartTime = request.StartTime.Value;
            
            if (request.EndTime.HasValue)
                schedule.EndTime = request.EndTime.Value;
            
            if (request.DurationMinutes.HasValue)
                schedule.DurationMinutes = request.DurationMinutes.Value;

            schedule.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return await GetScheduleByIdAsync(schedule.Id);
        }

        public async Task<bool> DeleteScheduleAsync(int scheduleId, int doctorId)
        {
            var schedule = await _context.Schedules
                .FirstOrDefaultAsync(s => s.Id == scheduleId && s.DoctorId == doctorId);

            if (schedule == null)
                return false;

            // Não precisa mais verificar agendamentos vinculados

            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ScheduleDto?> GetScheduleByIdAsync(int scheduleId)
        {
            var schedule = await _context.Schedules
                .Include(s => s.Doctor)
                .FirstOrDefaultAsync(s => s.Id == scheduleId);

            if (schedule == null)
                return null;

            var dto = MapToScheduleDto(schedule);
            dto.AvailableTimeSlots = GenerateTimeSlots(schedule);
            return dto;
        }

        public async Task<List<ScheduleDto>> GetSchedulesAsync(ScheduleSearchRequest request)
        {
            var query = _context.Schedules
                .Include(s => s.Doctor)
                .AsQueryable();

            if (request.DoctorId.HasValue)
                query = query.Where(s => s.DoctorId == request.DoctorId.Value);

            var schedules = await query.ToListAsync();
            return schedules.Select(s => {
                var dto = MapToScheduleDto(s);
                dto.AvailableTimeSlots = GenerateTimeSlots(s);
                return dto;
            }).ToList();
        }

        public async Task<List<TimeSlotDto>> GetAvailableTimeSlotsAsync(int doctorId, DateTime date)
        {
            var schedule = await _context.Schedules
                .FirstOrDefaultAsync(s => s.DoctorId == doctorId);

            if (schedule == null)
                return new List<TimeSlotDto>();

            // Buscar todos os appointments do médico para a data
            var appointments = await _context.Appointments
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate.Date == date.Date)
                .ToListAsync();

            var timeSlots = new List<TimeSlotDto>();
            var currentTime = schedule.StartTime;
            while (currentTime.Add(TimeSpan.FromMinutes(schedule.DurationMinutes)) <= schedule.EndTime)
            {
                var endTime = currentTime.Add(TimeSpan.FromMinutes(schedule.DurationMinutes));
                // Verifica se já existe appointment nesse slot (comparando apenas horas e minutos)
                var hasAppointment = appointments.Any(a =>
                    a.AppointmentDate.Date == date.Date &&
                    a.AppointmentDate.TimeOfDay.Hours == currentTime.Hours &&
                    a.AppointmentDate.TimeOfDay.Minutes == currentTime.Minutes);
                timeSlots.Add(new TimeSlotDto
                {
                    StartTime = currentTime,
                    EndTime = endTime,
                    IsAvailable = !hasAppointment
                });
                currentTime = endTime;
            }
            return timeSlots;
        }

        private static ScheduleDto MapToScheduleDto(Schedule schedule)
        {
            return new ScheduleDto
            {
                Id = schedule.Id,
                DoctorId = schedule.DoctorId,
                DoctorName = schedule.Doctor.Name,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                DurationMinutes = schedule.DurationMinutes,
                AvailableTimeSlots = GenerateTimeSlots(schedule)
            };
        }

        private static List<TimeSlotDto> GenerateTimeSlots(Schedule schedule)
        {
            var timeSlots = new List<TimeSlotDto>();
            var currentTime = schedule.StartTime;
            while (currentTime.Add(TimeSpan.FromMinutes(schedule.DurationMinutes)) <= schedule.EndTime)
            {
                var endTime = currentTime.Add(TimeSpan.FromMinutes(schedule.DurationMinutes));
                timeSlots.Add(new TimeSlotDto
                {
                    StartTime = currentTime,
                    EndTime = endTime,
                    IsAvailable = true
                });
                currentTime = endTime;
            }
            return timeSlots;
        }
    }
} 