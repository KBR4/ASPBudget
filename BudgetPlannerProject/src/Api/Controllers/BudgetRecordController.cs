using Microsoft.AspNetCore.Mvc;
using Application.Dtos;
using Application.Services;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class budgetRecordController : ControllerBase
    {
        private IBudgetRecordService budgetRecordService;
        public budgetRecordController(IBudgetRecordService budgetRecordService)
        {
            this.budgetRecordService = budgetRecordService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var budgetRecord = await budgetRecordService.GetById(id);
            return Ok(budgetRecord);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var budgetRecordzes = await budgetRecordService.GetAll();
            return Ok(budgetRecordzes);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] BudgetRecordDto budgetRecord)
        {
            await budgetRecordService.Add(budgetRecord);
            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] BudgetRecordDto budgetRecord)
        {
            var result = await budgetRecordService.Update(budgetRecord);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var result = await budgetRecordService.Delete(id);
            return Ok(result);
        }
    }
}



