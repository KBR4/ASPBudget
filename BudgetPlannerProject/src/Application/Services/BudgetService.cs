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
        private IMapper mapper;
        public BudgetService(IBudgetRepository budgetRepository, IMapper mapper)
        {
            this._budgetRepository = budgetRepository;
            this.mapper = mapper;
        }
        public async Task<int> Add(BudgetDto budget)
        {
            var mappedbudget = mapper.Map<Budget>(budget);
            if (mappedbudget != null)
            {
                await _budgetRepository.Create(mappedbudget);
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
            var mappedbudgets = budgets.Select(q => mapper.Map<BudgetDto>(q)).ToList();
            return mappedbudgets;
        }

        public async Task<BudgetDto?> GetById(int id)
        {
            var budget = await _budgetRepository.ReadById(id);
            var mappedbudget = mapper.Map<BudgetDto>(budget);
            return mappedbudget;
        }

        public async Task<bool> Update(BudgetDto budget)
        {
            var mappedbudget = mapper.Map<Budget>(budget);
            return await _budgetRepository.Update(mappedbudget);
        }
    }
}
