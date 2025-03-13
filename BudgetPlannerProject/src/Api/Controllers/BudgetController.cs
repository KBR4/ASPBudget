using Microsoft.AspNetCore.Mvc;
using Application.Dtos;
using Application.Services;
using Domain.Entities;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class budgetController : ControllerBase
    {
        private IBudgetService _budgetService;
        public budgetController(IBudgetService budgetService)
        {
            this._budgetService = budgetService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var budget = await _budgetService.GetById(id);
            if (budget == null)
            {
                return NotFound();
            }
            return Ok(budget);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var budgetzes = await _budgetService.GetAll();
            return Ok(budgetzes);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] BudgetDto budget)
        {
            if (budget == null)
            {
                return NotFound();
            }
            await _budgetService.Add(budget);
            return Ok(budget.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] BudgetDto budget)
        {
            var result = await _budgetService.Update(budget);
            if (!result)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _budgetService.Delete(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
