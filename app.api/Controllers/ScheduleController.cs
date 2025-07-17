using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using app.api.DTOs;
using app.api.Services;

namespace app.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ScheduleController : ControllerBase
    {
        private readonly ScheduleService _scheduleService;

        public ScheduleController(ScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpPost]
        public async Task<ActionResult<ScheduleDto>> CreateSchedule([FromBody] CreateScheduleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetCurrentUserId();
            var userType = GetCurrentUserType();

            if (userType != "Doctor")
                return Forbid();

            var schedule = await _scheduleService.CreateScheduleAsync(userId, request);
            
            if (schedule == null)
                return BadRequest(new { message = "Erro ao criar horário. Verifique se não há conflito de horários." });

            return CreatedAtAction(nameof(GetSchedule), new { id = schedule.Id }, schedule);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ScheduleDto>> UpdateSchedule(int id, [FromBody] UpdateScheduleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetCurrentUserId();
            var userType = GetCurrentUserType();

            if (userType != "Doctor")
                return Forbid();

            var schedule = await _scheduleService.UpdateScheduleAsync(id, userId, request);
            
            if (schedule == null)
                return NotFound(new { message = "Horário não encontrado ou você não tem permissão para editá-lo." });

            return Ok(schedule);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSchedule(int id)
        {
            var userId = GetCurrentUserId();
            var userType = GetCurrentUserType();

            if (userType != "Doctor")
                return Forbid();

            var success = await _scheduleService.DeleteScheduleAsync(id, userId);
            
            if (!success)
                return BadRequest(new { message = "Não foi possível excluir o horário. Verifique se não há agendamentos." });

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ScheduleDto>> GetSchedule(int id)
        {
            var schedule = await _scheduleService.GetScheduleByIdAsync(id);
            
            if (schedule == null)
                return NotFound(new { message = "Horário não encontrado." });

            return Ok(schedule);
        }

        [HttpGet]
        public async Task<ActionResult<List<ScheduleDto>>> GetSchedules([FromQuery] ScheduleSearchRequest request)
        {
            var schedules = await _scheduleService.GetSchedulesAsync(request);
            return Ok(schedules);
        }

        [HttpGet("available-slots/{doctorId}/{date:datetime}")]
        public async Task<ActionResult<List<TimeSlotDto>>> GetAvailableTimeSlots(int doctorId, DateTime date)
        {
            var dateUtc = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            var timeSlots = await _scheduleService.GetAvailableTimeSlotsAsync(doctorId, dateUtc);
            return Ok(timeSlots);
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return int.Parse(userIdClaim?.Value ?? "0");
        }

        private string GetCurrentUserType()
        {
            var userTypeClaim = User.FindFirst("UserType");
            return userTypeClaim?.Value ?? "";
        }
    }
} 