using Microsoft.AspNetCore.Mvc;
using Application.Dtos;
using Application.Services;
using Application.Requests;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BudgetRecordController : ControllerBase
    {
        private readonly IBudgetRecordService _budgetRecordService;

        public BudgetRecordController(IBudgetRecordService budgetRecordService)
        {
            _budgetRecordService = budgetRecordService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var budgetRecord = await _budgetRecordService.GetById(id);
            return Ok(budgetRecord);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var budgetRecords = await _budgetRecordService.GetAll();
            return Ok(budgetRecords);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateBudgetRecordRequest request)
        {
            var budgetRecordId = await _budgetRecordService.Add(request);
            var res = new { Id = budgetRecordId };
            return CreatedAtAction(nameof(GetById), new { id = budgetRecordId }, res);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBudgetRecordRequest request)
        {
            await _budgetRecordService.Update(request);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _budgetRecordService.Delete(id);
            return NoContent();
        }
    }
}