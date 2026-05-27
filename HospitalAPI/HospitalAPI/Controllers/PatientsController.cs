using HospitalAPI.DTOs;
using HospitalAPI.Exceptions;
using HospitalAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IDbService _dbService;
        public PatientsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPatients([FromQuery] string? search)
        {
            var result = await _dbService.GetPatientsWithSearchAsync(search);
            return Ok(result);
        }

        [HttpPost]
        [Route("{pesel}/bedassignments")]
        public async Task<IActionResult> CreateBedAssignment(string pesel, [FromBody] CreateBedAssignmentDto dto)
        {
            try
            {
                await _dbService.CreateBedAssignmentAsync(pesel, dto);
                return Created();
            }
            catch(NotFoundException e )
            {
                return NotFound(e.Message);
            }
        }
    }
}
