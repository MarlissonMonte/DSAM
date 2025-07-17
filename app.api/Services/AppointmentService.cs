using app.api.Data;
using app.api.DTOs;
using app.api.Models;
using Microsoft.EntityFrameworkCore;

namespace app.api.Services
{
    public class AppointmentService
    {
        private readonly ApplicationDbContext _context;

        public AppointmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AppointmentDto?> CreateAppointmentAsync(int patientId, CreateAppointmentRequest request)
        {
            try
            {
                Console.WriteLine($"[LOG] CreateAppointmentAsync chamado. patientId: {patientId}, DoctorId: {request.DoctorId}, AppointmentDate: {request.AppointmentDate}, Notes: {request.Notes}");
                // Verificar se o paciente existe
                var patient = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == patientId && u.UserType == UserType.Patient);
                
                if (patient == null)
                    return null;

                // Verificar se o médico existe
                var doctor = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == request.DoctorId && u.UserType == UserType.Doctor);
                
                if (doctor == null)
                    return null;

                // Normalizar data/hora para ignorar segundos e garantir Kind=Utc
                var normalizedDate = DateTime.SpecifyKind(
                    new DateTime(
                        request.AppointmentDate.Year,
                        request.AppointmentDate.Month,
                        request.AppointmentDate.Day,
                        request.AppointmentDate.Hour,
                        request.AppointmentDate.Minute,
                        0
                    ),
                    DateTimeKind.Utc
                );

                // Verificar se já existe um agendamento para este médico e este horário (independente do paciente)
                var conflict = await _context.Appointments
                    .FirstOrDefaultAsync(a => a.DoctorId == request.DoctorId &&
                                             a.AppointmentDate == normalizedDate &&
                                             a.Status != AppointmentStatus.Cancelled);
                if (conflict != null)
                    return null;

                var appointment = new Appointment
                {
                    DoctorId = request.DoctorId,
                    PatientId = patientId,
                    AppointmentDate = normalizedDate,
                    Notes = request.Notes,
                    Status = AppointmentStatus.Scheduled
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                return await GetAppointmentByIdAsync(appointment.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO] Exception em CreateAppointmentAsync: {ex.Message}");
                Console.WriteLine($"[ERRO] StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<AppointmentDto?> UpdateAppointmentAsync(int appointmentId, int userId, UpdateAppointmentRequest request)
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointmentId && 
                                        (a.DoctorId == userId || a.PatientId == userId));

            if (appointment == null)
                return null;

            if (request.AppointmentDate.HasValue)
                appointment.AppointmentDate = request.AppointmentDate.Value;
            
            if (request.Notes != null)
                appointment.Notes = request.Notes;
            
            if (request.Status != null && Enum.TryParse<AppointmentStatus>(request.Status, out var status))
                appointment.Status = status;

            appointment.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return await GetAppointmentByIdAsync(appointment.Id);
        }

        public async Task<bool> CancelAppointmentAsync(int appointmentId, int userId)
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointmentId && 
                                        (a.DoctorId == userId || a.PatientId == userId));

            if (appointment == null)
                return false;

            appointment.Status = AppointmentStatus.Cancelled;
            appointment.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<AppointmentDto?> GetAppointmentByIdAsync(int appointmentId)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
                return null;

            return MapToAppointmentDto(appointment);
        }

        public async Task<List<AppointmentDto>> GetAppointmentsAsync(AppointmentSearchRequest request)
        {
            var query = _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .AsQueryable();

            if (request.DoctorId.HasValue)
                query = query.Where(a => a.DoctorId == request.DoctorId.Value);

            if (request.PatientId.HasValue)
                query = query.Where(a => a.PatientId == request.PatientId.Value);

            if (request.StartDate.HasValue)
                query = query.Where(a => a.AppointmentDate >= request.StartDate.Value);

            if (request.EndDate.HasValue)
                query = query.Where(a => a.AppointmentDate <= request.EndDate.Value);

            if (!string.IsNullOrEmpty(request.Status))
                query = query.Where(a => a.Status.ToString() == request.Status);

            var appointments = await query.OrderBy(a => a.AppointmentDate).ToListAsync();
            return appointments.Select(MapToAppointmentDto).ToList();
        }

        public async Task<List<AppointmentDto>> GetUserAppointmentsAsync(int userId, bool isDoctor = false)
        {
            var query = _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .AsQueryable();

            if (isDoctor)
                query = query.Where(a => a.DoctorId == userId);
            else
                query = query.Where(a => a.PatientId == userId);

            var appointments = await query
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();

            return appointments.Select(MapToAppointmentDto).ToList();
        }

        private static AppointmentDto MapToAppointmentDto(Appointment appointment)
        {
            return new AppointmentDto
            {
                Id = appointment.Id,
                DoctorId = appointment.DoctorId,
                DoctorName = appointment.Doctor.Name,
                PatientId = appointment.PatientId,
                PatientName = appointment.Patient.Name,
                AppointmentDate = appointment.AppointmentDate.ToString("yyyy-MM-dd"),
                AppointmentTime = appointment.AppointmentDate.ToString("HH:mm"),
                Notes = appointment.Notes,
                Status = appointment.Status.ToString(),
                CreatedAt = appointment.CreatedAt.ToString("yyyy-MM-ddTHH:mm")
            };
        }
    }
} 