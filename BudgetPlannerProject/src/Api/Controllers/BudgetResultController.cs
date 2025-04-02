using Microsoft.AspNetCore.Mvc;
using Application.Dtos;
using Application.Services;
using Application.Requests;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BudgetResultController : ControllerBase
    {
        private readonly IBudgetResultService _budgetResultService;
        
        public BudgetResultController(IBudgetResultService budgetResultService)
        {
            _budgetResultService = budgetResultService;
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
            var budgetResults = await _budgetResultService.GetAll();
            return Ok(budgetResults);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateBudgetResultRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var budgetResultId = await _budgetResultService.Add(request);
            var res = new { Id = budgetResultId };
            return CreatedAtAction(nameof(GetById), new { id = budgetResultId }, res);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBudgetResultRequest request)
        {
            var result = await _budgetResultService.Update(request);
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