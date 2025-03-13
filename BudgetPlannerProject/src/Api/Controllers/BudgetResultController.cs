using Microsoft.AspNetCore.Mvc;
using Application.Dtos;
using Application.Services;
using Domain.Entities;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class budgetResultController : ControllerBase
    {
        private IBudgetResultService _budgetResultService;
        public budgetResultController(IBudgetResultService budgetResultService)
        {
            this._budgetResultService = budgetResultService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var budgetResult = await _budgetResultService.GetById(id);
            if (budgetResult == null)
            {
                return NotFound();
            }
            return Ok(budgetResult);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var budgetResultzes = await _budgetResultService.GetAll();
            return Ok(budgetResultzes);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] BudgetResultDto budgetResult)
        {
            if (budgetResult == null)
            {
                return NotFound();
            }
            await _budgetResultService.Add(budgetResult);
            return Ok(budgetResult.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] BudgetResultDto budgetResult)
        {
            var result = await _budgetResultService.Update(budgetResult);
            if (!result)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _budgetResultService.Delete(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}



