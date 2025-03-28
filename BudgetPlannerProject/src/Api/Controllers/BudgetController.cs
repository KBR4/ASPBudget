﻿using Microsoft.AspNetCore.Mvc;
using Application.Dtos;
using Application.Services;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetService _budgetService;

        public BudgetController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
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
            var budgets = await _budgetService.GetAll();
            return Ok(budgets);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] BudgetDto budget)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var budgetId = await _budgetService.Add(budget);
            var res = new { Id = budgetId };
            return CreatedAtAction(nameof(GetById), new { id = budgetId }, res);
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