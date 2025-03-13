using Microsoft.AspNetCore.Mvc;
using Application.Dtos;
using Application.Services;
using Domain.Entities;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class budgetRecordController : ControllerBase
    {
        private IBudgetRecordService _budgetRecordService;
        public budgetRecordController(IBudgetRecordService budgetRecordService)
        {
            this._budgetRecordService = budgetRecordService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var budgetRecord = await _budgetRecordService.GetById(id);
            if (budgetRecord == null)
            {
                return NotFound();
            }
            return Ok(budgetRecord);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var budgetRecordzes = await _budgetRecordService.GetAll();
            return Ok(budgetRecordzes);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] BudgetRecordDto budgetRecord)
        {
            if (budgetRecord == null)
            {
                return NotFound();
            }
            await _budgetRecordService.Add(budgetRecord);
            return Ok(budgetRecord.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] BudgetRecordDto budgetRecord)
        {
            var result = await _budgetRecordService.Update(budgetRecord);
            if (!result)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _budgetRecordService.Delete(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}



