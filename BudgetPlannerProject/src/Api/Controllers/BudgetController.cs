using Microsoft.AspNetCore.Mvc;
using Application.Dtos;
using Application.Services;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class budgetController : ControllerBase
    {
        private IBudgetService budgetService;
        public budgetController(IBudgetService budgetService)
        {
            this.budgetService = budgetService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var budget = await budgetService.GetById(id);
            return Ok(budget);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var budgetzes = await budgetService.GetAll();
            return Ok(budgetzes);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] BudgetDto budget)
        {
            await budgetService.Add(budget);
            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] BudgetDto budget)
        {
            var result = await budgetService.Update(budget);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var result = await budgetService.Delete(id);
            return Ok(result);
        }
    }
}
