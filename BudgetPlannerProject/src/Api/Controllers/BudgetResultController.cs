using Microsoft.AspNetCore.Mvc;
using Application.Dtos;
using Application.Services;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class budgetResultController : ControllerBase
    {
        private IBudgetResultService budgetResultService;
        public budgetResultController(IBudgetResultService budgetResultService)
        {
            this.budgetResultService = budgetResultService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var budgetResult = await budgetResultService.GetById(id);
            return Ok(budgetResult);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var budgetResultzes = await budgetResultService.GetAll();
            return Ok(budgetResultzes);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] BudgetResultDto budgetResult)
        {
            await budgetResultService.Add(budgetResult);
            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] BudgetResultDto budgetResult)
        {
            var result = await budgetResultService.Update(budgetResult);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var result = await budgetResultService.Delete(id);
            return Ok(result);
        }
    }
}



