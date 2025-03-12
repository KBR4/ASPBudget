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
        private IBudgetRepository budgetRepository;
        private IMapper mapper;
        public BudgetService(IBudgetRepository budgetRepository, IMapper mapper)
        {
            this.budgetRepository = budgetRepository;
            this.mapper = mapper;
        }
        public async Task Add(BudgetDto budget)
        {
            var mappedbudget = mapper.Map<Budget>(budget);
            if (mappedbudget != null)
            {
                await budgetRepository.Create(mappedbudget);
            }
        }

        public async Task<bool> Delete(int id)
        {
            return await budgetRepository.Delete(id);
        }

        public async Task<List<BudgetDto>> GetAll()
        {
            var budgets = await budgetRepository.ReadAll();
            var mappedbudgets = budgets.Select(q => mapper.Map<BudgetDto>(q)).ToList();
            return mappedbudgets;
        }

        public async Task<BudgetDto?> GetById(int id)
        {
            var budget = await budgetRepository.ReadById(id);
            var mappedbudget = mapper.Map<BudgetDto>(budget);
            return mappedbudget;
        }

        public async Task<bool> Update(BudgetDto budget)
        {
            var mappedbudget = mapper.Map<Budget>(budget);
            return await budgetRepository.Update(mappedbudget);
        }
    }
}
