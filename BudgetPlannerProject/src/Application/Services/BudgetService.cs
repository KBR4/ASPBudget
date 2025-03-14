using Application.Dtos;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BudgetService : IBudgetService
    {
        private IBudgetRepository _budgetRepository;
        private IMapper _mapper;

        public BudgetService(IBudgetRepository budgetRepository, IMapper mapper)
        {
            _budgetRepository = budgetRepository;
            _mapper = mapper;
        }

        public async Task<int> Add(BudgetDto budget)
        {
            var mappedBudget = _mapper.Map<Budget>(budget);
            if (mappedBudget != null)
            {
                await _budgetRepository.Create(mappedBudget);
            }
            return -1;
        }

        public async Task<bool> Delete(int id)
        {
            return await _budgetRepository.Delete(id);
        }

        public async Task<List<BudgetDto>> GetAll()
        {
            var budgets = await _budgetRepository.ReadAll();
            var mappedBudgets = budgets.Select(q => _mapper.Map<BudgetDto>(q)).ToList();
            return mappedBudgets;
        }

        public async Task<BudgetDto?> GetById(int id)
        {
            var budget = await _budgetRepository.ReadById(id);
            var mappedBudget = _mapper.Map<BudgetDto>(budget);
            return mappedBudget;
        }

        public async Task<bool> Update(BudgetDto budget)
        {
            if (budget == null)
            {
                return false;
            }
            var mappedBudget = _mapper.Map<Budget>(budget);
            return await _budgetRepository.Update(mappedBudget);
        }
    }
}
