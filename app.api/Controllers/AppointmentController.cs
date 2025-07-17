using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using app.api.DTOs;
using app.api.Services;
using System;

namespace app.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentService _appointmentService;

        public AppointmentController(AppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost]
        public async Task<ActionResult<AppointmentDto>> CreateAppointment([FromBody] CreateAppointmentRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userId = GetCurrentUserId();
                var userType = GetCurrentUserType();

                if (userType != "Patient")
                    return Forbid();

                var appointment = await _appointmentService.CreateAppointmentAsync(userId, request);
                
                if (appointment == null)
                    return BadRequest(new { message = "Erro ao criar agendamento. Verifique se o horário está disponível." });

                return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, appointment);
            }
            catch (Exception ex)
            {
                // Log da exceção para diagnóstico
                Console.WriteLine($"Erro ao criar agendamento: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return StatusCode(500, new { message = "Erro interno do servidor. Verifique os logs." });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AppointmentDto>> UpdateAppointment(int id, [FromBody] UpdateAppointmentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetCurrentUserId();
            var appointment = await _appointmentService.UpdateAppointmentAsync(id, userId, request);
            
            if (appointment == null)
                return NotFound(new { message = "Agendamento não encontrado ou você não tem permissão para editá-lo." });

            return Ok(appointment);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> CancelAppointment(int id)
        {
            var userId = GetCurrentUserId();
            var success = await _appointmentService.CancelAppointmentAsync(id, userId);
            
            if (!success)
                return NotFound(new { message = "Agendamento não encontrado ou você não tem permissão para cancelá-lo." });

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDto>> GetAppointment(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            
            if (appointment == null)
                return NotFound(new { message = "Agendamento não encontrado." });

            return Ok(appointment);
        }

        [HttpGet]
        public async Task<ActionResult<List<AppointmentDto>>> GetAppointments([FromQuery] AppointmentSearchRequest request)
        {
            var appointments = await _appointmentService.GetAppointmentsAsync(request);
            return Ok(appointments);
        }

        [HttpGet("my-appointments")]
        public async Task<ActionResult<List<AppointmentDto>>> GetMyAppointments()
        {
            var userId = GetCurrentUserId();
            var userType = GetCurrentUserType();
            var isDoctor = userType == "Doctor";

            var appointments = await _appointmentService.GetUserAppointmentsAsync(userId, isDoctor);
            return Ok(appointments);
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