using AutoMapper;
using BusinessAccess.Services.Interfaces;
using DataAccess.Entities;
using GenderHealcareSystem.CustomActionFilters;
using GenderHealcareSystem.DTO;
using GenderHealcareSystem.DTO.Request;
using Microsoft.AspNetCore.Mvc;

namespace GenderHealcareSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffConsultantController : ControllerBase
    {
        private readonly IStaffConsultantService _service;
        private readonly IMapper _mapper;

        public StaffConsultantController(IStaffConsultantService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        //UPDATE EXIST StaffConsultant
        [HttpPut("{id:guid}, {roleId:guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateStaffConsultantRequest dto, [FromRoute] Guid roleId)
        {
            //Convert Dto to domain model
            var StaffConsultantDomain = _mapper.Map<User>(dto);

            //Update user in DB
            StaffConsultantDomain = await _service.UpdateAsync(id, StaffConsultantDomain, roleId);

            if (StaffConsultantDomain == null)
                return NotFound();

            //Convert Domain model to Dto
            var StaffConsultantDto = _mapper.Map<StaffConsultantDto>(StaffConsultantDomain);

            return Ok(StaffConsultantDto);
        }

        //DELETE EXIST StaffConsultant
        [HttpDelete("{id:guid}, {roleId:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, [FromRoute] Guid roleId)
        {
            // Delete from DB by Id
            var isDeleted = await _service.DeleteAsync(id, roleId);

            if (!isDeleted)
                return NotFound();

            return NoContent();
        }

        // REVIVE EXIST StaffConsultant
        [HttpPut("{id:guid}, {roleId:guid}")]
        public async Task<IActionResult> Revive([FromRoute] Guid id, [FromRoute] Guid roleId)
        {
            // Revive from DB by Id
            var isRevived = await _service.ReviveAsync(id, roleId);

            if (!isRevived)
                return NotFound();

            return Ok();
        }
    }
}
